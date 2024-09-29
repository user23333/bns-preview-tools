namespace Xylia.Preview.Data.Models;
public sealed class SoulBoostMissionTask : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public string Name { get; set; }

	public string NameEn { get; set; }

	public string NameFr { get; set; }

	public string NameDe { get; set; }

	public string NamePt { get; set; }

	public string NameTh { get; set; }

	public string NameVn { get; set; }

	public Ref<Text> NameText { get; set; }

	public Ref<SoulBoostEvent> Event { get; set; }

	public Ref<SoulBoostMissionStep> MissionStep { get; set; }

	public sbyte TaskNumber { get; set; }

	public Ref<SoulBoostMission> Mission { get; set; }

	public ProceedableTypeSeq ProceedableType { get; set; }

	public enum ProceedableTypeSeq
	{
		None,
		BeforeOpen,
		AfterOpen,
		COUNT
	}

	public ActorTypeSeq ActorType { get; set; }

	public enum ActorTypeSeq
	{
		None,
		Character,
		Account,
		COUNT
	}

	public long GoalCount { get; set; }

	public int MissionPoint { get; set; }

	public sbyte MissionLevel { get; set; }

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