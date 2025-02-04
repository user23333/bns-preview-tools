using System.ComponentModel;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using Ookii.Dialogs.Wpf;
using Xylia.Preview.Common;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.GameUI.Scene.Game_Auction;
using Xylia.Preview.UI.Helpers.Output.Tables;
using Xylia.Preview.UI.Views.Dialogs;
using Xylia.Preview.UI.Views.Editor;
using Window = System.Windows.Window;
namespace Xylia.Preview.UI.ViewModels;
internal partial class ItemPageViewModel : ObservableObject
{
	#region Item   	
	[ObservableProperty] bool onlyUpdate;
	[ObservableProperty] string? hashPath;

	[RelayCommand]
	public void OpenSettings()
	{
		new DatabaseManager(Application.Current.MainWindow, true).ShowDialog();
	}

	[RelayCommand]
	private void BrowerItemList()
	{
		var dialog = new VistaOpenFileDialog() { Filter = @"|*.chv|All files|*.*" };
		if (dialog.ShowDialog() == true) HashPath = dialog.FileName;
	}

	[RelayCommand]
	private async Task CreateItemList()
	{
		#region Check
		if (!Directory.Exists(UserSettings.Default.GameFolder)) throw new WarningException(StringHelper.Get("Settings_GameDirectory_Invalid"));
		else if (!Directory.Exists(UserSettings.Default.OutputFolder)) throw new WarningException(StringHelper.Get("Settings_OutputDirectory_Invalid"));
		else if (OnlyUpdate == true && !File.Exists(HashPath)) throw new WarningException(StringHelper.Get("ItemList_Path_Invalid"));
		#endregion

		#region Load
		var fileMode = await Dialog.Show<FileModeDialog>().GetResultAsync<FileModeDialog.FileMode>();
		if (fileMode == FileModeDialog.FileMode.None) return;

		using var instance = new ItemOut() { OnlyUpdate = this.OnlyUpdate };
		await Task.Run(() =>
		{
			var start = DateTime.Now;
			var count = instance.Start(fileMode, HashPath);

			// send finish tootip
			Growl.Success(new GrowlInfo()
			{
				Message = StringHelper.Get("ItemList_TaskCompleted", count, (int)(DateTime.Now - start).TotalSeconds),
				StaysOpen = true,
			});
		});
		#endregion
	}
	#endregion

	#region Preview
	[RelayCommand]
	private async Task Preview(string name)
	{
		var type = Type.GetType($"Xylia.Preview.UI.GameUI.Scene.{name}");
		if (type is null) return;

		// init data
		await Task.Run(() =>
		{
			_ = Globals.GameData.Provider;
			_ = Globals.GameProvider;
		});

		// show
		switch (Activator.CreateInstance(type))
		{
			case Window window: window.Show(); break;
			case BnsCustomWindowWidget window: window.Show(); break;
		}
	}

	[RelayCommand]
	private void PreviewItem(string rule)
	{
		var panel = new LegacyAuctionPanel();
		panel.SetFilter(rule);
		panel.Show();
	}
	#endregion
}