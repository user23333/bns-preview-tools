using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.GameUI.Scene.Game_ItemStore;
internal class ItemStorePanelViewModel : ObservableObject
{
	#region Constructors
	public ItemStorePanelViewModel()
	{
		Source = CollectionViewSource.GetDefaultView(Globals.GameData.Provider.GetTable<Store2>());
		Source.Filter = OnFilter;
		Source.GroupDescriptions.Clear();
		Source.GroupDescriptions.Add(new StoreGroupDescription());
	}
	#endregion

	#region Fields
	public ICollectionView Source { get; }

	private string? _filter;
	public string? Filter
	{
		get => _filter;
		set
		{
			SetProperty(ref _filter, value);
		}
	}
	#endregion

	#region Methods
	private bool OnFilter(object obj)
	{
		if (obj is not Store2 store2) return false;

		var rule = Filter;
		return string.IsNullOrEmpty(rule) ||
			(store2.Alias != null && store2.Alias.Contains(rule, StringComparison.OrdinalIgnoreCase)) ||
			(store2.Name != null && store2.Name.Contains(rule, StringComparison.OrdinalIgnoreCase));
	}

	private class StoreGroupDescription : PropertyGroupDescription
	{
		private readonly Dictionary<Store2, UnlocatedStoreUi> dict = [];

		public StoreGroupDescription()
		{
			var UnlocatedStore = Globals.GameData.Provider.GetTable<UnlocatedStore>();
			var UnlocatedStoreUi = Globals.GameData.Provider.GetTable<UnlocatedStoreUi>();

			foreach (var record in UnlocatedStore)
			{
				var store2 = record.Store2.Value;
				if (store2 != null) dict[store2] = UnlocatedStoreUi[(sbyte)record.UnlocatedStoreType];
			}
		}

		public override object? GroupNameFromItem(object item, int level, CultureInfo culture)
		{
			if (item is Store2 store2)
			{
				if (dict.TryGetValue(store2, out var data))
				{
					return data?.TitleText.GetText();
				}
				else
				{
					return "UI.ItemStore.Title".GetText();
				}
			}

			return base.GroupNameFromItem(item, level, culture);
		}
	}
	#endregion
}