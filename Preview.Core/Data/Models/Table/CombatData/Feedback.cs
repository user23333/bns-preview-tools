namespace Xylia.Preview.Data.Models;
public sealed class Feedback : ModelElement 
{
	#region Attributes
	public string Alias { get; set; }

	public int MaxScoreExceptBossNpc { get; set; }

	public sbyte BossNpcKillCount { get; set; }

	public Ref<FeedbackBossNpc>[] BossNpc { get; set; }

	public bool UseProgressInfoUi { get; set; }

	public bool UseSimpleResultUi { get; set; }

	public bool UseCombatSignalUi { get; set; }

	public Ref<FeedbackRank> AttackDamageRank { get; set; }

	public Ref<FeedbackRank> ReceivedDamageRank { get; set; }

	public Ref<FeedbackRank> AttackResponseRank { get; set; }
	#endregion
}