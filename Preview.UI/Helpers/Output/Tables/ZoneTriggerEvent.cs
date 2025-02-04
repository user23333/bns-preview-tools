using System.Diagnostics;
using OfficeOpenXml;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using static Xylia.Preview.Data.Models.WeeklyTimeTable;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal class ZoneTriggerEventOut : OutSet
{
	public override bool Visible => false;

	protected override void CreateData(ExcelPackage package)
	{
		List<WeeklyTimePeriod> periods = [];
		foreach (var stage in Source.Provider.GetTable<ZoneTriggerEventStage>())
		{
			if (!stage.Alias.Contains("FieldEvent")) continue;

			foreach (var cond in stage.NextCond.Values())
			{
				if (cond is ZoneTriggerEventCond.WeeklyEvent weeklyEvent)
				{
					periods.AddRange(weeklyEvent.GetPeriods(stage));
				}
			}
		}

#if DEBUG
		foreach (var dayOfWeek in periods.GroupBy(x => x.DayOfWeek))
		{
			foreach (var period in dayOfWeek.GroupBy(x => x.StartHour).OrderBy(x => x.Key))
			{
				var first = period.First();
				var time = $"{first.StartHour}:{first.StartMinute:00}:00";
				var zone = string.Join(",", period.Select(x => string.Format("'{0}'", x.Data.GetName())));

				Debug.WriteLine($"\t\t['day'=>{(int)dayOfWeek.Key},'time'=>'{time}','zone'=>[{zone}]],");
			}
		}
#endif

		#region Output
		var sheet = CreateSheet(package);
		int column = 1;

		for (int h = 0; h <= 23; h++)
		{
			sheet.Cells[h + 2, column].SetValue($"{h} - {h + 1}");
		}

		foreach (var dayOfWeek in Enum.GetValues<DayOfWeekSeq>().Where(x => x < DayOfWeekSeq.COUNT))
		{
			int row = 1;
			sheet.Column(++column).Width = 50;
			sheet.Cells[row++, column].SetValue(dayOfWeek);

			for (int h = 0; h <= 23; h++)
			{
				var data = periods.Where(x => x.DayOfWeek == dayOfWeek && x.StartHour == h);
				if (!data.Any()) continue;

				sheet.Cells[row + h, column].SetValue(string.Join('\n', data));
			}
		}
		#endregion
	}
}