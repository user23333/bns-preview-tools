using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using HandyControl.Controls;
using OfficeOpenXml;
using Ookii.Dialogs.Wpf;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Helpers.Output;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Views.Editor;
public partial class DatabaseStudio
{
	#region Fields
	private readonly DatabaseStudioViewModel _viewModel;
	public readonly static string TOKEN = nameof(DatabaseStudio);
	#endregion

	#region Constructors
	static DatabaseStudio()
	{
		TextEditor.Register("Sql");
	}

	public DatabaseStudio()
	{
		DataContext = _viewModel = new DatabaseStudioViewModel(UpdateMessage);
		InitializeComponent();
		RegisterCommands(this.CommandBindings);

		PageHolder.SelectedItem = Page_SQL;
	}
	#endregion

	#region Command
	private void RegisterCommands(CommandBindingCollection commandBindings)
	{
		commandBindings.Add(new CommandBinding(ApplicationCommands.Print, RunCommand, CanExecuteRun));
	}

	private void CanExecuteRun(object sender, CanExecuteRoutedEventArgs e)
	{
		e.CanExecute = _viewModel.Database != null && _viewModel.Sql != null;
	}

	private async void RunCommand(object sender, RoutedEventArgs e)
	{
		try
		{
			this.Run.IsEnabled = false;

			var source = new CancellationTokenSource();
			await ExecuteSql(_viewModel.Sql, source.Token);
		}
		catch (Exception ex)
		{
			Growl.Error(ex.Message, nameof(DatabaseStudio));
		}
		finally
		{
			this.Run.IsEnabled = true;
		}
	}
	#endregion


	#region Methods
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
			_viewModel.ConnectStatus = false;
			tvwDatabase.Items.Add(new TreeViewItem() { Header = "loading..." });

			await Task.Run(() => _viewModel.Database!.Initialize());

			LoadTreeView();
			_viewModel.ConnectStatus = true;
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
			_viewModel.ConnectStatus = null;

			GC.Collect();
		}
	}

	/// <summary>
	/// update right-bottom message
	/// </summary>
	/// <param name="text"></param>
	private void UpdateMessage(string text)
	{
		Dispatcher.Invoke(() => MessageHolder.Text = text);
	}

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
					Tag = table,
					Header = text,
					Image = ImageHelper.Table,
					ContextMenu = this.TryFindResource("TableMenu") as ContextMenu,
					Margin = new Thickness(0, 0, 0, 2),
				});
			}
		}
		else throw new NotSupportedException();
	}

	private void Refresh_Click(object sender, RoutedEventArgs e)
	{
		LoadTreeView();
	}

	private void TvwDatabase_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (tvwDatabase.SelectedItem is FrameworkElement item)
		{
			_viewModel.CurrentTable = item.Tag as Table;
		}
	}

	private void TvwDatabase_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		if (tvwDatabase.SelectedItem is FrameworkElement item)
		{
			if (item.Tag is Table table)
			{
				string sql = $"SELECT * FROM \"{table.Name}\"\nLIMIT {_viewModel.LimitNum}";
				_viewModel.Append(new SQL(sql, table.Name));
			}
		}
	}

	// SQL Holder
	private async Task ExecuteSql(SQL sql, CancellationToken token)
	{
		ArgumentNullException.ThrowIfNull(_viewModel.Database);

		var startTime = DateTime.Now;
		var callback = new EventHandler((s, e) => UpdateMessage(string.Format("{0} {1:h\\:mm\\:ss\\.fff}", StringHelper.Get("DatabaseStudio_ExecutionTime"), DateTime.Now - startTime)));
		var timer = new DispatcherTimer(new TimeSpan(TimeSpan.TicksPerMillisecond * 50), DispatcherPriority.Normal, callback, Dispatcher);
		timer.Start();

		try
		{
			await Task.Run(() => sql.ReadResult(_viewModel.Database.Execute(sql.Text)), token);

			// update
			_viewModel.BindData(sql, QueryGrid);
			_viewModel.BindData(sql, QueryText);
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

	//------------------------------------------------------
	//
	//  Page_Definition
	//
	//------------------------------------------------------
	private void AttributeName_MouseDown(object sender, MouseButtonEventArgs e)
	{
		// copy attribute name
		if (sender is TextBlock textBlock && e.ClickCount == 2)
		{
			Clipboard.SetText(textBlock.Text);
		}
	}
	#endregion
}


internal static class ImageHelper
{
	public static BitmapImage Table { get; } = new BitmapImage(new Uri("/Resources/Images/table2.png", UriKind.Relative));

	public static BitmapImage TableSys { get; } = new BitmapImage(new Uri("/Resources/Images/table_set.png", UriKind.Relative));

	public static BitmapImage Database { get; } = new BitmapImage(new Uri("/Resources/Images/database.png", UriKind.Relative));

	public static BitmapImage Folder { get; } = new BitmapImage(new Uri("/Resources/Images/folder.png", UriKind.Relative));
}