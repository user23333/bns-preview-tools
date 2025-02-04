using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;
using Xylia.Preview.UI.Resources.Themes;
using Xylia.Preview.UI.Services;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views;
using Xylia.Preview.UI.Views.Dialogs;
using Xylia.Preview.UI.Views.Editor;
using Xylia.Preview.UI.Views.Pages;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Xylia.Preview.UI;
public partial class MainWindow
{
	#region Constructor
	public MainWindow()
	{
		InitializeComponent();

		#region page
		SideMenu.ItemsSource = new List<IPageController>()
		{
			new PageController<ItemPage>(),
			new PageController<DatabaseStudio>("DatabaseStudio_Name"),
			new PageController<TextView>("TextView_Name"),
			new PageController<GameResourcePage>(),
			new PageController<AbilityPage>(),
		};
		SideMenu.SelectedIndex = 0;
		#endregion
	}
	#endregion

	#region Methods
	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);

		this.Loaded += OnLoaded;
		this.GrowlHolder2.ItemsSource = Growl2.Source;

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

	protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
	{
		UserSettings.Default.ContainerSize = sizeInfo.NewSize;
	}

	private void OnLoaded(object? sender, EventArgs e)
	{
		if ((bool?)Application.Current.Properties["ShowLog"] == true)
			OpenUpdateLog(sender, e);
	}

	private void SideMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		SideMenuContainer.IsOpen = false;

		var page = (IPageController)SideMenu.SelectedItem;
		var content = page.Content;
		if (content is System.Windows.Window window)
		{
			window.Closed += (s, e) => page.Content = null;
			window.Show();
			window.Activate();
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
		Dialog.Show<UpdateLogDialog>("MainContainer");
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
		if (current == 0 && MessageBox.Show(StringHelper.Get("Settings_NightMode_Ask"), StringHelper.Get("Message_Tip"), MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;

		UserSettings.Default.NightMode = current switch
		{
			0 => 1,
			1 => 2,
			_ => 0,
		};
	}
	#endregion
}