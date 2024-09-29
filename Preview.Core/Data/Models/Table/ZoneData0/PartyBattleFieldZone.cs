using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public abstract class PartyBattleFieldZone : ModelElement, IAttraction
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public Ref<Zone> Zone { get; set; }

	public short PcMax { get; set; }

	public sbyte RequiredLevel { get; set; }

	public sbyte RequiredMasteryLevel { get; set; }

	public bool EnableTeamMatching { get; set; }

	public bool EnableUnratedMatching { get; set; }

	public bool EnableUnratedJoin { get; set; }

	public sbyte RequiredMemberCountTeamMatching { get; set; }

	public Ref<WeeklyTimeTable> AvailableSoloMatchingWeeklyTime { get; set; }

	public Ref<WeeklyTimeTable> AvailableTeamMatchingWeeklyTime { get; set; }

	public Ref<WeeklyTimeTable> AvailableRandomMatchingWeeklyTime { get; set; }

	public Ref<WeeklyTimeTable> DisableCalcRatingScoreWeeklyTime { get; set; }

	public Ref<WeeklyTimeTable> TimeEffectWeeklyTime { get; set; }

	public Ref<Text> TimeEffectWeeklyName { get; set; }

	public short ReadyDurationSecond { get; set; }

	public short PlayDurationSecond { get; set; }

	public short NoGameDecisionDurationSecond { get; set; }

	public Ref<ZonePcSpawn> EnterAlphaSidePcSpawn { get; set; }

	public Ref<ZonePcSpawn> EnterBetaSidePcSpawn { get; set; }

	public Ref<ZonePcSpawn> StartAlphaSidePcSpawn { get; set; }

	public Ref<ZonePcSpawn> StartBetaSidePcSpawn { get; set; }

	public Msec RespawnDelay { get; set; }

	public Ref<ZoneRespawn> AlphaSideRespawn { get; set; }

	public Ref<ZoneRespawn> BetaSideRespawn { get; set; }

	public short[] SetEnvTime { get; set; }

	public Ref<ZoneEnv2Spawn>[] SetEnv2Target { get; set; }

	public SetEnvOperationSeq[] SetEnvOperation { get; set; }

	public enum SetEnvOperationSeq
	{
		None,
		Open,
		Close,
		Enable,
		Disable,
		COUNT
	}

	public Ref<GameMessage> SetEnvOperationMessage { get; set; }

	public Ref<AttractionGroup> Group { get; set; }

	public Ref<Text> ZoneName2 { get; set; }

	public Ref<Text> ZoneDesc { get; set; }

	public ObjectPath ArenaMinimap { get; set; }

	public short KillScore { get; set; }

	public short GoalScore { get; set; }

	public bool EnableInfiniteHyperEnergy { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public bool UiFilterAttractionQuestOnly { get; set; }

	public Ref<Text> ZoneSubDesc { get; set; }

	public Ref<WeeklyTimeTable>[] WeeklyTimeTableForAddedReward { get; set; }

	public int[] BonusPointPercent { get; set; }

	public int[] BonusExpPercent { get; set; }

	public short VoteDurationSecond { get; set; }

	public int VoteRewardPoint { get; set; }

	public int VoteRewardExp { get; set; }

	public string[] ZoneEnterKismetSequenceName { get; set; }

	public string ResultKismetSequenceName { get; set; }

	public short[] NotifyMsgScoreValue { get; set; }

	public sealed class OccupationWar : PartyBattleFieldZone
	{
		[Name("control-point-1")]
		public Ref<ZoneEnv2Spawn> ControlPoint1 { get; set; }

		[Name("control-point-2")]
		public Ref<ZoneEnv2Spawn> ControlPoint2 { get; set; }

		[Name("control-point-3")]
		public Ref<ZoneEnv2Spawn> ControlPoint3 { get; set; }

		[Name("control-point-4")]
		public Ref<ZoneEnv2Spawn> ControlPoint4 { get; set; }

		[Name("control-point-5")]
		public Ref<ZoneEnv2Spawn> ControlPoint5 { get; set; }

		public short WholeOccupationBonusScore { get; set; }
	}

	public sealed class CaptureTheFlag : PartyBattleFieldZone
	{
		public Ref<ZoneEnv2Spawn> FlagSpawnEnv { get; set; }

		[Name("flag-spawn-env-respawn-duration-second-1")]
		public short FlagSpawnEnvRespawnDurationSecond1 { get; set; }

		[Name("flag-spawn-env-respawn-duration-second-2")]
		public short FlagSpawnEnvRespawnDurationSecond2 { get; set; }

		[Name("flag-spawn-env-respawn-duration-second-3")]
		public short FlagSpawnEnvRespawnDurationSecond3 { get; set; }

		[Name("flag-spawn-env-respawn-duration-second-4")]
		public short FlagSpawnEnvRespawnDurationSecond4 { get; set; }

		[Name("flag-spawn-env-respawn-duration-second-5")]
		public short FlagSpawnEnvRespawnDurationSecond5 { get; set; }

		public sbyte FlagSpawnLimitCount { get; set; }

		[Name("alpha-control-point-1")]
		public Ref<ZoneEnv2Spawn> AlphaControlPoint1 { get; set; }

		[Name("alpha-control-point-2")]
		public Ref<ZoneEnv2Spawn> AlphaControlPoint2 { get; set; }

		[Name("alpha-control-point-3")]
		public Ref<ZoneEnv2Spawn> AlphaControlPoint3 { get; set; }

		[Name("beta-control-point-1")]
		public Ref<ZoneEnv2Spawn> BetaControlPoint1 { get; set; }

		[Name("beta-control-point-2")]
		public Ref<ZoneEnv2Spawn> BetaControlPoint2 { get; set; }

		[Name("beta-control-point-3")]
		public Ref<ZoneEnv2Spawn> BetaControlPoint3 { get; set; }

		public short DoubleOccupationBonusScore { get; set; }

		public short VoteDelaySecond { get; set; }
	}

	public sealed class LeadTheBall : PartyBattleFieldZone
	{
		[Name("arrow-control-point-1")]
		public Ref<ZoneEnv2Spawn> ArrowControlPoint1 { get; set; }

		[Name("arrow-control-point-2")]
		public Ref<ZoneEnv2Spawn> ArrowControlPoint2 { get; set; }

		[Name("arrow-control-point-3")]
		public Ref<ZoneEnv2Spawn> ArrowControlPoint3 { get; set; }

		[Name("arrow-control-point-4")]
		public Ref<ZoneEnv2Spawn> ArrowControlPoint4 { get; set; }

		[Name("arrow-control-point-5")]
		public Ref<ZoneEnv2Spawn> ArrowControlPoint5 { get; set; }

		[Name("arrow-control-point-6")]
		public Ref<ZoneEnv2Spawn> ArrowControlPoint6 { get; set; }

		public Ref<ZoneEnv2Spawn> OpeningTipOffControlPoint { get; set; }

		public Ref<ZoneEnv2Spawn> AlphaGoalPost { get; set; }

		public Ref<ZoneEnv2Spawn> BetaGoalPost { get; set; }

		public short GoalPostEnableDelaySecond { get; set; }

		public sbyte BallSpawnMaxCount { get; set; }

		public short InitBallSpawnDelaySecond { get; set; }

		public Ref<Npc> BallNpcId { get; set; }

		[Name("play-section-duration-second-1")]
		public short PlaySectionDurationSecond1 { get; set; }

		[Name("play-section-duration-second-2")]
		public short PlaySectionDurationSecond2 { get; set; }

		[Name("ball-spawn-interval-second-1")]
		public short BallSpawnIntervalSecond1 { get; set; }

		[Name("ball-spawn-interval-second-2")]
		public short BallSpawnIntervalSecond2 { get; set; }

		[Name("ball-spawn-interval-second-3")]
		public short BallSpawnIntervalSecond3 { get; set; }

		[Name("ball-speed-effect-1")]
		public Ref<Effect> BallSpeedEffect1 { get; set; }

		[Name("ball-speed-effect-2")]
		public Ref<Effect> BallSpeedEffect2 { get; set; }

		[Name("ball-speed-effect-3")]
		public Ref<Effect> BallSpeedEffect3 { get; set; }

		[Name("goal-in-score-1")]
		public short GoalInScore1 { get; set; }

		[Name("goal-in-score-2")]
		public short GoalInScore2 { get; set; }

		[Name("goal-in-score-3")]
		public short GoalInScore3 { get; set; }

		[Name("consecutive-goal-bonus-1")]
		public short ConsecutiveGoalBonus1 { get; set; }

		[Name("consecutive-goal-bonus-2")]
		public short ConsecutiveGoalBonus2 { get; set; }

		[Name("consecutive-goal-bonus-3")]
		public short ConsecutiveGoalBonus3 { get; set; }

		public short ConsecutiveGoalBonusDurationSecond { get; set; }

		[Name("goal-in-effect-1")]
		public Ref<Effect> GoalInEffect1 { get; set; }

		[Name("goal-in-effect-2")]
		public Ref<Effect> GoalInEffect2 { get; set; }

		public ObjectPath FriendGoalInKismetName { get; set; }

		public ObjectPath EnemyGoalInKismetName { get; set; }

		public ObjectPath FriendAlphaGoalPostKismetName { get; set; }

		public ObjectPath FriendBetaGoalPostKismetName { get; set; }

		public ObjectPath EnemyAlphaGoalPostKismetName { get; set; }

		public ObjectPath EnemyBetaGoalPostKismetName { get; set; }

		public ObjectPath FriendAlphaGoalPostGoalInKismetName { get; set; }

		public ObjectPath FriendBetaGoalPostGoalInKismetName { get; set; }

		public ObjectPath EnemyAlphaGoalPostGoalInKismetName { get; set; }

		public ObjectPath EnemyBetaGoalPostGoalInKismetName { get; set; }
	}
	#endregion

	#region Enums
	public enum PartyBattleFieldZoneType
	{
		None,
		OccupationWar,
		CaptureTheFlag,
		LeadTheBall,
		COUNT
	}
	#endregion

	#region Methods
	public string Name => this.ZoneName2.GetText();

	public string Description => this.ZoneDesc.GetText();
	#endregion
}