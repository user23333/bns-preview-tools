using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public abstract class PublicRaid : ModelElement, IAttraction
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<Zone> Zone { get; set; }

	public int Id { get; set; }

	public sbyte MaxPcCount { get; set; }

	public sbyte MaxPartyCount { get; set; }

	public sbyte RoundId { get; set; }

	public sbyte RequiredLevel { get; set; }

	public sbyte RequiredMasteryLevel { get; set; }

	public Ref<Quest>[] RequiredPrecedingQuest { get; set; }

	public RequiredPrecedingQuestCheckSeq RequiredPrecedingQuestCheck { get; set; }

	public enum RequiredPrecedingQuestCheckSeq
	{
		And,
		Or,
		COUNT
	}

	public Ref<WeeklyTimeTable> RequiredAvailableWeeklyTime { get; set; }

	public Ref<WeeklyTimeTable> StartWeeklyTime { get; set; }

	public short StandByDurationSecond { get; set; }

	public Ref<AttractionGroup> Group { get; set; }

	public Ref<Zone> ArenaEntranceZone { get; set; }

	public Ref<ZonePcSpawn> EnterPcSpawn { get; set; }

	public Ref<Text> PublicraidName2 { get; set; }

	public Ref<Text> PublicraidDesc { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public ObjectPath PublicraidIcon { get; set; }

	public ObjectPath PublicraidImage { get; set; }

	public bool EnableResetCombatMode { get; set; }

	public Msec ResetCombatModeRecycleDuration { get; set; }

	public string[] PublicRaidKismetName { get; set; }

	public Ref<Npc>[] PublicRaidNpcForKismet { get; set; }

	public bool EnableCustomPouchDropPosition { get; set; }

	public float PouchPosX { get; set; }

	public float PouchPosY { get; set; }

	public float PouchPosZ { get; set; }

	public short EndByDurationSecond { get; set; }

	public string ExitEnvName { get; set; }

	public Ref<Npc>[] BossNpcAlias { get; set; }

	public Ref<Text>[] BossNpcSection { get; set; }

	public sealed class PublicRaid1 : PublicRaid
	{
		public bool EnablePublicRaidEvent { get; set; }
	}

	public sealed class PublicRaid2 : PublicRaid
	{
	}

	public sealed class PublicRaid3 : PublicRaid
	{

	}

	public sealed class PublicRaid4 : PublicRaid
	{
		public Ref<RaidDungeon> RaidDungeon { get; set; }
	}

	public sealed class PublicRaid5 : PublicRaid
	{

	}

	public sealed class InterBattleField : PublicRaid
	{
		public Ref<Item> PortalTicketItemInfo { get; set; }

		public Ref<ContentQuota> EntranceQuota { get; set; }

		public Ref<ArenaPortal> ArenaPortal { get; set; }

		public sbyte ArenaMoveMaxPartyMemberCount { get; set; }
	}

	public sealed class GuerrillaEvent : PublicRaid
	{
		public ObjectPath SystemMenuIcon { get; set; }

		public Ref<Text> HudNotificationMenuName { get; set; }

		public Ref<ArenaPortal> ArenaPortal { get; set; }
	}
	#endregion

	#region IAttraction
	public string Name => this.PublicraidName2.GetText();

	public string Description => this.PublicraidDesc.GetText();
	#endregion
}