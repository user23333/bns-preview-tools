using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.Win32;
using OfficeOpenXml;
using Ookii.Dialogs.Wpf;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Engine.BinData.Serialization;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Helpers.Output;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Views.Editor;
public partial class DatabaseStudio
{
	#region Constructors
	static DatabaseStudio()
	{
		TextEditor.Register("Sql");
	}

	public DatabaseStudio()
	{
		DataContext = _viewModel = new DatabaseStudioViewModel();
		InitializeComponent();
		RegisterCommands(this.CommandBindings);

		PageHolder.SelectedItem = Page_SQL;
	}
	#endregion

	#region Command
	private void RegisterCommands(CommandBindingCollection commandBindings)
	{
		commandBindings.Add(new CommandBinding(ApplicationCommands.Close, (_, _) => SaveMessage.Visibility = Visibility.Collapsed));
		commandBindings.Add(new CommandBinding(ApplicationCommands.Print, RunCommand, CanExecuteRun));
	}

	private void CanExecuteRun(object sender, CanExecuteRoutedEventArgs e)
	{
		e.CanExecute = _viewModel.Database != null && !string.IsNullOrEmpty(ActivateSql);
	}

	private async void RunCommand(object sender, RoutedEventArgs e)
	{
		try
		{
			this.Run.IsEnabled = false;

			var source = new CancellationTokenSource();
			await ExecuteSql(ActivateSql, source.Token);
		}
		catch (Exception ex)
		{
			Growl.Error(ex.Message, TOKEN);
		}
		finally
		{
			this.Run.IsEnabled = true;
		}
	}
	#endregion


	#region Methods (UI)
	private async void Connect_Click(object sender, RoutedEventArgs e)
	{
		if (_viewModel.Database == null)
		{
			var dialog = new DatabaseManager() { Owner = this };
			if (dialog.ShowDialog() != true) return;

			// init param	 
			ArgumentNullException.ThrowIfNull(dialog.Engine);
			_viewModel.Database = dialog.Engine;
			_viewModel.IsGlobalData = dialog.IsGlobalData;

			// loading
			Connect.IsEnabled = false;
			tvwDatabase.Items.Add(new TreeViewItem() { Header = "loading..." });

			await Task.Run(() => _viewModel.Database!.Initialize());

			LoadTreeView();
			Connect.IsEnabled = _viewModel.ConnectStatus = true;
		}
		else
		{
			if (_viewModel.IsGlobalData)
			{
				var result = HandyControl.Controls.MessageBox.Show(
					StringHelper.Get("DatabaseStudio_ConnectMessage1"),
					StringHelper.Get("Message_Tip"), MessageBoxButton.YesNo);

				if (result != MessageBoxResult.Yes) return;
				else FileCache.Data = null;
			}

			// disconnect
			_viewModel.Database.Dispose();
			_viewModel.Database = null;

			tvwDatabase.Items.Clear();
			_viewModel.ConnectStatus = false;

			GC.Collect();
		}
	}

	protected override void OnClosed(EventArgs e)
	{
		if (!_viewModel.IsGlobalData)
		{
			_viewModel.Database?.Dispose();
			_viewModel.Database = null;
		}

		GC.Collect();
		base.OnClosed(e);
	}


