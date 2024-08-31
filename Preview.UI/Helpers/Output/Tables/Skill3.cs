using OfficeOpenXml;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.UI.Documents.Primitives;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal class Skill3Out : OutSet
{
	protected override void CreateData(ExcelPackage package)
	{
		#region Title
		var sheet = CreateSheet(package);
		int column = 1, row = 1;

		sheet.SetColumn(column++, "key", 15);
		sheet.SetColumn(column++, "alias", 70);
		sheet.SetColumn(column++, "name", 20);
		sheet.SetColumn(column++, "main-info", 50);
		sheet.SetColumn(column++, "sub-info", 50);
		#endregion

		#region Data
		foreach (var record in Source!.Provider.GetTable<Skill3>())
		{
			row++;
			column = 1;

			sheet.Cells[row, column++].SetValue(record.PrimaryKey);
			sheet.Cells[row, column++].SetValue(record);
			sheet.Cells[row, column++].SetValue(record.Name);
			sheet.Cells[row, column++].SetValue(TextContainer.Cut(
				string.Join(BR.Tag, record.MainTooltip1.SelectNotNull(x => x.Instance)) + BR.Tag +
				string.Join(BR.Tag, record.MainTooltip2.SelectNotNull(x => x.Instance))));
			sheet.Cells[row, column++].SetValue(TextContainer.Cut(string.Join(BR.Tag, record.SubTooltip.SelectNotNull(x => x.Instance))));
		}
		#endregion
	}
}