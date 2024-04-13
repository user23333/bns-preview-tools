using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class ItemRandomOptionGroup : ModelElement
{
	#region Attributes
	public int Id { get; set; }
	public JobSeq Job { get; set; }

	public Ref<SkillTrainByItemList>[] SkillTrainByItemList { get; set; }
	public sbyte SkillTrainByItemListTotalCount { get; set; }
	public sbyte SkillTrainByItemListSelectMin { get; set; }
	public sbyte SkillTrainByItemListSelectMax { get; set; }
	public Ref<Text> SkillTrainByItemListTitle { get; set; }
	#endregion
}