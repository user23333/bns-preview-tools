using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class Cave2 : ModelElement, IAttraction
{
	#region Attributes
	public long AutoId { get; set; }

	public short Id { get; set; }

	public string Alias { get; set; }

	public Ref<AttractionGroup> Group { get; set; }

	public Ref<Zone>[] Zone { get; set; }

	public bool EnableHeartCount { get; set; }

	public sbyte MaxInstantHeartCount { get; set; }

	public short PcMax { get; set; }

	public bool EnableSkillScore { get; set; }

	public int SkillScoreRecommendGearScore { get; set; }

	public Msec[] SkillScoreRecommendClearTimeStep { get; set; }

	public short[] SkillScoreRecommendClearTimeBonus { get; set; }

	public short SkillScoreRecommandPartyMemberCountBonus { get; set; }

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

	public Ref<Text> Cave2Name2 { get; set; }

	public Ref<Text> Cave2Desc { get; set; }

	public Ref<Zone> ArenaEntranceZone { get; set; }

	public string ArenaMinimap { get; set; }

	public bool ArenaDisableZonePhase { get; set; }

	public sbyte RequiredLevel { get; set; }

	public sbyte RequiredMasteryLevel { get; set; }

	public Ref<Quest> QuestForIgnoringRequiredLevel { get; set; }

	public Ref<Item> GsItemBladeMaster { get; set; }

	public Ref<Item> GsItemKungFuFighter { get; set; }

	public Ref<Item> GsItemForceMaster { get; set; }

	public Ref<Item> GsItemDestroyer { get; set; }

	public Ref<Item> GsItemSummoner { get; set; }

	public Ref<Item> GsItemAssassin { get; set; }

	public Ref<Item> GsItemSwordMaster { get; set; }

	public Ref<Item> GsItemWarlock { get; set; }

	public Ref<Item> GsItemSoulFighter { get; set; }

	public Ref<Item> GsItemShooter { get; set; }

	public Ref<Item> GsItemWarrior { get; set; }

	public Ref<Item> GsItemArcher { get; set; }

	public Ref<Item> GsItemSpearMaster { get; set; }

	public Ref<Item> GsItemThunderer { get; set; }

	public Ref<Item> GsItemDualBlader { get; set; }

	public Ref<Item> GsItemBard { get; set; }

	public short RecommendAttackPower { get; set; }

	public Ref<Item> StandardGearWeapon { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public sbyte RecommandLevelMin { get; set; }

	public sbyte RecommandLevelMax { get; set; }

	public sbyte RecommandMasteryLevelMin { get; set; }

	public sbyte RecommandMasteryLevelMax { get; set; }

	public Ref<Quest>[] DisplayQuests { get; set; }

	public Ref<Text> Tactic { get; set; }

	public Ref<ContentsJournalRecommendItem> RecommendAlias { get; set; }
	#endregion

	#region Methods
	public string Name => this.Cave2Name2.GetText();
	public string Description => this.Cave2Desc.GetText();
	#endregion
}