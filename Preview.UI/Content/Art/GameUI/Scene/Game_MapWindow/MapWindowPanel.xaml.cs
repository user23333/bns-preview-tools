using System.Windows;
using System.Windows.Data;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls;

namespace Xylia.Preview.UI.GameUI.Scene.Game_MapWindow;
public partial class MapWindowPanel
{
	#region Constructors
	readonly MapWindowPanelViewModel _viewModel;

	public MapWindowPanel()
	{
		InitializeComponent();

		DataContext = _viewModel = new MapWindowPanelViewModel();
		_viewModel.Source = CollectionViewSource.GetDefaultView(Globals.GameData.Provider.GetTable<MapInfo>());
		MapWindowPanel_MapList.ItemsSource = _viewModel.Source.Groups;
		MapWindowPanel_UnitFilterList.ItemsSource = MapWindow_Minimap.UnitFilters;
	}
	#endregion

	#region Methods	  
	// Search
	private void SelectedMapChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.NewValue is MapInfo record)
			MapWindow_Minimap.MapInfo = record;
	}

	private void UpdateList(object? sender, EventArgs e)
	{
		Dispatcher.BeginInvoke(() =>
		{
			_viewModel.Source.Refresh();
		});
	}

	//  Map
	private void MapWindow_Minimap_MapChanged(object? sender, MapInfo value)
	{
		value.IsSelected = true;
	}

	private void MapWindow_Minimap_OnClick(object sender, Vector32 value)
	{
		MapWindowPanel_PositionHolder_X.Text = value.X.ToString();
		MapWindowPanel_PositionHolder_Y.Text = value.Y.ToString();
	}

	private void OnPositionChanged(object sender, RoutedEventArgs e)
	{
		if (MapWindow_Minimap.MapInfo is null ||
			!int.TryParse(MapWindowPanel_PositionHolder_X.Text, out var x) ||
			!int.TryParse(MapWindowPanel_PositionHolder_Y.Text, out var y)) return;

		var point = new Vector32(x, y, 0);
		MapWindow_Minimap.AddChild(point, null, new BnsCustomImageWidget()
		{
			Tag = MapUnit.CategorySeq.Player,
			BaseImageProperty = new ImageProperty() { EnableImageSet = true, ImageSet = new MyFPackageIndex("00009499.Teleport_point_current_normal") },
		});
	}
	#endregion
}