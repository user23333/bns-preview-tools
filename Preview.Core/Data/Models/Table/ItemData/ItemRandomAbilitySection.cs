namespace Xylia.Preview.Data.Models;
public sealed class ItemRandomAbilitySection : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public int VariationValueMin { get; set; }

	public int VariationValueMax { get; set; }

	public int VariationValueWithSpecialItemMin { get; set; }

	public int VariationValueWithSpecialItemMax { get; set; }
	#endregion
}