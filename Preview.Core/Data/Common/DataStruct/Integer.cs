using System.ComponentModel;
using System.Globalization;
using System.Text;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.Data.Common.DataStruct;
[TypeConverter(typeof(IntegerConverter))]
public readonly struct Integer(long value) : IFormattable
{
	#region Override Methods
	private long Value { get; } = value;

	public static implicit operator Integer(long value) => new(value);
	public static implicit operator long(Integer struc) => struc.Value;

	public override string ToString() => ToString(null, null);

	public string ToString(string format, IFormatProvider formatProvider)
	{
		var prop = this.GetProperty(format);
		if (prop != null) return prop.GetValue(this)?.ToString();
		else return Value.ToString();
	}
	#endregion


	#region Float
	public readonly string FloatDot0 => $"{Value / 1000d: 0}";
	public readonly string FloatDot1 => $"{Value / 1000d: 0.0}";
	public readonly string FloatDot2 => $"{Value / 1000d: 0.00}";
	public readonly string FloatDot3 => $"{Value / 1000d: 0.000}";

	//integer02	comma	comma5
	#endregion

	#region Money
	//moneyGold
	//moneySilver
	//moneyBronze

	public readonly string Money => ToMoney(false);
	public readonly string MoneyDefault => ToMoney(true);
	//money-disable
	public readonly string MoneyNonTooltip => ToMoney(false, false);
	//money-text

	private readonly string ToMoney(bool IsDefault, bool tooltip = true)
	{
		var gold = (int)(Value / 10000);
		var silver = (int)(Value % 10000 / 100);
		var copper = (int)(Value % 100);

		var builder = new StringBuilder();
		if (IsDefault || gold > 0)
		{
			builder.Append(gold + """<image enablescale="true" imagesetpath="00009076.GuildBank_Coin_Gold" scalerate="1.2"/>""");
		}

		if (IsDefault || silver > 0)
		{
			builder.Append(silver + """<image enablescale="true" imagesetpath="00009076.GuildBank_Coin_Silver" scalerate="1.2"/>""");
		}

		if (IsDefault || copper > 0)
		{
			builder.Append(copper + """<image enablescale="true" imagesetpath="00009076.GuildBank_Coin_Bronze" scalerate="1.2"/>""");
		}

		return builder.ToString();
	}
	#endregion

	#region Time
	//date-gmtime	
	//public readonly string DateGmtime => $"#{Value} DateGmtime";
	//public readonly string DateGmtime24 => $"#{Value} DateGmtime";
	//date-ymd
	//date-ymd-gmtime

	//public readonly string Time => $"#{Value}Time";
	//public readonly string Timedate => $"#{Value}Timedate";
	//public readonly string Timeymd => $"#{Value}Timeymd";
	//public readonly string TimeRounddown => $"#{Value}time-rounddown";
	//public readonly string TimeDhm => $"#{Value}time-dhm";
	//public readonly string TimeHm => $"#{Value}time-hm";
	//time-hour
	//time-sec
	//time-hms
	//time-hms-centisecond
	//time-hms-centisecond-colon
	//time-colon
	//SecondDot1  	
	//SecondDot2
	//week
	#endregion

	#region	Secondary
	public readonly string SecondaryMoneyBlue => Value + " <replace p=\"UI.Common.SecondaryMoney.IconOnly\"/>";
	public readonly string SecondaryMoneyRed => Value + " <replace p=\"UI.Common.SecondaryMoney.Red.IconOnly\"/>";
	#endregion
}

internal class IntegerConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type type)
	{
		if (type == typeof(sbyte) || type == typeof(short) || type == typeof(int) || type == typeof(long) ||
			type == typeof(Msec) || type == typeof(Distance) || type == typeof(Velocity) ||
			type == typeof(double)) return true;  // Double is not used at game

		return base.CanConvertFrom(context, type);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) => value switch
	{
		Msec m => new Integer(m.Value),
		Distance d => new Integer((int)(d.Value * 8 * 0.01)),
		Velocity v => new Integer((int)(v.Value * 8 * 0.01)),
		double d => new Integer(double.IsNaN(d) ? -1 : (int)(d * 100000)),
		_ => new Integer(Convert.ToInt64(value)),
	};
}