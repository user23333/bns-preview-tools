using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class FeedbackSkillScore : ModelElement 
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<Skill3> Skill { get; set; }

	public SkillResult[] SkillResult { get; set; }
	#endregion
}