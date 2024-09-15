using System.Windows;
using System.Windows.Input;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Extensions;
using static Xylia.Preview.Data.Models.Skill3;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
public partial class Skill3ToolTipPanel_1
{
	#region Constructors
	public Skill3ToolTipPanel_1()
	{
		InitializeComponent();
#if DEVELOP
		OldSkill = FileCache.Data.Provider.GetTable<Skill3>()["Summoner_S1_1_BackStep"];
		DataContext = FileCache.Data.Provider.GetTable<Skill3>()["Summoner_S0_1_BackStep"];
#endif
	}
	#endregion

	#region Methods
	protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
	{
		base.OnPreviewMouseDown(e);
		DataContext = FileCache.Data.Provider.GetTable<Skill3>()[Clipboard.GetText()];
	}

	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not Skill3 record) return;

		#region Common
		Skill3ToolTipPanel_1_Name.Arguments = [record, record];
		Skill3ToolTipPanel_1_Main_Icon.ExpansionComponentList["IconImage"]?.SetValue(record.FrontIcon);
		Skill3ToolTipPanel_1_Main_Icon.ExpansionComponentList["KEYCOMMAND"]?.SetValue(record.CurrentShortCutKey);
		Skill3ToolTipPanel_1_Main_Icon.ExpansionComponentList["SkillSkin"]?.SetExpansionShow(false);
		Skill3ToolTipPanel_1_Main_Icon.InvalidateVisual();

		// TODO: add reinforce
		Skill3ToolTipPanel_1_SkillReinforce.Visibility = Visibility.Collapsed;
		Skill3ToolTipPanel_1_SkillReinforceDescription.Visibility = Visibility.Collapsed;

		var ItemProbability = record.RevisedEventProbabilityInExec[0];
		Skill3ToolTipPanel_1_ItemProbability.SetVisiable(ItemProbability != 100);
		Skill3ToolTipPanel_1_ItemProbability.String.LabelText = "UI.Skill.ItemProbability.PrevTag".GetText() + BR.Tag + "UI.Skill.ItemProbability".GetText([ItemProbability]);
		#endregion

		#region Tooltip
		Skill3ToolTipPanel_1_Main_Description.String.LabelText =
			SkillTooltip.Compare(record.MainTooltip1, OldSkill?.MainTooltip1) + BR.Tag +
			SkillTooltip.Compare(record.MainTooltip2, OldSkill?.MainTooltip2);

		Skill3ToolTipPanel_1_Sub_Description.String.LabelText = string.Join(BR.Tag, SkillTooltip.Compare(record.SubTooltip, OldSkill?.SubTooltip));

		Skill3ToolTipPanel_1_ConditionTitle.SetVisiable(record.ConditionTooltip.Any(x => x.HasValue));
		Skill3ToolTipPanel_1_ConditionText.String.LabelText = string.Join(BR.Tag, SkillTooltip.Compare(record.ConditionTooltip, OldSkill?.ConditionTooltip));
		
		Skill3ToolTipPanel_1_StanceTitle.SetVisiable(record.StanceTooltip.Any(x => x.HasValue));
		Skill3ToolTipPanel_1_StanceText.String.LabelText = string.Join(BR.Tag, SkillTooltip.Compare(record.StanceTooltip, OldSkill?.StanceTooltip));

		Skill3ToolTipPanel_1_ItemTitle.Visibility = Skill3ToolTipPanel_1_ItemName.Visibility = Visibility.Collapsed;
		Skill3ToolTipPanel_1_SkillSkinDescription_Title.Visibility = Skill3ToolTipPanel_1_SkillSkinDescription.Visibility = Visibility.Collapsed;
		#endregion

		#region InfoHolder
		Skill3ToolTipPanel_1_DamageInfo_PvEInfo.ExpansionComponentList["Info"]?.SetValue("UI.SkillTooltip.DamageInfo.StandardStats".GetText([record, record.DamageRateStandardStats]));
		Skill3ToolTipPanel_1_DamageInfo_PvEInfo.ExpansionComponentList["Icon"]?.SetValue(GetDamageRateIcon(record.DamageRateStandardStats));
		Skill3ToolTipPanel_1_DamageInfo_PvPInfo.ExpansionComponentList["Info"]?.SetValue("UI.SkillTooltip.DamageInfo.PVP".GetText([record, record.DamageRatePvp]));
		Skill3ToolTipPanel_1_DamageInfo_PvPInfo.ExpansionComponentList["Icon"]?.SetValue(GetDamageRateIcon(record.DamageRatePvp));
		Skill3ToolTipPanel_1_DamageInfo_PvEInfo.InvalidateVisual();
		Skill3ToolTipPanel_1_DamageInfo_PvPInfo.InvalidateVisual();

		Skill3ToolTipPanel_1_CastingRange.String.LabelText = record.CastingRange;
		Skill3ToolTipPanel_1_CastingTime.String.LabelText = (record as ActiveSkill)?.CastDuration.ToString(MsecFormat.MsecFormatType.hms) ?? "Name.Skill.CastTime.cast-instant".GetText();
		Skill3ToolTipPanel_1_RecycleTime.String.LabelText = (record as ActiveSkill)?.RecycleGroupDuration.ToString(MsecFormat.MsecFormatType.hms) ?? "Name.Skill.RecycleTime.Instant".GetText();

		if (record is ActiveSkill activeskill)
		{
			var GatherRange = activeskill.GatherRange.Instance;
			if (GatherRange != null)
			{
				var ExecGatherType = activeskill.Attributes.Get<GatherType>("exec-gather-type-1");

				Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage_Diff_Before"]?.SetExpansionShow(false);
				Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage_Diff_Next"]?.SetExpansionShow(false);

				if (ExecGatherType == GatherType.Target)
				{
					Skill3ToolTipPanel_1_ScaleType.String.LabelText = "Name.Skill.ScaleRange.Default".GetText();
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage"]?.SetExpansionShow(false);
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["Multi"]?.SetExpansionShow(false);
				}
				else if (ExecGatherType == GatherType.TargetAndLinkTarget)
				{

				}
				else if (ExecGatherType == GatherType.Laser || ExecGatherType == GatherType.ShiftingLaser)
				{
					var GatherLaserFrontDistance = GatherRange.GatherLaserFrontDistanceMax[0];
					var GatherLaserWidth = GatherRange.GatherLaserWidthMax[0];
					var path = GatherLaserFrontDistance > GatherLaserWidth ? "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Laser_Narrow" :
						GatherLaserFrontDistance == GatherLaserWidth ? "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Laser_Square" :
							"BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Laser_Wide";

					Skill3ToolTipPanel_1_ScaleType.String.LabelText = "Name.Skill.ScaleRange.WidthHeight".GetText([null, this, GatherLaserWidth, GatherLaserFrontDistance]);
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage"]?.SetValue(new ImageProperty() { EnableImageSet = true, ImageSet = new MyFPackageIndex(path) });
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage"]?.SetExpansionShow(true);
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["Multi"]?.SetExpansionShow(false);
				}
				else
				{
					var path = ExecGatherType switch
					{
						GatherType.Target360 => "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Target_360",
						GatherType.TargetFront180 => "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Target_Front_180",
						GatherType.TargetBack180 => "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Target_Back_180",
						GatherType.TargetFront90 => "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Target_Front_90",
						GatherType.TargetBack90 => "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Target_Back_90",
						GatherType.TargetFront15 => "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Target_Front_15",
						GatherType.TargetFront30 => "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Target_Front_30",
						GatherType.TargetFront45 => "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Target_Front_45",
						GatherType.TargetFront60 => "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Target_Front_60",
						GatherType.TargetFront120 => "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Target_Front_120",
						GatherType.TargetFront270 => "BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/SkillGatherType/Target_Front_270",
						_ => null
					};

					Skill3ToolTipPanel_1_ScaleType.String.LabelText = "Name.Skill.ScaleRange".GetText([null, this, GatherRange.GatherRadiusMax[0]]);
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage"]?.SetValue(new ImageProperty() { EnableImageSet = true, ImageSet = new MyFPackageIndex(path) });
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage"]?.SetExpansionShow(true);
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["Multi"]?.SetExpansionShow(false);
				}


				// Name.Skill.ScaleRange.Icon.AreaUp
				// Name.Skill.ScaleRange.Icon.AreaUp.Diff

				//Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage_Diff_Before"]?.SetValue();
				//Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage_Diff_Next"]?.SetValue();
				//Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["Multi"]?.SetValue();
			}


			//	<record alias="Name.Skill.ConsumeValue"><arg p="2:skill.ui-stance.secondgauge-name"/> 消耗 <arg p="3:integer"/></record>
			//	<record alias="Name.Skill.SpHeal.Zero"><arg p="2:skill.ui-stance.secondgauge-name"/> 恢复0</record>
			//	<record alias="Name.Skill.SpHeal.ZeroPercent">恢复当前<arg p="2:skill.ui-stance.secondgauge-name"/>的0%</record>
			//	<record alias="Name.Skill.HpConsumeValue">生命消耗 <arg p="3:integer"/></record>
			//	<record alias="Name.Skill.ConsumeValue.CurrentPercent">消耗当前 <arg p="2:skill.ui-stance.secondgauge-name"/>的<arg p="3:integer"/>%</record>
			//	<record alias="Name.Skill.SpHeal"><arg p="2:skill.ui-stance.secondgauge-name"/> 恢复<arg p="2:skill.ui-sp-heal-value"/></record>

			Skill3ToolTipPanel_1_ConsumeInfo.String.LabelText =
				activeskill.ConsumeHpValue > 0 ? $"hp: {activeskill.ConsumeHpValue}" :
				activeskill.ConsumeSpValue[0] > 0 ? $"sp: {activeskill.ConsumeSpValue[0]}" :
				activeskill.ConsumeSpValue[1] > 0 ? $"sp2: {activeskill.ConsumeSpValue[1]}" :
				activeskill.ConsumeSummonedHpValue > 0 ? $"shp: {activeskill.ConsumeSummonedHpValue}" :
				null;
		}
		#endregion
	}

	private static FPackageIndex GetDamageRateIcon(short value)
	{
		return new MyFPackageIndex(value > 1000 ?
			"BNSR/Content/Art/UI/GameUI/Resource/GameUI_Window2/DamageInfo_Up" : value == 1000 ?
			"BNSR/Content/Art/UI/GameUI/Resource/GameUI_Window2/DamageInfo_Equal" :
			"BNSR/Content/Art/UI/GameUI/Resource/GameUI_Window2/DamageInfo_Down");
	}
	#endregion


	#region Private Fields
	private Skill3? OldSkill;
	#endregion
}