namespace Xylia.Preview.Data.Models;
public sealed class ClassicFieldZone : ModelElement, IAttraction
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public Ref<Zone>[] Zone { get; set; }

	public Ref<AttractionGroup> Group { get; set; }

	public Ref<Quest>[] AttractionQuest { get; set; }

	public bool UiFilterAttractionQuestOnly { get; set; }

	public Ref<Text> RespawnConfirmText { get; set; }

	public Ref<Text> EscapeCaveConfirmText { get; set; }

	public short RecommendAttackPower { get; set; }

	public Ref<Item> StandardGearWeapon { get; set; }

	public Ref<Text> ClassicFieldZoneName2 { get; set; }

	public Ref<Text> ClassicFieldZoneDesc { get; set; }

	public string ThumbnailImage { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public sbyte UiTextGrade { get; set; }

	public Ref<Text> Tactic { get; set; }

	public Ref<ContentsJournalRecommendItem> RecommendAlias { get; set; }

	public sbyte RecommendLevelMin { get; set; }

	public sbyte RecommendLevelMax { get; set; }

	public sbyte RecommendMasteryLevelMin { get; set; }

	public sbyte RecommendMasteryLevelMax { get; set; }
	#endregion

	#region IAttraction
	public string Name => this.ClassicFieldZoneName2.GetText();
	public string Description => this.ClassicFieldZoneDesc.GetText();
	#endregion
}