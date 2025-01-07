using System.Collections;

namespace Xylia.Preview.Data.Models;
public sealed class SkillBuildUpGroupList : ModelElement, IEnumerable
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<SkillBuildUpGroup>[] SkillBuildUpGroup { get; set; }

	public short[] SkillBuildUpGroupWeight { get; set; }

	public int SkillBuildUpGroupTotalWeight { get; set; }

	public sbyte SkillBuildUpGroupTotalCount { get; set; }

	public bool DrawEnable { get; set; }
	#endregion

	#region Methods
	public IEnumerator GetEnumerator()
	{
		for (int i = 0; i < SkillBuildUpGroupTotalCount; i++)
		{
			var group = SkillBuildUpGroup[i].Value;
			var weight = SkillBuildUpGroupWeight[i];

			if (group is null) continue;

			foreach (var item in group)
			{
				var w = (double)weight / SkillBuildUpGroupTotalWeight / group.SkillBuildUpSkillTotalCount;
				yield return new Tuple<string, double>(item , w);
			}
		}

		yield break;
	}
	#endregion
}		