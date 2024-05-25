using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class ItemCombat : ModelElement
{
	#region Attributes
	public JobStyleSeq JobStyle { get; set; }

	public Ref<ItemSkill>[] ItemSkill { get; set; }

	public Ref<ItemSkill>[] ItemSkillSecond { get; set; }

	public Ref<ItemSkill>[] ItemSkillThird { get; set; }

	public Ref<SkillModifyInfoGroup> SkillModifyInfoGroup { get; set; }
	#endregion

	#region Methods
	public string Description
	{
		get
		{
			Ref<ItemSkill>[] ItemSkills = [.. ItemSkill, .. ItemSkillSecond, .. ItemSkillThird];

			return SkillModifyInfoGroup.Instance?.Description + string.Join("<br/>", ItemSkills.SelectNotNull(x => x.Instance?.Description));
		}
	}
	#endregion
}