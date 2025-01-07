using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.UI.Common.Converters;
public class SizeConverter : MarkupExtension, IValueConverter
{
	public override object ProvideValue(IServiceProvider serviceProvider) => this;

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var size = System.Convert.ToDouble(value);
		return BinaryExtension.GetReadableSize(size);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}