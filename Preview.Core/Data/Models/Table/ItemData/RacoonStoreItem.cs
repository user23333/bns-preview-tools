namespace Xylia.Preview.Data.Models;
public sealed class RacoonStoreItem : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public int SlotGroup { get; set; }

	public Ref<Item> Item { get; set; }

	public short ItemCount { get; set; }

	public ItemGradeSeq ItemGrade { get; set; }

	public enum ItemGradeSeq
	{
		None,
		Good,
		Great,
		BigGreat,
		COUNT
	}

	public short ItemProbWeight { get; set; }

	public int ItemCost { get; set; }

	public CostTypeSeq CostType { get; set; }

	public enum CostTypeSeq
	{
		None,
		Item,
		Stone,
		RedStone,
		Money,
		COUNT
	}

	public Ref<Item> CostItem { get; set; }
	#endregion
}