using System.Runtime.InteropServices;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;

[StructLayout(LayoutKind.Sequential)]
public class Creature
{
	#region Fields
	public short WorldId;
	public RaceSeq Race;
	public SexSeq Sex;
	public JobSeq Job;
	public sbyte[] Appearance = new sbyte[96];
	public string Name;

	public short GeoZone;
	public int X;
	public int Y;
	public int Z;
	public sbyte Yaw;

	public sbyte Level;
	public int Exp;
	public sbyte MasteryLevel;
	public long MasteryExp;

	public long Hp { get; set; }
	public int GuardGauge { get; set; }
	public int Money { get; set; }
	public int MoneyDiff { get; set; }


	public long MaxHp { get; set; }
	public int MaxHpEquip { get; set; }
	public int MaxGuardGauge { get; set; }
	public int MaxGuardGaugeEquip { get; set; }
	public short MaxSp { get; set; }
	public short MaxSp2 { get; set; }
	public Velocity Speed { get; set; }
	public Velocity VehicleSpeed { get; set; }
	public short ModifyCastSpeedPercent { get; set; }
	public long HpRegen { get; set; }
	public int HpRegenEquip { get; set; }
	public int HpRegenCombat { get; set; }
	public int HpRegenCombatEquip { get; set; }
	public short AttackHitBasePercent { get; set; }
	public short AttackHitValue { get; set; }
	public short AttackHitValueEquip { get; set; }
	public short AttackPierceBasePercent { get; set; }
	public short AttackParryPiercePercent { get; set; }
	public short AttackPierceValue { get; set; }
	public short AttackPierceValueEquip { get; set; }
	public short AttackCriticalBasePercent { get; set; }
	public short AttackCriticalDamagePercent { get; set; }
	public int AttackCriticalValue { get; set; }
	public short AttackCriticalValueEquip { get; set; }
	public int AttackCriticalDamageValue { get; set; }
	public short AttackCriticalDamageValueEquip { get; set; }
	public short DefendCriticalBasePercent { get; set; }
	public short DefendCriticalDamagePercent { get; set; }
	public int DefendCriticalValue { get; set; }
	public short DefendCriticalValueEquip { get; set; }
	public short DefendBouncePercent { get; set; }
	public short DefendDodgeBasePercent { get; set; }
	public short DefendDodgeValue { get; set; }
	public short DefendDodgeValueEquip { get; set; }
	public short DefendParryBasePercent { get; set; }
	public short DefendParryValue { get; set; }
	public short DefendParryValueEquip { get; set; }
	public short DefendParryReducePercent { get; set; }
	public short DefendParryReduceDiff { get; set; }
	public short DefendPerfectParryBasePercent { get; set; }
	public short DefendImmuneBasePercent { get; set; }
	public int AttackPowerCreatureMin { get; set; }
	public int AttackPowerCreatureMax { get; set; }
	public short AttackPowerEquipMin { get; set; }
	public short AttackPowerEquipMax { get; set; }
	public int DefendPowerCreatureValue { get; set; }
	public int DefendPowerEquipValue { get; set; }
	public short DefendResistPowerCreatureValue { get; set; }
	public short DefendResistPowerEquipValue { get; set; }
	public short DefendPhysicalDamageReducePercent { get; set; }
	public short DefendForceDamageReducePercent { get; set; }
	public short AttackDamageModifyPercent { get; set; }
	public int AttackDamageModifyDiff { get; set; }
	public short DefendDamageModifyPercent { get; set; }
	public int DefendDamageModifyDiff { get; set; }
	public short DefendMissBasePercent { get; set; }
	public short AttackStiffDurationBasePercent { get; set; }
	public short AttackStiffDurationValue { get; set; }
	public short DefendStiffDurationBasePercent { get; set; }
	public short DefendStiffDurationValue { get; set; }
	public short CastDurationBasePercent { get; set; }
	public short CastDurationValue { get; set; }
	public short AttackConcentrateValue { get; set; }
	public short AttackConcentrateValueEquip { get; set; }
	public short DefendPerfectParryReducePercent { get; set; }
	public short DefendCounterReducePercent { get; set; }
	public int PveBossLevelNpcAttackPowerCreatureMin { get; set; }
	public int PveBossLevelNpcAttackPowerCreatureMax { get; set; }
	public short PveBossLevelNpcAttackPowerEquipMin { get; set; }
	public short PveBossLevelNpcAttackPowerEquipMax { get; set; }
	public int PveBossLevelNpcDefendPowerCreatureValue { get; set; }
	public int PveBossLevelNpcDefendPowerEquipValue { get; set; }
	public int PvpAttackPowerCreatureMin { get; set; }
	public int PvpAttackPowerCreatureMax { get; set; }
	public short PvpAttackPowerEquipMin { get; set; }
	public short PvpAttackPowerEquipMax { get; set; }
	public int PvpDefendPowerCreatureValue { get; set; }
	public int PvpDefendPowerEquipValue { get; set; }
	public int JobAbility1 { get; set; }
	public int JobAbility2 { get; set; }
	public short HealPowerBasePercent { get; set; }
	public int HealPowerValue { get; set; }
	public int HealPowerDiff { get; set; }
	public short AoeDefendBasePercent { get; set; }
	public short AoeDefendPowerValue { get; set; }
	public short AbnormalAttackBasePercent { get; set; }
	public int AbnormalAttackPowerValue { get; set; }
	public int AbnormalAttackPowerValueEquip { get; set; }
	public short AbnormalDefendBasePercent { get; set; }
	public short AbnormalDefendPowerValue { get; set; }
	public short HateBasePercent { get; set; }
	public short HatePowerCreatureValue { get; set; }
	public short HatePowerEquipValue { get; set; }
	public short AdditionalExpDiffByKill { get; set; }
	public short AdditionalExpPercentByKill { get; set; }
	public short AdditionalMasteryExpDiffByKill { get; set; }
	public short AdditionalMasteryExpPercentByKill { get; set; }
	public short AdditionalFactionScoreMaxPercent { get; set; }
	public short AdditionalSealedDungeonExpDiffByKill { get; set; }
	public short AdditionalSealedDungeonExpPercentByKill { get; set; }
	public int AttackAttributeValue { get; set; }
	public int AttackAttributeValueEquip { get; set; }
	public short AttackAttributeBasePercent { get; set; }
	public short DefendDifficultyTypeDamageReducePercent { get; set; }
	public short RaceType1AttackDamageModifyPercent { get; set; }
	public short RaceType2AttackDamageModifyPercent { get; set; }
	public short RaceType3AttackDamageModifyPercent { get; set; }
	public short RaceType4AttackDamageModifyPercent { get; set; }
	public short RaceType5AttackDamageModifyPercent { get; set; }
	public short RaceType6AttackDamageModifyPercent { get; set; }
	public short RaceType7AttackDamageModifyPercent { get; set; }
	public short RaceType1DefendDamageModifyPercent { get; set; }
	public short RaceType2DefendDamageModifyPercent { get; set; }
	public short RaceType3DefendDamageModifyPercent { get; set; }
	public short RaceType4DefendDamageModifyPercent { get; set; }
	public short RaceType5DefendDamageModifyPercent { get; set; }
	public short RaceType6DefendDamageModifyPercent { get; set; }
	public short RaceType7DefendDamageModifyPercent { get; set; }
	public short AttributeType1AttackDamageModifyPercent { get; set; }
	public short AttributeType2AttackDamageModifyPercent { get; set; }
	public short AttributeType3AttackDamageModifyPercent { get; set; }
	public short AttributeType4AttackDamageModifyPercent { get; set; }
	public short AttributeType5AttackDamageModifyPercent { get; set; }
	public short AttributeType6AttackDamageModifyPercent { get; set; }
	public short AttributeType7AttackDamageModifyPercent { get; set; }
	public short AttributeType8AttackDamageModifyPercent { get; set; }
	public short AttributeType9AttackDamageModifyPercent { get; set; }
	public short AttributeType10AttackDamageModifyPercent { get; set; }
	public short AttributeType11AttackDamageModifyPercent { get; set; }
	public short AttributeType12AttackDamageModifyPercent { get; set; }
	public short AttributeType1DefendDamageModifyPercent { get; set; }
	public short AttributeType2DefendDamageModifyPercent { get; set; }
	public short AttributeType3DefendDamageModifyPercent { get; set; }
	public short AttributeType4DefendDamageModifyPercent { get; set; }
	public short AttributeType5DefendDamageModifyPercent { get; set; }
	public short AttributeType6DefendDamageModifyPercent { get; set; }
	public short AttributeType7DefendDamageModifyPercent { get; set; }
	public short AttributeType8DefendDamageModifyPercent { get; set; }
	public short AttributeType9DefendDamageModifyPercent { get; set; }
	public short AttributeType10DefendDamageModifyPercent { get; set; }
	public short AttributeType11DefendDamageModifyPercent { get; set; }
	public short AttributeType12DefendDamageModifyPercent { get; set; }
	public short DefendCriticalDamageValue { get; set; }
	public short DefendCriticalDamageValueEquip { get; set; }

