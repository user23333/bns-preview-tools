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

		Column1.String.LabelText = StringHelper.Get("Text.Name");
		Column2.String.LabelText = StringHelper.Get("Text.Group");
		Column3.String.LabelText = StringHelper.Get("Text.Info");
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		// TODO: Add column properties
		ColumnList.Children.Clear();
		ColumnList.Children.Add(Column1);
		ColumnList.Children.Add(Column2);
		ColumnList.Children.Add(Column3);

		int row = 0;
		switch (e.NewValue)
		{
			case Reward record:
			{
				foreach (var info in record.GetInfo())
				{
					row++;

					ColumnList.AddChild(new BnsCustomLabelWidget()
					{
						DataContext = info.Data,
						String = new StringProperty(info.Element),
						ToolTip = new BnsTooltipHolder(),
					}, row, 0);
					ColumnList.AddChild(new BnsCustomLabelWidget()
					{	
						Tag = info.Group.Item1,
						String = new StringProperty(info.Group.Item2)
					}, row, 1);
					ColumnList.AddChild(new BnsCustomLabelWidget()
					{
						String = new StringProperty(info.ProbabilityInfo)
					}, row, 2);
				}

				break;
			}

			case GlyphReward record:
			{
				foreach (var info in record.GetInfo())
				{
					row++;

					ColumnList.AddChild(new BnsCustomLabelWidget()
					{
						DataContext = info.Data,
						String = new StringProperty(info.Data.GlyphName),
						ToolTip = new BnsTooltipHolder(),
					}, row, 0);
					ColumnList.AddChild(new BnsCustomLabelWidget()
					{
						Tag = info.Group,
						String = new StringProperty(info.Group)
					}, row, 1);
					ColumnList.AddChild(new BnsCustomLabelWidget()
					{
						String = new StringProperty(info.ProbabilityInfo)
					}, row, 2);
				}

				break;
			}

			case ItemCombination record:
			{
				foreach (var info in record.GetInfo())
				{
					row++;

					ColumnList.AddChild(new BnsCustomLabelWidget()
					{
						DataContext = info.Data,
						String = new StringProperty(info.Data.ItemName),
						ToolTip = new BnsTooltipHolder(),
					}, row, 0);
					ColumnList.AddChild(new BnsCustomLabelWidget()
					{
						Tag = info.Group,
						String = new StringProperty(info.Group)
					}, row, 1);
					ColumnList.AddChild(new BnsCustomLabelWidget()
					{
						String = new StringProperty(info.ProbabilityInfo)
					}, row, 2);
				}	 

				break;
			}

			case WorldAccountCombination record:
			{
				foreach (var info in record.GetInfo())
				{
					row++;

					ColumnList.AddChild(new BnsCustomLabelWidget()
					{
						DataContext = info.Data,
						String = new StringProperty(info.Data.ItemName),
						ToolTip = new BnsTooltipHolder(),
					}, row, 0);
					ColumnList.AddChild(new BnsCustomLabelWidget()
					{
						Tag = info.Group,
						String = new StringProperty(info.Group)
					}, row, 1);
					ColumnList.AddChild(new BnsCustomLabelWidget()
					{
						String = new StringProperty(info.ProbabilityInfo)
					}, row, 2);
				}

				break;
			}
		}
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