using Xylia.Preview.Data.Models.Sequence;
using static Xylia.Preview.Data.Models.WeeklyTimeTable;

namespace Xylia.Preview.Data.Models;
public abstract class ZoneTriggerEventCond : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public CondContentsTypeSeq CondContentsType { get; set; }

	public enum CondContentsTypeSeq
	{
		None,
		FactionStageFaction1Win,
		FactionStageFaction2Win,
		COUNT
	}

	public Ref<GameMessage> CondEventMessage { get; set; }

	public sealed class WeeklyEvent : ZoneTriggerEventCond
	{
		public EventDayOfWeekSeq EventDayOfWeek { get; set; }

		public enum EventDayOfWeekSeq
		{
			Sun,
			Mon,
			Tue,
			Wed,
			Thu,
			Fri,
			Sat,
			ThuFri,
			MonTueWedThu,
			FriSatSun,
			TueThu,
			Weekend,
			EntireWeek,
			COUNT
		}

		public sbyte[] EventHour { get; set; }

		public sbyte[] EventMinute { get; set; }

		#region Methods
		public List<WeeklyTimePeriod> GetPeriods(ZoneTriggerEventStage stage)
		{
			#region DayOfWeek
			List<DayOfWeekSeq> days = EventDayOfWeek switch
			{
				EventDayOfWeekSeq.Sun => [DayOfWeekSeq.Sun],
				EventDayOfWeekSeq.Mon => [DayOfWeekSeq.Mon],
				EventDayOfWeekSeq.Tue => [DayOfWeekSeq.Tue],
				EventDayOfWeekSeq.Wed => [DayOfWeekSeq.Wed],
				EventDayOfWeekSeq.Thu => [DayOfWeekSeq.Thu],
				EventDayOfWeekSeq.Fri => [DayOfWeekSeq.Fri],
				EventDayOfWeekSeq.Sat => [DayOfWeekSeq.Sat],
				EventDayOfWeekSeq.ThuFri => [DayOfWeekSeq.Thu, DayOfWeekSeq.Fri],
				EventDayOfWeekSeq.MonTueWedThu => [DayOfWeekSeq.Mon, DayOfWeekSeq.Tue, DayOfWeekSeq.Wed, DayOfWeekSeq.Thu],
				EventDayOfWeekSeq.FriSatSun => [DayOfWeekSeq.Fri, DayOfWeekSeq.Sat, DayOfWeekSeq.Sun],
				EventDayOfWeekSeq.TueThu => [DayOfWeekSeq.Tue, DayOfWeekSeq.Thu],
				EventDayOfWeekSeq.Weekend => [DayOfWeekSeq.Fri, DayOfWeekSeq.Sat, DayOfWeekSeq.Sun],
				EventDayOfWeekSeq.EntireWeek => [DayOfWeekSeq.Sun, DayOfWeekSeq.Mon, DayOfWeekSeq.Tue, DayOfWeekSeq.Wed, DayOfWeekSeq.Thu, DayOfWeekSeq.Fri, DayOfWeekSeq.Sat],
				_ => throw new NotSupportedException()
			};
			#endregion

			#region Period
			var duration = stage.Attributes.Get<short>("total-duration-minute-1");
			var periods = new List<WeeklyTimePeriod>();

			for (int i = 0; i < EventHour.Length; i++)
			{
				var eventHour = EventHour[i];
				var eventMinute = EventMinute[i];
				if (eventHour == -1) continue;

				periods.AddRange(days.Select(day => new WeeklyTimePeriod()
				{
					Data = stage,
					DayOfWeek = day,
					StartHour = eventHour,
					StartMinute = eventMinute,
					EndHour = eventHour,
					EndMinute = (sbyte)(EventMinute[i] + duration),
				}));
			}

			return periods;
			#endregion
		}
		#endregion
	}

	public sealed class TimeoutEvent : ZoneTriggerEventCond { }
	public sealed class NpcKilledEvent : ZoneTriggerEventCond { }
	public sealed class NpcKilledEvent2 : ZoneTriggerEventCond { }
	public sealed class NpcSurvivedScoreDecisionTimeoutEvent : ZoneTriggerEventCond { }
	public sealed class NpcSurvivedTimeoutEvent : ZoneTriggerEventCond { }
	public sealed class BossChallengeTimeoutEvent : ZoneTriggerEventCond { }
	public sealed class BossChallengeRoundScoreEvent : ZoneTriggerEventCond { }
	public sealed class BossChallengeBossNpcKilledEvent : ZoneTriggerEventCond { }
	#endregion
}