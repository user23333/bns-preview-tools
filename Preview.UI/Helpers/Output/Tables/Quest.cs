using OfficeOpenXml;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal sealed class QuestOut : OutSet
{
    protected override void CreateData(ExcelWorksheet sheet)
    {
        #region Title
        sheet.SetColumn(Column++, "id", 10);
        sheet.SetColumn(Column++, "alias", 15);
        sheet.SetColumn(Column++, "name", 30);
        sheet.SetColumn(Column++, "group", 25);
        sheet.SetColumn(Column++, "category", 10);
        sheet.SetColumn(Column++, "content-type", 10);
        sheet.SetColumn(Column++, "reset-type", 10);
        sheet.SetColumn(Column++, "retired", 10);
        sheet.SetColumn(Column++, "tutorial", 10);
        #endregion


        foreach (var Quest in Source.Provider.GetTable<Quest>().OrderBy(x => x.PrimaryKey))
        {
            Row++;

            int column = 1;
            sheet.Cells[Row, column++].SetValue(Quest.PrimaryKey);
            sheet.Cells[Row, column++].SetValue(Quest.Attributes["alias"]);
            sheet.Cells[Row, column++].SetValue(Quest.Name);
            sheet.Cells[Row, column++].SetValue(Quest.Title);
            sheet.Cells[Row, column++].SetValue(Quest.Attributes["category"]);
            sheet.Cells[Row, column++].SetValue(Quest.Attributes["content-type"]);
            sheet.Cells[Row, column++].SetValue(Quest.Attributes["reset-type"]);
            sheet.Cells[Row, column++].SetValue(Quest.Attributes["retired"]);
            sheet.Cells[Row, column++].SetValue(Quest.Attributes["tutorial"]);
        }
    }
}