namespace Xylia.Preview.Data.Models;
public sealed class SoulBoostMissionStep : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public Ref<SoulBoostEvent> Event { get; set; }

	public sbyte StepNumber { get; set; }

	public OpenConditionTypeSeq OpenConditionType { get; set; }

	public enum OpenConditionTypeSeq
	{
		None,
		Free,
		Season,
		ParticipateCharacter,
		NewbieEvent,
		MissionTaskCount,
		CharacterLevel,
		CharacterMasteryLevel,
		COUNT
	}

	public short OpenConditionValue { get; set; }

	public ViewableTypeSeq ViewableType { get; set; }

	public enum ViewableTypeSeq
	{
		None,
		BeforeOpen,
		AfterOpen,
		COUNT
	}

	public Ref<SoulBoostMissionTask>[] MissionTask { get; set; }

	public Ref<Item>[] MissionStepRewardItem { get; set; }

	public short[] MissionStepRewardItemCount { get; set; }

	public string Name { get; set; }

	public string NameEn { get; set; }

	public string NameFr { get; set; }

	public string NameDe { get; set; }

	public string NamePt { get; set; }

	public string NameTh { get; set; }

	public string NameVn { get; set; }

	public Ref<Text> NameText { get; set; }

	public string Description { get; set; }

	public string DescriptionEn { get; set; }

	public string DescriptionFr { get; set; }

	public string DescriptionDe { get; set; }

	public string DescriptionPt { get; set; }

	public string DescriptionTh { get; set; }

	public string DescriptionVn { get; set; }

	public Ref<Text> DescriptionText { get; set; }
	#endregion
}