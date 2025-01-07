using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CUE4Parse.Compression;
using HandyControl.Interactivity;
using HandyControl.Tools.Extension;
using Ookii.Dialogs.Wpf;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Views.Dialogs;
[ObservableObject]
[DesignTimeVisible(false)]
public partial class VfsFileInfoDialog : IDialogResultable<PackageParam.FileParam>
{
	#region Constructor
	public VfsFileInfoDialog()
	{
		DataContext = this;
		InitializeComponent();

		this.CommandBindings.Add(new CommandBinding(ControlCommands.Close, CloseCommand));
	}
	#endregion

	#region Methods
	private void OnBrowseFile(object sender, RoutedEventArgs e)
	{
		var dialog = new VistaOpenFileDialog();
		if (dialog.ShowDialog() == true)
		{
			Result!.Path = dialog.FileName;
			OnPropertyChanged(nameof(Result));
		}
	}

	private void Ok_Click(object sender, RoutedEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(Result.Path);
		ArgumentNullException.ThrowIfNull(Result.Vfs);

		CloseAction?.Invoke();
	}

	private void CloseCommand(object sender, RoutedEventArgs e)
	{
		Result = null;
		CloseAction?.Invoke();
	}
	#endregion

	#region Interface
	public ObservableCollection<CompressionMethod> CompressionModes => new(Enum.GetValues<CompressionMethod>());

	[ObservableProperty]
	private PackageParam.FileParam? result = new();

	public Action? CloseAction { get; set; }
	#endregion
}