using System.Windows;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Extensions;
using static Xylia.Preview.Data.Models.Achievement;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Achievement;
public partial class AchievementDetailPanel
{
	#region Constructors
	public AchievementDetailPanel()
	{
		InitializeComponent();
#if DEVELOP
		DataContext = Xylia.Preview.Common.Globals.GameData.Provider.GetTable<Achievement>()["1155_consumable_BackChung_Season_08_step4"];
#endif
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not Achievement record) return;

		AchievementDetailPanel_Info_Name.String.LabelText = record.Name2.GetText();
		AchievementDetailPanel_Info_Description.String.LabelText = record.Description2.GetText();
		AchievementDetailPanel_Info_Icon.ExpansionComponentList["IconImage"]!.SetValue(record.Icon);
		AchievementDetailPanel_Info_Score.String.LabelText = record.CurrentStepScore.ToString();

		AchievementDetailPanel_Info_Set_Title.SetVisiable(record.TitleName.HasValue);
		AchievementDetailPanel_Info_Set_Title.String.fontset = new MyFPackageIndex(record.TitleFontset);
		AchievementDetailPanel_Info_Set_Title.String.LabelText = record.TitleName.GetText();
		AchievementDetailPanel_Info_Set_Ability.SetVisiable(record.CompletedEffect.HasValue);
		AchievementDetailPanel_Info_Set_Ability.String.LabelText = record.CompletedEffect.Value?.Name;

		#region Register
		for (int i = 0; i < 5; i++)
		{
			var register = record.RegisterRef[i].Value;
			var holder = this.GetChild<BnsCustomImageWidget>("AchievementDetailPanel_Register_" + (i + 1), true)!;
			if (!holder.SetVisiable(register != null)) continue;

			// Name
			var Name = holder.GetChild<BnsCustomLabelWidget>("Name")!;
			Name.String.LabelText = register!.Name;

			// SlotName
			var SlotName = holder.GetChild<BnsCustomLabelWidget>("Slot_Name")!;
			var slot = register!.Attributes.Get<Text[]>("slot-name")?.Where(x => x != null).Select(x => x.text);
			if (slot is null) SlotName.SetVisiable(false);
			else
			{
				int half = (int)Math.Ceiling(slot.Count() / 2M);
				SlotName.SetVisiable(true);
				SlotName.String.LabelText = string.Join(BR.Tag, slot.Take(half));
				SlotName.ExpansionComponentList["Right_Column_Name"]!.SetValue(string.Join(BR.Tag, slot.Skip(half)));
			}

			// ProgressBar
			var ProgressBar = holder.GetChild<BnsCustomProgressBarWidget>("Progress_Bar")!;
			ProgressBar.SetVisiable(record.RegisterType[i] == RegisterTypeSeq.Above);
			ProgressBar.MaxProgressValue = record.RegisterValue[i];
			ProgressBar.ProgressValue = 0;
		}
		#endregion

		#region Step
		AchievementDetailPanel_Step.SetVisiable(record.RegisterType[0] == RegisterTypeSeq.Above);

		for (short i = 1; i <= 8; i++)
		{
			var icon = AchievementDetailPanel_Step.GetChild<BnsCustomImageWidget>("Icon_" + i)!;
			var step = record.Provider.GetTable<Achievement>()[new AchievementKey(record.Id, i, record.Job)];

			if (icon.SetVisiable(step != null))
			{
				icon.DataContext = step;
				icon.ToolTip = new BnsTooltipHolder();
				icon.ExpansionComponentList["IconImage"]!.SetValue(step.Icon);
				icon.ExpansionComponentList["RegisterCount"]!.SetValue(step.RegisterValue[0]);
				icon.ExpansionComponentList["NewImage"]!.SetExpansionShow(false);
				icon.ExpansionComponentList["MouseOverImage"]!.SetExpansionShow(false);
				icon.ExpansionComponentList["MousePressImage"]!.SetExpansionShow(false);
			}
		}
		#endregion

		#region Reward
		AchievementDetailPanel_IconHolder.SetVisiable(false);
		AchievementDetailPanel_Info_GoodsStone_Btn.SetVisiable(false);

		switch (record.StepCompleteRewardType)
		{
			case StepCompleteRewardTypeSeq.Item:
			{
				AchievementDetailPanel_IconHolder.SetVisiable(true);

				for (int i = 0; i < 5; i++)
				{
					var widget = AchievementDetailPanel_IconHolder.GetChild<BnsCustomImageWidget>("Reward_Icon_" + (i + 1))!;
					var item = record.StepCompleteRewardItem[i].Value;
					var count = record.StepCompleteRewardItemCount[i];

					if (widget.SetVisiable(item != null))
					{
						widget.DataContext = item;
						widget.ToolTip = new BnsTooltipHolder();
						widget.ExpansionComponentList["IconImage"]!.SetValue(item.FrontIcon);
						widget.ExpansionComponentList["Count"]!.SetValue(count);
						widget.ExpansionComponentList["CanSaleItem"]!.SetValue(item.CanSaleItemImage);
						widget.ExpansionComponentList["SymbolImage"]!.SetExpansionShow(false);
						widget.ExpansionComponentList["SymbolImage_Chacked"]!.SetExpansionShow(false);
						widget.ExpansionComponentList["MouseOverImage"]!.SetExpansionShow(false);
						widget.ExpansionComponentList["MousePressImage"]!.SetExpansionShow(false);
						widget.ExpansionComponentList["Selected"]!.SetExpansionShow(false);
					}
				}
			}
			break;

			case StepCompleteRewardTypeSeq.GameCash:
			{
				AchievementDetailPanel_Info_GoodsStone_Btn.SetVisiable(true);
				AchievementDetailPanel_Info_GoodsStone_Btn.String.LabelText = (record.StepCompleteRewardGameCashType switch
				{
					StepCompleteRewardGameCashTypeSeq.GameCash => "UI.Achievement.Detail.GameCashReward",
					StepCompleteRewardGameCashTypeSeq.Blue => "UI.Achievement.Detail.GameSecondaryMoneyBlueReward",
					StepCompleteRewardGameCashTypeSeq.Red => "UI.Achievement.Detail.GameSecondaryMoneyRedReward",
					_ => throw new NotSupportedException()
				}).GetText([record.StepCompleteRewardGameCash]);
			}
			break;

			case StepCompleteRewardTypeSeq.SkillBuildUpPoint:
			{
				AchievementDetailPanel_Info_GoodsStone_Btn.SetVisiable(true);
				AchievementDetailPanel_Info_GoodsStone_Btn.String.LabelText = "UI.Achievement.Detail.SkillBuildUpPointReward"
					.GetText([record.StepCompleteRewardSkillBuildUpPoint]);
			}
			break;
		}
		#endregion
	}
	#endregion
}