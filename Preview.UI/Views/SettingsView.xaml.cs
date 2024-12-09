using System.Windows;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views.Editor;

namespace Xylia.Preview.UI.Views;
public partial class SettingsView : Window
{
	public SettingsView()
	{
		Owner = Application.Current.MainWindow;
		DataContext = UserSettings.Default;
		InitializeComponent();
#if !DEBUG
		Models.Visibility = Visibility.Collapsed;
#endif
	}

	#region Methods
	public static bool TryBrowseFolder(out string path)
	{
		var dialog = new OpenFolderDialog()
		{

		};

		if (dialog.ShowDialog() == true)
		{
			path = dialog.FolderName;
			return true;
		}

		path = string.Empty;
		return false;
	}

	public static bool TryBrowse(out string path, string? filter = null)
	{
		var dialog = new VistaOpenFileDialog()
		{
			Filter = filter,
			RestoreDirectory = false
		};

		if (dialog.ShowDialog() == true)
		{
			path = dialog.FileName;
			return true;
		}

		path = string.Empty;
		return false;
	}

	private void OnBrowseGameDirectory(object sender, RoutedEventArgs e)
	{
		new DatabaseManager(this, true).ShowDialog();
	}

	private void OnBrowseOutputDirectory(object sender, RoutedEventArgs e)
	{
		if (TryBrowseFolder(out var path)) UserSettings.Default.OutputFolder = path;
	}

	private void OnBrowseResourceDirectory(object sender, RoutedEventArgs e)
	{
		if (TryBrowseFolder(out var path)) UserSettings.Default.OutputFolderResource = path;
	}

	private void OnConfirm(object sender, RoutedEventArgs e)
	{
		Close();
	}
	#endregion
}