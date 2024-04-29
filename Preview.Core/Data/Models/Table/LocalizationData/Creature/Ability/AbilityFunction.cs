namespace Xylia.Preview.Data.Models.Creature;
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
    public double μ;
    public double Φ;
    public List<LevelFactor> LevelFactors = [];
    #endregion

    #region Methods
    public override string ToString() => this.Type.ToString();

    public double GetPercent(double value, sbyte level)
    {
        double factor = 0;

        try
        {
            factor = GetFactor(level);
        }
        catch
        {
            var o = LevelFactors.Find(f => f.Level == level);
            if (o is null) return double.NaN;

            factor = o.Value;
        }

        return GetPercent(value, factor);
    }

    internal double GetPercent(double value, double factor)
    {
        double percent = (double)value * (0.01 * K) / (value + factor);
        return percent + 0.01 * C;
    }
    #endregion

    #region Factor
    /// <summary>
    ///  Get factor by level
    /// </summary>
    public double GetFactor(sbyte level)
    {
        if (μ == 0) throw new ArgumentException(nameof(μ));
        else if (Φ == 0) throw new ArgumentException(nameof(Φ));

        return μ * Math.Exp(Φ * level);
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


    #region Instance
    public static AbilityFunction AttackHit => new()
    {
        Type = CreatureField.AttackHitBasePercent,
        C = 85,
        K = 96,
        LevelFactors = [new(60, 6081.99)]
    };

    public static AbilityFunction AttackPierce => new()
    {
        Type = CreatureField.AttackPierceBasePercent,
        K = 95,
        μ = 87.7627795879303,
        Φ = 0.0796897978783624,
    };

    public static AbilityFunction AttackParryPierce => new()
    {
        Type = CreatureField.AttackParryPiercePercent,
        K = 97,
        LevelFactors = [new(45, 6392.52), new(60, 20963.86)]
    };

    public static AbilityFunction AttackCritical => new()
    {
        Type = CreatureField.AttackCriticalBasePercent,
        K = 97,
        LevelFactors = [new(60, 7937.55)]
    };

    public static AbilityFunction AttackCriticalDamage => new()
    {
        Type = CreatureField.AttackCriticalDamagePercent,
        C = 125,
        K = 291,
        LevelFactors = [new(60, 7201.28)]
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
        LevelFactors = [new(60, 10464.33)]
    };

    public static AbilityFunction DefendParry => new()
    {
        Type = CreatureField.DefendParryBasePercent,
        K = 97,
        LevelFactors = [new(60, 5239.02)]
    };

    public static AbilityFunction DefendParryReducePercent => new()
    {
        //格挡伤害减免
        Type = CreatureField.DefendParryReducePercent,
        C = 30,
        K = 98,
        LevelFactors = [new(60, 21701.77)]
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

    public static AbilityFunction DefendCounterReducePercent => new()
    {
        Type = CreatureField.DefendCounterReducePercent,
    };

    //反击武功强化
    //public static AbilityFunction CounterEnhance => new()
    //{
    //    Type = CreatureField.CounterEnhance,
    //    K = 285,
    //    LevelFactors = [new(60, 9835.18)]
    //};

    //防御武功 强化
    //public static AbilityFunction DefenceParryDamageReducePercent => new()
    //{
    //    Type = CreatureField.DefenceParryDamageReduce,
    //    K = 291,
    //    LevelFactors = [new(60, 10042.45)]
    //};



    public static AbilityFunction HealPower => new()
    {
        Type = CreatureField.HealPowerBasePercent,
        C = 100,
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
        C = 100,
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
        C = 100,
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
    #endregion
}