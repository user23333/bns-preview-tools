using System.Windows;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip2;
public partial class ItemGraphReceipeTooltipPanel
{
	public ItemGraphReceipeTooltipPanel()
	{
		InitializeComponent();
	}

	#region Methods
	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);

		ItemGraphReceipeTooltipPanel_2.Visibility =
		ItemGraphReceipeTooltipPanel_3.Visibility =
		ItemGraphReceipeTooltipPanel_4.Visibility = Visibility.Collapsed;
	}

	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not ItemGraph.Edge edge) return;

		ItemGraphReceipeTooltipPanel_1_Title.String.LabelText = edge.Title;

		var item = edge.Recipe.MainItem;
		ItemGraphReceipeTooltipPanel_1_ItemIcon.ExpansionComponentList["IconImage"]!.SetValue(item.FrontIcon?.GetImage());
		ItemGraphReceipeTooltipPanel_1_ItemIcon.ExpansionComponentList["UnusableImage"]!.SetExpansionShow(false);
		ItemGraphReceipeTooltipPanel_1_ItemIcon.ExpansionComponentList["SearchedImage"]!.SetExpansionShow(false);
		ItemGraphReceipeTooltipPanel_1_ItemIcon.ExpansionComponentList["CanSaleItem"]!.SetValue(item.CanSaleItemImage);
		ItemGraphReceipeTooltipPanel_1_ItemIcon.ExpansionComponentList["SymbolImage"]!.SetExpansionShow(false);
		ItemGraphReceipeTooltipPanel_1_ItemIcon.ExpansionComponentList["SymbolImage_Chacked"]!.SetExpansionShow(false);
		ItemGraphReceipeTooltipPanel_1_ItemIcon.ExpansionComponentList["StackableLabel"]!.SetValue(edge.Recipe.MainItemCount);
		ItemGraphReceipeTooltipPanel_1_ItemIcon_Name.String.LabelText = edge.Recipe.MainItem.ItemName;
		ItemGraphReceipeTooltipPanel_1_ItemIcon_Desc.String.LabelText = null;

		ItemGraphReceipeTooltipPanel_1_Price.String.LabelText = edge.Recipe.Price;
		ItemGraphReceipeTooltipPanel_1_DiscountPrice.String.LabelText = edge.Recipe.DiscountPrice;
	}
	#endregion
}