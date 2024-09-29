namespace Xylia.Preview.Data.Models;
public sealed class FeedbackRank : ModelElement 
{
	#region Attributes
	public string Alias { get; set; }

	public RankTypeSeq RankType { get; set; }

	public enum RankTypeSeq
	{
		None,
		AttackDamage,
		ReceivedDamage,
		AttackResponse,
		COUNT
	}

	public int[] RankScore { get; set; }

	public Ref<Text>[] RankTitle { get; set; }
	#endregion
}