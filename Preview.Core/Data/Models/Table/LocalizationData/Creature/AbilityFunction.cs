using Xylia.Preview.Common;

namespace Xylia.Preview.Data.Models;
public class AbilityFunction
{
	#region Fields
	public CreatureField Type;

	/// <summary>
	/// Change rate of percent
	/// </summary>
	public int K;

	/// <summary>
	/// Constant percent value
	/// </summary>
	public int C;

	// level factor param
	public double μ = double.NaN;
	public double Φ = double.NaN;
	public List<LevelFactor> LevelFactors = [];
	#endregion

	#region Factor
	/// <summary>
	///  Get factor by level
	/// </summary>
	public double GetFactor(sbyte level)
	{
		var f = μ * Math.Exp(Φ * level);
		if (!double.IsNaN(f)) return f;

		return LevelFactors.Find(f => f.Level == level)?.Value ?? double.NaN;
	}

	/// <summary>
	/// Get factor by value
	/// </summary>
	/// <param name="value"></param>
	/// <param name="percent"></param>
	/// <returns></returns>
	public double GetFactor(double value, double percent)
	{
		if (K == 0) throw new ArgumentException(nameof(K));

		return value * K / (percent - C) - value;
	}

	/// <summary>
	/// Get params by two level factor
	/// </summary>
	public void GetFactorParam(LevelFactor factor1, LevelFactor factor2)
	{
		Φ = factor1.CalΦ(factor2);
		μ = factor1.Calμ(Φ);
	}

	public class LevelFactor(sbyte level, double value)
	{
		public sbyte Level = level;

		public double Value = value;

		public double CalΦ(LevelFactor factor2) => Math.Log(Value / factor2.Value) / (Level - factor2.Level);

		public double Calμ(double Φ) => Value / Math.Exp(Φ * Level);
	}
	#endregion

	#region Methods		
	public bool IsValid => K > 0 && Type != CreatureField.Creature_field_none;

	public string Text => Globals.TextProvider.Get($"AbilityName.{Type}");

	public double GetPercent(double value, sbyte level, int? basepercent = null)
	{
		return GetPercent(value, GetFactor(level), basepercent);
	}

	internal double GetPercent(double value, double factor, int? basepercent)
	{
		return 0.001 * (basepercent ?? C) +
			0.01 * value * K / (value + factor);
	}
	#endregion


	#region Instance
	public static AbilityFunction AttackHit => new()
	{
		Type = CreatureField.AttackHitBasePercent,
		C = 850,
		K = 96,
		LevelFactors =
		[
			new(60, 6081.99)
		]
	};

	public static AbilityFunction AttackPierce => new()
	{
		Type = CreatureField.AttackPierceBasePercent,
		K = 95,
		LevelFactors =
		[
			new(45, 3120.8726144142975),
			new(46, 3389.7093900796485)
		]
	};

	public static AbilityFunction AttackParryPierce => new()
	{
		Type = CreatureField.AttackParryPiercePercent,
		K = 97,
		LevelFactors =
		[
			new(45, 6394.600774438795),
			new(46, 6945.536728505597),
			new(60, 20963.86)
		]
	};

	public static AbilityFunction AttackCritical => new()
	{
		Type = CreatureField.AttackCriticalBasePercent,
		K = 97,
		LevelFactors =
		[
			new(45, 2366.906630984764),
			new(46, 2570.5592931167057),
			new(60, 7937.55)
		]
	};

	public static AbilityFunction AttackCriticalDamage => new()
	{
		Type = CreatureField.AttackCriticalDamagePercent,
		C = 1250,
		K = 291,
		LevelFactors =
		[
			new(45, 3337.8069282316364),
			new(46, 3513.3636567558337),
			new(60, 7204.277159395931)
		]
	};


	public static AbilityFunction DefendPower => new()
	{
		Type = CreatureField.DefendPowerCreatureValue,
		K = 95,
		LevelFactors =
		[
			new(45, 3121.455509836698),
			new(46, 3389.733541564487),
		]
	};

