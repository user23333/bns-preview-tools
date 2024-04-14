using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public sealed class ItemSkill : ModelElement, IAttraction
{
	#region Attributes
	public int SkillId { get; set; }

	public sbyte[] SkillVariationId { get; set; }

	public bool IncludeInheritanceSkill { get; set; }

	public Ref<Skill3> ItemSimSkill { get; set; }

	public Ref<Text> Name2 { get; set; }

	public Ref<Text> Description2 { get; set; }

	public Ref<Text> ItemSkillTooltip { get; set; }
	#endregion

	#region Methods
	public string Name => Name2.GetText();

	public string Description => Description2.GetText();
	#endregion
}