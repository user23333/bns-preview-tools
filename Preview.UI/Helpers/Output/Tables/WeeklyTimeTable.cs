using OfficeOpenXml;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal class WeeklyTimeTableOut : OutSet
{
	protected override void CreateData(ExcelPackage package)
	{
		var periods = Source.Provider.GetTable<WeeklyTimeTable>().Where(x => x.Enable).SelectMany(x => x.GetPeriods());

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