	private void TvwDatabase_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (tvwDatabase.SelectedItem is FrameworkElement item && item.DataContext is Table table)
		{
			_viewModel.CurrentTable = table;
		}
		else
		{
			_viewModel.CurrentTable = null;
		}
	}

	private void TvwDatabase_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		if (tvwDatabase.SelectedItem is FrameworkElement item)
		{
			if (item.DataContext is Table table)
			{
				string sql = $"SELECT * FROM \"{table.Name}\"\nLIMIT {_viewModel.LimitNum}";
				AddTab(sql, table.Name);
			}
		}
	}

	private void Refresh_Click(object sender, RoutedEventArgs e)
	{
		LoadTreeView();
	}


	private void NewSql_Click(object sender, RoutedEventArgs e)
	{
		AddTab(string.Empty);
	}

	private void LoadSql_Click(object sender, RoutedEventArgs e)
	{
		var dialog = new VistaOpenFileDialog();
		if (dialog.ShowDialog() != true) return;

		// valid
		var text = File.ReadAllText(dialog.FileName);
		var header = Path.GetFileName(dialog.FileName);
		AddTab(text, header);
	}

	private void SaveSql_Click(object sender, RoutedEventArgs e)
	{
		// valid
		var text = ActivateSql;
		if (text is null) return;

		// save
		var dialog = new VistaSaveFileDialog()
		{
			Filter = "|*.sql",
			FileName = "Query.sql",
		};
		if (dialog.ShowDialog() == true) File.WriteAllText(dialog.FileName, text);
	}

	private void OutputExcel_Click(object sender, RoutedEventArgs e)
	{
		var save = new VistaSaveFileDialog
		{
			Filter = "Excel Files|*.xlsx",
			FileName = $"query.xlsx",
		};
		if (save.ShowDialog() != true) return;


		#region Sheet
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		var package = new ExcelPackage();
		var sheet = package.Workbook.Worksheets.Add("Sheet");
		sheet.Cells.Style.Font.Name = "宋体";
		sheet.Cells.Style.Font.Size = 11F;
		sheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
		sheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
		#endregion

		#region Title
		for (int j = 0; j < QueryGrid.Columns.Count; j++)
		{
			sheet.SetColumn(j + 1, QueryGrid.Columns[j].Header.ToString());
		}
		#endregion

		#region Row
		for (int i = 0; i < QueryGrid.Items.Count; i++)
		{
			var row = i + 2;
			var item = QueryGrid.Items[i] as DataRowView;

			for (int j = 0; j < QueryGrid.Columns.Count; j++)
			{
				var col = QueryGrid.Columns[j];

				var cell = sheet.Cells[row, j + 1];
				cell.SetValue(item![col.Header.ToString()!]);
			}
		}
		#endregion

		package.SaveAs(save.FileName);
	}

	private void OutputText_Click(object sender, RoutedEventArgs e)
	{
		var save = new VistaSaveFileDialog
		{
			Filter = "Text Files|*.txt",
			FileName = $"query.txt",
		};
		if (save.ShowDialog() != true) return;

		File.WriteAllText(save.FileName, QueryText.Text);
	}


	private void ViewTable_Click(object sender, RoutedEventArgs e)
	{
		if (_viewModel.CurrentTable is null) return;

		var window = new TableView { Table = _viewModel.CurrentTable };
		window.Show();
	}

	private void ViewDefinition_Click(object sender, RoutedEventArgs e)
	{
		if (_viewModel.CurrentTable is null) return;

		_viewModel.CurrentDefinition = _viewModel.CurrentTable.Definition;
		PageHolder.SelectedItem = Page_Definition;
	}

	private async void TableExport_Click(object sender, RoutedEventArgs e)
	{
		if (_viewModel.CurrentTable is null) return;

		await ExportAsync(_viewModel.CurrentTable);
	}

	private async void TableExportAll_Click(object sender, RoutedEventArgs e)
	{
		if (_viewModel.Database is not BnsDatabase database) return;

		await ExportAsync([.. database.Provider.Tables]);
	}

	private async void Import_Click(object sender, RoutedEventArgs e)
	{
		if (_viewModel.Database is not BnsDatabase database) return;

		try
		{
			DateTime dt = DateTime.Now;
			Growl.Info("Start import", TOKEN);

			serialize = new ProviderSerialize(database.Provider);
			await serialize.ImportAsync(_viewModel.SaveDataPath);

			Growl.Success(new GrowlInfo()
			{
				Token = TOKEN,
				Message = "Import finished, " + (DateTime.Now - dt).TotalSeconds,
				StaysOpen = false,
			});
		}
		catch (Exception ex)
		{
			Growl.Error(ex.Message, TOKEN);
		}
	}

	private async void Save_Click(object sender, RoutedEventArgs e)
	{
		if (_viewModel.Database is not BnsDatabase database) return;

		var dialog = new OpenFolderDialog();
		if (dialog.ShowDialog() == true)
		{
			serialize ??= new ProviderSerialize(database.Provider);
			await serialize.SaveAsync(dialog.FolderName);

			Growl.Success(new GrowlInfo()
			{
				Token = TOKEN,
				Message = "Save finished",
				StaysOpen = true,
			});
		}
	}


	//------------------------------------------------------
	//
	//  Page_Definition
	//
	//------------------------------------------------------
	private void ReturnBtn_Click(object sender, RoutedEventArgs e)
	{
		PageHolder.SelectedItem = Page_SQL;
	}

	private void AttributeName_MouseDown(object sender, MouseButtonEventArgs e)
	{
		// copy attribute name
		if (sender is TextBlock textBlock && e.ClickCount == 2)
		{
			Clipboard.SetText(textBlock.Text);
		}
	}
	#endregion

	#region Methods
	private void LoadTreeView()
	{
		tvwDatabase.Items.Clear();
		if (_viewModel.Database is null) return;

		// system nodes  		
		var root = new TreeViewImageItem { Image = ImageHelper.Database, Header = _viewModel.Database.Name, IsExpanded = true };
		var system = new TreeViewImageItem { Image = ImageHelper.Folder, Header = "System", IsExpanded = false };
		tvwDatabase.Items.Add(root);
		root.Items.Add(system);

		if (_viewModel.Database is BnsDatabase bns)
		{
			system.Items.Add(new TreeViewImageItem { Image = ImageHelper.TableSys, Header = "Publisher: " + bns.Provider.Locale.Publisher });
			system.Items.Add(new TreeViewImageItem { Image = ImageHelper.TableSys, Header = "Created: " + bns.Provider.CreatedAt });
			system.Items.Add(new TreeViewImageItem { Image = ImageHelper.TableSys, Header = "Version: " + bns.Provider.Locale.ProductVersion });  //ClientVersion

			// table nodes
			foreach (var table in bns.Provider.Tables.OrderBy(x => x.Type))
			{
				// text
				var text = table.Type.ToString();
				if (table.Name != null) text = $"{table.Name.ToLower()} ({table.Type})";

				// node
				root.Items.Add(new TreeViewImageItem
				{
					DataContext = table,
					Header = text,
					Image = ImageHelper.Table,
					ContextMenu = this.TryFindResource("TableMenu") as ContextMenu,
					Margin = new Thickness(0, 0, 0, 2),
				});
			}
		}
		else throw new NotSupportedException();
	}

	/// <summary>
	/// update right-bottom message
	/// </summary>
	/// <param name="text"></param>
	private void UpdateMessage(string text)
	{
		MessageHolder.Text = text;
	}

	/// <summary>
	/// append new sql tab
	/// </summary>
	/// <param name="sql"></param>
	/// <param name="title"></param>
	private void AddTab(string sql, string? title = null)
	{
		title ??= ("Page" + (editors.Items.Count + 1));
		var item = new HandyControl.Controls.TabItem()
		{
			Content = new ICSharpCode.AvalonEdit.TextEditor() { Text = sql },
			DataContext = null,   // fix issue with expand name, don't remove
			Header = title,
			ToolTip = title,
		};

		editors.Items.Insert(0, item);
		editors.SelectedItem = item;
	}

	private string ActivateSql
	{
		get
		{
			var item = (editors.SelectedItem as ContentControl)?.Content;
			if (item is not ICSharpCode.AvalonEdit.TextEditor editor) return string.Empty;

			return editor.Text;
		}
	}

	private async Task ExecuteSql(string sql, CancellationToken token)
	{
		ArgumentNullException.ThrowIfNull(_viewModel.Database);

		var startTime = DateTime.Now;
		var callback = new EventHandler((s, e) => UpdateMessage(string.Format("{0} {1:h\\:mm\\:ss\\.fff}", "Execution Time:", DateTime.Now - startTime)));
		var timer = new DispatcherTimer(new TimeSpan(TimeSpan.TicksPerMillisecond * 50), DispatcherPriority.Normal, callback, Dispatcher);
		timer.Start();

		try
		{
			await Task.Run(() => _viewModel.ReadResult(_viewModel.Database.Execute(sql)), token);

			// update
			_viewModel.BindData(this.QueryGrid);
			_viewModel.BindData(this.QueryText);
			PageHolder.SelectedItem = Page_SQL;
		}
		catch
		{
			throw;
		}
		finally
		{
			timer.Stop();
			callback.Invoke(timer, EventArgs.Empty);
		}
	}

	private async Task ExportAsync(params Table[] tables)
	{
		if (_viewModel.Database is not BnsDatabase database) return;

		var progress = new Action<int, int>((current, total) => Dispatcher.Invoke(() =>
		{
			SaveMessage.Visibility = Visibility.Visible;
			SaveMessage.Text = current != tables.Length ?
				StringHelper.Get("DatabaseStudio_TaskMessage1", current, tables.Length, (double)current / tables.Length) :
				StringHelper.Get("DatabaseStudio_TaskMessage2", tables.Length);
		}));

		serialize = new ProviderSerialize(database.Provider);
		await serialize.ExportAsync(_viewModel.SaveDataPath, progress, tables);
	}
	#endregion

	#region Private Fields
	internal static string TOKEN = nameof(DatabaseStudio);

	private ProviderSerialize? serialize;
	private readonly DatabaseStudioViewModel _viewModel;
	#endregion
}

internal static class ImageHelper
{
	public static BitmapImage Table { get; } = new BitmapImage(new Uri("/Resources/Images/table2.png", UriKind.Relative));

	public static BitmapImage TableSys { get; } = new BitmapImage(new Uri("/Resources/Images/table_set.png", UriKind.Relative));

	public static BitmapImage Database { get; } = new BitmapImage(new Uri("/Resources/Images/database.png", UriKind.Relative));

	public static BitmapImage Folder { get; } = new BitmapImage(new Uri("/Resources/Images/folder.png", UriKind.Relative));
}