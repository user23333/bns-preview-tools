using System.Collections;

namespace Xylia.Preview.Data.Models;
public sealed class SkillBuildUpGroup : ModelElement, IEnumerable<string>
{
	#region Attributes
	public Ref<SkillBuildUp>[] SkillBuildUpSkill { get; set; }

	public sbyte SkillBuildUpSkillTotalCount { get; set; }

	public short[] SkillBuildUpSkillLevelMin { get; set; }

	public short[] SkillBuildUpSkillLevelMax { get; set; }

	public Ref<RandomDistribution>[] SkillBuildUpSkillLevelDistribution { get; set; }
	#endregion

	#region Methods
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public IEnumerator<string> GetEnumerator()
	{
		var table = Provider.GetTable<Skill3>();

		for (int i = 0; i < SkillBuildUpSkillTotalCount; i++)
		{
			var build = SkillBuildUpSkill[i].Value;
			if (build is null) continue;

			var min = SkillBuildUpSkillLevelMin[i];
			var max = SkillBuildUpSkillLevelMax[i];
			var skill = table[build.ParentSkill3Id + ((long)1 << 32)];

			var text = min != max ?
				"UI.ItemRandomOption.SkillEnhancement.Probability.Title".GetText([skill?.Name, min, max]) :
				"UI.ItemRandomOption.SkillEnhancement.Probability.Title.OnlyOne".GetText([skill?.Name, min]);

			yield return text;
		}

		yield break;
	}
	#endregion
}