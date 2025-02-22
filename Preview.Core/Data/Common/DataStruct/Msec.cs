﻿using System.ComponentModel;
using System.Text;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using static Xylia.Preview.Data.Common.DataStruct.MsecFormat;

namespace Xylia.Preview.Data.Common.DataStruct;
/// <summary>
///  Represents a time interval.
/// </summary>
public struct Msec : IFormattable
{
	#region Const
	/// <summary>
	/// Represents the number of ticks in 1 millisecond. This field is constant.
	/// </summary>
	public const int TicksPerMillisecond = 1;

	public const int TicksPerSecond = TicksPerMillisecond * 1000;   // 10,000

	public const int TicksPerMinute = TicksPerSecond * 60;         // 600,000

	public const int TicksPerHour = TicksPerMinute * 60;        // 36,000,000

	public const int TicksPerDay = TicksPerHour * 24;          // 864,000,000
	#endregion

	#region Constructors
	public Msec(int hours, int minutes, int seconds) : this(((hours * 60 + minutes) * 60 + seconds) * 1000) { }

	public Msec(long value)
	{
		ArgumentOutOfRangeException.ThrowIfGreaterThan(value, int.MaxValue);
		this.Value = (int)value;
	}
	#endregion

	#region Properties
	public readonly int Value;

	private readonly int AbsValue => Math.Abs(Value);

	public readonly bool IsPositive => Value > 0;

	public readonly int Days => AbsValue / TicksPerDay;

	public readonly int Hours => AbsValue / TicksPerHour % 24;

	public readonly int Minutes => AbsValue / TicksPerMinute % 60;

	public readonly int Seconds => AbsValue / TicksPerSecond % 60;

	public readonly int Milliseconds => (AbsValue / TicksPerMillisecond % 1000);

	public readonly double TotalDays => (double)AbsValue / TicksPerDay;

	public readonly double TotalHours => (double)AbsValue / TicksPerHour;

	public readonly double TotalMinutes => (double)AbsValue / TicksPerMinute;

	public readonly double TotalSeconds => (double)AbsValue / TicksPerSecond;
	#endregion

	#region Methods	   	
	public override readonly string ToString() => Value.ToString();
	public readonly string ToString(string format, IFormatProvider formatProvider) => ToString(format.ToEnum<MsecFormatType>(), formatProvider);
	public readonly string ToString(MsecFormatType format, IFormatProvider formatProvider = null) => MsecFormat.Format(this, format, formatProvider);

	public readonly bool Equals(Msec other) => Value == other.Value;
	public override readonly bool Equals(object obj) => obj is Msec other && Equals(other);
	public override readonly int GetHashCode() => HashCode.Combine(Value);
	#endregion

	#region Operator
	public static implicit operator Msec(long value) => new(value);

	public static bool operator ==(Msec a, Msec b) => a.Value == b.Value;
	public static bool operator !=(Msec a, Msec b) => !(a == b);
	public static Msec operator +(Msec a, Msec b) => a.Value + b.Value;
	public static Msec operator -(Msec a, Msec b) => a.Value - b.Value;
	public static int operator /(Msec a, Msec b) => a.Value / b.Value;

	public static Msec operator *(Msec a, int b) => a.Value * b;
	public static Msec operator /(Msec a, int b) => a.Value / b;
	#endregion
}

public static class MsecFormat
{
	public enum MsecFormatType
	{
		None,

		[Description("hms")]
		hms,

		[Description("hms-format-colon")]
		hmsFormatColon,

		[Description("hm-format-colon-plusonemin")]
		hmFormatColonPlusonemin,

		[Description("ms-format-colon-plusonesec")]
		msFormatColonPlusonesec,

		[Description("hms-rounddown")]
		hmsRounddown,

		[Description("hour")]
		hour,

		[Description("min")]
		min,

		[Description("sec")]
		sec,

		[Description("hms-plusonesec")]
		hmsPlusonesec,

		[Description("sec-float1")]
		secFloat1,

		[Description("hms-rounddown-plusonesec")]
		hmsRounddownPlusonesec,

		[Description("hour-plusonesec")]
		hourPlusonesec,

		[Description("min-plusonesec")]
		minPlusonesec,

		[Description("sec-plusonesec")]
		secPlusonesec,

		[Description("dhm-plusonesec")]
		dhmPlusonesec,

		[Description("dhm-rounddown")]
		dhmRounddown,

		[Description("dhms-plusonesec")]
		dhmsPlusonesec,

		[Description("dhms-rounddown")]
		dhmsRounddown,

		[Description("dhms-rounddown2-plusonesec")]
		dhmsRounddown2Plusonesec,
	}

	internal static string Format(Msec value, MsecFormatType format, IFormatProvider formatProvider)
	{
		if (format == MsecFormatType.None) return value.ToString();
		if (format == MsecFormatType.hmsFormatColon) return $"{value.Days}.{value.Hours:00}:{value.Minutes:00}:{value.Seconds:00}";

		// load text resource
		var MorningName = "Name.Time.Morning".GetText();
		var AfternoonName = "Name.Time.Afternoon".GetText();
		var DayName = "Name.Time.day".GetText() ?? ":";
		var HourName = "Name.Time.hour".GetText() ?? ":";
		var MinuteName = "Name.Time.minute".GetText() ?? ":";
		var SecondName = "Name.Time.second".GetText();

		// text build
		var sb = new StringBuilder(256);

		switch (format)
		{
			case MsecFormatType.hms:
				if (!value.IsPositive) sb.Append('-');
				if (value.Days > 0) sb.Append(value.Days + DayName);
				if (value.Hours > 0) sb.Append(value.Hours + HourName);
				if (value.Minutes > 0) sb.Append(value.Minutes + MinuteName);
				if (value.Seconds > 0) sb.Append(value.Seconds + SecondName);
				break;
			case MsecFormatType.hmsFormatColon:
				break;
			case MsecFormatType.hmFormatColonPlusonemin:
				break;
			case MsecFormatType.msFormatColonPlusonesec:
				break;
			case MsecFormatType.hmsRounddown:
				break;
			case MsecFormatType.hour: return value.TotalHours + HourName;
			case MsecFormatType.min: return value.TotalMinutes + MinuteName;
			case MsecFormatType.sec: return value.TotalSeconds + SecondName;
			case MsecFormatType.hmsPlusonesec:
				break;
			case MsecFormatType.secFloat1: return $"{value.TotalSeconds:0.0}" + SecondName;
			case MsecFormatType.hmsRounddownPlusonesec:
				break;
			case MsecFormatType.hourPlusonesec:
				break;
			case MsecFormatType.minPlusonesec:
				break;
			case MsecFormatType.secPlusonesec:
				break;
			case MsecFormatType.dhmPlusonesec:
				break;
			case MsecFormatType.dhmRounddown:
				break;
			case MsecFormatType.dhmsPlusonesec:
				break;
			case MsecFormatType.dhmsRounddown:
				break;
			case MsecFormatType.dhmsRounddown2Plusonesec:
				break;
			default:
				break;
		}

		// return null if empty
		return sb.Length == 0 ? null : sb.ToString();
	}
}