using System.Collections;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.Data.Models;
public sealed class SkillBuildUpGroup : ModelElement, IEnumerable
{
	#region Attributes
	public long[] SkillBuildUpSkill { get; set; }

	public sbyte SkillBuildUpSkillTotalCount { get; set; }

	public short[] SkillBuildUpSkillLevelMin { get; set; }

	public short[] SkillBuildUpSkillLevelMax { get; set; }

	public Ref<RandomDistribution>[] SkillBuildUpSkillLevelDistribution { get; set; }
	#endregion

	#region Methods
	public IEnumerator GetEnumerator()
	{
		var result = new List<Tuple<string, string>>();
		var table = FileCache.Data.Provider.GetTable<Skill3>();

		for (int i = 0; i < SkillBuildUpSkillTotalCount; i++)
		{
			var skill = table[SkillBuildUpSkill[i] + ((long)1 << 32)];
			var min = SkillBuildUpSkillLevelMin[i];
			var max = SkillBuildUpSkillLevelMax[i];

			var text = min != max ?
				"UI.ItemRandomOption.SkillEnhancement.Probability.Title".GetText([skill?.Name, min, max]) :
				"UI.ItemRandomOption.SkillEnhancement.Probability.Title.OnlyOne".GetText([skill?.Name, min]);

			result.Add(new(text, null));
		}

		return result.GetEnumerator();
	}
	#endregion
}