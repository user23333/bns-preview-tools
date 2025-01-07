using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using Xylia.Preview.Common;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Common.Interactivity;
using Xylia.Preview.UI.Controls;
using static Xylia.Preview.Data.Models.MarketCategory2Group;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Auction;
public partial class LegacyAuctionPanel
{
	#region Constructor
	public LegacyAuctionPanel()
	{
		DataContext = _viewModel = new AuctionPanelViewModel();
		InitializeComponent();

		_viewModel.Changed += UpdateList;
		_viewModel.Source = CollectionViewSource.GetDefaultView(Globals.GameData.Provider.GetTable<Item>());
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
		var dialog = new OpenFileDialog() { Filter = @"|*.chv|All files|*.*" };
		_viewModel.HashList = dialog.ShowDialog() == true ? new HashList(dialog.FileName) : null;
	}

	private void Comapre_Unchecked(object sender, RoutedEventArgs e) => _viewModel.HashList = null;

	private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		_viewModel.Category = e.NewValue;
	}

	private void GradeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.AddedItems[0] is not FrameworkElement element) return;

		_viewModel.Grade = element.DataContext.To<sbyte>();
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

	public void SetFilter(string? name)
	{
		_viewModel.NameFilter = name;
	}
	#endregion

	#region Fields
	private readonly AuctionPanelViewModel _viewModel;

	private ContextMenu? ItemMenu;
	private ToolTip? TooltipHolder;
	#endregion
}