namespace Xylia.Preview.Data.Models;
public class SkillSystematization : ModelElement
{
	#region Attributes
	public SystematizationSeq Systematization { get; set; }
	public enum SystematizationSeq
	{
		None,
		Kneel,
		Stun,
		Knockback,
		Down,
		Catch,
		Mount,
		Inhalation,
		Speeddown,
		Midair,
		Freezing,
		Stiff,
		Pull,
		InternalInjury,
		Flash,
		BlockMove,
		Bleeding,
		Swoon,
		ImmuneProjectile,
		ImmuneCC,
		ImmuneDamage,
		PartyBuff,
		PartyProtect,
		PerfectParry,
		Counter,
		Bounce,
		Protect,
		SpHeal,
		HpHeal,
		AttackdefenceBlock,
		DefenceBlock,
		DashBlock,
		Escape,
		Speedup,
		Hyper,
		Tumbling,
		Dash,
		Soulmask,
		Provocation,
		Hate,
		Passive,
		Summoned,
		Projectile,
		DefencePierce,
		BouncePierce,
		DefenseCrush,
		Union,
		Link,
		Defence,
		Attackdefence,
		SingleTarget,
		AoeTarget,
		TeamBuff,
		TeamProtect,

		COUNT
	}

	public string Name { get; set; }

	public Ref<Text> Description { get; set; }

	public sbyte SortNo { get; set; }

	public Ref<SkillSystematizationGroup> Group { get; set; }
	#endregion


	#region Methods
	public string Name2 => this.Attributes.Get<Record>("name2").GetText() + " ";
	#endregion
}