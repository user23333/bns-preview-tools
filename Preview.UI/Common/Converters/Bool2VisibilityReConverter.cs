using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Xylia.Preview.UI.Common.Converters;
public class Bool2VisibilityReConverter : MarkupExtension, IValueConverter
{
	public override object ProvideValue(IServiceProvider serviceProvider) => this;

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is false ? Visibility.Visible : Visibility.Collapsed;
	}

	public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is not Visibility.Visible;
	}
}