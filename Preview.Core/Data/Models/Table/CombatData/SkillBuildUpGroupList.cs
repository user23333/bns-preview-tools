namespace Xylia.Preview.Data.Models;
public sealed class SkillBuildUpGroupList : ModelElement
{
	#region Attributes
	public Ref<SkillBuildUpGroup>[] SkillBuildUpGroup { get; set; }

	public short[] SkillBuildUpGroupWeight { get; set; }

	public int SkillBuildUpGroupTotalWeight { get; set; }

	public sbyte SkillBuildUpGroupTotalCount { get; set; }
	#endregion
}