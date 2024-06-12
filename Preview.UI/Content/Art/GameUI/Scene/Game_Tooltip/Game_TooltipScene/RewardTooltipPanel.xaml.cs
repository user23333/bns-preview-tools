using System.Windows;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
public partial class RewardTooltipPanel	: BnsCustomWindowWidget
{
	#region Constructors
	public RewardTooltipPanel()
	{
		InitializeComponent();
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not Reward record) return;

		// TODO: Add column properties
		ColumnList.Children.Clear();
		ColumnList.Children.Add(Column1);
		ColumnList.Children.Add(Column2);
		ColumnList.Children.Add(Column3);
		ColumnList.Children.Add(Column4);

		int row = 0;
		foreach (var info in record.GetInfos())
		{
			row++;

			AddChild(new BnsCustomLabelWidget()
			{
				Text = info.Item.ItemNameOnly,
				ToolTip = new ItemTooltipPanel() { DataContext = info.Item }
			}, row, 0);
			AddChild(new BnsCustomLabelWidget() { Text = info.Group.Item2, Tag = info.Group.Item1 }, row, 1);
			AddChild(new BnsCustomLabelWidget() { Text = info.ProbabilityInfo }, row, 2);
			AddChild(new BnsCustomLabelWidget() { Text = info.CountInfo }, row, 3);
		}

		ColumnList.InvalidateVisual();
	}

	private void AddChild(FrameworkElement element, int row, int column)
	{
		element.HorizontalAlignment = HorizontalAlignment.Center;
		element.VerticalAlignment = VerticalAlignment.Center;
		BnsCustomColumnListWidget.SetRow(element, row);
		BnsCustomColumnListWidget.SetColumn(element, column);

		ColumnList.Children.Add(element);
	}

	private void ColumnList_CellMerge(object sender, MergeEventArgs e)
	{
		if (e.Item1 is BnsCustomLabelWidget label1 && e.Item2 is BnsCustomLabelWidget label2)
		{
			e.Handled = label1.Tag != null && label1.Tag == label2.Tag && label1.Text == label2.Text;
		}
	}
	#endregion
}