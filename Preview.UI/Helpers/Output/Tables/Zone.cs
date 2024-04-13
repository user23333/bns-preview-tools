// Temporary output method
// delete after adding linked table query in SQL

using OfficeOpenXml;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal sealed class ZoneOut : OutSet
{
    protected override void CreateData(ExcelWorksheet sheet)
    {
        #region Title
        sheet.SetColumn(Column++, "id", 10);
        sheet.SetColumn(Column++, "alias", 30);
        sheet.SetColumn(Column++, "name", 30);
		sheet.SetColumn(Column++, "zone-type2", 20);
		sheet.SetColumn(Column++, "attraction", 50);
		#endregion

		foreach (var record in Source.Provider.GetTable<Zone>().OrderBy(x => x.PrimaryKey))
        {
            Row++;

            int column = 1;
            sheet.Cells[Row, column++].SetValue(record.PrimaryKey);
            sheet.Cells[Row, column++].SetValue(record.Attributes["alias"]);
			sheet.Cells[Row, column++].SetValue(record.Area.Instance?.Text);
			sheet.Cells[Row, column++].SetValue(record.Attributes["zone-type2"]);
			sheet.Cells[Row, column++].SetValue(record.Attributes["attraction"]);
		}
	}
}

internal sealed class NpcOut : OutSet
{
	protected override void CreateData(ExcelWorksheet sheet)
	{
		#region Title
		sheet.SetColumn(Column++, "alias", 30);
		sheet.SetColumn(Column++, "name2", 30);
		sheet.SetColumn(Column++, "title2", 30);
		#endregion


		foreach (var record in Source.Provider.GetTable<Npc>())
		{
			Row++;

			int column = 1;
			sheet.Cells[Row, column++].SetValue(record.Attributes["alias"]);
			sheet.Cells[Row, column++].SetValue(record.Attributes["name2"].GetText());
			sheet.Cells[Row, column++].SetValue(record.Attributes["title2"].GetText());
		}
	}
}