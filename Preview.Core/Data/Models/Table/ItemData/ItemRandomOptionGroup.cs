using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class ItemRandomOptionGroup : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public JobSeq Job { get; set; }

	public string Alias { get; set; }

	public Ref<EffectList> EffectList { get; set; }

	public Ref<AbilityList>[] AbilityList { get; set; }

	public sbyte AbilityListTotalCount { get; set; }

	public Ref<SkillBuildUpGroupList>[] SkillBuildUpGroupList { get; set; }

	public sbyte SkillBuildUpGroupListTotalCount { get; set; }

	public Ref<SkillTrainByItemList>[] SkillTrainByItemList { get; set; }

	public sbyte SkillTrainByItemListTotalCount { get; set; }

	public sbyte SkillTrainByItemListSelectMin { get; set; }

	public sbyte SkillTrainByItemListSelectMax { get; set; }

	public Ref<Text> SkillTrainByItemListTitle { get; set; }

	public bool DuplicationEnable { get; set; }

	public bool UnlimitedDraw { get; set; }

	public sbyte DrawEnableCount { get; set; }

	public int[] DrawCostMoney { get; set; }

	public sbyte DrawCostTotalCount { get; set; }

	public Ref<Item>[] DrawCostMainItem { get; set; }

	public short[] DrawCostMainItemCount { get; set; }

	[Name("draw-cost-sub-item-1")]
	public Ref<Item>[] DrawCostSubItem1 { get; set; }

	[Name("draw-cost-sub-item-count-1")]
	public short[] DrawCostSubItemCount1 { get; set; }

	[Name("draw-cost-sub-item-2")]
	public Ref<Item>[] DrawCostSubItem2 { get; set; }

	[Name("draw-cost-sub-item-count-2")]
	public short[] DrawCostSubItemCount2 { get; set; }

	[Name("draw-cost-sub-item-3")]
	public Ref<Item>[] DrawCostSubItem3 { get; set; }

	[Name("draw-cost-sub-item-count-3")]
	public short[] DrawCostSubItemCount3 { get; set; }

	[Name("draw-cost-sub-item-4")]
	public Ref<Item>[] DrawCostSubItem4 { get; set; }

	[Name("draw-cost-sub-item-count-4")]
	public short[] DrawCostSubItemCount4 { get; set; }
	#endregion
}