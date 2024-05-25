using System.Windows;
using HandyControl.Data;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Xylia.Preview.Data.Models.Creature;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Views.Pages;
public partial class AbilityPage
{
	#region Constructor
	public AbilityPage()
	{
		InitializeComponent();
		DataContext = _viewModel = new AbilityPageViewModel();
	}
	#endregion

	#region Methods
	private void OpenSetting_Click(object sender, RoutedEventArgs e)
	{
		new AbilitySetting().Show();
	}

	private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.NewValue is not AbilityFunction ability) return;

		_viewModel.Selected = ability;
		LevelText.IsEnabled = ability.Φ != 0 || ability.LevelFactors.Count > 1;
		LevelText.Value = DEFAULT_LEVEL;
	}

	private void Level_Changed(object sender, FunctionEventArgs<double> e)
	{
		var level = (sbyte)e.Info;
		var selected = _viewModel.Selected;
		if (selected is null) return;

		#region Chart	
		// valid 
		if (double.IsNaN(selected.GetPercent(0, level)))
		{
			Chart.Series = [];
		}
		else
		{
			int CHART_MAX_VALUE = 20000;
			int CHART_INTERVAL = 500;

			var values = new ChartValues<ObservablePoint>();
			for (int i = 0; i <= CHART_MAX_VALUE; i += CHART_INTERVAL)
				values.Add(new(i, selected.GetPercent(i, level)));

			Chart.Series =
			[
				new LineSeries
				{
					Title = $"{selected.Type} converted percent in Lv{level}",
					Values = values,
					LineSmoothness = 1,
				}
			];
		}
		#endregion
	}
	#endregion

	#region Private Fields
	private AbilityPageViewModel _viewModel;
	const sbyte DEFAULT_LEVEL = 60;
	#endregion
}