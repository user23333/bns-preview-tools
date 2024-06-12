namespace Xylia.Preview.Data.Models;
public sealed class QuestBonusReward : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public sbyte NormalBonusRewardTotalCount { get; set; }

	public Ref<Item>[] FixedItem { get; set; }

	public short[] FixedItemCount { get; set; }

	public sbyte FixedItemTotalCount { get; set; }

	public Ref<Item>[] RandomItem { get; set; }

	public short[] RandomItemStackCountMin { get; set; }

	public short[] RandomItemStackCountMax { get; set; }

	public sbyte RandomItemSelectedCount { get; set; }

	public sbyte RandomItemTotalInputCount { get; set; }

	public Ref<Text> RandomItemTooltipText { get; set; }

	public sbyte PaidBonusRewardTotalCount { get; set; }

	public int PaidItemCost { get; set; }

	public Ref<Item>[] PaidFixedItem { get; set; }

	public short[] PaidFixedItemCount { get; set; }

	public sbyte PaidFixedItemTotalCount { get; set; }

	public Ref<Item>[] PaidRandomItem { get; set; }

	public short[] PaidRandomItemStackCountMin { get; set; }

	public short[] PaidRandomItemStackCountMax { get; set; }

	public sbyte PaidRandomItemSelectedCount { get; set; }

	public sbyte PaidRandomItemTotalInputCount { get; set; }

	public Ref<Text> PaidRandomItemTooltipText { get; set; }
	#endregion
}