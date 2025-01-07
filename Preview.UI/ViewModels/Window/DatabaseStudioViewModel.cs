using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Win32;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Engine.BinData.Serialization;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Services;
using Xylia.Preview.UI.Views;
using Xylia.Preview.UI.Views.Editor;

namespace Xylia.Preview.UI.ViewModels;
internal partial class DatabaseStudioViewModel(Action<string> Message) : ObservableObject
{
	#region Common
	[ObservableProperty] private int _selectedPage;
	[ObservableProperty] private bool? _connectStatus;

	public bool UseImport => UserService.Instance?.Role >= UserRole.Advanced;

	[RelayCommand]
	private void SwitchPage(object param)
	{
		SelectedPage = param.To<int>();
	}
	#endregion

	#region Provider
	public IEngine? Database;
	public bool IsGlobalData = false;
	private ProviderSerialize? serialize;

	private string SaveDataPath => Path.Combine(UserSettings.Default.OutputFolder, Database!.Desc, Database!.CreatedAt.ToString("yyMMdd"));

	[RelayCommand]
	private async Task Export()
	{
		ArgumentNullException.ThrowIfNull(CurrentTable);
		await ExportAsync(CurrentTable);
	}

	[RelayCommand]
	private async Task ExportAll()
	{
		if (Database is not BnsDatabase database) return;

		await ExportAsync([.. database.Provider.Tables]);
		database.Provider.Locale.Save(SaveDataPath);
	}

	private async Task ExportAsync(params Table[] tables)
	{
		if (Database is not BnsDatabase database) return;

		serialize = new ProviderSerialize(database.Provider);
		await serialize.ExportAsync(SaveDataPath, (current, total) =>
			Message(current != tables.Length ?
				StringHelper.Get("DatabaseStudio_ExportMessage1", current, tables.Length, (double)current / tables.Length) :
				StringHelper.Get("DatabaseStudio_ExportMessage2", tables.Length))
		, tables);
	}

	[RelayCommand]
	private async Task Import()
	{
		if (Database is not BnsDatabase database) throw new NotSupportedException();

		try
		{
			Growl.Info(StringHelper.Get("DatabaseStudio_ImportMessage0"), DatabaseStudio.TOKEN);

			serialize = new ProviderSerialize(database.Provider);
			await serialize.ImportAsync(SaveDataPath, (code, arg0) => Message(StringHelper.Get("DatabaseStudio_ImportMessage" + code, arg0)));

			Growl.Success(StringHelper.Get("DatabaseStudio_ImportMessage4"), DatabaseStudio.TOKEN);
		}
		catch (Exception ex)
		{
			Growl.Error(ex.Message, DatabaseStudio.TOKEN);
		}
	}

	[RelayCommand]
	private async Task Save()
	{
		if (Database is not BnsDatabase database) throw new NotSupportedException();

		var dialog = new OpenFolderDialog();
		if (dialog.ShowDialog() == true)
		{
			serialize ??= new ProviderSerialize(database.Provider);
			await serialize.SaveAsync(dialog.FolderName);

			Growl.Success(new GrowlInfo()
			{
				Token = DatabaseStudio.TOKEN,
				Message = StringHelper.Get("DatabaseStudio_SaveMessage"),
				StaysOpen = true,
			});
		}
	}
	#endregion

	#region Table
	[ObservableProperty]
	private Table? _currentTable;

	[RelayCommand]
	private void ViewTable()
	{
		if (CurrentTable is null) return;

		var window = new TableView { Table = CurrentTable };
		window.Show();
	}
	#endregion

	#region Query
	public ObservableCollection<SQL> Sqls { get; } = [];

	[ObservableProperty]
	private SQL? _sql;

	[ObservableProperty]
	private int _limitNum = 2000;

	[ObservableProperty]
	private bool _indentText = true;

	[RelayCommand]
	private void SqlNew()
	{
		Append(new SQL(string.Empty));
	}

	[RelayCommand]
	private void SqlLoad()
	{
		var dialog = new VistaOpenFileDialog();
		if (dialog.ShowDialog() != true) return;

		var text = File.ReadAllText(dialog.FileName);
		var header = Path.GetFileName(dialog.FileName);
		Append(new SQL(text, header));
	}

	public void Append(SQL sql)
	{
		sql.Title ??= "Page" + (Sqls.Count + 1);

		Sqls.Insert(0, sql);
		Sql = sql;
	}

	public void BindData(SQL sql, DataGrid grd)
	{
		ArgumentNullException.ThrowIfNull(sql.QueryResult);

		using var dt = new System.Data.DataTable();

		if (sql.QueryResult.Count == 0)
		{
			dt.Columns.Add("no-data");
			dt.Rows.Add("[no result]");
		}
		else
		{
			foreach (var value in sql.QueryResult)
			{
				var doc = value.IsDocument ? value.AsDocument : new AttributeDocument { ["[value]"] = value };
				if (doc.Count == 0) doc["[root]"] = "{}";

				var row = dt.NewRow();

				foreach (var key in doc)
				{
					if (!dt.Columns.Contains(key.Key)) dt.Columns.Add(key.Key);

					row[key.Key] = value.IsDocument ? value[key.Key] : value;
				}

				dt.Rows.Add(row);
			}
		}

		grd.ItemsSource = dt.DefaultView;
	}

	public void BindData(SQL sql, ICSharpCode.AvalonEdit.TextEditor editor)
	{
		ArgumentNullException.ThrowIfNull(sql.QueryResult);

		var builder = new StringBuilder();
		var settings = new JsonSerializerSettings()
		{
			Formatting = IndentText ? Formatting.Indented : Formatting.None,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		};

		if (sql.QueryResult.Count == 0)
		{
			builder.AppendLine("no result");
		}
		else
		{
			var index = 1;
			foreach (var value in sql.QueryResult)
			{
				builder.AppendLine($"/* {index++} */");
				builder.AppendLine(JsonConvert.SerializeObject(value, settings));
				builder.AppendLine();
			}

			// LimitExceeded
			if (false)
			{
				builder.AppendLine();
				builder.AppendLine("/* Limit exceeded */");
			}
		}

		editor.Text = builder.ToString();
	}
	#endregion
}

internal partial class SQL(string text, string? title = null) : ObservableObject
{
	#region Property
	[ObservableProperty]
	private string? _title = title;

	[ObservableProperty]
	private string? _text = text;

	private TextDocument? _textDocument;
	public TextDocument TextDocument
	{
		get
		{
			if (_textDocument is null)
			{
				var doc = new TextDocument(Text);
				doc.TextChanged += (_, _) => Text = doc.Text;

				return _textDocument = doc;
			}

			return _textDocument;
		}
	}

	public List<AttributeValue>? QueryResult { get; set; }
	#endregion

	#region Methods
	[RelayCommand]
	private void Save()
	{
		if (Text is null) return;

		// save
		var dialog = new SaveFileDialog()
		{
			Filter = "|*.sql",
			FileName = "Query.sql",
		};
		if (dialog.ShowDialog() == true) File.WriteAllText(dialog.FileName, Text);
	}

	public void ReadResult(IDataReader reader)
	{
		QueryResult = [];

		while (reader.Read())
		{
			QueryResult.Add(reader.Current);
		}
	}
	#endregion
}