	public short AttackStiffDurationEquipValue { get; set; }
	public short DefendStiffDurationEquipValue { get; set; }
	public short AoeDefendPowerValueEquip { get; set; }
	public short HealPowerEquipValue { get; set; }
	public short DefendStrengthCreatureValue { get; set; }
	public short DefendStrengthEquipValue { get; set; }
	public short AttackPreciseCreatureValue { get; set; }
	public short AttackPreciseEquipValue { get; set; }
	public short AttackAoePierceValue { get; set; }
	public short AttackAoePierceValueEquip { get; set; }
	public short AttackAbnormalHitBasePercent { get; set; }
	public short AttackAbnormalHitValue { get; set; }
	public short AttackAbnormalHitEquipValue { get; set; }
	public short DefendAbnormalDodgeBasePercent { get; set; }
	public short DefendAbnormalDodgeValue { get; set; }
	public short DefendAbnormalDodgeEquipValue { get; set; }
	public short SupportPowerBasePercent { get; set; }
	public short SupportPowerValue { get; set; }
	public short SupportPowerEquipValue { get; set; }
	public short HypermovePowerValue { get; set; }
	public short HypermovePowerEquipValue { get; set; }
	#endregion

	#region Methods
	public void TestMethod()
	{
		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.attack-power".GetText([
			null,
			(AttackPowerCreatureMin + AttackPowerCreatureMax + AttackPowerEquipMin + AttackPowerEquipMax) / 2,
			(AttackPowerCreatureMin + AttackPowerCreatureMax) / 2,
			(AttackPowerEquipMin + AttackPowerEquipMax) / 2,
		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.max-hp".GetText([null, MaxHp + MaxHpEquip, MaxHp, MaxHpEquip]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.attack-pierce".GetText([
			null,
			AttackPierceValue + AttackPierceValueEquip,
			AttackPierceValue,
			AttackPierceValueEquip,
			AbilityFunction.AttackPierce.GetPercent(AttackPierceValue + AttackPierceValueEquip, Level, AttackPierceBasePercent),
			AbilityFunction.AttackParryPierce.GetPercent(AttackPierceValue + AttackPierceValueEquip, Level, AttackParryPiercePercent),
		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.attack-hit".GetText([
			null,
			AttackHitValue + AttackHitValueEquip,
			AttackHitValue,
			AttackHitValueEquip,
			AbilityFunction.AttackHit.GetPercent(AttackHitValue + AttackHitValueEquip, Level, AttackHitBasePercent)
		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.attack-critical".GetText([
			null,
			AttackCriticalValue + AttackCriticalValueEquip,
			AttackCriticalValue,
			AttackCriticalValueEquip,
			AbilityFunction.AttackCritical.GetPercent(AttackCriticalValue + AttackCriticalValueEquip, Level ,AttackCriticalBasePercent),
		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.attack-critical-damage".GetText([
			null,
			AttackCriticalDamageValue + AttackCriticalDamageValueEquip,
			AttackCriticalDamageValue,
			AttackCriticalDamageValueEquip,
			AbilityFunction.AttackCriticalDamage.GetPercent(AttackCriticalDamageValue + AttackCriticalDamageValueEquip, Level ,AttackCriticalDamagePercent),
		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.attack-damage".GetText([
			null,
			AttackDamageModifyDiff,
			AbilityFunction.AttackCritical.GetPercent(AttackDamageModifyDiff, Level , AttackDamageModifyPercent),
		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.attack-concentrate".GetText([
			null,
			AttackConcentrateValue + AttackConcentrateValueEquip,
			AttackConcentrateValue,
			AttackConcentrateValueEquip,
			//
			//
		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.attack-stiff-duration".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.defend-stiff-duration".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.hate".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.defend-power".GetText([
			null,
			DefendPowerCreatureValue + DefendPowerEquipValue,
			DefendPowerCreatureValue,
			DefendPowerEquipValue,
			AbilityFunction.DefendPower.GetPercent(DefendPowerCreatureValue + DefendPowerEquipValue, Level),
			AoeDefendPowerValue	+ AoeDefendPowerValueEquip,
			AbilityFunction.AoeDefend.GetPercent(AoeDefendPowerValue + AoeDefendPowerValueEquip, Level, AoeDefendBasePercent)
		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.defend-dodge".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.defend-parry".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.hp-regen".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.heal".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.defend-damage".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.defend-critical2".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.defend-critical_damage_reduce".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.DefencePowerAoe".GetText([
			null,
			AoeDefendPowerValue + AoeDefendPowerValueEquip,
			AbilityFunction.AoeDefend.GetPercent(AoeDefendPowerValue + AoeDefendPowerValueEquip, Level, AoeDefendBasePercent)
		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.defend-critical".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.abnormal".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.attack-abnormal".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.boss-attack-power".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.boss-defend-power".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.pc-attack-power".GetText([

		]));

		Console.WriteLine("UI.CharacterInfo.Ability.Tooltip.pc-defend-power".GetText([

		]));
	}
	#endregion
}