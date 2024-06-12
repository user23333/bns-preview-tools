using System.IO;
using CUE4Parse.Utils;
using HandyControl.Controls;
using OfficeOpenXml;
using Ookii.Dialogs.Wpf;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.UI.Common.Converters;

namespace Xylia.Preview.UI.Helpers.Output;
public abstract class OutSet
{
	#region Fields
	protected ExcelPackage? Package;
	protected BnsDatabase Source { get; set; } = FileCache.Data;
	#endregion

	#region Properies
	public virtual string Name => GetType().Name.SubstringBefore("Out", StringComparison.OrdinalIgnoreCase);
	#endregion


	#region Methods
	protected ExcelWorksheet CreateSheet(string? name = null)
	{
		ArgumentNullException.ThrowIfNull(Package);

		var sheet = Package.Workbook.Worksheets.Add(name ?? this.Name);
		sheet.Cells.Style.Font.Name = "宋体";
		sheet.Cells.Style.Font.Size = 11F;
		sheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
		sheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

		return sheet;
	}

	/// <summary>
	/// Create xlsx file
	/// </summary>
	/// <param name="sheet"></param>
	protected abstract void CreateData();

	/// <summary>
	/// Create text file
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	protected virtual void CreateText() => throw new NotImplementedException();


	public Task Output(FileInfo path) => Task.Run(() =>
	{
		ArgumentNullException.ThrowIfNull(path);

		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		Package = new ExcelPackage();

		//main sheet
		CreateData();
		Package.SaveAs(path.FullName);

		GC.Collect();
	});

	/// <summary>
	/// entry method for output
	/// </summary>
	public static async Task Start<T>() where T : OutSet, new()
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

		Growl.Success(StringHelper.Get("Text.TaskCompleted2", TimeConverter.Convert(DateTime.Now - dt, null)));
	}
	#endregion
}