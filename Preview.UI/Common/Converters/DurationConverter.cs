using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.UI.Common.Converters;
public class DurationConverter : MarkupExtension, IValueConverter
{
	#region Properties
	public ResetType Type { get; set; }
	#endregion

	#region Methods
	public override object ProvideValue(IServiceProvider serviceProvider) => this;

	public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int integer)
		{
			var time = new TimeSpan(Type switch
			{
				ResetType.Hourly => TimeSpan.TicksPerHour * integer,
				ResetType.Daily => TimeSpan.TicksPerDay * integer,
				ResetType.Weekly => TimeSpan.TicksPerDay * integer * 7,
				ResetType.Monthly => TimeSpan.TicksPerDay * integer * 30,
				_ => TimeSpan.TicksPerMillisecond * integer
			});

			// create text
			var builder = new StringBuilder();
			if (time.Ticks == 0) return StringHelper.Get(key: "Text_Time_None");
			if (time.Days > 0) builder.Append(time.Days + StringHelper.Get("Text_Time_Day"));
			if (time.Hours > 0) builder.Append(time.Hours + StringHelper.Get("Text_Time_Hour"));
			if (time.Minutes > 0) builder.Append(time.Minutes + StringHelper.Get("Text_Time_Minute"));
			if (time.Seconds > 0) builder.Append(time.Seconds + StringHelper.Get("Text_Time_Second"));

			return builder.ToString();
		}

		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value;
	}
	#endregion
}