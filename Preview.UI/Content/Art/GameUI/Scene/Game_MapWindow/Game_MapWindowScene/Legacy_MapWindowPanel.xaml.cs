using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Converters;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Xylia.Preview.UI.GameUI.Scene.Game_MapWindow;
public partial class Legacy_MapWindowPanel
{
	#region Constructors
	public Legacy_MapWindowPanel()
	{
		InitializeComponent();

		// data
		source = CollectionViewSource.GetDefaultView(FileCache.Data.Provider.GetTable<MapInfo>());
		source.Filter = Filter;
		source.GroupDescriptions.Add(new MapGroupDescription());

		TreeView.ItemsSource = source.Groups;
	}

	private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.NewValue is MapInfo record)
			LoadData(record);
	}
	#endregion


	#region Private Methods
	// Search
	private void SearchStarted(object sender, TextChangedEventArgs e)
	{
		source?.Refresh();
	}

	private bool Filter(object obj)
	{
		if (obj is not MapInfo mapinfo) return false;

		var rule = SearcherRule.Text;
		return string.IsNullOrEmpty(rule) ||
			(mapinfo.Name != null && mapinfo.Name.Contains(rule, StringComparison.OrdinalIgnoreCase));
	}

	//  Map
	private void LoadData(MapInfo MapInfo)
	{
		if (MapInfo is null) return;

		// get current map depth
		MapDepth = MapInfo.GetMapDepth(MapInfo);
		MapPanel.BaseImageProperty = new ImageProperty() { ImageSet = new MyFPackageIndex(MapInfo.Imageset) };
		MapPanel.Width = MapPanel.Height = MapInfo.ImageSize;
		MapPanel.Children.Clear();

		LoadMapUint(MapInfo, []);
	}

	private void LoadMapUint(MapInfo MapInfo, List<MapInfo> MapTree)
	{
		MapTree.Add(MapInfo);
		GetMapUnit(MapInfo, MapTree);

		// children
		//if (MapInfo.Alias != "World")
		//{
		//	FileCache.Data.Provider.GetTable<MapInfo>()
		//		.Where(o => o.ParentMapinfo == MapInfo)
		//		.ForEach(o => LoadMapUint(o, new(MapTree)));
		//}
	}

	private void GetMapUnit(MapInfo MapInfo, List<MapInfo> MapTree)
	{
		var MapUnits = FileCache.Data.Provider.GetTable<MapUnit>().Where(o => o.Mapid == MapInfo.Id && o.MapDepth <= this.MapDepth);
		//MapInfo = MapTree[0]; // update to selected map

		foreach (var mapunit in MapUnits)
		{
			#region Initialize
			if (mapunit is MapUnit.Quest || CategoryFilter.Contains(mapunit.Category)) continue;

			var widget = new BnsCustomImageWidget()
			{
				BaseImageProperty = new ImageProperty() { ImageSet = new MyFPackageIndex(mapunit.Imageset) },
			};
			#endregion

			#region Tooltip
			string tooltip = mapunit.Name2.GetText();
			if (mapunit is MapUnit.Attraction)
			{
				var obj = new Ref<ModelElement>(mapunit.Attributes.Get<string>("attraction")).Instance;
				if (obj is IAttraction attraction)
				{
					tooltip = attraction.Name + "\n" + attraction.Description;
				}
				else if (obj != null)
				{
					tooltip = obj.ToString();
				}
			}
			else if (mapunit is MapUnit.Npc)
			{
				var npc = mapunit.Attributes.Get<Record>("npc")?.As<Npc>();
				if (npc != null)
				{
					tooltip = npc.Name;

					for (int i = 0; i < npc.ForwardingTypes.Length; i++)
					{
						var forwardingType = npc.ForwardingTypes[i];
						if (forwardingType == ForwardingType.AcquireQuest) tooltip += $"<br/><arg id=\"quest:{npc.Quests[i]}\" p=\"id:quest.front-icon.scale.150\"/> <arg id=\"quest:{npc.Quests[i]}\" p=\"id:quest.name2\"/>";
					}
				}
			}
			else if (mapunit is MapUnit.Boss)
			{
				var npc = mapunit.Attributes.Get<Record>("npc")?.As<Npc>();
				if (npc != null) tooltip = npc.Name;
			}
			else if (mapunit is MapUnit.Link)
			{
				widget.MouseLeftButtonDown += new((_, _) =>
				{
					var map = FileCache.Data.Provider.GetTable<MapInfo>()[mapunit.Attributes["link-mapid"]?.ToString()];
					LoadData(map);
				});
			}

			widget.ToolTip = tooltip;
			widget.MouseLeftButtonDown += new((_, _) => Debug.WriteLine(mapunit.Attributes));
			#endregion

			#region Position
			// The axis direction is diffrent with the layout direction
			float posX = (mapunit.PositionX - MapInfo.LocalAxisX) / MapInfo.Scale;
			float posY = (mapunit.PositionY - MapInfo.LocalAxisY) / MapInfo.Scale;

			LayoutData.SetAlignments(widget, new FVector2D(0.5f, 0.5f));
			LayoutData.SetOffsets(widget, new FLayoutData.Offset(posY, MapInfo.ImageSize - posX, mapunit.SizeX, mapunit.SizeY));
			MapPanel.Children.Add(widget);
			#endregion
		}
	}
	#endregion

	#region Private Fields
	private ICollectionView? source;

	private MapUnit.MapDepthSeq MapDepth;
	List<MapUnit.CategorySeq> CategoryFilter = [];
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