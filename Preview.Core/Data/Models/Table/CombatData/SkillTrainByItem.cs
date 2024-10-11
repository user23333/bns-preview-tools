using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class SkillTrainByItem : ModelElement
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public Ref<Skill3> MainOriginSkill { get; set; }

	public Ref<Skill3> MainChangeSkill { get; set; }

	public Ref<Skill3>[] SubOriginSkill { get; set; }

	public Ref<Skill3>[] SubChangeSkill { get; set; }

	public Icon Icon { get; set; }

	public Ref<Text> Description { get; set; }

	public EquipType ItemEquipType { get; set; }

	public JobSeq Job { get; set; }

	public Ref<ExtractSkillTrainByItem> ExtractSkillTrainByItem { get; set; }
	#endregion


	#region Methods
	public string Description2 => Description.GetText();
	#endregion
}