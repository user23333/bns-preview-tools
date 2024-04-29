using System.Collections;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.Data.Models;
public sealed class SkillBuildUpGroup : ModelElement
{
	#region Attributes
	public long[] SkillBuildUpSkill { get; set; }

	public sbyte SkillBuildUpSkillTotalCount { get; set; }

	public short[] SkillBuildUpSkillLevelMin { get; set; }

	public short[] SkillBuildUpSkillLevelMax { get; set; }

	public Ref<RandomDistribution>[] SkillBuildUpSkillLevelDistribution { get; set; }
	#endregion

	#region Methods
	public IEnumerable GetSkills()
	{
		var result = new List<Tuple<string, string>>();
		var table = FileCache.Data.Provider.GetTable<Skill3>();

		for (int i = 0; i < SkillBuildUpSkillTotalCount; i++)
		{
			// UI.ItemTooltip.SkillBuildUpLevel.Penalty
			// UI.ItemTooltip.SkillBuildUpLevel.Disable
			var skill = table[SkillBuildUpSkill[i] + ((long)1 << 32)];
			var text = "UI.ItemTooltip.SkillBuildUpLevel.Enahnce".GetText()
				.Replace("<arg p=\"3:integer\"/>", "<arg p=\"3:integer\"/>-<arg p=\"4:integer\"/>")
				.Replace([null, skill?.Name, SkillBuildUpSkillLevelMin[i], SkillBuildUpSkillLevelMax[i]]);

			result.Add(new(text, null));
		}

		return result;
	}
	#endregion
}