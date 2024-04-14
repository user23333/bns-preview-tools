using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models.Creature;
public static class AbilityEx
{
	public static string GetText(this MainAbility ability, long value = 0) => GetAbilityText(ability, value);

	public static string GetText(this AttachAbility ability, long value = 0) => GetAbilityText(ability, value);

	private static string GetAbilityText(Enum ability, long value)
	{
		if (ability == default) return null;

		var name = SequenceExtensions.GetText(ability);
		return value == 0 ? name : string.Format("{0} {1}", name, 
			ability.ToString().EndsWith("percent", StringComparison.OrdinalIgnoreCase) ?
			new Integer(value).FloatDot0 + "%" : value.ToString());
	}
}