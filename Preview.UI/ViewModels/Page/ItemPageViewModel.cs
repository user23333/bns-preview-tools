using System.ComponentModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using Ookii.Dialogs.Wpf;
using Xylia.Preview.UI.GameUI.Scene.Game_Auction;
using Xylia.Preview.UI.Helpers.Output.Tables;
using Xylia.Preview.UI.Views;
using Xylia.Preview.UI.Views.Selector;

namespace Xylia.Preview.UI.ViewModels;
internal partial class ItemPageViewModel : ObservableObject
{
	#region Item
	[ObservableProperty]
	bool onlyUpdate;

	[ObservableProperty]
	string hashPath;


	[RelayCommand]
	public void OpenSettings()
	{
		new SettingsView().ShowDialog();
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

		using var Out = new ItemOut() { OnlyUpdate = this.OnlyUpdate };
		await Task.Run(() =>
		{
			var start = DateTime.Now;
			var count = Out.Start(fileMode, HashPath);

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
	private void Preview(string name)
	{
		name = $"Xylia.Preview.UI.GameUI.Scene.{name}";

		var type = Type.GetType(name);
		if (type is null) return;

		(Activator.CreateInstance(type) as System.Windows.Window)?.Show();
	}

	[RelayCommand]
	private void PreviewItem(string rule)
	{
		var panel = new Game_AuctionScene().AuctionPanel_C;
		panel.SetFilter(rule);
		panel.Show();
	}
	#endregion
}