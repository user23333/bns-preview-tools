using System.IO;
using System.Windows;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Properties;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views.Editor;
using static Xylia.Preview.Data.Models.MarketCategory2Group;
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

		// timer
		TestLabel.SetTimer(1, 1722541536967);
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

	private void TestButton1_Click(object sender, RoutedEventArgs e)
	{
		var dir = new DirectoryInfo(@"D:\Tencent\BnsData\GameData_ZNcs\20241011");
		var table = FileCache.Data.Provider.GetTable("text");
		LocalProvider.ReplaceText(table, dir.GetFiles("*.x16"));
	}
	#endregion
}