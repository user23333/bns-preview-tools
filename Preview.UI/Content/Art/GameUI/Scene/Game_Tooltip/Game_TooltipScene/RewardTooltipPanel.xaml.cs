using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
public partial class RewardTooltipPanel
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

		int row = 0;
		foreach (var info in record.GetInfos())
		{
			row++;

			ColumnList.AddChild(new BnsCustomLabelWidget()
			{
				String = new StringProperty()
				{
				   LabelText = info.Element,
				   HorizontalAlignment = HAlignment.HAlign_Center,
				   VerticalAlignment = VAlignment.VAlign_Center,
				},
				ToolTip = new ItemTooltipPanel() { DataContext = info.Item },
			}, row, 0);
			ColumnList.AddChild(new BnsCustomLabelWidget()
			{
				String = new StringProperty()
				{
					LabelText = info.Category.Item2,
					HorizontalAlignment = HAlignment.HAlign_Center,
					VerticalAlignment = VAlignment.VAlign_Center,
				},
				Tag = info.Category.Item1 
			}, row, 1);
			ColumnList.AddChild(new BnsCustomLabelWidget()
			{
				String = new StringProperty()
				{
					LabelText = info.ProbabilityInfo,
					HorizontalAlignment = HAlignment.HAlign_Center,
					VerticalAlignment = VAlignment.VAlign_Center,
				}
			}, row, 2);
		}

		//layout
		ColumnList.InvalidateVisual();
		this.InvalidateMeasure();
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