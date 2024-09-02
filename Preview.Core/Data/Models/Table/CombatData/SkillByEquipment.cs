namespace Xylia.Preview.Data.Models;
public sealed class SkillByEquipment : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public int[] Skill3Id { get; set; }

	public int[] BlockSkill3Id { get; set; }

	public int[] SkillTreeId { get; set; }

	public Ref<ContextScript>[] ContextScript { get; set; }

	public Ref<Text>[] TooltipText { get; set; }

	public int[] SkillAttackPowerMinParam1Slot { get; set; }

	public int[] SkillAttackPowerMaxParam1Slot { get; set; }

	public int[] SkillAttackPowerMinParam2Slot { get; set; }

	public int[] SkillAttackPowerMaxParam2Slot { get; set; }

	public int[] SkillAttackPowerMinParam3Slot { get; set; }

	public int[] SkillAttackPowerMaxParam3Slot { get; set; }

	public int[] SkillAttackPowerMinParam4Slot { get; set; }

	public int[] SkillAttackPowerMaxParam4Slot { get; set; }
	#endregion

	#region Methods
	public string GetTooltipText(int index) => TooltipText[index].GetText(
	[
		null, null,
		SkillAttackPowerMinParam1Slot[index], SkillAttackPowerMaxParam1Slot[index],
		SkillAttackPowerMinParam2Slot[index], SkillAttackPowerMaxParam2Slot[index],
		SkillAttackPowerMinParam3Slot[index], SkillAttackPowerMaxParam3Slot[index],
		SkillAttackPowerMinParam4Slot[index], SkillAttackPowerMaxParam4Slot[index],
	]);
	#endregion
}