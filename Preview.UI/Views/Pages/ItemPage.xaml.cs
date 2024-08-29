using System.Windows;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.UI.Helpers.Output;
using Xylia.Preview.UI.Helpers.Output.Tables;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views.Editor;
using Xylia.Preview.UI.Views.Selector;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Xylia.Preview.UI.Views.Pages;
public partial class ItemPage
{
	#region Constructors
	public ItemPage()
	{
		DataContext = new ItemPageViewModel();
		InitializeComponent();

#if DEBUG
		TestHolder.Visibility = Visibility.Visible;
		TestHolder.IsSelected = true;

		TestListHolder.ItemsSource = new List<string>()
		{
			"<br/>123456<br/>1111111<br/><br/>222222"
		};
		TestListHolder.TestMethod();

		// timer
		TestLabel.Timers[1] = new Time64(1722541536967);
		var timer = new System.Windows.Threading.DispatcherTimer();
		timer.Tick += ((s, e) => TestLabel?.InvalidateVisual());
		timer.Interval = new TimeSpan(0, 0, 0, 1);
		timer.IsEnabled = true;
		timer.Start();

		//Debug.WriteLine("stringstringstring   <arg p=\"2:string\"/>".Replace([null, "test"]));  
#endif
	}
	#endregion

	#region Methods
	private void DatabaseGui_Click(object sender, RoutedEventArgs e) => new DatabaseStudio().Show();

	private void ClearCacheData_Click(object sender, RoutedEventArgs e)
	{
		if (MessageBox.Show(StringHelper.Get("DatabaseStudio_ConnectMessage1"), StringHelper.Get("Message_Tip"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
		{
			FileCache.Clear();
			ProcessFloatWindow.ClearMemory();
		}
	}

	private async void TestButton1_Click(object sender, RoutedEventArgs e)
	{
		await OutSet.Start<WeeklyTimeTableOut>();
	}
	#endregion
}