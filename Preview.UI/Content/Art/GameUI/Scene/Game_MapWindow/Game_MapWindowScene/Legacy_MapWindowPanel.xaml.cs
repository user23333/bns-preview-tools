using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls;

namespace Xylia.Preview.UI.GameUI.Scene.Game_MapWindow;
public partial class Legacy_MapWindowPanel
{
	#region Constructors
	public Legacy_MapWindowPanel()
	{
		InitializeComponent();

		MapWindow_Minimap.MapChanged += MapWindow_Minimap_MapChanged;
		MapWindowPanel_UnitFilterList.ItemsSource = MapWindow_Minimap.UnitFilters;

		// data
		source = CollectionViewSource.GetDefaultView(FileCache.Data.Provider.GetTable<MapInfo>());
		source.Filter += OnFilter;
		source.GroupDescriptions.Clear();
		source.GroupDescriptions.Add(new MapGroupDescription());
		TreeView.ItemsSource = source.Groups;
	}
	#endregion

	#region Private Methods	  
	// Search
	private Zone? _zone;

	private void SelectedMapChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.NewValue is MapInfo record)
			MapWindow_Minimap.MapInfo = record;
	}

	private void SearchStarted(object sender, TextChangedEventArgs e)
	{
		_zone = FileCache.Data.Provider.GetTable<Zone>()[SearcherRule.Text];
		source.Refresh();
	}

	private bool OnFilter(object obj)
	{
		var rule = SearcherRule.Text;
		if (string.IsNullOrEmpty(rule)) return true;

		// common
		if (obj is not MapInfo mapinfo) return false;
		if (mapinfo.Alias != null && mapinfo.Alias.Contains(rule, StringComparison.OrdinalIgnoreCase)) return true;
		if (mapinfo.Name != null && mapinfo.Name.Contains(rule, StringComparison.OrdinalIgnoreCase)) return true;

		// zone
		if (_zone != null)
		{
			if (_zone.Map.HasValue && _zone.Map == mapinfo) return true;
		}

		return false;
	}

	//  Map
	private void MapWindow_Minimap_MapChanged(object? sender, MapInfo MapInfo)
	{
		MapInfo.IsSelected = true;
	}

	private void MapWindow_Minimap_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{
		var offset = Mouse.GetPosition(MapWindow_Minimap);
		var vector = (Vector32)MapWindow_Minimap.Parse(offset);

		MapWindowPanel_PositionHolder_X.Text = vector.X.ToString();
		MapWindowPanel_PositionHolder_Y.Text = vector.Y.ToString();
	}

	private void MapWindow_Minimap_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.Add)
		{
			switch (e.Key)
			{
				case Key.Add: MapWindow_Minimap.Zoom += 0.1; break;
			}
		}
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
			BaseImageProperty = new ImageProperty() { EnableImageSet = true, ImageSet = new MyFPackageIndex("/Game/Art/UI/GameUI/Resource/GameUI_Map_Indicator/teleport_point_current_normal") },
		});
	}
	#endregion

	#region Private Fields
	private readonly ICollectionView source;
	#endregion

	#region Helpers
	private class MapGroupDescription : PropertyGroupDescription
	{
		public override object? GroupNameFromItem(object item, int level, CultureInfo culture)
		{
			if (item is MapInfo MapInfo) return MapInfo.MapGroup1.Instance?.Name;

			return base.GroupNameFromItem(item, level, culture);
		}
	}
	#endregion
}