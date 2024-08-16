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
		if (e.NewValue is not Npc record) return;

		ColumnList.Children.Clear();

		int row = 0;
		var creature = record.AbilityTest();
		AddProperty(row++, "name", creature.Name);
		AddProperty(row++, "level", "UI.CharacterInfo.Level".GetText([creature.Level]) + "UI.CharacterInfo.MasteryLevel".GetTextIf(creature.MasteryLevel > 0, [creature.MasteryLevel]));
		AddProperty(row++, "UI.CharacterInfo.Ability.MaxHp.Main".GetText(), creature.MaxHp.ToString());
		AddProperty(row++, "UI.CharacterInfo.Ability.AttackPower.Main".GetText(), "<arg p='2'/>", "UI.CharacterInfo.Ability.Tooltip.attack-power".GetText(),
		[
			null,
			(creature.AttackPowerCreatureMin + creature.AttackPowerCreatureMax + creature.AttackPowerEquipMin + creature.AttackPowerEquipMax) / 2,
			(creature.AttackPowerCreatureMin + creature.AttackPowerCreatureMax) / 2,
			(creature.AttackPowerEquipMin + creature.AttackPowerEquipMax) / 2,
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.DefencePower.Main".GetText(), "<arg p='2'/>", "UI.CharacterInfo.Ability.Tooltip.defend-power".GetText(),
		[
			null,
			creature.DefendPowerCreatureValue + creature.DefendPowerEquipValue,
			creature.DefendPowerCreatureValue,
			creature.DefendPowerEquipValue,
			AbilityFunction.DefendPower.GetPercent(creature.DefendPowerCreatureValue + creature.DefendPowerEquipValue, creature.Level),
			creature.AoeDefendPowerValue + creature.AoeDefendPowerValueEquip,
			AbilityFunction.AoeDefend.GetPercent(creature.AoeDefendPowerValue + creature.AoeDefendPowerValueEquip, creature.Level, creature.AoeDefendBasePercent)
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.DefenceParry.Main".GetText(), "<arg p='2'/>", "UI.CharacterInfo.Ability.Tooltip.defend-parry".GetText(),
		[
			null,
			creature.DefendParryValue + creature.DefendParryValueEquip,
			creature.DefendParryValue,
			creature.DefendParryValueEquip,
			AbilityFunction.DefendParry.GetPercent(creature.DefendParryValue + creature.DefendParryValueEquip, creature.Level, creature.DefendParryBasePercent),
			AbilityFunction.DefendParryReducePercent.GetPercent(creature.DefendParryValue + creature.DefendParryValueEquip, creature.Level),
			-1,
		]);
		AddProperty(row++, "UI.CharacterInfo.Ability.DefenceDodge.Main".GetText(), "<arg p='2'/>", "UI.CharacterInfo.Ability.Tooltip.defend-dodge".GetText(),
		[
			null,
			creature.DefendDodgeValue + creature.DefendDodgeValueEquip,
			creature.DefendDodgeValue,
			creature.DefendDodgeValueEquip,
			AbilityFunction.DefendDodge.GetPercent(creature.DefendDodgeValue + creature.DefendDodgeValueEquip, creature.Level),
			-1,
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
				HorizontalAlignment = HAlignment.HAlign_Center,
				VerticalAlignment = VAlignment.VAlign_Center,
			},
		}, row, 0);
		ColumnList.AddChild(new BnsCustomLabelWidget()
		{
			String = new StringProperty()
			{
				LabelText = value?.ToString(),
				HorizontalAlignment = HAlignment.HAlign_Center,
				VerticalAlignment = VAlignment.VAlign_Center,
			},
			ToolTip = tooltip
		}, row, 1);
	}

	private void AddProperty(int row, string name, string? value, string? tooltip, TextArguments arguments)
	{
		AddProperty(row, name, value?.Replace(arguments), tooltip?.Replace(arguments));
	}
	#endregion
}