using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class MarketCategory3Group : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public bool Visible { get; set; }

	public MarketCategory3Seq[] MarketCategory3 { get; set; }

	public AddtionalFilteringTypeSeq AddtionalFilteringType { get; set; }

	public enum AddtionalFilteringTypeSeq
	{
		None,
		SkillTrainByItemEquipTypeNeck,
		SkillTrainByItemEquipTypeFinger,
		SkillTrainByItemEquipTypeEar,
		SkillTrainByItemEquipTypeWrist,
		SkillTrainByItemEquipTypeWaist,
		SkillTrainByItemEquipTypeGloves,
		COUNT
	}

	public Ref<Text> Name2 { get; set; }
	#endregion
}