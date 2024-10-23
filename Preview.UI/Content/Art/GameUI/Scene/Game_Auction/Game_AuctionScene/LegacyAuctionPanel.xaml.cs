using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Common.Interactivity;
using Xylia.Preview.UI.Controls;
using static Xylia.Preview.Data.Models.MarketCategory2Group;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Auction;
public partial class LegacyAuctionPanel
{
	#region Constructor
	public LegacyAuctionPanel()
	{
		#region Initialize 
		DataContext = _viewModel = new AuctionPanelViewModel();
		InitializeComponent();

		_viewModel.Changed += UpdateList;
		_viewModel.Source = CollectionViewSource.GetDefaultView(FileCache.Data.Provider.GetTable<Item>());
		#endregion

		#region Category
		TreeView.Items.Add(new TreeViewItem() { Header = new BnsCustomLabelWidget() { Text = "UI.Market.Category.All".GetText() } });

		var MarketCategory2Group = FileCache.Data.Provider.GetTable<MarketCategory2Group>();
		if (MarketCategory2Group is null)
		{
			var IsNeo = FileCache.Data.Provider.Locale.IsNeo;

			TreeView.Items.Add(new TreeViewItem() { Tag = new Favorite(), Header = new BnsCustomLabelWidget() { Text = "UI.Market.Category.Favorites".GetText() } });
			if (IsNeo) TreeView.Items.Add(new TreeViewItem() { Tag = new WorldBoss(), Header = new BnsCustomLabelWidget() { Text = "UI.Market.Category.WorldBoss".GetText() } });

			foreach (var category2 in SequenceExtensions.MarketCategory2Group(IsNeo))
			{
				if (category2.Key == MarketCategory2Seq.None) continue;

				var node = new TreeViewItem() { Tag = new MarketCategory2() { Marketcategory2 = category2.Key }, Header = category2.Key.GetText() };
				TreeView.Items.Add(node);

				foreach (var category3 in category2.Value)
				{
					node.Items.Add(new TreeViewItem()
					{
						Tag = new MarketCategory3Group() { MarketCategory3 = [category3] },
						Header = category3.GetText(),
					});
				}
			}
		}
		else
		{
			foreach (var category2 in MarketCategory2Group.OrderBy(x => x.SortNo))
			{
				var node = new TreeViewItem() { Tag = category2, Header = new BnsCustomLabelWidget() { Text = category2.Name2.GetText() } };
				TreeView.Items.Add(node);

				if (category2 is MarketCategory2Group.MarketCategory2 MarketCategory2)
				{
					foreach (var category3 in MarketCategory2.MarketCategory3Group.Values())
					{
						node.Items.Add(new TreeViewItem()
						{
							Tag = category3,
							Header = new BnsCustomLabelWidget() { Text = category3.Name2.GetText() }
						});
					}
				}
			}
		}
		#endregion
	}
	#endregion

	#region Override Methods
	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);

		TooltipHolder = (ToolTip)TryFindResource("TooltipHolder");
		ItemMenu = (ContextMenu)TryFindResource("ItemMenu");

		var SetFavoriteCommand = new SetFavoriteCommand();
		SetFavoriteCommand.Executed += UpdateList2;
		RecordCommand.Bind(SetFavoriteCommand, ItemMenu);
		RecordCommand.Find("item", (act) => RecordCommand.Bind(act, ItemMenu));
	}

	protected override void OnPreviewKeyDown(KeyEventArgs e)
	{
		switch (e.Key == Key.System ? e.SystemKey : e.Key)
		{
			case Key.LeftCtrl when TooltipHolder != null:
			{
				TooltipHolder.StaysOpen = true;
				TooltipHolder.IsOpen = true;
				TooltipHolder.Visibility = Visibility.Visible;
				break;
			}

			case Key.LeftShift when TooltipHolder != null:
			{
#if DEBUG
				(TooltipHolder.Content as BnsCustomWindowWidget)?.Show();
#endif
				break;
			}
		}
	}

	protected override void OnPreviewKeyUp(KeyEventArgs e)
	{
		base.OnPreviewKeyUp(e);

		if (e.Key == Key.LeftCtrl && TooltipHolder != null)
		{
			TooltipHolder.IsOpen = false;
		}
	}
	#endregion

	#region Methods
	private void Comapre_Checked(object sender, RoutedEventArgs e)
	{
		var dialog = new Microsoft.Win32.OpenFileDialog() { Filter = @"|*.chv|All files|*.*" };
		_viewModel.HashList = dialog.ShowDialog() == true ? new HashList(dialog.FileName) : null;
	}

	private void Comapre_Unchecked(object sender, RoutedEventArgs e) => _viewModel.HashList = null;

	private void GradeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.AddedItems[0] is not FrameworkElement element) return;

		_viewModel.Grade = element.DataContext.To<sbyte>();
	}

	private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.OldValue == e.NewValue) return;
		if (e.NewValue is not FrameworkElement item) return;

		_viewModel.Category = item.Tag;
	}

	public void SetFilter(string? name)
	{
		_viewModel.NameFilter = name;
	}

	private void UpdateList(object? sender, EventArgs e)
	{
		Dispatcher.BeginInvoke(() =>
		{
			_viewModel.Source.Refresh();
			_viewModel.Source.MoveCurrentToFirst();
			ItemList.ScrollIntoView(_viewModel.Source.CurrentItem);
		});
	}

	private void UpdateList2(object? sender, EventArgs e)
	{
		if (_viewModel.Category is Favorite) UpdateList(sender, e);
	}
	#endregion

	#region Fields
	private readonly AuctionPanelViewModel _viewModel;

	private ContextMenu? ItemMenu;
	private ToolTip? TooltipHolder;
	#endregion
}