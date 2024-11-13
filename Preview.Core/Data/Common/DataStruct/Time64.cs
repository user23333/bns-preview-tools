using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Data.Common.DataStruct;
/// <summary>
/// Represents an instant in time and related to time zone offset of publisher (global field).
/// </summary>
public struct Time64(long ticks) : IFormattable, ITime, IComparable<Time64>
{
	#region Properties
	private const int HoursPerDay = 24;
	internal const long TicksPerSecond = 1;
	internal const long TicksPerMinute = TicksPerSecond * 60;
	internal const long TicksPerHour = TicksPerMinute * 60;
	internal const long TicksPerDay = TicksPerHour * HoursPerDay;

	// Number of milliseconds per time unit
	private const int MillisPerSecond = 1000;
	private const int MillisPerMinute = MillisPerSecond * 60;
	private const int MillisPerHour = MillisPerMinute * 60;
	private const int MillisPerDay = MillisPerHour * HoursPerDay;

	// Number of days in a non-leap year
	private const int DaysPerYear = 365;
	// Number of days in 4 years
	internal const int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461
																  // Number of days in 100 years
	internal const int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
																  // Number of days in 400 years
	internal const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097
																  // Number of days from 1/1/0001 to 12/31/1969
	internal const int DaysTo1970 = DaysPer400Years * 4 + DaysPer100Years * 3 + DaysPer4Years * 17 + DaysPerYear; // 719,162
																												  // Number of days from 1/1/0001 to 12/31/9999
	internal const int DaysTo10000 = DaysPer400Years * 25 - 366;  // 3652059

	internal const long MinTicks = 0;
	internal const long MaxTicks = DaysTo10000 * TicksPerDay - 1;

	// Euclidean Affine Functions Algorithm (EAF) constants

	// Constants used for fast calculation of following subexpressions
	//      x / DaysPer4Years
	//      x % DaysPer4Years / 4
	internal const uint EafMultiplier = (int)(((1UL << 32) + DaysPer4Years - 1) / DaysPer4Years);   // 2,939,745
	internal const uint EafDivider = EafMultiplier * 4;                                              // 11,758,980

	internal const ulong TicksPer6Hours = TicksPerHour * 6;
	internal const int March1BasedDayOfNewYear = 306;              // Days between March 1 and January 1

	public readonly ulong Ticks => (ulong)ticks;

	public readonly int Year
	{
		get
		{
			// y100 = number of whole 100-year periods since 1/1/0001
			// r1 = (day number within 100-year period) * 4
			(uint y100, uint r1) = Math.DivRem(((uint)(Ticks / TicksPer6Hours) | 3U), DaysPer400Years);

			return 1970 + (int)(100 * y100 + (r1 | 3u) / DaysPer4Years);
		}
	}

	public readonly int Month
	{
		get
		{
			// r1 = (day number within 100-year period) * 4
			uint r1 = (((uint)(Ticks / TicksPer6Hours) | 3U) + 1224) % DaysPer400Years;
			long u2 = Math.BigMul((int)EafMultiplier, (int)(r1 | 3U));
			ushort daySinceMarch1 = (ushort)((uint)u2 / EafDivider);
			int n3 = 2141 * daySinceMarch1 + 197913;
			return (ushort)(n3 >> 16) - (daySinceMarch1 >= March1BasedDayOfNewYear ? 12 : 0);
		}
	}

	public readonly int Day
	{
		get
		{
			// r1 = (day number within 100-year period) * 4
			uint r1 = (((uint)(Ticks / TicksPer6Hours) | 3U) + 1224) % DaysPer400Years;
			long u2 = Math.BigMul((int)EafMultiplier, (int)(r1 | 3U));
			ushort daySinceMarch1 = (ushort)((uint)u2 / EafDivider);
			int n3 = 2141 * daySinceMarch1 + 197913;
			// Return 1-based day-of-month
			return Math.Max(1, (ushort)n3 / 2141);
		}
	}

	public readonly sbyte Hour => (sbyte)((uint)(Ticks / TicksPerHour) % 24);

	public readonly int Minute => (int)((Ticks / TicksPerMinute) % 60);

	public readonly int Second => (int)((Ticks / TicksPerSecond) % 60);
	#endregion


	#region Override Methods
	public readonly override string ToString() => ToString(null, null);
	public readonly string ToString(string format) => ToString(format, null);
	public readonly string ToString(string format, IFormatProvider formatProvider) => BnsTimeFormat.Format(this + BnsTimeZoneInfo.FromPublisher()!.Offset, format, formatProvider);

	public readonly bool Equals(Time64 other) => Ticks == other.Ticks;
	public readonly override bool Equals(object obj) => obj is Time64 other && Equals(other);
	public readonly override int GetHashCode() => HashCode.Combine(Ticks);
	public readonly int CompareTo(Time64 other) => this.Ticks.CompareTo(other.Ticks);
	#endregion

	#region Operators
	public static bool operator ==(Time64 a, Time64 b) => a.CompareTo(b) == 0;
	public static bool operator !=(Time64 a, Time64 b) => a.CompareTo(b) != 0;
	public static bool operator <(Time64 a, Time64 b) => a.CompareTo(b) < 0;
	public static bool operator >(Time64 a, Time64 b) => a.CompareTo(b) > 0;

	public static Msec operator -(Time64 a, Time64 b) => ((long)a.Ticks - (long)b.Ticks) * 1000;
	public static Msec operator +(Time64 a, Time64 b) => ((long)a.Ticks + (long)b.Ticks) * 1000;
	public static Time64 operator -(Time64 a, Msec b) => (long)(a.Ticks - b.TotalSeconds);
	public static Time64 operator +(Time64 a, Msec b) => (long)(a.Ticks + b.TotalSeconds);


	public static implicit operator Time64(long ticks) => new(ticks);
	public static implicit operator long(Time64 time) => (long)time.Ticks;

	public static implicit operator Time64(DateTime dateTime) => Parse(dateTime);

	public static Time64 Parse(string s, EPublisher publisher) => Parse(DateTime.TryParse(s, out var result) ? result : default, publisher);

	public static Time64 Parse(DateTime time, EPublisher? publisher = null)
	{
		return new Time64((time - DateTime.UnixEpoch).Ticks / 10000000) - BnsTimeZoneInfo.FromPublisher(publisher).Offset;
	}
	#endregion
}