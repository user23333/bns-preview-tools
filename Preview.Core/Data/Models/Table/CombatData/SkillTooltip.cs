using System.Text;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.Data.Models;
public class SkillTooltip : ModelElement
{
	#region Attributes
	public Ref<Skill3> Skill { get; set; }

	public enum TooltipGroupSeq
	{
		M1,
		M2,
		SUB,
		STANCE,
		CONDITION,
	}

	public TooltipGroupSeq TooltipGroup { get; set; }

	public enum EctOrderSeq
	{
		CTE,
		CET,
		TCE,
		TEC,
		ECT,
		ETC
	}

	public EctOrderSeq EctOrder { get; set; }

	public EctOrderSeq EctOrderEnglish { get; set; }

	public EctOrderSeq EctOrderFrench { get; set; }

	public EctOrderSeq EctOrderGerman { get; set; }

	public EctOrderSeq EctOrderRussian { get; set; }

	public EctOrderSeq EctOrderBportuguese { get; set; }

	public Ref<SkillTooltipAttribute> EffectAttribute { get; set; }

	public string[] EffectArg { get; set; }

	public Ref<SkillTooltipAttribute> ConditionAttribute { get; set; }

	public string[] ConditionArg { get; set; }

	public Ref<SkillTooltipAttribute> TargetAttribute { get; set; }

	public Ref<SkillTooltipAttribute> BeforeStanceAttribute { get; set; }

	public Ref<SkillTooltipAttribute> AfterStanceAttribute { get; set; }

	public Ref<Text> DefaultText { get; set; }

	public Ref<Text> AttributeColorText { get; set; }

	public sbyte SkillModifyDiffRepeatCount { get; set; }

	public short SkillAttackAttributeCoefficientPercent { get; set; }

	public Ref<Text> ItemDefaultText { get; set; }

	public Ref<Text> ItemReplaceText { get; set; }
	#endregion

	#region Methods
	public override string ToString() => ToString();

	private string ToString(int mode = 0)
	{
		#region Text
		StringBuilder builder = new();

		builder.Append(DefaultText.GetText());
		builder.Append(AttributeColorText.GetText());
		builder.Append(ItemDefaultText.GetText());
		builder.Append(ItemReplaceText.GetText());
		#endregion

		#region ECT
		var EffectAttributeText = EffectAttribute.Instance?.ToString([Skill.Instance, .. EffectArg], SkillAttackAttributeCoefficientPercent);
		var ConditionAttributeText = ConditionAttribute.Instance?.ToString([Skill.Instance, .. ConditionArg], SkillAttackAttributeCoefficientPercent);
		var TargetAttributeText = TargetAttribute.Instance?.ToString();

		builder.Append(string.Join(string.Empty, EctOrder switch
		{
			EctOrderSeq.CTE => [ConditionAttributeText, TargetAttributeText, EffectAttributeText],
			EctOrderSeq.CET => [ConditionAttributeText, EffectAttributeText, TargetAttributeText],
			EctOrderSeq.TCE => [TargetAttributeText, ConditionAttributeText, EffectAttributeText],
			EctOrderSeq.TEC => [TargetAttributeText, EffectAttributeText, ConditionAttributeText],
			EctOrderSeq.ECT => [EffectAttributeText, ConditionAttributeText, TargetAttributeText],
			EctOrderSeq.ETC => [EffectAttributeText, TargetAttributeText, ConditionAttributeText],
			_ => [],
		}));
		#endregion

		#region Stance
		if (AfterStanceAttribute.HasValue)
		{
			var AfterStanceAttributeText = AfterStanceAttribute.Instance?.ToString();
			var BeforeStanceAttributeText = BeforeStanceAttribute.Instance?.ToString();

			builder.Append(BeforeStanceAttribute.HasValue ?
				"SkillTooltipAttr.stance.before-after".GetText([BeforeStanceAttributeText, AfterStanceAttributeText]) :
				"SkillTooltipAttr.stance.after-only".GetText([AfterStanceAttributeText]));
		}
		#endregion

		#region Fontset
		var text = builder.ToString();
		return mode switch
		{
			1 => "UI.TooltipArea.SkillTrain.Modify.Fontset".GetText([text]) + " " + "UI.TooltipArea.SkillTrain.Modify.Icon".GetText(),
			2 => "UI.TooltipArea.SkillTrain.Add.Fontset".GetText([text]) + " " + "UI.TooltipArea.SkillTrain.Add.Icon".GetText(),
			3 => "UI.TooltipArea.SkillTrain.Del.Fontset".GetText([text]),
			_ => text
		};
		#endregion
	}

	public static string Compare(Ref<SkillTooltip>[] current, Ref<SkillTooltip>[] other)
	{
		var ia = other?.SelectNotNull(x => x.Instance) ?? [];
		var ib = current?.SelectNotNull(x => x.Instance) ?? [];

		var del = ia.Except(ib);
		var add = ib.Except(ia);
		var normal = ib.Intersect(ia);

		return string.Join("<br/>",
		[
			.. add.Select(x => x.ToString(2)),
			.. del.Select(x => x.ToString(3)),
			.. normal.Select(x => x.ToString(0))
		]);
	}
	#endregion
}