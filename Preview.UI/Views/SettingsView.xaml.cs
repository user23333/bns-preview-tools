using System.Windows;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Views;
public partial class SettingsView : Window
{
	public SettingsView()
	{
		DataContext = UserSettings.Default;
		InitializeComponent();
#if !DEBUG
		Models.Visibility = Visibility.Collapsed;
#endif
	}

	#region Methods
	private void OnClosing(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void OnBrowseDirectories(object sender, RoutedEventArgs e)
	{
		if (!TryBrowseFolder(out var path))
			return;

		UserSettings.Default.GameFolder = path;
		// var Locale = new Locale(new DirectoryInfo(path));
	}

	private void OnBrowseDirectories2(object sender, RoutedEventArgs e)
	{
		if (TryBrowseFolder(out var path)) UserSettings.Default.OutputFolder = path;
	}

	private void OnBrowseDirectories3(object sender, RoutedEventArgs e)
	{
		if (TryBrowseFolder(out var path)) UserSettings.Default.OutputFolderResource = path;
	}


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

	public static bool TryBrowse(out string path, string filter = null)
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
	#endregion
}