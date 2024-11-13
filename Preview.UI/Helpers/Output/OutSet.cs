using System.IO;
using System.Reflection;
using CUE4Parse.Utils;
using HandyControl.Controls;
using OfficeOpenXml;
using Ookii.Dialogs.Wpf;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Client;
using Xylia.Preview.UI.Common.Converters;

namespace Xylia.Preview.UI.Helpers.Output;
internal abstract class OutSet
{
	#region Constructor

	static OutSet()
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
	}

	#endregion

	#region Properties
	public virtual string Name => GetType().Name.SubstringBefore("Out", StringComparison.OrdinalIgnoreCase);

	protected virtual BnsDatabase? Source { get; set; } = Globals.GameData;
	#endregion

	#region Methods
	protected ExcelWorksheet CreateSheet(ExcelPackage package, string? name = null)
	{
		ArgumentNullException.ThrowIfNull(package);

		var sheet = package.Workbook.Worksheets.Add(name ?? this.Name);
		sheet.Cells.Style.Font.Name = "Microsoft YaHei";
		sheet.Cells.Style.Font.Size = 11F;
		sheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
		sheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
		sheet.Cells.Style.WrapText = true;
		return sheet;
	}

	/// <summary>
	/// Create xlsx file
	/// </summary>
	/// <param name="sheet"></param>
	protected abstract void CreateData(ExcelPackage package);

	/// <summary>
	/// Create text file
	/// </summary>
	/// <exception cref="NotImplementedException"></exception>
	protected virtual void CreateText(TextWriter writer) => throw new NotImplementedException();

	public void Execute()
	{
		var save = new VistaSaveFileDialog
		{
			Filter = "Excel Files|*.xlsx",
			FileName = $"{Name} ({DateTime.Now:yyyyMMdd}).xlsx",
		};
		if (save.ShowDialog() != true) throw new OperationCanceledException(); 

		Execute(save.FileName);
	}

	public void Execute(string path)
	{
		ArgumentNullException.ThrowIfNull(path);

		var package = new ExcelPackage();
		CreateData(package);
		package.SaveAs(path);

		GC.Collect();
	}


	public static IEnumerable<OutSet> Find()
	{
		var assembly = Assembly.GetExecutingAssembly();
		var baseType = typeof(OutSet);

		foreach (var definedType in assembly.DefinedTypes)
		{
			if (definedType.IsAbstract || definedType.IsInterface || !baseType.IsAssignableFrom(definedType)) continue;

			if (Activator.CreateInstance(definedType) is OutSet instance)
				yield return instance;
		}

		yield break;
	}

	/// <summary>
	/// entry method for output
	/// </summary>
	internal static async Task Start<T>() where T : OutSet, new()
	{
		DateTime dt = DateTime.Now;
		await Task.Run(() => new T().Execute());

		Growl.Success(StringHelper.Get("Text.TaskCompleted2", TimeConverter.Convert(DateTime.Now - dt, null)));
	}
	#endregion
}