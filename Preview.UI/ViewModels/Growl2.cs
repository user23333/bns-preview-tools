using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Data;

namespace Xylia.Preview.UI.ViewModels;
internal partial class Growl2 : ObservableObject
{
	#region Constructor
	public Growl2()
	{
		Type = InfoType.Info;
	}
	#endregion

	#region Instance	
	[ObservableProperty] Geometry? icon;
	[ObservableProperty] Brush? brush;
	[ObservableProperty] string? message;
	[ObservableProperty] bool visible = true;
	public Action? Stop;

	public InfoType Type
	{
		set
        {
			Application.Current.Dispatcher.Invoke(() =>
			{
                Visible = true;
				Icon = Application.Current.Resources[value + "Geometry"] as Geometry;
				Brush = Application.Current.Resources[value + "Brush"] as Brush;
			});
		}
	}

	[RelayCommand]
	public void Close()
	{
		Visible = false;
	}
	#endregion

	#region Helpers
	public static ObservableCollection<Growl2> Source = [];

	public static Growl2 Add(Growl2 growl)
	{
		Application.Current.Dispatcher.Invoke(() => Source.Add(growl));
		return growl;
	}
	#endregion
}