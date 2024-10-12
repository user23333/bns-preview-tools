using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Common.Converters;
/// <summary>
/// Oneway converter for obtaining text of record 
/// </summary>
public class RecordNameConverter : MarkupExtension, IValueConverter
{
	public override object ProvideValue(IServiceProvider serviceProvider) => this;

	internal string Convert(object value)
	{
		return Convert(value, typeof(string), null, null) as string ?? "";
	}

	public object? Convert(object value, Type targetType, object? parameter, CultureInfo? culture)
	{
		var str = value?.ToString();
		if (value is Record record)
		{
			switch (record.OwnerName)
			{
				case "text": return record.Attributes.Get<string>("text");
				case "zone": return record.Attributes.Get<MapArea>("area")?.Name ?? str;

				default:
				{
					var text = record.Attributes["name2"]?.GetText();
					if (text != null) return text;
					break;
				}
			}
		}

		// if parameter exists and its value is BooleanBoxes.False means that return Null
		return parameter is false ? null : str;
	}

	public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo? culture)
	{
		throw new NotImplementedException();
	}
}