using System.Collections;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.Data.Models;
public sealed class SkillBuildUpGroupList : ModelElement, IEnumerable
{
	#region Attributes
	public Ref<SkillBuildUpGroup>[] SkillBuildUpGroup { get; set; }

	public short[] SkillBuildUpGroupWeight { get; set; }

	public int SkillBuildUpGroupTotalWeight { get; set; }

	public sbyte SkillBuildUpGroupTotalCount { get; set; }
	#endregion

	#region Methods
	public IEnumerator GetEnumerator()
	{
		foreach (var SkillBuildUpGroup in SkillBuildUpGroup.SelectNotNull(x => x.Instance))
		{
			foreach (var skill in SkillBuildUpGroup)
			{
				yield return skill;
			}

		}

		yield break;
	}
	#endregion
}