using System.Collections.ObjectModel;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.ViewModels;
internal class AbilityPageViewModel : ObservableObject
{
	#region Constructors
	public AbilityPageViewModel()
	{
		var source = typeof(AbilityFunction)
			.GetProperties(BindingFlags.Static | BindingFlags.Public)
			.Select(x => x.GetValue(null))
			.Where(x => x is AbilityFunction ability && ability.IsValid)
			.OfType<AbilityFunction>();

		Source = new(source);
	}
	#endregion

	#region Properties
	public ObservableCollection<AbilityFunction> Source { get; private set; }

	public Func<double, string> PercentFormatter { get; } = (d) => $"{d:P2}";


	AbilityFunction? selected;
	public AbilityFunction? Selected
	{
		get => selected;
		set
		{
			SetProperty(ref selected, value);
			OnPropertyChanged(nameof(Series));
			OnPropertyChanged(nameof(ResultText));
		}
	}

	sbyte _level;
	public sbyte Level
	{
		get => _level;
		set
		{
			SetProperty(ref _level, value);
			OnPropertyChanged(nameof(Series));
			OnPropertyChanged(nameof(ResultText));
		}
	}

	int _value;
	public int Value
	{
		get => _value;
		set
		{
			SetProperty(ref _value, value);
			OnPropertyChanged(nameof(ResultText));
		}
	}

	double _extra;
	public double Extra
	{
		get => _extra;
		set
		{
			SetProperty(ref _extra, value);
			OnPropertyChanged(nameof(ResultText));
		}
	}
	#endregion

	#region Methods
	public SeriesCollection? Series
	{
		get
		{
			if (selected is null || double.IsNaN(selected.GetFactor(_level))) return null;

			int CHART_MAX_VALUE = 20000;
			int CHART_INTERVAL = 500;

			var values = new ChartValues<ObservablePoint>();
			for (int i = 0; i <= CHART_MAX_VALUE; i += CHART_INTERVAL)
				values.Add(new(i, selected.GetPercent(i, _level)));

			return
			[
				new LineSeries
				{
					LineSmoothness = 1,
					Values = values,
					Title = StringHelper.Get("AbilityPage_ChartTitle", selected.Text ,_level),
				}
			];
		}
	}

	public string ResultText => StringHelper.Get("AbilityPage_Result", Level, Value, GetPercent());

	private double GetPercent()
	{
		if (Selected is null) return double.NaN;
		return Selected.GetPercent(Value, Level) + Extra * 0.01;
	}
	#endregion
}