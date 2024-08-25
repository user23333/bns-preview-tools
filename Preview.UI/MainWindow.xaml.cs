using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Xylia.Preview.UI.Resources.Themes;
using Xylia.Preview.UI.Services;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views;
using Xylia.Preview.UI.Views.Editor;
using Xylia.Preview.UI.Views.Pages;
using Xylia.Preview.UI.Views.Selector;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Xylia.Preview.UI;
public partial class MainWindow
{
	#region Constructor
	public MainWindow()
	{
		InitializeComponent();

		#region page
		SideMenu.ItemsSource = new List<object>()
		{
			new PageController<ItemPage>(),
			new PageController<DatabaseStudio>("DatabaseStudio_Name"),
			new PageController<TextView>("TextView_Name"),
			new PageController<GameResourcePage>(),
			new PageController<AbilityPage>(),
		};
		SideMenu.SelectedIndex = 0;
		SideMenu_Switch(SideMenu, new RoutedEventArgs());
		#endregion
	}
	#endregion

	#region Methods
	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);

		this.GrowlHolder2.ItemsSource = Growl2.Source;
		this.Loaded += OpenUpdateLog;
		this.MinWidth = this.Width;
		this.MinHeight = this.Height;

		// service
		new ServiceManager() { new UpdateService(), new RegisterService() }.RegisterAll();
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		var result = MessageBox.Show(StringHelper.Get("Application_ExitMessage"), StringHelper.Get("Message_Tip"), MessageBoxButton.YesNo);
		if (result != MessageBoxResult.Yes)
		{
			e.Cancel = true;
			return;
		}

		Application.Current.Shutdown();
	}

	private void SideMenu_Switch(object sender, RoutedEventArgs e)
	{
		SideMenuContainer.IsOpen = false;
		var page = (IPageController)SideMenu.SelectedItem;

		var content = page.Content;
		if (content is Window window)
		{
			window.Closed += (s, e) => page.Content = null;
			window.Show();
		}
		else if (content is FrameworkElement element)
		{
			Presenter.Content = element;
		}
	}

	private void OpenSettings(object sender, RoutedEventArgs e)
	{
		new SettingsView().ShowDialog();
	}

	private void OpenUpdateLog(object? sender, EventArgs e)
	{
		// OnLoaded
		if (sender == this && !UpdateService.ShowLog) return;

		HandyControl.Controls.Dialog.Show<UpdateLogDialog>("MainContainer");
	}

	private void OpenPopupSkin(object sender, RoutedEventArgs e) => PopupSkin.IsOpen = true;

	private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
	{
		if (e.OriginalSource is Button { Tag: SkinType skinType })
		{
			PopupSkin.IsOpen = false;
			UserSettings.Default.SkinType = skinType;
		}
	}

	private void SwitchNight_OnClick(object sender, RoutedEventArgs e)
	{
		var current = UserSettings.Default.NightMode;
		if (current == null && MessageBox.Show(StringHelper.Get("Settings_NightMode_Ask"), StringHelper.Get("Message_Tip"), MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;

		UserSettings.Default.NightMode = current switch
		{
			null => false,
			false => true,
			_ => null,
		};
	}
	#endregion
}