namespace Xylia.Preview.Data.Models;
public class ItemRandomOptionGroup : ModelElement
{
	#region Attributes
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
	#endregion
}