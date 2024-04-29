using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Documents;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.GameUI.Scene.Game_ItemGrowth2;
public partial class ItemGrowth2TooltipPanel
{
	#region Constructors
	public ItemGrowth2TooltipPanel()
	{
#if DEVELOP
		// General_Emblem_RynSword_Style22_0003_108
		DataContext = Helpers.TestProvider.Provider.GetTable<Item>()["Test_N-ShopAccountShippingItem"].Source;
#endif

		InitializeComponent();
	}
	#endregion

	#region Methods
	protected override void OnLoaded(RoutedEventArgs e)
	{
		if (DataContext is not Record record) return;
		ItemGrowth2TooltipPanel_Contents.Items.OfType<UIElement>().ForEach(e => e.Visibility = Visibility.Collapsed);

		// query
		var ImproveId = record.Attributes.Get<int>("improve-id");
		if (ImproveId != 0)
		{
			this.ImproveLevel = record.Attributes.Get<sbyte>("improve-level");
			var Improves = FileCache.Data.Provider.GetTable<ItemImprove>().Where(record => record.Id == ImproveId);

			// the highest stage may not have improve data, so we will add one stage
			var levels = Improves.Select(record => record.Level);
			if (levels.Any()) levels = levels.Append((sbyte)(levels.Max(x => x) + 1));

			// update
			ItemGrowth2TooltipPanel_DrawImproveOption.Visibility = Visibility.Visible;
			DrawImproveOption_Before.ItemsSource = Improves.Where(record => record.SuccessOptionListId != 0);
			DrawImproveOption_Level.ItemsSource = levels;
			DrawImproveOption_Level.SelectedItem = ImproveLevel;
		}

		var RandomOptionGroupId = record.Attributes.Get<int>("random-option-group-id");
		if (RandomOptionGroupId != 0)
		{
			var Job = UserSettings.Default.Job;
			var RandomOptionGroup = FileCache.Data.Provider.GetTable<ItemRandomOptionGroup>()[RandomOptionGroupId + ((long)Job << 32)];

			if (RandomOptionGroup.AbilityListTotalCount > 0)
			{
				RandomOption_Ability.ItemsSource = RandomOptionGroup.AbilityList.SelectNotNull(x => x.Instance);
				RandomOption_Ability.SelectedIndex = 0;
			}

			if (RandomOptionGroup.SkillBuildUpGroupListTotalCount > 0)
			{
				RandomOption_SkillBuildUpGroup.ItemsSource = RandomOptionGroup.SkillBuildUpGroupList.SelectNotNull(x => x.Instance);
				RandomOption_SkillBuildUpGroup.SelectedIndex = 0;
			}

			// update
			ItemGrowth2TooltipPanel_RandomOption.Visibility = Visibility.Visible;
		}
	}
	#endregion


	#region DrawImproveOption

	private sbyte ImproveLevel = 0;

	private void DrawImproveOption_Level_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.AddedItems[0] is not sbyte level) return;

		ImproveLevel = level;
		DrawImproveOption_Before_SelectionChanged(DrawImproveOption_Before, null);
	}

	private void DrawImproveOption_Before_SelectionChanged(object sender, SelectionChangedEventArgs? e)
	{
		if (sender is not Selector selector || selector.SelectedItem is not ItemImprove Improve) return;

		// search options
		var OptionList = FileCache.Data.Provider.GetTable<ItemImproveOptionList>()[Improve.SuccessOptionListId + ((long)JobSeq.JobNone << 32)];

		DrawImproveOption_After.ItemsSource = OptionList.GetOptions(ImproveLevel);
		DrawImproveOption_Desc.String.LabelText = string.Join(BR.Tag, OptionList.CreateRecipe());
	}
	#endregion

	#region RandomOption
	private void RandomOption_Ability_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (sender is not Selector selector || selector.SelectedItem is not AbilityList list) return;

		RandomOption_AbilityList.ItemsSource = list.GetAbilities();
	}

	private void RandomOption_SkillBuildUpGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (sender is not Selector selector || selector.SelectedItem is not SkillBuildUpGroupList list) return;

		RandomOption_SkillBuildUpGroupList.ItemsSource = list.SkillBuildUpGroup[0].Instance.GetSkills();
	}
	#endregion
}