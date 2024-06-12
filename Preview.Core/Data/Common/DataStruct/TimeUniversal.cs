using static Xylia.Preview.Data.Common.DataStruct.Time64;

namespace Xylia.Preview.Data.Common.DataStruct;
/// <summary>
///  Represents an instant in time
/// </summary>
public struct TimeUniversal(long ticks) : IFormattable, ITime
{
	#region Properties
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
	public string ToString(string format, IFormatProvider formatProvider) => TimeFormat.Format(this, format, formatProvider);

	public override string ToString() => ToString(null, null);

	public override int GetHashCode() => HashCode.Combine(Ticks);
	#endregion

	#region Static Methods
	public static implicit operator TimeUniversal(long ticks) => new(ticks);
	public static implicit operator long(TimeUniversal time) => (long)time.Ticks;

	public static implicit operator TimeUniversal(DateTime dateTime) => Parse(dateTime);

	public static TimeUniversal Parse(string s) => Parse(DateTime.TryParse(s, out var result) ? result : default);

	public static TimeUniversal Parse(DateTime time) => (time - new DateTime(1970, 1, 1)).Ticks / 10000000;
	#endregion
}