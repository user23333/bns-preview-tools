using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Helpers.Output;
using Xylia.Preview.UI.Helpers.Output.Tables;
using static Xylia.Preview.Data.Models.Quest;

namespace Xylia.Preview.UI.GameUI.Scene.Game_QuestJournal;
public partial class Legacy_QuestJournalPanel
{
	#region Methods
	protected override void OnLoading()
	{
		InitializeComponent();

		// Filter
		Filter_Category.ItemsSource = Enum.GetValues<CategorySeq>().Where(x => x < CategorySeq.COUNT);
		Filter_ContentType.ItemsSource = Enum.GetValues<ContentTypeSeq>().Where(x => x > ContentTypeSeq.None && x < ContentTypeSeq.COUNT);
		Filter_ResetType.ItemsSource = Enum.GetValues<ResetType>().Where(x => x > ResetType.None && x < ResetType.COUNT);

		// Progress
		source = CollectionViewSource.GetDefaultView(FileCache.Data.Provider.GetTable<Quest>().OrderBy(x => x.PrimaryKey));
		source.Filter = OnFilter;
		QuestJournal_ProgressQuestList.ItemsSource = source;

		// Completed
		List<Quest> CompletedQuest = [];
		QuestEpic.GetEpic(CompletedQuest.Add);
		QuestJournal_CompletedQuestList.ItemsSource = CompletedQuest.GroupBy(o => o.Title).Select(o => new TreeViewItem { Header = o.Key, ItemsSource = o.ToList(), });
	}

	// ------------------------------------------------------------------
	// 
	//  ProgressTab
	// 
	// ------------------------------------------------------------------
	private void SearcherRule_GotFocus(object sender, RoutedEventArgs e)
	{
		SearcherOption.IsOpen = true;
	}

	private void SearcherRule_LostFocus(object sender, RoutedEventArgs e)
	{
		if (!IsHoverOption) SearcherOption.IsOpen = false;
	}

	private void SearcherRule_TextChanged(object sender, TextChangedEventArgs e)
	{
		source?.Refresh();
	}

	private void SearcherOption_MouseEnter(object sender, MouseEventArgs e)
	{
		IsHoverOption = true;
	}

	private void SearcherOption_MouseLeave(object sender, MouseEventArgs e)
	{
		IsHoverOption = false;
		SearcherOption.IsOpen = false;
		source?.Refresh();
	}

	private void SearcherOption_Checked(object sender, RoutedEventArgs e)
	{
		if (e.Source is not FrameworkElement element) return;

		if (element.DataContext is Enum seq)
		{
			seq.SetFlag(ref FilterMask, seq switch
			{
				CategorySeq => MASK_Category,
				ContentTypeSeq => MASK_ContentType,
				ResetType => MASK_ResetType,
				_ => throw new NotSupportedException()
			});
		}
	}

	private void SearcherOption_Unchecked(object sender, RoutedEventArgs e)
	{
		if (e.Source is not FrameworkElement element) return;

		if (element.DataContext is Enum seq)
		{
			seq.SetFlag(ref FilterMask, seq switch
			{
				CategorySeq => MASK_Category,
				ContentTypeSeq => MASK_ContentType,
				ResetType => MASK_ResetType,
				_ => throw new NotSupportedException()
			}, false);
		}
	}

	private bool OnFilter(object obj)
	{
		if (obj is not Quest quest) return false;

		// filter
		var category = (short)(FilterMask >> MASK_Category);
		if (category != 0 && !quest.Category.InFlag(category)) return false;

		var contentType = (short)(FilterMask >> MASK_ContentType);
		if (contentType != 0 && !quest.ContentType.InFlag(contentType)) return false;

		var resetType = (byte)(FilterMask >> MASK_ResetType);
		if (resetType != 0 && !quest.ResetType.InFlag(resetType)) return false;


		// check rule
		var rule = SearcherRule.Text;
		var IsEmpty = string.IsNullOrEmpty(rule);
		if (IsEmpty) return true;

		// filter rule 
		if (int.TryParse(rule, out int id)) return quest.PrimaryKey == id;
		if (quest.Attributes.Get<string>("alias")?.Equals(rule, StringComparison.OrdinalIgnoreCase) ?? false) return true;
		if (quest.Name?.Contains(rule, StringComparison.OrdinalIgnoreCase) ?? false) return true;
		if (quest.Title?.Contains(rule, StringComparison.OrdinalIgnoreCase) ?? false) return true;

		return false;
	}

	// ------------------------------------------------------------------
	// 
	//  CompletedTab
	// 
	// ------------------------------------------------------------------
	private void QuestJournal_CompletedQuestList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.OldValue == e.NewValue) return;
		if (e.NewValue is not Quest quest) return;

		QuestJournal_CompletedQuestDesc.String.LabelText = quest.Attributes["completed-desc"].GetText();
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
	private bool IsHoverOption;

	private long FilterMask;
	private const int MASK_Category = 0;     //short (flags gather than 255)
	private const int MASK_ContentType = 16; //short (flags gather than 255)
	private const int MASK_ResetType = 32;   //byte
	#endregion
}