using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public abstract class DuelBotChallenge : ModelElement, IAttraction
{
	#region Attributes
	public int Id { get; set; }

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

	public Msec DuelBotSpawnDelay { get; set; }

	public Msec ReadyDuration { get; set; }

	public short FloorTimeoutDurationSecond { get; set; }

	public short[] FinishTimeSection { get; set; }

	public short[] FinishTimeSectionFloorSetup { get; set; }

	public short MaxFloor { get; set; }

	public Msec CardSelectDelayDuration { get; set; }

	public Msec ChangeFloorDelayDuration { get; set; }

	public string[] ChangeFloorKismetName { get; set; }

	public string[] ChangeFloorShowName { get; set; }

	public Msec ResultShowDelayDuration { get; set; }

	public string BladeMasterFloorKismetName { get; set; }

	public string KungFuFighterFloorKismetName { get; set; }

	public string ForceMasterFloorKismetName { get; set; }

	public string DestroyerFloorKismetName { get; set; }

	public string SummonerFloorKismetName { get; set; }

	public string AssassinFloorKismetName { get; set; }

	public string SwordMasterFloorKismetName { get; set; }

	public string WarlockFloorKismetName { get; set; }

	public string SoulFighterFloorKismetName { get; set; }

	public string ShooterFloorKismetName { get; set; }

	public string WarriorFloorKismetName { get; set; }

	public string ArcherFloorKismetName { get; set; }

	public string SpearMasterFloorKismetName { get; set; }

	public string ThundererFloorKismetName { get; set; }

	public string DualBladerFloorKismetName { get; set; }

	public string BardFloorKismetName { get; set; }

	public string CountdownSoundName { get; set; }

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

	public sealed class TimeAttackMode : DuelBotChallenge
	{
		public short TotalTimeoutDurationSecond { get; set; }
	}

	public sealed class RoundMode : DuelBotChallenge
	{
		public sbyte TotalRound { get; set; }
	}
	#endregion

	#region IAttraction
	public string Name => this.DungeonName2.GetText();
	public string Description => this.DungeonDesc.GetText();
	#endregion
}