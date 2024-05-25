using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.Properties;

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

	public string Icon { get; set; }

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
		var provider = FileCache.Data.Provider;

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
				ArgTypeSeq.Time => "UI.Tooltip.Sequence.time".GetText([value[0] * 0.001]),
				ArgTypeSeq.StackCount => "UI.Tooltip.Sequence.stack-count".GetText([value[0]]),
				ArgTypeSeq.Effect => $"<font name=\"00008130.Program.Fontset_ItemGrade_6\">{provider.GetTable<Effect>()[arg]?.Name}</font>",
				ArgTypeSeq.HealPercent => "UI.Tooltip.Sequence.heal-percent".GetText([value[0]]),
				ArgTypeSeq.DrainPercent => "UI.Tooltip.Sequence.drain-percent".GetText([value[0]]),
				ArgTypeSeq.Skill => $"<font name=\"00008130.Program.Fontset_ItemGrade_4\">{provider.GetTable<Skill3>()[arg]?.Name}</font>",
				ArgTypeSeq.ConsumePercent => "UI.Tooltip.Sequence.consume-percent".GetText([value[0]]),
				ArgTypeSeq.ProbabilityPercent => "UI.Tooltip.Sequence.probability-percent".GetText([value[0]]),
				ArgTypeSeq.StanceType => "UI.Tooltip.Sequence.stance-type".GetText([arg.ToEnum<StanceSeq>().GetText()]),
				ArgTypeSeq.Percent => "UI.Tooltip.Sequence.percent".GetText([value[0]]),
				ArgTypeSeq.Counter => "UI.Tooltip.Sequence.counter".GetText([value[0]]),
				ArgTypeSeq.Distance => "UI.Tooltip.Sequence.distance".GetText([value[0] * 0.01]),
				ArgTypeSeq.KeyCommand => "UI.Tooltip.Sequence.key-command".GetText([provider.GetTable<Skill3>()[arg]?.CurrentShortCutKey]),
				ArgTypeSeq.Number => "UI.Tooltip.Sequence.number".GetText([value[0]]),
				ArgTypeSeq.TextAlias => arg.GetText(),
				_ => null,
			};
			#endregion
		}

		return IconTexture.Parse(Icon)?.Tag + Text.GetText(arguments);
	}
	#endregion


	#region Helpers
	internal static string GetDamageInfo(int vmin, int vmax, short AttributeCoefficient = 0)
	{
		var mode = Settings.Default.Skill_DamageMode;
		if (mode == DamageMode.Source)
		{
			return (vmax == 0 || vmin == vmax ? $"{vmin}xAP" : $"{vmin}~{vmax} xAP") +
				(AttributeCoefficient > 0 ? "<image enablescale='true' imagesetpath='00009076.CharInfo_tooltip_power' scalerate='1.4'/>" : null);
		}
		else
		{
			// get attack power
			var power = Settings.Default.Skill_AttackPower * 0.01;
			if (AttributeCoefficient > 0) power = power * (AttributeCoefficient * 0.01) * (Settings.Default.Skill_AttackAttributePercent * 0.97);

			// get display damage (without critical)
			var dmin = (power * vmin * 0.985).ToString("N0");
			var dmax = (power * vmax * 1.015).ToString("N0");

			return vmax == 0 ?
				"UI.Tooltip.Sequence.damage-percent".GetText([dmin]) :
				"UI.Tooltip.Sequence.damage-percent-min-max".GetText([dmin, dmax]);
		}
	}

	public enum DamageMode
	{
		Source,
		Default,
		Critical,
	}
	#endregion
}