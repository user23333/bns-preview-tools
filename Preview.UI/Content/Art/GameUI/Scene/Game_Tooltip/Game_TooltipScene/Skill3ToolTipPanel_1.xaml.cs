using System.Diagnostics;
using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Documents;
using Xylia.Preview.UI.Extensions;
using static Xylia.Preview.Data.Common.DataStruct.MsecFormat;
using static Xylia.Preview.Data.Models.Skill3;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
public partial class Skill3ToolTipPanel_1
{
	#region Constructors
	public Skill3ToolTipPanel_1()
	{
#if DEVELOP
		DataContext = Helpers.TestProvider.Provider.GetTable<Skill3>()["SwordMaster_S1_2_Lightning_TwicePierce"];
#endif
		InitializeComponent();
		this.PreviewMouseDown += (s, e) => Load(FileCache.Data.Provider.GetTable<Skill3>()[Clipboard.GetText()]);
	}
	#endregion

	#region Methods
	protected override void OnLoaded(RoutedEventArgs e)
	{
		if (DataContext is not Skill3 record) return;

		#region Common
		Skill3ToolTipPanel_1_Name.Arguments = [record, record];
		Skill3ToolTipPanel_1_Main_Icon.ExpansionComponentList["IconImage"]?.SetValue(record.Icon);
		Skill3ToolTipPanel_1_Main_Icon.ExpansionComponentList["KEYCOMMAND"]?.SetValue(record.CurrentShortCutKey);
		Skill3ToolTipPanel_1_Main_Icon.ExpansionComponentList["SkillSkin"]?.SetShow(false);

		// TODO: add reinforce
		Skill3ToolTipPanel_1_SkillReinforce.Visibility = Visibility.Collapsed;
		Skill3ToolTipPanel_1_SkillReinforceDescription.Visibility = Visibility.Collapsed;

		var ItemProbability = record.RevisedEventProbabilityInExec[0];
		Skill3ToolTipPanel_1_ItemProbability.SetVisibility(ItemProbability != 100);
		Skill3ToolTipPanel_1_ItemProbability.String.LabelText = "UI.Skill.ItemProbability.PrevTag".GetText() + BR.Tag + "UI.Skill.ItemProbability".GetText([ItemProbability]);

		Skill3ToolTipPanel_1_Main_Description.String.LabelText =
			string.Join(BR.Tag, record.MainTooltip1.SelectNotNull(x => x.Instance)) + BR.Tag +
			string.Join(BR.Tag, record.MainTooltip2.SelectNotNull(x => x.Instance));

		Skill3ToolTipPanel_1_Sub_Description.String.LabelText = string.Join(BR.Tag, record.SubTooltip.SelectNotNull(x => x.Instance));
		#endregion

		#region Tooltip
		var ConditionTooltips = record.ConditionTooltip.SelectNotNull(x => x.Instance);
		Skill3ToolTipPanel_1_ConditionTitle.SetVisibility(ConditionTooltips.Any());
		Skill3ToolTipPanel_1_ConditionText.String.LabelText = string.Join("<br/>", ConditionTooltips);

		var StanceTooltips = record.StanceTooltip.SelectNotNull(x => x.Instance);
		Skill3ToolTipPanel_1_StanceTitle.SetVisibility(StanceTooltips.Any());
		Skill3ToolTipPanel_1_StanceText.String.LabelText = string.Join("<br/>", StanceTooltips);

		Skill3ToolTipPanel_1_ItemTitle.Visibility = Skill3ToolTipPanel_1_ItemName.Visibility = Visibility.Collapsed;
		Skill3ToolTipPanel_1_SkillSkinDescription_Title.Visibility = Skill3ToolTipPanel_1_SkillSkinDescription.Visibility = Visibility.Collapsed;
		#endregion

		#region InfoHolder
		Skill3ToolTipPanel_1_DamageInfo_PvEInfo.ExpansionComponentList["Info"]?.SetValue($"<image enablescale='true' imagesetpath='00009076.CharInfo_AttackPower' scalerate='1.2'/> x {record.DamageRateStandardStats * 0.001:0.000}");
		Skill3ToolTipPanel_1_DamageInfo_PvEInfo.ExpansionComponentList["Icon"]?.SetValue(GetDamageRateIcon(record.DamageRateStandardStats));
		Skill3ToolTipPanel_1_DamageInfo_PvPInfo.ExpansionComponentList["Info"]?.SetValue($"<image enablescale='true' imagesetpath='00009076.CharInfo_PcAttackPower' scalerate='1.2'/> x {record.DamageRatePvp * 0.001:0.000}");
		Skill3ToolTipPanel_1_DamageInfo_PvPInfo.ExpansionComponentList["Icon"]?.SetValue(GetDamageRateIcon(record.DamageRatePvp));
		Skill3ToolTipPanel_1_DamageInfo_PvEInfo.InvalidateVisual();
		Skill3ToolTipPanel_1_DamageInfo_PvPInfo.InvalidateVisual();

		Skill3ToolTipPanel_1_CastingRange.String.LabelText = record.CastingRange;
		Skill3ToolTipPanel_1_CastingTime.String.LabelText = (record as ActiveSkill)?.CastDuration.ToString(MsecFormatType.hms) ?? "Name.Skill.CastTime.cast-instant".GetText();
		Skill3ToolTipPanel_1_RecycleTime.String.LabelText = (record as ActiveSkill)?.RecycleGroupDuration.ToString(MsecFormatType.hms) ?? "Name.Skill.RecycleTime.Instant".GetText();

		if (record is ActiveSkill activeskill)
		{
			var GatherRange = activeskill.GatherRange.Instance;
			if (GatherRange != null)
			{
				var ExecGatherType = activeskill.Attributes["exec-gather-type-1"].ToEnum<GatherType>();

				Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage_Diff_Before"]?.SetShow(false);
				Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage_Diff_Next"]?.SetShow(false);

				if (ExecGatherType == GatherType.Target)
				{
					Skill3ToolTipPanel_1_ScaleType.String.LabelText = "Name.Skill.ScaleRange.Default".GetText();
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage"]?.SetShow(false);
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["Multi"]?.SetShow(false);
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
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage"]?.SetShow(true);
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["Multi"]?.SetShow(false);
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
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["TypeImage"]?.SetShow(true);
					Skill3ToolTipPanel_1_ScaleType.ExpansionComponentList["Multi"]?.SetShow(false);
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


	private void Load(Skill3? record)
	{
		Trace.WriteLine("loading data");

		DataContext = record;
		OnLoaded(new RoutedEventArgs());
	}

	private static FPackageIndex GetDamageRateIcon(short value)
	{
		return new MyFPackageIndex(
			value > 1000 ?
			"BNSR/Content/Art/UI/GameUI/Resource/GameUI_Window2/DamageInfo_Up" :
			value == 1000 ?
			"BNSR/Content/Art/UI/GameUI/Resource/GameUI_Window2/DamageInfo_Equal" :

			"BNSR/Content/Art/UI/GameUI/Resource/GameUI_Window2/DamageInfo_Down");
	}
	#endregion
}