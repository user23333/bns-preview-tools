namespace Xylia.Preview.Data.Models;
public sealed class SkillBuildUp : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public int ParentSkill3Id { get; set; }

	public sbyte MaxLevel { get; set; }

	public sbyte[] RequiredBuildUpPoint { get; set; }

	public short[] RequiredBuildUpPointLevel { get; set; }

	public Ref<SkillModifyInfo>[] SkillModifyInfo { get; set; }
	#endregion
}