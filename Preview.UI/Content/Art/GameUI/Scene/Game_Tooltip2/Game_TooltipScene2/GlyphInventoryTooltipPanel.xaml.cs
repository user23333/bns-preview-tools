using System.Windows;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip2;
public partial class GlyphInventoryTooltipPanel
{
	#region Constructors
	public GlyphInventoryTooltipPanel()
	{
		InitializeComponent();
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not Glyph record) return;

		GlyphInventoryTooltipPanel_Name.String.LabelText = record.GlyphName;
		GlyphInventoryTooltipPanel_ItemIcon.ExpansionComponentList["IconImage"]?.SetValue(record.Icon?.GetImage());
		GlyphInventoryTooltipPanel_ItemIcon.InvalidateVisual();
		GlyphInventoryTooltipPanel_GlyphColor.String.LabelText = $"UI.GlyphToolTip.MainInfo.{record.GlyphType}.{record.Color}".GetText();
		GlyphInventoryTooltipPanel_GlyphDescription.String.LabelText = record.GlyphDescription;
		GlyphInventoryTooltipPanel_Description.String.LabelText = record.Description;
		GlyphInventoryTooltipPanel_FlaverText.String.LabelText = record.FlavorText.GetText();
		GlyphInventoryTooltipPanel_DisableFilter.String.LabelText = record.DungeonCondition.Instance?.Description.GetText();
		GlyphInventoryTooltipPanel_Equiped.String.LabelText = "UI.GlyphToolTip.EquippedNum".GetText([1]);
		GlyphInventoryTooltipPanel_Count.String.LabelText = "UI.GlyphToolTip.AcquiredNum".GetText([1]);
	}
	#endregion
}