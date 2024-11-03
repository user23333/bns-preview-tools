namespace Xylia.Preview.Data.Models;
public sealed class JackpotBossZone : ModelElement, IAttraction
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public Ref<Zone> Zone { get; set; }

	public Ref<AttractionGroup> Group { get; set; }

	public Ref<Quest>[] AttractionQuest { get; set; }

	public bool UiFilterAttractionQuestOnly { get; set; }

	public Ref<Text> Name2 { get; set; }

	public Ref<Text> Desc { get; set; }

	public sbyte UiTextGrade { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }
	#endregion

	#region IAttraction
	public string Name => this.Name2.GetText();
	public string Description => this.Desc.GetText();
	#endregion
}