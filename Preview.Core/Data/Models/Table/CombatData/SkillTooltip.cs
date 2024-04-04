using System.Text;

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
	public override string ToString()
	{
		StringBuilder builder = new();

		builder.Append(DefaultText.GetText());
		builder.Append(AttributeColorText.GetText());
		builder.Append(ItemDefaultText.GetText());
		builder.Append(ItemReplaceText.GetText());

		#region ECT
		var EffectAttributeText = EffectAttribute.Instance?.ToString( [Skill.Instance, .. EffectArg] , SkillAttackAttributeCoefficientPercent);
		var ConditionAttributeText = ConditionAttribute.Instance?.ToString( [Skill.Instance, .. ConditionArg] , SkillAttackAttributeCoefficientPercent);
		var TargetAttributeText = TargetAttribute.Instance?.ToString();

		builder.Append(string.Join(string.Empty, EctOrder switch
		{
			EctOrderSeq.CTE => [ConditionAttributeText, TargetAttributeText, EffectAttributeText],
			EctOrderSeq.CET => [ConditionAttributeText, EffectAttributeText, TargetAttributeText],
			EctOrderSeq.TCE => [TargetAttributeText, ConditionAttributeText, EffectAttributeText],
			EctOrderSeq.TEC => [TargetAttributeText, EffectAttributeText, ConditionAttributeText],
			EctOrderSeq.ECT => [EffectAttributeText, ConditionAttributeText, TargetAttributeText],
			EctOrderSeq.ETC => [EffectAttributeText, TargetAttributeText, ConditionAttributeText],
			_ => new List<string>(),
		}));
		#endregion

		#region Stance
		if (AfterStanceAttribute.Instance != null)
		{
			var BeforeStanceAttributeText = BeforeStanceAttribute.Instance?.ToString();
			var AfterStanceAttributeText = AfterStanceAttribute.Instance?.ToString();

			builder.Append(BeforeStanceAttribute.Instance != null ?
				"SkillTooltipAttr.stance.before-after".GetText([BeforeStanceAttributeText, AfterStanceAttributeText]) :
				"SkillTooltipAttr.stance.after-only".GetText([AfterStanceAttributeText]));
		}
		#endregion

		return builder.ToString();
	}
	#endregion
}