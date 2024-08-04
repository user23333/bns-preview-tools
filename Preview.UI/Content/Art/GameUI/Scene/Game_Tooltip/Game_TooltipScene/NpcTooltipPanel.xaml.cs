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

		var creature = record.AbilityTest();

		int row = 1;
		AddProperty(row++, "name", creature.Name);
		AddProperty(row++, "level", "UI.CharacterInfo.Level".GetText([creature.Level]) + "UI.CharacterInfo.MasteryLevel".GetTextIf([creature.MasteryLevel], creature.MasteryLevel > 0));
		AddProperty(row++, "max-hp", creature.MaxHp.ToString());
	}

	private void AddProperty(int row, string name, string value, string? tooltip = null)
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
				LabelText = value,
				HorizontalAlignment = HAlignment.HAlign_Center,
				VerticalAlignment = VAlignment.VAlign_Center,
			},
			ToolTip = tooltip
		}, row, 1);
	}
	#endregion
}