using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Controls;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Auction;
public partial class LegacyAuctionPanel
{
	#region Constructor
	public LegacyAuctionPanel()
	{
		#region Initialize 
		InitializeComponent();
		DataContext = _viewModel = new AuctionPanelViewModel();
		#endregion

		#region Category
		TreeView.Items.Add(new TreeViewItem() { Tag = "all", Header = new BnsCustomLabelWidget() { Text = "UI.Market.Category.All".GetText() } });
		TreeView.Items.Add(new TreeViewItem() { Tag = "WorldBoss", Header = new BnsCustomLabelWidget() { Text = "UI.Market.Category.WorldBoss".GetText(), FontSize = 15 } });
		TreeView.Items.Add(new TreeViewItem() { Tag = "favorites", Header = new BnsCustomLabelWidget() { Text = "UI.Market.Category.Favorites".GetText() } });

		foreach (var category2 in SequenceExtensions.MarketCategory2Group(FileCache.Data.Provider.IsNeo))
		{
			if (category2.Key == MarketCategory2Seq.None) continue;

			var node = new TreeViewItem() { Tag = category2.Key, Header = category2.Key.GetText() };
			TreeView.Items.Add(node);

			foreach (var category3 in category2.Value)
			{
				node.Items.Add(new TreeViewItem()
				{
					Tag = category3,
					Header = category3.GetText(),
				});
			}
		}
		#endregion

		// data
		_viewModel.Changed += RefreshList;
		_viewModel.Source = CollectionViewSource.GetDefaultView(FileCache.Data.Provider.GetTable<Item>());
		_viewModel.Source.Filter = OnFilter;
		ItemList.ItemsSource = _viewModel.Source;
	}
	#endregion

	#region Override Methods
	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);

		TooltipHolder = (ToolTip)TryFindResource("TooltipHolder");
	}

	protected override void OnPreviewKeyDown(KeyEventArgs e)
	{
		base.OnPreviewKeyDown(e);

		if (e.Key == Key.LeftCtrl && TooltipHolder != null)
		{
			TooltipHolder.StaysOpen = true;
			TooltipHolder.IsOpen = true;
			TooltipHolder.Visibility = Visibility.Visible;
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

	private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.OldValue == e.NewValue) return;
		if (e.NewValue is not FrameworkElement item) return;

		_viewModel.ByTag(item.Tag);
	}

	public void SetFilter(string? name)
	{
		_viewModel.NameFilter = name;
	}

	private bool OnFilter(object obj)
	{
		#region Initialize
		if (obj is Record record) { }
		else if (obj is ModelElement model) record = model.Source;
		else return false;

		if (_viewModel.HashList?.CheckFailed(record.PrimaryKey) ?? false) return false;

		var rule = _viewModel.NameFilter!;
		var IsEmpty = string.IsNullOrEmpty(rule);
		#endregion

		#region Category
		if (_viewModel.WorldBoss)
		{
			if (!record.Attributes.Get<BnsBoolean>("world-boss-auctionable")) return false;
		}
		else if (_viewModel.MarketCategory2 == default && _viewModel.MarketCategory3 == default)  // all
		{
			if (_viewModel.HashList is null && IsEmpty) return false;
		}
		else
		{
			var MarketCategory2 = record.Attributes["market-category-2"].ToEnum<MarketCategory2Seq>();
			var MarketCategory3 = record.Attributes["market-category-3"].ToEnum<MarketCategory3Seq>();

			if (_viewModel.MarketCategory3 != default && _viewModel.MarketCategory3 != MarketCategory3) return false;
			else if (_viewModel.MarketCategory2 != default && _viewModel.MarketCategory2 != MarketCategory2) return false;
		}
		#endregion


		#region Filter
		if (_viewModel.Auctionable &&
			!record.Attributes.Get<BnsBoolean>("auctionable") &&
			!record.Attributes.Get<BnsBoolean>("seal-renewal-auctionable")) return false;

		// rule
		if (IsEmpty) return true;
		if (int.TryParse(rule, out int id)) return record.PrimaryKey.Id == id;
		if (record.Attributes.Get<string>("alias")?.Contains(rule, StringComparison.OrdinalIgnoreCase) ?? false) return true;
		if (record.Attributes.Get<Record>("name2").GetText()?.Contains(rule, StringComparison.OrdinalIgnoreCase) ?? false) return true;

		return false;
		#endregion
	}

	private void RefreshList(object? sender, EventArgs e)
	{
		Dispatcher.BeginInvoke(() =>
		{
			_viewModel.Source.Refresh();
			_viewModel.Source.MoveCurrentToFirst();
			ItemList.ScrollIntoView(_viewModel.Source.CurrentItem);
		});
	}
	#endregion

	#region Fields
	private AuctionPanelViewModel _viewModel;
	private ToolTip? TooltipHolder;
	#endregion
}