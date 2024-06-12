using System.Windows;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.UI.Helpers.Output;
using Xylia.Preview.UI.Helpers.Output.Tables;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views.Editor;
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

		List<string> source = ["a", "b", "c"];  //FileCache.Data.Provider.GetTable<Quest>().Take(30);
		Test.ItemsSource = source;
		Test.TestMethod();
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

			//OutSet.Start<ItemCombinationOut>();
		}
	}
	#endregion
}