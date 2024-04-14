using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class WeeklyTimeTable : ModelElement
{
	#region Attributes
	public bool Enable { get; set; }

	public sbyte[] SunStartHour { get; set; }

	public sbyte[] SunEndHour { get; set; }

	public sbyte[] MonStartHour { get; set; }

	public sbyte[] MonEndHour { get; set; }

	public sbyte[] TueStartHour { get; set; }

	public sbyte[] TueEndHour { get; set; }

	public sbyte[] WedStartHour { get; set; }

	public sbyte[] WedEndHour { get; set; }

	public sbyte[] ThuStartHour { get; set; }

	public sbyte[] ThuEndHour { get; set; }

	public sbyte[] FriStartHour { get; set; }

	public sbyte[] FriEndHour { get; set; }

	public sbyte[] SatStartHour { get; set; }

	public sbyte[] SatEndHour { get; set; }

	public sbyte StartMinute { get; set; }

	public sbyte EndMinute { get; set; }
	#endregion

	#region Helpers
	public struct WeeklyTimePeriod
	{
		public DayOfWeekSeq DayOfWeek;
		public sbyte StartHour;
		public sbyte StartMinute;
		public sbyte EndHour;
		public sbyte EndMinute;
		public WeeklyTimeTable Data;

		public override string ToString() => $"[{StartHour}:{StartMinute:00}~{EndHour}:{EndMinute:00}] {Data}";
	}
	#endregion
}