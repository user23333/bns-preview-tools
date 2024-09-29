using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class DuelNpcChallenge : ModelElement, IAttraction
{
	#region Attributes
	public int Id { get; set; }

	public bool SeasonEnable { get; set; }

	public Ref<Zone> Zone { get; set; }

	public string Alias { get; set; }

	public Ref<AttractionGroup> Group { get; set; }

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

	public short RecommendAttackPower { get; set; }

	public Ref<Item> StandardGearWeapon { get; set; }

	public float CenterX { get; set; }

	public float CenterY { get; set; }

	public float CenterZ { get; set; }

	public float Radius { get; set; }

	public Msec DuelNpcSpawnDelay { get; set; }

	public Msec ReadyDuration { get; set; }

	public Msec WaitingTime { get; set; }

	public Msec StandByTime { get; set; }

	public Msec ChooseSkillTime { get; set; }

	public Msec ChooseSkillTimeOnly { get; set; }

	public Msec WatingLeaveTime { get; set; }

	public sbyte TotalRound { get; set; }

	public short MaxFloor { get; set; }

	public short UsableFloor { get; set; }

	public Ref<DuelNpcChallengeGroup>[] DuelNpcChallengeGroup { get; set; }

	public Msec CardSelectDelayDuration { get; set; }

	public Msec ChangeFloorDelayDuration { get; set; }

	public string[] ChangeFloorKismetName { get; set; }

	public string[] ChangeFloorShowName { get; set; }

	public Msec ResultShowDelayDuration { get; set; }

	public ObjectPath ScrollBossImageset { get; set; }

	public string CountdownSoundName { get; set; }

	public sbyte ActiveStrategicSkillCount { get; set; }

	public sbyte PassiveStrategicSkillCount { get; set; }

	public Ref<Text> Name2 { get; set; }

	public Ref<Text> DungeonName2 { get; set; }

	public Ref<Text> DungeonDesc { get; set; }

	public string ArenaMinimap { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public sbyte UiTextGrade { get; set; }

	public sbyte RecommandLevelMin { get; set; }

	public sbyte RecommandLevelMax { get; set; }

	public sbyte RecommandMasteryLevelMin { get; set; }

	public sbyte RecommandMasteryLevelMax { get; set; }

	public Ref<Quest>[] DisplayQuests { get; set; }

	public Ref<Text> Tactic { get; set; }

	public Ref<ContentsJournalRecommendItem> RecommendAlias { get; set; }

	public Ref<ContentQuota> EntranceQuota { get; set; }

	public Ref<ContentsReset> ContentsReset { get; set; }
	#endregion

	#region IAttraction
	public string Name => this.DungeonName2.GetText();
	public string Description => this.DungeonDesc.GetText();
	#endregion
}