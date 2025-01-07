using OfficeOpenXml;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal sealed class QuestOut : OutSet
{
	protected override void CreateData(ExcelPackage package)
	{
		#region Title
		var sheet = CreateSheet(package);
		int column = 1, row = 1;
		sheet.SetColumn(column++, "id", 10);
		sheet.SetColumn(column++, "alias", 20);
		sheet.SetColumn(column++, "name", 40);
		sheet.SetColumn(column++, "group", 25);
		sheet.SetColumn(column++, "category", 15);
		sheet.SetColumn(column++, "content-type", 15);
		sheet.SetColumn(column++, "reset-type", 15);
		sheet.SetColumn(column++, "retired", 15);
		sheet.SetColumn(column++, "tutorial", 15);
		#endregion

		#region Data
		foreach (var Quest in Source.Provider.GetTable<Quest>().OrderBy(x => x.PrimaryKey))
		{
			row++;
			column = 1;

			sheet.Cells[row, column++].SetValue(Quest.PrimaryKey);
			sheet.Cells[row, column++].SetValue(Quest.Attributes["alias"]);
			sheet.Cells[row, column++].SetValue(Quest.Name);
			sheet.Cells[row, column++].SetValue(Quest.Group);
			sheet.Cells[row, column++].SetValue(Quest.Attributes["category"]);
			sheet.Cells[row, column++].SetValue(Quest.Attributes["content-type"]);
			sheet.Cells[row, column++].SetValue(Quest.Attributes["reset-type"]);
			sheet.Cells[row, column++].SetValue(Quest.Attributes["retired"]);
			sheet.Cells[row, column++].SetValue(Quest.Attributes["tutorial"]);
		}
		#endregion
	}
}