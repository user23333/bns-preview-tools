namespace Xylia.Preview.Data.Models;
public sealed class BossChallenge : ModelElement, IAttraction
{
	#region Attributes
	public Ref<Zone> Zone { get; set; }

	public int Id { get; set; }

	public short RequireScore { get; set; }

	public short LastRound { get; set; }

	public string Alias { get; set; }

	public Ref<AttractionGroup> Group { get; set; }

	public Ref<Quest>[] AttractionQuest { get; set; }

	public bool EnableHeartCount { get; set; }

	public sbyte MaxInstantHeartCount { get; set; }

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

	public Ref<Text> BossChallengeName2 { get; set; }

	public Ref<Text> BossChallengeDesc { get; set; }

	public sbyte UiTextGrade { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public sbyte RecommandLevelMin { get; set; }

	public sbyte RecommandLevelMax { get; set; }

	public sbyte RecommandMasteryLevelMin { get; set; }

	public sbyte RecommandMasteryLevelMax { get; set; }

	public short RecommendAttackPower { get; set; }

	public Ref<Item> StandardGearWeapon { get; set; }

	public Ref<Quest>[] DisplayQuests { get; set; }

	public bool EnableInfiniteHyperEnergy { get; set; }

	public Ref<Text> Tactic { get; set; }

	public Ref<ContentsJournalRecommendItem> RecommendAlias { get; set; }
	#endregion

	#region Methods
	public string Name => this.BossChallengeName2.GetText();

	public string Description => this.BossChallengeDesc.GetText();
	#endregion
}