	public static AbilityFunction DefendCritical => new()
	{
		Type = CreatureField.DefendCriticalBasePercent,
		K = 97,
		LevelFactors = [new(45, 1891.2283)]
	};

	public static AbilityFunction DefendCriticalDamage => new()
	{
		Type = CreatureField.DefendCriticalDamagePercent,
		K = 291,
		LevelFactors = [new(60, 2374.28)]
	};

	public static AbilityFunction DefendBounce => new()
	{
		Type = CreatureField.DefendBouncePercent,
	};

	public static AbilityFunction DefendDodge => new()
	{
		Type = CreatureField.DefendDodgeBasePercent,
		K = 95,
		LevelFactors =
		[
			new(45, 3121.196188691034),
			new(50, 4713.973699811077),
			new(60, 10464.33)
		]
	};

	public static AbilityFunction DefendCounterEnhance => new()
	{
		//Type = CreatureField.CounterEnhance,
		K = 285,
		LevelFactors =
		[
			new(45, 2842.410631019564),
			new(50, 4363.411140868513),
			new(60, 9835.18)
		]
	};


	public static AbilityFunction DefendParry => new()
	{
		Type = CreatureField.DefendParryBasePercent,
		K = 97,
		LevelFactors =
		[
			new(60, 5239.02)
		]
	};

	public static AbilityFunction DefendParryReducePercent => new()
	{
		Type = CreatureField.DefendParryReducePercent,
		C = 300,
		K = 98,
		LevelFactors =
		[
			new(60, 21701.77)
		]
	};

	public static AbilityFunction DefendPerfectParry => new()
	{
		Type = CreatureField.DefendPerfectParryBasePercent,
	};

	public static AbilityFunction DefendImmune => new()
	{
		Type = CreatureField.DefendImmuneBasePercent,
	};

	public static AbilityFunction DefendMiss => new()
	{
		Type = CreatureField.DefendMissBasePercent,
	};

	public static AbilityFunction DefendPerfectParryReducePercent => new()
	{
		Type = CreatureField.DefendPerfectParryReducePercent,
	};


	//public static AbilityFunction DefenceParryDamageReducePercent => new()
	//{
	//    Type = CreatureField.DefenceParryDamageReduce,
	//    K = 291,
	//    LevelFactors = [new(60, 10042.45)]
	//};


	public static AbilityFunction HealPower => new()
	{
		Type = CreatureField.HealPowerBasePercent,
		C = 1000,
		K = 54,
		LevelFactors = [new(60, 2796.48)]
	};

	public static AbilityFunction AoeDefend => new()
	{
		Type = CreatureField.AoeDefendBasePercent,
	};

	public static AbilityFunction AbnormalAttack => new()
	{
		Type = CreatureField.AbnormalAttackBasePercent,
		C = 1000,
		K = 291,
		LevelFactors = [new(60, 12744.27)]
	};

	public static AbilityFunction AbnormalDefend => new()
	{
		Type = CreatureField.AbnormalDefendBasePercent,
	};

	public static AbilityFunction Hate => new()
	{
		Type = CreatureField.HateBasePercent,
	};

	public static AbilityFunction AttackAttribute => new()
	{
		Type = CreatureField.AttackAttributeBasePercent,
		C = 1000,
		K = 291,
		LevelFactors = [new(60, 12002.79)]
	};

	public static AbilityFunction AttackAbnormalHit => new()
	{
		Type = CreatureField.AttackAbnormalHitBasePercent,
	};

	public static AbilityFunction DefendAbnormalDodge => new()
	{
		Type = CreatureField.DefendAbnormalDodgeBasePercent,
	};

	public static AbilityFunction SupportPower => new()
	{
		Type = CreatureField.SupportPowerBasePercent,
	};



	public static AbilityFunction CastDuration => new()
	{
		Type = CreatureField.CastDurationBasePercent,
	};

	public static AbilityFunction AttackStiffDuration => new()
	{
		Type = CreatureField.AttackStiffDurationBasePercent,
	};

	public static AbilityFunction DefendStiffDuration => new()
	{
		Type = CreatureField.DefendStiffDurationBasePercent,
	};
	#endregion
}