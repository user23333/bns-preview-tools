using System.ComponentModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using HandyControl.Tools.Extension;

namespace Xylia.Preview.UI.Views.Selector;

[ObservableObject]
[DesignTimeVisible(false)]
public partial class FileModeDialog : IDialogResultable<FileModeDialog.FileMode>
{
	#region Constructors
	public FileModeDialog()
	{
		DataContext = this;
		InitializeComponent();
	}
	#endregion

	#region Methods
	private void TextFile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		Result = FileMode.Txt;
		CloseAction?.Invoke();
	}

	private void ExcelFile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		Result = FileMode.Xlsx;
		CloseAction?.Invoke();
	}
	#endregion


	#region Interface
	public Action? CloseAction { get; set; }

	[ObservableProperty]
	private FileMode result;

	public enum FileMode
	{
		None,
		Txt,
		Xlsx,
	}
	#endregion
}