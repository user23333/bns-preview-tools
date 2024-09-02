using OfficeOpenXml;

namespace Xylia.Preview.UI.Helpers.Output;
public static class FileExtension
{
    public static void SetColumn(this ExcelWorksheet sheet, int index, string? header, int width = 10)
    {
		sheet.Column(index).Width = width;
        sheet.Cells[1, index].Value = header;
    }

    public static void SetValue(this ExcelRange cell, object? value)
    {
        if (value is null) return;

		cell.Value = value;
	}
}