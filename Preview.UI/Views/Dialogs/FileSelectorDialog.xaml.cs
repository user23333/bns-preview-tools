using System.ComponentModel;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Tools.Extension;

namespace Xylia.Preview.UI.Views.Dialogs;
[ObservableObject]
[DesignTimeVisible(false)]
public partial class FileSelectorDialog : IDialogResultable<FileSelectorDialog>
{
	#region Constructor	  
	public FileSelectorDialog()
	{
		InitializeComponent();
		DataContext = this;
	}
	#endregion

	#region Fields
	public bool Status = false;

	public string? Filter = null;

	[ObservableProperty]
	private string? path1;

	[ObservableProperty]
	private string? path2;
	#endregion

	#region Methods
	public FileSelectorDialog Result { get => this; set => throw new NotSupportedException(); }

	public Action? CloseAction { get; set; }

	[RelayCommand]
	private void BrowseFile(object parameter)
	{
		if (!SettingsView.TryBrowse(out var path, Filter)) return;

		(parameter as TextBox)!.Text = path;
	}

	[RelayCommand]
	private void Close()
	{
		Status = false;
		CloseAction?.Invoke();
	}

	[RelayCommand]
	private void Success()
	{
		Status = true;
		CloseAction?.Invoke();
	}
	#endregion
}