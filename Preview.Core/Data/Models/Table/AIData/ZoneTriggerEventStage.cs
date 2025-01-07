using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public abstract class ZoneTriggerEventStage : ModelElement , IHaveName
{
	#region Attributes
	public int Zone { get; set; }

	public sbyte ZoneModeSetId { get; set; }

	public sbyte ZoneMode { get; set; }

	public sbyte BranchId { get; set; }

	public string Alias { get; set; }

	public Ref<ZoneTriggerEventCond>[] NextCond { get; set; }

	public sbyte[] NextCondBranchId { get; set; }

	public BroadcastContextSeq BroadcastContext { get; set; }

	public enum BroadcastContextSeq
	{
		None,
		TimeoutTime,
		BossChallengeAttractionRound,
		COUNT
	}

	public string StartStageKismet { get; set; }

	public string EndStageKismet { get; set; }

	public sealed class StageStandByClassicField : ZoneTriggerEventStage { }

	public sealed class StageStandByGuildBattleFieldEntrance : ZoneTriggerEventStage
	{
		public sbyte MinGrowingChannel { get; set; }

		public short[] TotalDurationMinute { get; set; }
	}

	public sealed class StageStandByPersistantZone : ZoneTriggerEventStage
	{
		public bool AllChannel { get; set; }

		public sbyte MaxChannel { get; set; }

		public sbyte MaxEventChannel { get; set; }

		public sbyte MinGrowingChannel { get; set; }

		public short[] TotalDurationMinute { get; set; }

		public PersistantZoneSubtypeSeq PersistantZoneSubtype { get; set; }

		public enum PersistantZoneSubtypeSeq
		{
			None,
			InvadeTown,
			FactionStage,
			COUNT
		}

		public Ref<Text> EventChannelText { get; set; }

		public Ref<Zone>[] EventNotifyDiffZone { get; set; }

		public Ref<GameMessage> EventNotifyDiffZoneMsg { get; set; }

		public Ref<GameMessage> EventNotifyDiffChannelMsg { get; set; }

		public Ref<GameMessage> EventNotifyMsg { get; set; }
	}

	public sealed class StageStandByInstantZone : ZoneTriggerEventStage { }

	public sealed class Stage : ZoneTriggerEventStage
	{
		public Ref<ZoneTriggerEventCond>[] FailCond { get; set; }
	}
	#endregion

	#region Methods
	string IHaveName.Name => Provider.GetTable<Zone>()[Zone]?.Name;
	#endregion
}								 