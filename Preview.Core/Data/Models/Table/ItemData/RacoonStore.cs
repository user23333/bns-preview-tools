using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class RacoonStore : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public sbyte StoreTabSortNo { get; set; }

	public Ref<Text> StoreTabName { get; set; }

	public TimeUniversal StartDate { get; set; }

	public TimeUniversal EndDate { get; set; }

	public bool IsRetire { get; set; }

	public int[] SlotGroup { get; set; }

	public sbyte FreeResetAmount { get; set; }

	public sbyte PaidResetAmount { get; set; }

	public PaidResetCostTypeSeq PaidResetCostType { get; set; }

	public enum PaidResetCostTypeSeq
	{
		None,
		Item,
		Stone,
		RedStone,
		Money,
		COUNT
	}

	public Ref<Item> PaidResetCostItem { get; set; }

	public long PaidResetCostAmount { get; set; }

	public TimeUniversal AutoResetTime { get; set; }
	#endregion
}