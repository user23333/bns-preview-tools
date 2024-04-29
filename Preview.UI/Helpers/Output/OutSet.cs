using System.IO;
using CUE4Parse.Utils;
using HandyControl.Controls;
using OfficeOpenXml;
using Ookii.Dialogs.Wpf;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.UI.Helpers.Output;
public abstract class OutSet
{
	#region Fields
	protected ExcelPackage? package;
	protected BnsDatabase Source { get; set; } = FileCache.Data;
	#endregion

	#region Properies
	public virtual string Name => GetType().Name.SubstringBefore("Out", StringComparison.OrdinalIgnoreCase);

	public int Column { get; protected set; } = 1;

	public int Row { get; protected set; } = 1;
	#endregion


	#region Methods
	public Task Output(FileInfo path) => Task.Run(() =>
	{
		ArgumentNullException.ThrowIfNull(path);

		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		package = new ExcelPackage();
		var sheet = package.Workbook.Worksheets.Add(Name);
		sheet.Cells.Style.Font.Name = "宋体";
		sheet.Cells.Style.Font.Size = 11F;
		sheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
		sheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

		CreateData(sheet);

		package.SaveAs(path.FullName);

		GC.Collect();
	});

	/// <summary>
	/// Create xlsx file
	/// </summary>
	/// <param name="sheet"></param>
	protected abstract void CreateData(ExcelWorksheet sheet);

	/// <summary>
	/// Create text file
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	protected virtual void CreateText() => throw new NotImplementedException();


	/// <summary>
	/// entry method for output
	/// </summary>
	public static async void Start<T>() where T : OutSet, new()
	{
		var instance = new T();

		var save = new VistaSaveFileDialog
		{
			Filter = "Excel Files|*.xlsx",
			FileName = $"{instance.Name} ({DateTime.Now:yyyyMMdd}).xlsx",
		};
		if (save.ShowDialog() != true) return;

		DateTime dt = DateTime.Now;
		await instance.Output(new FileInfo(save.FileName));

		Growl.Success(StringHelper.Get("ItemList_TaskCompleted2", 0, (DateTime.Now - dt).TotalSeconds));
	}
	#endregion
}