using System.Text;
using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Extensions;
using static Xylia.Preview.Data.Models.AttractionRewardSummary;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
public partial class AttractionMapUnitToolTipPanel
{
	#region Constructors
	public AttractionMapUnitToolTipPanel()
	{
		InitializeComponent();
#if DEVELOP
		DataContext = FileCache.Data.Provider.GetTable<Dungeon>()["Dungeon_DongHae_chungkak_A_3"];
#endif
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not IAttraction attraction) return;

		AttractionMapUnitToolTipPanel_Name.String.LabelText = "UI.AttractionTooltip.Name.Dungeon.5".GetText([null, attraction.Name]);
		AttractionMapUnitToolTipPanel_Contents.String.LabelText = """<image enablescale="true" imagesetpath="00015590.Tag_Persons" scalerate="1.2"/>""" + "UI.AttractionMap.PCMax".GetText([0]);
		AttractionMapUnitToolTipPanel_Desc.String.LabelText = attraction.Description;
		AttractionMapUnitToolTipPanel_Newbie_Title.SetVisiable(false);
		AttractionMapUnitToolTipPanel_Newbie.SetVisiable(false);

		var reward = attraction.RewardSummary.Instance;
		if (reward != null)
		{
			var vertical = AttractionMapUnitToolTipPanel_Reward.Children.Add(new VerticalBox(), FLayoutData.Anchor.Full);
			vertical.MaxWidth = 500;

			var builder = new StringBuilder();
			foreach (var group in LinqExtensions.Tuple(reward.RewardItemCommon, reward.RewardItemCommonCategory, reward.RewardItemCommonConditionType).GroupBy(x => x.Item2))
			{
				if (group.Key == ItemCategorySeq.None) continue;

				builder.Append(group.Key.GetText());
				foreach (var item in group)
				{
					builder.Append("UI.AttractionTooltip.Reward.Item".GetText([null, item.Item1.Instance]));
				}
			}

			vertical.Children.Add(new BnsCustomLabelWidget() { Text = "UI.AttractionTooltip.Reward.Item.Common".GetText() });
			vertical.Children.Add(new BnsCustomLabelWidget() { Text = builder.ToString() });
		}
	}
	#endregion
}