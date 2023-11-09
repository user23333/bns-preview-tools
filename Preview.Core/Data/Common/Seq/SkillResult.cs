﻿namespace Xylia.Preview.Data.Common.Seq;
public enum SkillResult
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
}

public enum SkillEventType
{
	None,
	Attack,
	Attacked,
}