namespace Xylia.Preview.Data.Models;
public sealed class ExtractSkillTrainByItem : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public Ref<SkillTrainByItem> SkillTrainByItem { get; set; }

	public Ref<CostGroup>[] SkillTrainByItemExtractCostGroup { get; set; }

	public Ref<Item> ExtractSkillTrainByitem { get; set; }
	#endregion
}