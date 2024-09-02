using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Extensions;
public class TextResource : MarkupExtension, IValueConverter
{
	public string? Format { get; set; }

	#region Methods
	public override object ProvideValue(IServiceProvider serviceProvider) => this;

	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (Format != null) value = string.Format(Format, value);

		return value.GetText();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
	#endregion
}