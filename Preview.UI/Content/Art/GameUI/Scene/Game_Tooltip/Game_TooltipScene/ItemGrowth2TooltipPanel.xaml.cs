using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Xylia.Preview.Common;
using Xylia.Preview.Common.Extension;
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
		DataContext = Globals.GameData.Provider.GetTable<Item>()["Test_N-ShopAccountShippingItem"].Source;
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
					var Improves = Globals.GameData.Provider.GetTable<ItemImprove>().Where(record => record.Id == ImproveId);

					// the max stage may not have improve data, so we will add one stage
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
		var record = Globals.GameData.Provider.GetTable<ItemImproveOptionList>()[Improve.SuccessOptionListId + ((long)JobSeq.JobNone << 32)];

		DrawImproveOption_After.ItemsSource = record.GetOptions(ImproveLevel);
		DrawImproveOption_Desc.String.LabelText = string.Join(BR.Tag, record.GetRecipes());
	}
	#endregion

	#region RandomOption   
	private void RandomOption_Load(Record record, int RandomOptionGroupId)
	{
		var source = new List<NameObject<object>>();
		var jobs = record.Attributes.Get<JobSeq[]>("equip-job-check").Where(x => x != JobSeq.JobNone);
		if (!jobs.Any()) jobs = [UserSettings.Default.Job];

		var group = Globals.GameData.Provider.GetTable<ItemRandomOptionGroup>()[RandomOptionGroupId + ((long)jobs.FirstOrDefault() << 32)];
		if (group != null)
		{
			if (group.EffectList.HasValue)
			{
				source.Add(new(group.EffectList.Instance, "UI.ItemRandomOption.EffectOption.Title".GetText()));
			}

			if (group.AbilityListTotalCount > 0)
			{
				int index = 0;
				group.AbilityList.Values().ForEach(x => source.Add(new(x, "UI.ItemRandomOption.SubAbility.Title".GetText([++index]))));
			}

			if (group.SkillTrainByItemListTotalCount > 0)
			{
				var min = group.SkillTrainByItemListSelectMin;
				var max = group.SkillTrainByItemListSelectMax;
				source.Add(new(RandomDistribution.Equal(min, max), "UI.RandomOption.Probability.SkillOptionSlot.1DepthTitle".GetText([min, max])));
			}

			if (group.SkillBuildUpGroupListTotalCount > 0)
			{
				int index = 0;
				group.SkillBuildUpGroupList.Values().ForEach(x => source.Add(new(x, "UI.ItemRandomOption.SkillEnhancement.Title".GetText([++index]))));
			}
		}

		RandomOption_Groups.ItemsSource = source;
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