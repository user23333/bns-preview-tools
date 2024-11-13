using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Common.Converters;
/// <summary>
/// Oneway converter for obtaining text of record 
/// </summary>
public class RecordNameConverter : MarkupExtension, IValueConverter
{
	public override object ProvideValue(IServiceProvider serviceProvider) => this;

	public object? Convert(object value, Type targetType, object? parameter, CultureInfo? culture)
	{
		// if parameter exists and its value is BooleanBoxes.False means that return Null
		var str = parameter is false ? null : value?.ToString();

		if (value is Record record) return Convert(record) ?? str;
		return str;
	}

	public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo? culture)
	{
		throw new NotImplementedException();
	}


	internal static string? Convert(Record record)
	{
		switch (record.OwnerName)
		{
			case "item-combination" or "world-account-combination": return record.Attributes.Get<Text>("material-group-name")?.text;
			case "text": return record.To<Text>().text;
			case "zone": return record.Attributes.Get<MapArea>("area")?.Name;

			default:
			{
				var text = record.Attributes["name2"]?.GetText();
				if (text != null) return text;
				break;
			}
		}

		return null;
	}
}