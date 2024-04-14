﻿using System.Windows;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.UI.Helpers.Output;
using Xylia.Preview.UI.Helpers.Output.Tables;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views.Editor;

namespace Xylia.Preview.UI.Views.Pages;
public partial class ItemPage
{
	#region Constructors
	public ItemPage()
	{
		DataContext = new ItemPageViewModel();
		InitializeComponent();

		// debug
		List<string> source = ["a", "b", "c"];  //FileCache.Data.Provider.GetTable<Quest>().Take(30);
		Test.ItemsSource = source;
		Test.TestMethod();
	}
	#endregion

	#region Methods
	private void DatabaseGui_Click(object sender, RoutedEventArgs e) => new DatabaseStudio().Show();

	private void ClearCacheData_Click(object sender, RoutedEventArgs e)
	{
		FileCache.Clear();
		ProcessFloatWindow.ClearMemory();

		// OutSet.Start<WeeklyTimeTableOut>();
	}
	#endregion
}