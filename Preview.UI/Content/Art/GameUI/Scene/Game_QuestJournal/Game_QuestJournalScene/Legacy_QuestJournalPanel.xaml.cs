using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Helpers.Output;
using Xylia.Preview.UI.Helpers.Output.Tables;

namespace Xylia.Preview.UI.GameUI.Scene.Game_QuestJournal;
public partial class Legacy_QuestJournalPanel
{
	#region Methods
	protected override void OnLoading()
	{
		InitializeComponent();

		// Progress
		source = CollectionViewSource.GetDefaultView(FileCache.Data.Provider.GetTable<Quest>().OrderBy(x => x.PrimaryKey));
		source.Filter = OnFilter;
		QuestJournal_ProgressQuestList.ItemsSource = source;

		// Completed
		List<Quest> CompletedQuest = [];
		QuestEpic.GetEpic(CompletedQuest.Add);
		TreeView2.ItemsSource = CompletedQuest.GroupBy(o => o.Title).Select(o => new TreeViewItem
		{
			Header = o.Key,
			ItemsSource = o.ToList(),
		});
	}


	// ------------------------------------------------------------------
	// 
	//  ProgressTab
	// 
	// ------------------------------------------------------------------
	private void SearchStarted(object sender, TextChangedEventArgs e)
	{
		source?.Refresh();
	}

	private bool OnFilter(object obj)
	{
		if (obj is not Quest quest) return false;

		// valid
		var rule = SearcherRule.Text;
		var IsEmpty = string.IsNullOrEmpty(rule);
		if (IsEmpty) return true;

		// filter 
		if (int.TryParse(rule, out int id)) return quest.PrimaryKey == id;

		if (quest.Name != null && quest.Name.Contains(rule, StringComparison.OrdinalIgnoreCase)) return true;
		if (quest.Title != null && quest.Title.Contains(rule, StringComparison.OrdinalIgnoreCase)) return true;

		return false;
	}

	// ------------------------------------------------------------------
	// 
	//  CompletedTab
	// 
	// ------------------------------------------------------------------
	private void CompletedTab_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.OldValue == e.NewValue) return;
		if (e.NewValue is not Quest quest) return;

		TextBlock2.String.LabelText = quest.Attributes["completed-desc"].GetText();
	}

	// ------------------------------------------------------------------
	// 
	//  Extract
	// 
	// ------------------------------------------------------------------
	private void Extract_QuestList_Click(object sender, RoutedEventArgs e) => OutSet.Start<QuestOut>();

	private void Extract_EpicQuestList_Click(object sender, RoutedEventArgs e) => OutSet.Start<QuestEpic>();
	#endregion

	#region Private Fields
	private ICollectionView? source;
	#endregion
}