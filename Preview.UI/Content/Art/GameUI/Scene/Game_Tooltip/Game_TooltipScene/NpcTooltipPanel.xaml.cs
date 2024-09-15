using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
public partial class NpcTooltipPanel
{
	#region Constructors
	public NpcTooltipPanel()
	{
		InitializeComponent();
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		ColumnList.Children.Clear();
		if (e.NewValue is not Npc record) return;

		int row = 0;
		var creature = record.AbilityTest();

		AddProperty(row++, "name", creature.Name);
		AddProperty(row++, "level", "UI.CharacterInfo.Level".GetText([creature.Level]) +
			(creature.MasteryLevel > 0 ? "UI.CharacterInfo.MasteryLevel".GetText([creature.MasteryLevel]) : null));

		AddProperty(row++, "UI.CharacterInfo.Ability.MaxHp.Main".GetText(), creature.MaxHp.ToString());
		AddProperty(row++, "UI.CharacterInfo.Ability.AttackPower.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.attack-power".GetText(),
		[
			null,
			(creature.AttackPowerCreatureMin + creature.AttackPowerCreatureMax + creature.AttackPowerEquipMin + creature.AttackPowerEquipMax) / 2,
			(creature.AttackPowerCreatureMin + creature.AttackPowerCreatureMax) / 2,
			(creature.AttackPowerEquipMin + creature.AttackPowerEquipMax) / 2,
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.Pierce.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.attack-pierce".GetText(),
		[
			 null,
			 creature.AttackPierceValue + creature.AttackPierceValueEquip,
			 creature.AttackPierceValue,
			 creature.AttackPierceValueEquip,
			 AbilityFunction.AttackPierce.GetPercent(creature.AttackPierceValue + creature.AttackPierceValueEquip, creature.Level, creature.AttackPierceBasePercent),
			 -1
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.AttackHit.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.attack-hit".GetText(),
		[
			null,
			creature.AttackHitValue + creature.AttackHitValueEquip,
			creature.AttackHitValue,
			creature.AttackHitValueEquip,
			AbilityFunction.AttackHit.GetPercent(creature.AttackHitValue + creature.AttackHitValueEquip, creature.Level, creature.AttackHitBasePercent),
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.AttackCritical.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.attack-critical".GetText(),
		[
			null,
			creature.AttackCriticalValue + creature.AttackCriticalValueEquip,
			creature.AttackCriticalValue,
			creature.AttackCriticalValueEquip,
			AbilityFunction.AttackCritical.GetPercent(creature.AttackCriticalValue + creature.AttackCriticalValueEquip, creature.Level, creature.AttackCriticalBasePercent),
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.AttackCriticalDamage.Value".GetText(), "UI.CharacterInfo.Ability.Tooltip.attack-critical-damage".GetText(),
		[
			null,
			creature.AttackCriticalDamageValue + creature.AttackCriticalDamageValueEquip,
			creature.AttackCriticalDamageValue,
			creature.AttackCriticalDamageValueEquip,
			AbilityFunction.AttackCriticalDamage.GetPercent(creature.AttackCriticalDamageValue + creature.AttackCriticalDamageValueEquip, creature.Level, creature.AttackCriticalDamagePercent),
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.AttackDamage".GetText(), "UI.CharacterInfo.Ability.Tooltip.attack-damage".GetText(),
		[
			null,
			creature.AttackDamageModifyDiff,
			creature.AttackDamageModifyPercent
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.AttackConcentrate.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.attack-concentrate".GetText(),
		[

		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.AttackStiff".GetText(), "UI.CharacterInfo.Ability.Tooltip.attack-stiff-duration".GetText(),
		[

		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.DefenceStiff".GetText(), "UI.CharacterInfo.Ability.Tooltip.defend-stiff-duration".GetText(),
		[

		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.Hate".GetText(), "UI.CharacterInfo.Ability.Tooltip.hate".GetText(),
		[
			null,
			creature.HatePowerCreatureValue + creature.HatePowerEquipValue,
			creature.HatePowerCreatureValue,
			creature.HatePowerEquipValue,
			AbilityFunction.Hate.GetPercent(creature.HatePowerCreatureValue + creature.HatePowerEquipValue, creature.Level, creature.HateBasePercent)
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.DefencePower.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.defend-power".GetText(),
		[
			null,
			creature.DefendPowerCreatureValue + creature.DefendPowerEquipValue,
			creature.DefendPowerCreatureValue,
			creature.DefendPowerEquipValue,
			AbilityFunction.DefendPower.GetPercent(creature.DefendPowerCreatureValue + creature.DefendPowerEquipValue, creature.Level),
			creature.AoeDefendPowerValue + creature.AoeDefendPowerValueEquip,
			AbilityFunction.AoeDefend.GetPercent(creature.AoeDefendPowerValue + creature.AoeDefendPowerValueEquip, creature.Level, creature.AoeDefendBasePercent)
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.DefenceDodge.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.defend-dodge".GetText(),
		[
			null,
			creature.DefendDodgeValue + creature.DefendDodgeValueEquip,
			creature.DefendDodgeValue,
			creature.DefendDodgeValueEquip,
			AbilityFunction.DefendDodge.GetPercent(creature.DefendDodgeValue + creature.DefendDodgeValueEquip, creature.Level),
			AbilityFunction.DefendCounterEnhance.GetPercent(creature.DefendDodgeValue + creature.DefendDodgeValueEquip, creature.Level),
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.DefenceParry.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.defend-parry".GetText(),
		[
			null,
			creature.DefendParryValue + creature.DefendParryValueEquip,
			creature.DefendParryValue,
			creature.DefendParryValueEquip,
			AbilityFunction.DefendParry.GetPercent(creature.DefendParryValue + creature.DefendParryValueEquip, creature.Level, creature.DefendParryBasePercent),
			AbilityFunction.DefendParryReducePercent.GetPercent(creature.DefendParryValue + creature.DefendParryValueEquip, creature.Level),
			-1,
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.HpRegen.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.hp-regen".GetText(),
		[
			null,
			creature.HpRegen + creature.HpRegenEquip,
			creature.HpRegenCombat + creature.HpRegenCombatEquip,
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.Heal".GetText(), "UI.CharacterInfo.Ability.Tooltip.heal".GetText(),
		[
			null,
			AbilityFunction.HealPower.GetPercent(creature.HealPowerValue + creature.HealPowerEquipValue, creature.Level, creature.HealPowerBasePercent),
			creature.HealPowerValue + creature.HealPowerEquipValue,
			creature.HealPowerDiff
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.DefenceDamage".GetText(), "UI.CharacterInfo.Ability.Tooltip.defend-damage".GetText(),
		[
			null,
			creature.DefendDamageModifyDiff,
			creature.DefendDamageModifyPercent
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.DefenceCritical.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.defend-critical2".GetText(),
		[
			null,
			creature.DefendCriticalValue + creature.DefendCriticalValueEquip,
			AbilityFunction.DefendCritical.GetPercent(creature.DefendCriticalValue + creature.DefendCriticalValueEquip, creature.Level, creature.DefendCriticalBasePercent),
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.DefenceCriticalDamageReduce.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.defend-critical_damage_reduce".GetText(),
		[
			null,
			creature.DefendCriticalDamageValue + creature.DefendCriticalDamageValueEquip,
			AbilityFunction.DefendCritical.GetPercent(creature.DefendCriticalDamageValue + creature.DefendCriticalDamageValueEquip, creature.Level, creature.DefendCriticalDamagePercent),
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.DefencePowerAoe".GetText(), "UI.CharacterInfo.Ability.Tooltip.DefencePowerAoe".GetText(),
		[
			null,
			creature.AoeDefendPowerValue + creature.AoeDefendPowerValueEquip,
			AbilityFunction.AoeDefend.GetPercent(creature.AoeDefendPowerValue + creature.AoeDefendPowerValueEquip, creature.Level, creature.AoeDefendBasePercent),
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.Abnormal".GetText(), "UI.CharacterInfo.Ability.Tooltip.abnormal".GetText(),
		[
			null,
			creature.AbnormalDefendPowerValue,
			AbilityFunction.AbnormalDefend.GetPercent(creature.AbnormalDefendPowerValue, creature.Level, creature.AbnormalDefendBasePercent),
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.AttackAbnormal".GetText(), "UI.CharacterInfo.Ability.Tooltip.attack-abnormal".GetText(),
		[
			null,
			creature.AbnormalAttackPowerValue + creature.AbnormalAttackPowerValueEquip,
			AbilityFunction.AbnormalAttack.GetPercent(creature.AbnormalAttackPowerValue + creature.AbnormalAttackPowerValueEquip, creature.Level, creature.AbnormalAttackBasePercent),
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.Attack.Value".GetText(), "UI.CharacterInfo.Ability.Tooltip.only-one".GetText(),
		[
			null,
			creature.AttackAttributeValue + creature.AttackAttributeValueEquip,
			creature.AttackAttributeValue,
			creature.AttackAttributeValueEquip,
			AbilityFunction.AttackAttribute.GetPercent(creature.AttackAttributeValue + creature.AttackAttributeValueEquip, creature.Level, creature.AttackAttributeBasePercent),
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.BossAttackPower.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.boss-attack-power".GetText(),
		[
			null,
			(creature.PveBossLevelNpcAttackPowerCreatureMin + creature.PveBossLevelNpcAttackPowerCreatureMax + creature.PveBossLevelNpcAttackPowerEquipMin + creature.PveBossLevelNpcAttackPowerEquipMax) / 2,
			(creature.PveBossLevelNpcAttackPowerCreatureMin + creature.PveBossLevelNpcAttackPowerCreatureMax) / 2,
			(creature.PveBossLevelNpcAttackPowerEquipMin + creature.PveBossLevelNpcAttackPowerEquipMax) / 2,
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.BossDefencePower.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.boss-defend-power".GetText(),
		[
			null,
			creature.PveBossLevelNpcDefendPowerCreatureValue + creature.PveBossLevelNpcDefendPowerEquipValue,
			creature.PveBossLevelNpcDefendPowerCreatureValue,
			creature.PveBossLevelNpcDefendPowerEquipValue,
			-1, //AbilityFunction.PveDefend.GetPercent(creature.PvpDefendPowerCreatureValue + creature.PvpDefendPowerEquipValue, creature.Level),
			creature.AoeDefendPowerValue + creature.AoeDefendPowerValueEquip,
			AbilityFunction.AoeDefend.GetPercent(creature.AoeDefendPowerValue + creature.AoeDefendPowerValueEquip, creature.Level, creature.AoeDefendBasePercent)
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.PcAttackPower.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.pc-attack-power".GetText(),
		[
			null,
			(creature.PvpAttackPowerCreatureMin + creature.PvpAttackPowerCreatureMax + creature.PvpAttackPowerEquipMin + creature.PvpAttackPowerEquipMax) / 2,
			(creature.PvpAttackPowerCreatureMin + creature.PvpAttackPowerCreatureMax) / 2,
			(creature.PvpAttackPowerEquipMin + creature.PvpAttackPowerEquipMax) / 2,
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.PcDefencePower.Main".GetText(), "UI.CharacterInfo.Ability.Tooltip.pc-defend-power".GetText(),
		[
			null,
			creature.PvpDefendPowerCreatureValue + creature.PvpDefendPowerEquipValue,
			creature.PvpDefendPowerCreatureValue,
			creature.PvpDefendPowerEquipValue,
			-1, //AbilityFunction.PvpDefend.GetPercent(creature.PvpDefendPowerCreatureValue + creature.PvpDefendPowerEquipValue, creature.Level),
			creature.AoeDefendPowerValue + creature.AoeDefendPowerValueEquip,
			AbilityFunction.AoeDefend.GetPercent(creature.AoeDefendPowerValue + creature.AoeDefendPowerValueEquip, creature.Level, creature.AoeDefendBasePercent)
		]);

		AddProperty(row++, "UI.Fatigability.Consume".GetText(), record.FatigabilityConsumeAmount.ToString());
	}

	private void AddProperty(int row, string name, string? value, string? tooltip = null)
	{
		ColumnList.AddChild(new BnsCustomLabelWidget()
		{
			String = new StringProperty()
			{
				LabelText = name,
				HorizontalAlignment = EHorizontalAlignment.HAlign_Center,
				VerticalAlignment = EVerticalAlignment.VAlign_Center,
			},
		}, row, 0);
		ColumnList.AddChild(new BnsCustomLabelWidget()
		{
			String = new StringProperty()
			{
				LabelText = value?.ToString(),
				HorizontalAlignment = EHorizontalAlignment.HAlign_Center,
				VerticalAlignment = EVerticalAlignment.VAlign_Center,
			},
			ToolTip = tooltip
		}, row, 1);
	}

	private void AddProperty(int row, string name, string? tooltip, TextArguments arguments, string? value = null)
	{
		AddProperty(row, name, (value ?? "<arg p='2'/>").Replace(arguments), tooltip?.Replace(arguments));
	}
	#endregion
}