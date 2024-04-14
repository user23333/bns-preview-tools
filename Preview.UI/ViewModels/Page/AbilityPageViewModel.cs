using System.Collections.ObjectModel;
using System.Reflection;

using CommunityToolkit.Mvvm.ComponentModel;
using Xylia.Preview.Data.Models.Creature;

namespace Xylia.Preview.UI.ViewModels;
internal class AbilityPageViewModel : ObservableObject
{
	#region Constructors
	public AbilityPageViewModel()
	{
		var source = typeof(AbilityFunction)
			.GetProperties(BindingFlags.Static | BindingFlags.Public)
			.Select(x => x.GetValue(null))
			.Where(x => x is AbilityFunction ability && ability.K != 0)
			.OfType<AbilityFunction>();

		Source = new(source);
	}
	#endregion

	#region Properies
	public ObservableCollection<AbilityFunction> Source { get; private set; }

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

	public string Text => StringHelper.Get("AbilityPage_Result", Level, Value, GetPercent());
	#endregion


	#region Methods
	private double GetPercent()
	{
		if (Selected == null) return 0;

		double extra = 0;   // double)numericUpDown1.Value * 0.01;
		return Selected.GetPercent(Value, Level) + extra;
	}

	private void Refresh()
	{
		OnPropertyChanged(nameof(Text));
	}
	#endregion
}