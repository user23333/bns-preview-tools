using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Xylia.Preview.UI.Common.Converters;
internal class Enum2VisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var current = System.Convert.ToInt32(value);
		var target = System.Convert.ToInt32(parameter);

		return current == target ? Visibility.Visible : Visibility.Collapsed;
	}

	public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}