using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class ItemSpirit : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public int Id { get; set; }

	public sbyte Level { get; set; }

	public Ref<Item> MainIngredient { get; set; }

	public EquipType[] ApplicablePart { get; set; }

	public bool UseRandomAbilityValue { get; set; }

	public sbyte SuccessProbability { get; set; }

	public int MoneyCost { get; set; }

	public Ref<RandomDistribution> DistributionType { get; set; }

	public Ref<Item>[] FixedIngredient { get; set; }

	public short[] FixedIngredientStackCount { get; set; }

	public AttachAbilitySeq[] AttachAbility { get; set; }

	public int[] AbilityMin { get; set; }

	public int[] AbilityMax { get; set; }

	public int[] OnceAttachAbilityMin { get; set; }

	public int[] OnceAttachAbilityMax { get; set; }

	public WarningSeq Warning { get; set; }

	public enum WarningSeq
	{
		None,
		Fail,
		COUNT
	}
	
	public bool UseRandomAbilityValueSelect { get; set; }

	public int SelectCount { get; set; }
	#endregion
}