using System.Text;
using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Extensions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
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
		
		AttractionMapUnitToolTipPanel_Name.String.LabelText = attraction.GetName();
		AttractionMapUnitToolTipPanel_Contents.String.LabelText = attraction.GetPcMax();
		AttractionMapUnitToolTipPanel_Desc.String.LabelText = attraction.Description;
		AttractionMapUnitToolTipPanel_Newbie_Title.SetVisiable(false);
		AttractionMapUnitToolTipPanel_Newbie.SetVisiable(false);

		var reward = attraction.RewardSummary.Instance;
		if (reward != null)
		{
			var vertical = AttractionMapUnitToolTipPanel_Reward.Children.Add(new VerticalBox(), FLayoutData.Anchor.Full);

			void BuildText(string title, Ref<ModelElement>[] items, ItemCategorySeq[] categorys, ItemConditionType[] types)
			{
				var builder = new StringBuilder();
				foreach (var group in LinqExtensions.Tuple(items, categorys, types).GroupBy(x => x.Item2))
				{
					if (group.Key == ItemCategorySeq.None) continue;
					builder.Append(group.Key.GetText() + BR.Tag);

					foreach (var (element, category, type) in group)
					{
						switch (element.Instance)
						{
							case Item item: builder.Append("UI.AttractionTooltip.Reward.Item".GetText([null, item])); break;
							case ItemBrand brand: builder.Append("UI.AttractionTooltip.Reward.Brand".GetText([null, brand.GetTooltip(type)])); break;
						}
					}

					builder.Append(BR.Tag);
				}

				if (builder.Length > 0)
				{
					vertical.Children.Add(new BnsCustomLabelWidget() { Text = title });
					vertical.Children.Add(new BnsCustomLabelWidget() { Text = builder.ToString() });
				}
			}

			BuildText("UI.AttractionTooltip.Reward.Item.Common".GetText(), reward.RewardItemCommon, reward.RewardItemCommonCategory, reward.RewardItemCommonConditionType);
			BuildText("UI.AttractionTooltip.Reward.Item.Easy".GetText(), reward.RewardItemEasy, reward.RewardItemEasyCategory, reward.RewardItemEasyConditionType);
			BuildText("UI.AttractionTooltip.Reward.Item.Normal".GetText(), reward.RewardItemNormal, reward.RewardItemNormalCategory, reward.RewardItemNormalConditionType);
			BuildText("UI.AttractionTooltip.Reward.Item.Hard".GetText(), reward.RewardItemHard, reward.RewardItemHardCategory, reward.RewardItemHardConditionType);
			
			//BuildText("UI.AttractionTooltip.Reward.Item.Common".GetText(), reward.AdditionalRewardItem, reward.AdditionalRewardItemCategory, reward.AdditionalRewardItemConditionType);
			//BuildText("UI.AttractionTooltip.Reward.Item.Common".GetText(), reward.BonusRewardItemCommon, reward.BonusRewardItemCommonCategory, reward.BonusRewardItemCommonConditionType);
			//BuildText("UI.AttractionTooltip.Reward.Item.Easy".GetText(), reward.BonusRewardItemEasy, reward.BonusRewardItemEasyCategory, reward.BonusRewardItemEasyConditionType);
			//BuildText("UI.AttractionTooltip.Reward.Item.Normal".GetText(), reward.BonusRewardItemNormal, reward.BonusRewardItemNormalCategory, reward.BonusRewardItemNormalConditionType);
			//BuildText("UI.AttractionTooltip.Reward.Item.Hard".GetText(), reward.BonusRewardItemHard, reward.BonusRewardItemHardCategory, reward.BonusRewardItemHardConditionType);
		}
	}
	#endregion
}