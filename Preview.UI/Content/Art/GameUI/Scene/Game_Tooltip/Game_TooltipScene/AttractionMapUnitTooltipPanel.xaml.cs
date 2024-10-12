using System.Windows;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
public partial class AttractionMapUnitToolTipPanel
{
	#region Constructors
	public AttractionMapUnitToolTipPanel()
	{
		InitializeComponent();
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not IAttraction attraction) return;

		AttractionMapUnitToolTipPanel_Name.String.LabelText = attraction.Name;
		AttractionMapUnitToolTipPanel_Desc.String.LabelText = attraction.Description;
	}
	#endregion
}