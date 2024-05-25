using OfficeOpenXml;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Documents;
using Xylia.Preview.UI.Documents.Primitives;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal class Skill3Out : OutSet
{
	protected override void CreateData(ExcelWorksheet sheet)
	{
		#region Title
		sheet.SetColumn(Column++, "key", 15);
		sheet.SetColumn(Column++, "alias", 70);
		sheet.SetColumn(Column++, "name", 20);
		sheet.SetColumn(Column++, "main-info", 50);
		sheet.SetColumn(Column++, "sub-info", 50);
		#endregion

		#region Data
		foreach (var record in Source.Provider.GetTable<Skill3>())
		{
			Row++;
			int column = 1;

			sheet.Cells[Row, column++].SetValue(record.PrimaryKey);
			sheet.Cells[Row, column++].SetValue(record);
			sheet.Cells[Row, column++].SetValue(record.Name);
			sheet.Cells[Row, column++].SetValue(TextContainer.Cut(
				string.Join(BR.Tag, record.MainTooltip1.SelectNotNull(x => x.Instance)) + BR.Tag +
				string.Join(BR.Tag, record.MainTooltip2.SelectNotNull(x => x.Instance))));
			sheet.Cells[Row, column++].SetValue(TextContainer.Cut(string.Join(BR.Tag, record.SubTooltip.SelectNotNull(x => x.Instance))));
		}
		#endregion
	}
}