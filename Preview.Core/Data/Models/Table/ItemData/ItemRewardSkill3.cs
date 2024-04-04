using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class ItemRewardSkill3 : ModelElement
{
	#region Attributes
	public JobSeq Job { get; set; }
	public long HeadSkillId { get; set; }
	public long[] ChangeSkillId { get; set; }
	public Ref<ItemRewardSkillAcquireRoute> HeadSkillAcquireRoute { get; set; }
	public Ref<ItemRewardSkillAcquireRoute>[] ChangeAcquireRoute { get; set; }
	#endregion
}