using OfficeOpenXml;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal class WeeklyTimeTableOut : OutSet
{
	protected override void CreateData(ExcelWorksheet sheet)
	{
		#region Data
		List<WeeklyTimeTable.WeeklyTimePeriod> periods = [];

		foreach (var record in Source.Provider.GetTable<WeeklyTimeTable>().Where(x => x.Enable))
		{
			void CreatePeriod(DayOfWeekSeq dayOfWeek, sbyte[] startHours, sbyte[] endHours)
			{
				for (int i = 0; i < startHours.Length; i++)
				{
					var startHour = startHours[i];
					if (startHour == -1) continue;

					periods.Add(new WeeklyTimeTable.WeeklyTimePeriod()
					{
						Data = record,
						DayOfWeek = dayOfWeek,
						StartHour = startHour,
						EndHour = endHours[i],
						//StartMinute = record.StartMinute,
						//EndMinute = record.EndMinute
					});
				}
			}

			CreatePeriod(DayOfWeekSeq.Sun, record.SunStartHour, record.SunEndHour);
			CreatePeriod(DayOfWeekSeq.Mon, record.MonStartHour, record.MonEndHour);
			CreatePeriod(DayOfWeekSeq.Tue, record.TueStartHour, record.TueEndHour);
			CreatePeriod(DayOfWeekSeq.Wed, record.WedStartHour, record.WedEndHour);
			CreatePeriod(DayOfWeekSeq.Thu, record.ThuStartHour, record.ThuEndHour);
			CreatePeriod(DayOfWeekSeq.Fri, record.FriStartHour, record.FriEndHour);
			CreatePeriod(DayOfWeekSeq.Sat, record.SatStartHour, record.SatEndHour);
		}
		#endregion

		#region Output
		for (int h = 0; h <= 23; h++)
		{
			sheet.Cells[h + 2, Column].SetValue($"{h} - {h + 1}");
		}

		foreach (var dayOfWeek in Enum.GetValues<DayOfWeekSeq>().Where(x => x < DayOfWeekSeq.COUNT))
		{
			int row = 1;
			sheet.Column(++Column).Width = 50;
			sheet.Cells[row++, Column].SetValue(dayOfWeek);

			for (int h = 0; h <= 23; h++)
			{
				var data = periods.Where(x => x.DayOfWeek == dayOfWeek && x.StartHour == h);
				if (!data.Any()) continue;

				sheet.Cells[row + h, Column].SetValue(string.Join('\n', data));
			}
		}
		#endregion
	}
}