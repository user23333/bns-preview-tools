using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class SkillTooltipAttribute : ModelElement
{
	#region Attributes
	public ArgTypeSeq[] ArgType { get; set; }

	public enum ArgTypeSeq
	{
		None,
		DamagePercentMinMax,
		DamagePercent,
		Time,
		StackCount,
		Effect,
		HealPercent,
		DrainPercent,
		Skill,
		ConsumePercent,
		ProbabilityPercent,
		StanceType,
		Percent,
		Counter,
		Distance,
		KeyCommand,
		Number,
		TextAlias,
		rHypermove,
		rHealPercent,
		rHealDiff,
		rShieldPercent,
		rShieldDiff,
		rSupportPercent,
		rSupportDiff,
		COUNT
	}

	public Ref<Text> Text { get; set; }

	public Icon Icon { get; set; }

	public ModifyType SkillModifyType { get; set; }

	public enum ModifyType
	{
		None,
		RecycleDuration,
		SpConsume,
		Damage,
		HpDrain,
		HealPercent,
		COUNT
	}
	#endregion

	#region Methods
	public override string ToString() => ToString([], 0);

	public string ToString(TextArguments arguments, short AttributeCoefficient)
	{
		for (int x = 1; x < arguments.Count; x++)
		{
			var type = ArgType[x - 1];
			if (type == ArgTypeSeq.None) continue;

			// get argument value
			var value = new int[2];
			var arg = arguments[x] as string;
			if (arg != null)
			{
				var v = arg.Split(',');
				if (v.Length >= 1 && int.TryParse(v[0], out var v1)) value[0] = v1;
				if (v.Length >= 2 && int.TryParse(v[1], out var v2)) value[1] = v2;
			}

			#region replace to text
			arguments[x] = type switch
			{
				ArgTypeSeq.DamagePercentMinMax => GetDamageInfo(value[0], value[1], AttributeCoefficient),
				ArgTypeSeq.DamagePercent => GetDamageInfo(value[0], 0, AttributeCoefficient),
				ArgTypeSeq.Time => "UI.Tooltip.Sequence.time".GetText([new Msec(value[0]).TotalSeconds]),
				ArgTypeSeq.StackCount => "UI.Tooltip.Sequence.stack-count".GetText([value[0]]),
				ArgTypeSeq.Effect => $"<font name=\"00008130.Program.Fontset_ItemGrade_6\">{Provider.GetTable<Effect>()[arg]?.Name}</font>",
				ArgTypeSeq.HealPercent => "UI.Tooltip.Sequence.heal-percent".GetText([value[0]]),
				ArgTypeSeq.DrainPercent => "UI.Tooltip.Sequence.drain-percent".GetText([value[0]]),
				ArgTypeSeq.Skill => $"<font name=\"00008130.Program.Fontset_ItemGrade_4\">{Provider.GetTable<Skill3>()[arg]?.Name}</font>",
				ArgTypeSeq.ConsumePercent => "UI.Tooltip.Sequence.consume-percent".GetText([value[0]]),
				ArgTypeSeq.ProbabilityPercent => "UI.Tooltip.Sequence.probability-percent".GetText([value[0]]),
				ArgTypeSeq.StanceType => "UI.Tooltip.Sequence.stance-type".GetText([arg.ToEnum<StanceSeq>().GetText()]),
				ArgTypeSeq.Percent => "UI.Tooltip.Sequence.percent".GetText([value[0]]),
				ArgTypeSeq.Counter => "UI.Tooltip.Sequence.counter".GetText([value[0]]),
				ArgTypeSeq.Distance => "UI.Tooltip.Sequence.distance".GetText([value[0] * 0.01]),
				ArgTypeSeq.KeyCommand => "UI.Tooltip.Sequence.key-command".GetText([Provider.GetTable<Skill3>()[arg]?.CurrentShortCutKey]),
				ArgTypeSeq.Number => "UI.Tooltip.Sequence.number".GetText([value[0]]),
				ArgTypeSeq.TextAlias => arg.GetText(),
				_ => null,
			};
			#endregion
		}

		return Icon?.GetImage()?.Tag + Text.GetText(arguments)
			+ (AttributeCoefficient > 0 ? "UI.Tooltip.Attack.Icon.Attribute.only-one".GetText() : null);
	}
	#endregion


	#region Helpers
	internal static string GetDamageInfo(int min, int max, short AttributeCoefficient = 0)
	{
		return (max == 0 || min == max ?
			$"UI.Tooltip.Skill.damage-percent" :
			$"UI.Tooltip.Skill.damage-percent-min-max").GetText([min, max]);

		//// get attack power
		//var power = Settings.Default.Skill_AttackPower * 0.01;
		//if (AttributeCoefficient > 0) power = power * (AttributeCoefficient * 0.01) * (Settings.Default.Skill_AttackAttributePercent * 0.97);

		//// get display damage (without critical)
		//var dmin = (power * vmin * 0.985).ToString("N0");
		//var dmax = (power * vmax * 1.015).ToString("N0");

		//return vmax == 0 ?
		//	"UI.Tooltip.Sequence.damage-percent".GetText([dmin]) :
		//	"UI.Tooltip.Sequence.damage-percent-min-max".GetText([dmin, dmax]);
	}
	#endregion
}