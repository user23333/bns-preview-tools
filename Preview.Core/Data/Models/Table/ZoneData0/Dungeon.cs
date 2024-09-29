using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public  class Dungeon : ModelElement, IAttraction
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public DungeonTypeSeq DungeonType { get; set; }

	public enum DungeonTypeSeq
	{
		Unbind,
		Bind,
		COUNT
	}

	public Ref<AttractionGroup> Group { get; set; }

	public Ref<Zone>[] ZoneNeutral { get; set; }

	public Ref<Zone>[] Zone { get; set; }

	public sbyte[] ZoneMissionStep { get; set; }

	public Ref<Feedback> Feedback { get; set; }

	public Ref<Zone> ArenaEntranceZone { get; set; }

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

	public bool EnableHeartCount { get; set; }

	public sbyte[] MaxInstantHeartCountDifficultyType { get; set; }

	public Ref<Item> GsItemBladeMaster { get; set; }

	public Ref<Item> GsItemBladeMasterHard { get; set; }

	public Ref<Item> GsItemBladeMasterEasy { get; set; }

	public Ref<Item> GsItemKungFuFighter { get; set; }

	public Ref<Item> GsItemKungFuFighterHard { get; set; }

	public Ref<Item> GsItemKungFuFighterEasy { get; set; }

	public Ref<Item> GsItemForceMaster { get; set; }

	public Ref<Item> GsItemForceMasterHard { get; set; }

	public Ref<Item> GsItemForceMasterEasy { get; set; }

	public Ref<Item> GsItemDestroyer { get; set; }

	public Ref<Item> GsItemDestroyerHard { get; set; }

	public Ref<Item> GsItemDestroyerEasy { get; set; }

	public Ref<Item> GsItemSummoner { get; set; }

	public Ref<Item> GsItemSummonerHard { get; set; }

	public Ref<Item> GsItemSummonerEasy { get; set; }

	public Ref<Item> GsItemAssassin { get; set; }

	public Ref<Item> GsItemAssassinHard { get; set; }

	public Ref<Item> GsItemAssassinEasy { get; set; }

	public Ref<Item> GsItemSwordMaster { get; set; }

	public Ref<Item> GsItemSwordMasterHard { get; set; }

	public Ref<Item> GsItemSwordMasterEasy { get; set; }

	public Ref<Item> GsItemWarlock { get; set; }

	public Ref<Item> GsItemWarlockHard { get; set; }

	public Ref<Item> GsItemWarlockEasy { get; set; }

	public Ref<Item> GsItemSoulFighter { get; set; }

	public Ref<Item> GsItemSoulFighterHard { get; set; }

	public Ref<Item> GsItemSoulFighterEasy { get; set; }

	public Ref<Item> GsItemShooter { get; set; }

	public Ref<Item> GsItemShooterHard { get; set; }

	public Ref<Item> GsItemShooterEasy { get; set; }

	public Ref<Item> GsItemWarrior { get; set; }

	public Ref<Item> GsItemWarriorHard { get; set; }

	public Ref<Item> GsItemWarriorEasy { get; set; }

	public Ref<Item> GsItemArcher { get; set; }

	public Ref<Item> GsItemArcherHard { get; set; }

	public Ref<Item> GsItemArcherEasy { get; set; }

	public Ref<Item> GsItemSpearMaster { get; set; }

	public Ref<Item> GsItemSpearMasterHard { get; set; }

	public Ref<Item> GsItemSpearMasterEasy { get; set; }

	public Ref<Item> GsItemThunderer { get; set; }

	public Ref<Item> GsItemThundererHard { get; set; }

	public Ref<Item> GsItemThundererEasy { get; set; }

	public Ref<Item> GsItemDualBlader { get; set; }

	public Ref<Item> GsItemDualBladerHard { get; set; }

	public Ref<Item> GsItemDualBladerEasy { get; set; }

	public Ref<Item> GsItemBard { get; set; }

	public Ref<Item> GsItemBardHard { get; set; }

	public Ref<Item> GsItemBardEasy { get; set; }

	public sbyte ApplyContentsBanId { get; set; }

	public short PcMax { get; set; }

	public bool IgnorePartyDifficultyType { get; set; }

	public Ref<Npc>[] NpcForStep { get; set; }

	public short[] PcSpawnForStep { get; set; }

	public int[] MoneyForStep { get; set; }

	public Ref<Item>[] ItemForStep { get; set; }

	public short[] ItemCountForStep { get; set; }

	public Ref<Quest>[] QuestForStep { get; set; }

	public Ref<Quest> Quest { get; set; }

	public Ref<Quest>[] DungeonQuestDifficultyType { get; set; }

	public Ref<Effect>[] EffectPcDifficultyType { get; set; }

	public Ref<Effect>[] EffectNpcDifficultyType { get; set; }

	public bool EnableSkillScore { get; set; }

	public int SkillScoreRecommendGearScore { get; set; }

	public Msec[] SkillScoreRecommendClearTimeStep { get; set; }

	public short[] SkillScoreRecommendClearTimeBonus { get; set; }

	public short[] SkillScoreRecommandPartyMemberCountBonusDifficultyType { get; set; }

	public int[] SkillScoreRecommendHpStep { get; set; }

	public short[] SkillScoreRecommendHpBonus { get; set; }

	public short SkillScoreRecommendUseHeartCount { get; set; }

	public short SkillScoreRecommendUseHeartCountBonus { get; set; }

	public int SkillScoreRecommendUseItemPoint { get; set; }

	public short SkillScoreRecommendUseItemBonus { get; set; }

	public Ref<Npc> SkillScoreBossNpc { get; set; }

	public short SkillScoreBossNpcBonus { get; set; }

	public Ref<Quest>[] AttractionQuest { get; set; }

	public bool UiFilterAttractionQuestOnly { get; set; }

	public Ref<Text> RespawnConfirmText { get; set; }

	public Ref<Text> EscapeCaveConfirmText { get; set; }

	public sbyte UiTextGrade { get; set; }

	public Ref<Text> DungeonName2 { get; set; }

	public Ref<Text> DungeonDesc { get; set; }

	public bool FactionBattleField { get; set; }

	public ObjectPath ArenaMinimap { get; set; }

	public short RecommendAttackPowerEasy { get; set; }

	public short RecommendAttackPowerNormal { get; set; }

	public short RecommendAttackPowerHard { get; set; }

	public Ref<Item> StandardGearWeaponEasy { get; set; }

	public Ref<Item> StandardGearWeaponNormal { get; set; }

	public Ref<Item> StandardGearWeaponHard { get; set; }

	public bool UseDifficultyNormal { get; set; }

	public bool UseDifficultyHard { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public sbyte RecommandLevelMin { get; set; }

	public sbyte RecommandLevelMax { get; set; }

	public sbyte RecommandMasteryLevelMin { get; set; }

	public sbyte RecommandMasteryLevelMax { get; set; }

	public Ref<Quest>[] DisplayQuests { get; set; }

	public Ref<Text> TacticNormal { get; set; }

	public Ref<Text> TacticHard { get; set; }

	public Ref<Text> TacticEasy { get; set; }

	public Ref<ContentsJournalRecommendItem> RecommendAliasNormal { get; set; }

	public Ref<ContentsJournalRecommendItem> RecommendAliasHard { get; set; }

	public Ref<ContentsJournalRecommendItem> RecommendAliasEasy { get; set; }

	public bool BossUiExtendDistance { get; set; }

	public Ref<Npc>[] BossNpcAlias { get; set; }

	public Ref<Text>[] BossNpcSection { get; set; }

	public sealed class None : Dungeon
	{

	}

	public sealed class Normal : Dungeon
	{

	}

	public sealed class Sealed : Dungeon
	{
		public sbyte MaxSealedLevel { get; set; }

		public Ref<SealedDungeonModify> ModifyData { get; set; }

		public Ref<ContentQuota> BindQuota { get; set; }

		public short ThemeVersion { get; set; }

		public Ref<Zone> ThemeArenaEntranceZone { get; set; }

		public TimeUniversal GimmickWeekStartDateTime { get; set; }

		public sbyte UiScrollSlotIndex { get; set; }
	}

	public sealed class Wave : Dungeon
	{
		public sbyte MaxWave { get; set; }

		public Ref<ContentQuota> EntranceQuota { get; set; }

		public short RewardBoxDataId { get; set; }

		public short RewardDataId { get; set; }

		public Ref<ContentsReset> ContentsReset { get; set; }
	}
	#endregion

	#region IAttraction
	public string Name => this.DungeonName2.GetText();

	public string Description => this.DungeonDesc.GetText();
	#endregion
}