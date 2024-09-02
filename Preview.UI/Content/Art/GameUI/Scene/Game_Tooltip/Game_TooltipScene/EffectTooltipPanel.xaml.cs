using System.Windows;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
public partial class EffectTooltipPanel
{
	#region Constructors
	public EffectTooltipPanel()
	{
		InitializeComponent();
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not Effect record) return;

		EffectTooltipPanel_Name.String.LabelText = record.Name2.GetText();
		EffectTooltipPanel_Description.String.LabelText = record.Description2.GetText();
		EffectTooltipPanel_Icon.ExpansionComponentList["IconImage"]?.SetValue(record.FrontIcon);
		EffectTooltipPanel_Icon.InvalidateVisual();
	}
	#endregion
}