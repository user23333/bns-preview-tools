using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Common;
using Xylia.Preview.UI.Extensions;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
public partial class ItemGrowth2TooltipPanel
{
	#region Constructors
	public ItemGrowth2TooltipPanel()
	{
		InitializeComponent();
		DataContextChanged += OnDataChanged;
#if DEVELOP
		DataContext = FileCache.Data.Provider.GetTable<Item>()["Test_N-ShopAccountShippingItem"].Source;
#endif
	}
	#endregion

	#region Methods
	private void OnDataChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		#region Tabs 
		var tabs = ItemGrowth2TooltipPanel_Contents.Items.OfType<UIElement>();
		tabs.ForEach(e => e.Visibility = Visibility.Collapsed);
		#endregion

		#region Contents
		if (e.NewValue is Record record)
		{
			if (record.OwnerName == "item")
			{
				var ImproveId = record.Attributes.Get<int>("improve-id");
				if (ImproveId != 0)
				{
					this.ImproveLevel = record.Attributes.Get<sbyte>("improve-level");
					var Improves = FileCache.Data.Provider.GetTable<ItemImprove>().Where(record => record.Id == ImproveId);

					// the highest stage may not have improve data, so we will add one stage
					var levels = Improves.Select(record => record.Level);
					if (levels.Any()) levels = levels.Append((sbyte)(levels.Max(x => x) + 1));

					ItemGrowth2TooltipPanel_DrawImproveOption.Visibility = Visibility.Visible;
					DrawImproveOption_Before.ItemsSource = Improves.Where(record => record.SuccessOptionListId != 0);
					DrawImproveOption_Level.ItemsSource = levels;
					DrawImproveOption_Level.SelectedItem = ImproveLevel;
				}

				var RandomOptionGroupId = record.Attributes.Get<int>("random-option-group-id");
				if (RandomOptionGroupId != 0)
				{
					ItemGrowth2TooltipPanel_RandomOption.Visibility = Visibility.Visible;
					RandomOption_Load(record, RandomOptionGroupId);
				}
			}
		}
		else if (e.NewValue is IEnumerable<NameObject<object>> objs)
		{
			ItemGrowth2TooltipPanel_RandomOption.Visibility = Visibility.Visible;
			RandomOption_Groups.ItemsSource = objs.Where(x => x.Value != null);
		}
		#endregion

		ItemGrowth2TooltipPanel_Contents.SelectedItem = tabs.FirstOrDefault(e => e.Visibility == Visibility.Visible);
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
	private void RandomOption_Load(Record record, int RandomOptionGroupId)
	{
		var EquipJobCheck = record.Attributes.Get<object[]>("equip-job-check").Cast<string>();
		var job = UserSettings.Default.Job;

		var data = new List<NameObject<object>>();
		var group = FileCache.Data.Provider.GetTable<ItemRandomOptionGroup>()[RandomOptionGroupId + ((long)job << 32)];
		if (group != null)
		{
			if (group.EffectList.HasValue)
			{
				data.Add(new(group.EffectList.Instance, "UI.ItemRandomOption.EffectOption.Title".GetText()));
			}

			if (group.AbilityListTotalCount > 0)
			{
				int index = 0;
				group.AbilityList.Select(x => x.Instance).ForEach(x => data.Add(new(x, "UI.ItemRandomOption.SubAbility.Title".GetText([++index]))));
			}

			if (group.SkillTrainByItemListTotalCount > 0)
			{
				data.Add(new(group.SkillBuildUpGroupList.Values(), "UI.RandomOption.Probability.SkillOptionSlot.1DepthTitle".GetText([group.SkillTrainByItemListSelectMin, group.SkillTrainByItemListSelectMax])));
			}

			if (group.SkillBuildUpGroupListTotalCount > 0)
			{
				int index = 0;
				group.SkillBuildUpGroupList.Values().ForEach(x => data.Add(new(x, "UI.ItemRandomOption.SkillEnhancement.Title".GetText([++index]))));
			}
		}

		RandomOption_Groups.ItemsSource = data;
	}

	private void RandomOption_Initialized(object sender, EventArgs e)
	{
		var element = (FrameworkElement)sender;
		var CommonContent = (FrameworkElement)element.FindName("RandomOption_CommonContent");
		var RewardContent = (FrameworkElement)element.FindName("RandomOption_RewardContent");

		bool IsReward = element.DataContext is Reward;
		CommonContent.SetVisiable(!IsReward);
		RewardContent.SetVisiable(IsReward);
	}
	#endregion
}