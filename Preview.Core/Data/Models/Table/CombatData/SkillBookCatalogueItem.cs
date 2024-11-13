using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class SkillBookCatalogueItem : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public Ref<Skill3> ParentSkill { get; set; }

	public Ref<Skill3> BaseSkill { get; set; }

	public Ref<Skill3>[] ChangeSkill { get; set; }

	public sbyte Row { get; set; }

	public sbyte Column { get; set; }

	public JobSeq Job { get; set; }

	public EquipType EquipType { get; set; }

	public sbyte Tier { get; set; }

	public Ref<ItemRewardSkillAcquireRoute> BaseSkillAcquireRoute { get; set; }

	public Ref<ItemRewardSkillAcquireRoute>[] ChangeSkillAcquireRoute { get; set; }
	#endregion
}