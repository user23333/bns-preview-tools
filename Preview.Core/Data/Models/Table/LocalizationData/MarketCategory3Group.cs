using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class MarketCategory3Group : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public bool Visible { get; set; }

	[Name("market-category-3")]
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

	#region Methods
	private HashSet<MarketCategory3Seq> _marketCategory3Hash;
	private HashSet<MarketCategory3Seq> MarketCategory3Hash => _marketCategory3Hash ??= [.. MarketCategory3.Where(x => x > MarketCategory3Seq.None && x < MarketCategory3Seq.COUNT)];

	public bool Filter(Record record)
	{
		if (!MarketCategory3Hash.Contains(record.Attributes.Get<MarketCategory3Seq>("market-category-3"))) return false;
		if (AddtionalFilteringType != AddtionalFilteringTypeSeq.None)
		{
			var type = record.Attributes.Get<SkillTrainByItem>("skill-train-by-item-for-transmit")?.ItemEquipType ?? EquipType.None;
			return AddtionalFilteringType switch
			{
				AddtionalFilteringTypeSeq.SkillTrainByItemEquipTypeNeck => type == EquipType.Necklace,
				AddtionalFilteringTypeSeq.SkillTrainByItemEquipTypeFinger => type == EquipType.Ring,
				AddtionalFilteringTypeSeq.SkillTrainByItemEquipTypeEar => type == EquipType.Earring,
				AddtionalFilteringTypeSeq.SkillTrainByItemEquipTypeWrist => type == EquipType.Bracelet,
				AddtionalFilteringTypeSeq.SkillTrainByItemEquipTypeWaist => type == EquipType.Belt,
				AddtionalFilteringTypeSeq.SkillTrainByItemEquipTypeGloves => type == EquipType.Gloves,
				_ => true,
			};
		}

		return true;
	}
	#endregion
}