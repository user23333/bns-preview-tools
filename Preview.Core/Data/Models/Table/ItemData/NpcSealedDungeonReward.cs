namespace Xylia.Preview.Data.Models;
public sealed class NpcSealedDungeonReward : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public sbyte SealedLevel { get; set; }

	public string Alias { get; set; }

	public Ref<Reward> RewardDefault { get; set; }

	public Ref<Reward> RewardEvent { get; set; }

	public Ref<Reward> RewardPersonalDroppedPouch { get; set; }
	#endregion
}