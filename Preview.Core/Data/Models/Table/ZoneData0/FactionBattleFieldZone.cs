namespace Xylia.Preview.Data.Models;
public sealed class FactionBattleFieldZone : ModelElement, IAttraction
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public Ref<Zone> Zone { get; set; }

	public Ref<AttractionGroup> Group { get; set; }

	public bool UiFilterAttractionQuestOnly { get; set; }

	public Ref<Text> RespawnConfirmText { get; set; }

	public Ref<Quest>[] AttractionQuest { get; set; }

	public sbyte RequiredLevel { get; set; }

	public sbyte RequiredFactionLevel { get; set; }

	public Ref<Text> FactionBattleFieldZoneName2 { get; set; }

	public Ref<Text> FactionBattleFieldZoneDesc { get; set; }

	public string ThumbnailImage { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public Ref<ContentQuota> EntranceQuota { get; set; }
	#endregion

	#region IAttraction
	public string Name => this.FactionBattleFieldZoneName2.GetText();

	public string Description => this.FactionBattleFieldZoneDesc.GetText();
	#endregion
}