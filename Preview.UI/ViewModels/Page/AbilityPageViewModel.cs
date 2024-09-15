using System.Collections.ObjectModel;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
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
        set { SetProperty(ref selected, value); Refresh(); }
    }

    sbyte _level;
    public sbyte Level
    {
        get => _level;
        set { SetProperty(ref _level, value); Refresh(); }
    }

    int _value;
    public int Value
    {
        get => _value;
        set { SetProperty(ref _value, value); Refresh(); }
    }

    double _extra;
    public double Extra
    {
        get => _extra;
        set { SetProperty(ref _extra, value); Refresh(); }
    }

    public string Text => StringHelper.Get("AbilityPage_Result", Level, Value, GetPercent());
    #endregion


    #region Methods
    private double GetPercent()
    {
        if (Selected is null) return double.NaN;
        return Selected.GetPercent(Value, Level) + Extra * 0.01;
    }

    private void Refresh()
    {
        OnPropertyChanged(nameof(Text));
    }
    #endregion
}