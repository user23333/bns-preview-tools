using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Common.Interactivity;

namespace Xylia.Preview.UI.GameUI.Scene.Game_MapWindow;
internal class MapWindowPanelViewModel : ObservableObject, IRecordFilter
{
	#region Properties
	private ICollectionView? _source;
	public ICollectionView Source
	{
		get => _source;
		set
		{
			_source = value;
			_source.Filter += OnFilter;
			_source.GroupDescriptions.Clear();
			_source.GroupDescriptions.Add(new MapGroupDescription());
		}
	}

	private string? _searchRule;
	public string? SearchRule
	{
		get => _searchRule;
		set
		{
			SetProperty(ref _searchRule, value);
			IsExpand = !string.IsNullOrWhiteSpace(value);

			_zone = Globals.GameData.Provider.GetTable<Zone>()[value];
			_source?.Refresh();
		}
	}

	private bool isExpand;
	public bool IsExpand { get => isExpand; private set => SetProperty(ref isExpand, value); }
	#endregion

	#region Methods
	private Zone? _zone;

	public bool OnFilter(object obj)
	{
		if (string.IsNullOrEmpty(_searchRule)) return true;
		if (obj is not MapInfo mapinfo) return false;
		if (mapinfo.Alias != null && mapinfo.Alias.Contains(_searchRule, StringComparison.OrdinalIgnoreCase)) return true;
		if (mapinfo.Name != null && mapinfo.Name.Contains(_searchRule, StringComparison.OrdinalIgnoreCase)) return true;

		// zone
		if (_zone != null)
		{
			if (_zone.Map.HasValue && _zone.Map == mapinfo) return true;
		}

		return false;
	}
	#endregion

	#region Helpers
	private class MapGroupDescription : PropertyGroupDescription
	{
		public override object? GroupNameFromItem(object item, int level, CultureInfo culture)
		{
			if (item is MapInfo MapInfo) return MapInfo.MapGroup1.Value?.Name;

			return base.GroupNameFromItem(item, level, culture);
		}
	}
	#endregion
}