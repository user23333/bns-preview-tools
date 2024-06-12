using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal sealed class QuestOut : OutSet
{
    protected override void CreateData()
    {
		#region Title
		var sheet = CreateSheet();
		int column = 1, row = 1;
		sheet.SetColumn(column++, "id", 10);
        sheet.SetColumn(column++, "alias", 15);
        sheet.SetColumn(column++, "name", 30);
        sheet.SetColumn(column++, "group", 25);
        sheet.SetColumn(column++, "category", 10);
        sheet.SetColumn(column++, "content-type", 10);
        sheet.SetColumn(column++, "reset-type", 10);
        sheet.SetColumn(column++, "retired", 10);
        sheet.SetColumn(column++, "tutorial", 10);
		#endregion

		#region Data
		foreach (var Quest in Source.Provider.GetTable<Quest>().OrderBy(x => x.PrimaryKey))
        {
			row++;
            column = 1;

            sheet.Cells[row, column++].SetValue(Quest.PrimaryKey);
            sheet.Cells[row, column++].SetValue(Quest.Attributes["alias"]);
            sheet.Cells[row, column++].SetValue(Quest.Name);
            sheet.Cells[row, column++].SetValue(Quest.Title);
            sheet.Cells[row, column++].SetValue(Quest.Attributes["category"]);
            sheet.Cells[row, column++].SetValue(Quest.Attributes["content-type"]);
            sheet.Cells[row, column++].SetValue(Quest.Attributes["reset-type"]);
            sheet.Cells[row, column++].SetValue(Quest.Attributes["retired"]);
            sheet.Cells[row, column++].SetValue(Quest.Attributes["tutorial"]);
        }
		#endregion
	}
}