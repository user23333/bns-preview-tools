namespace Xylia.Preview.Data.Models.Sequence;
public enum ConsumeTypeSeq
{
	Point,
	PointBelow,
	PointAbove,
	BaseMaxPercent,
	TotalMaxPercent,
	CurrentPercent,
	COUNT
}

public enum SkillResultSeq
{
	None,
	Hit,
	Miss,
	Dodge,
	Parry,
	PerfectParry,
	Bounce,
	Counter,
	CriticalHit,
	HitCriticalHit,
	BackHitCriticalHit,
	NotHit,
	All,
	HitCriticalHitParry,
	ParryPerfectParry,
	FrontHitCriticalHit,
	ParryPerfectParryCounter,
	ParryPerfectParryDodge,
	ParryDodge,
	COUNT
}

public enum SkillEventTypeSeq
{
	None,
	Attack,
	Attacked,
	COUNT
}

public enum SkillTypeSeq
{
	Caster,
	Target,
	Ground,
	Chain,
	NoneTarget,
	Summon,
	BossRush,
	BossMultiground,
	BossLinklaser,
	MakeCampfire,
	Succession,
	GroundSuccession,
	DuelTag,
	DuelInterference,
	COUNT
}