using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Common.Interactivity;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Extensions;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
using Xylia.Preview.UI.Helpers.Output;
using Xylia.Preview.UI.Helpers.Output.Tables;

namespace Xylia.Preview.UI.GameUI.Scene.Game_ItemStore;
public partial class LegacyItemStorePanel
{
	#region OnInitialize
	protected override void OnLoading()
	{
		InitializeComponent();

		// data
		source = CollectionViewSource.GetDefaultView(FileCache.Data.Provider.GetTable<Store2>());
		source.Filter = OnFilter;
		source.GroupDescriptions.Clear();
		source.GroupDescriptions.Add(new StoreGroupDescription());
		TreeView.ItemsSource = source.Groups;
	}

	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);

		ItemMenu = (ContextMenu)TryFindResource("ItemMenu");
		RecordCommand.Find("item", (act) => RecordCommand.Bind(act, ItemMenu));
	}

	private void ItemStore_ItemList_Column_1_1_Initialized(object sender, EventArgs e)
	{
		// data
		var widget = (BnsCustomImageWidget)sender;
		var data = (Tuple<Item, ItemBuyPrice>)widget.DataContext;
		var item = data.Item1;
		var itemBuyPrice = data.Item2;

		// update
		var Name = widget.GetChild<BnsCustomLabelWidget>("Name");
		if (Name != null) Name.String.LabelText = item.ItemName;

		var IconImage = widget.GetChild<BnsCustomImageWidget>("IconImage");
		if (IconImage != null)
		{
			IconImage.DataContext = item;
			IconImage.ContextMenu = ItemMenu;
			IconImage.ToolTip = new ItemTooltipPanel() { DataContext = item };
			IconImage.ExpansionComponentList["BackGroundFrameImage"]?.SetValue(item.BackIcon);
			IconImage.ExpansionComponentList["IconImage"]?.SetValue(item.FrontIcon);
			IconImage.ExpansionComponentList["Grade_Image"]?.SetValue(null);
			IconImage.ExpansionComponentList["UnusableImage"]?.SetValue(null);
			IconImage.ExpansionComponentList["CanSaleItem"]?.SetValue(item.CanSaleItemImage);
			IconImage.ExpansionComponentList["DisableBuyImage"]?.SetExpansionShow(itemBuyPrice is null);
		}

		var PriceHoler = widget.GetChild<BnsCustomImageWidget>("PriceHoler");
		if (PriceHoler != null)
		{
			if (itemBuyPrice is null)
			{
				PriceHoler.SetVisiable(false);
			}
			else
			{
				PriceHoler.GetChild<BnsCustomLabelWidget>("money")!.String.LabelText = itemBuyPrice.money;
				PriceHoler.GetChild<BnsCustomLabelWidget>("Coin")!.String.LabelText = itemBuyPrice.Coin;

				var ExtraCost = PriceHoler.GetChild<BnsCustomImageWidget>("ExtraCost");
				if (ExtraCost != null)
				{
					var ItemBrand = ExtraCost.GetChild<BnsCustomImageWidget>("ItemBrand")!;
					ItemBrand.SetVisiable(itemBuyPrice.ItemBrand != null);
					ItemBrand.ExpansionComponentList["IconImage"]?.SetValue(itemBuyPrice.ItemBrand?.FrontIcon);

					DisposeItem_Initialized(ExtraCost.GetChild<BnsCustomImageWidget>("DisposeItem_1")!, itemBuyPrice.RequiredItem[0], itemBuyPrice.RequiredItemCount[0]);
					DisposeItem_Initialized(ExtraCost.GetChild<BnsCustomImageWidget>("DisposeItem_2")!, itemBuyPrice.RequiredItem[1], itemBuyPrice.RequiredItemCount[1]);
					DisposeItem_Initialized(ExtraCost.GetChild<BnsCustomImageWidget>("DisposeItem_3")!, itemBuyPrice.RequiredItem[2], itemBuyPrice.RequiredItemCount[2]);
					DisposeItem_Initialized(ExtraCost.GetChild<BnsCustomImageWidget>("DisposeItem_4")!, itemBuyPrice.RequiredItem[3], itemBuyPrice.RequiredItemCount[3]);
				}
			}
		}

		var Limit = widget.GetChild<BnsCustomLabelWidget>("Limit");
		if (Limit != null)
		{
			var ContentQuota = itemBuyPrice?.CheckContentQuota.Instance;
			if (ContentQuota is null) Limit.Visibility = Visibility.Hidden;
			else
			{
				Limit.Visibility = Visibility.Visible;
				Limit.String.LabelText = ContentQuota.ItemStoreText;
				Limit.ToolTip = ContentQuota.ItemStoreDesc;
			}
		}
	}

	private static void DisposeItem_Initialized(BnsCustomImageWidget widget, Item item, short count)
	{
		if (item is null)
		{
			widget.Visibility = Visibility.Collapsed;
			return;
		}

		widget.Visibility = Visibility.Visible;
		widget.ToolTip = new ItemTooltipPanel() { DataContext = item };
		widget.ExpansionComponentList["IconImage"]?.SetValue(item.FrontIcon);
		widget.ExpansionComponentList["Count"]?.SetValue(count.ToString());
		widget.ExpansionComponentList["CanSaleItem"]!.bShow = item.Auctionable;
		//widget.InvalidateVisual();
	}
	#endregion

	#region Methods 
	private void SelectedStoreChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.OldValue == e.NewValue || e.NewValue is not Store2 record) return;

		// update source
		ItemStore_ItemList.ItemsSource = LinqExtensions.Tuple(
			record.Item.Select(x => x.Instance).ToArray(),
			record.BuyPrice.Select(x => x.Instance).ToArray())
			.Where(x => x.Item1 != null);
	}

	private void SearchStarted(object sender, TextChangedEventArgs e)
	{
		source?.Refresh();
	}

	private bool OnFilter(object obj)
	{
		if (obj is not Store2 store2) return false;

		var rule = SearcherRule.Text;
		return string.IsNullOrEmpty(rule) ||
			(store2.Alias != null && store2.Alias.Contains(rule, StringComparison.OrdinalIgnoreCase)) ||
			(store2.Name != null && store2.Name.Contains(rule, StringComparison.OrdinalIgnoreCase));
	}


	private async void ExtractPrice_Click(object sender, RoutedEventArgs e) => await OutSet.Start<ItemBuyPriceOut>();

	private async void ExtractCloset_Click(object sender, RoutedEventArgs e) => await OutSet.Start<ItemCloset>();
	#endregion


	#region Helpers
	private ICollectionView? source;
	private ContextMenu? ItemMenu;

	private class StoreGroupDescription : PropertyGroupDescription
	{
		private readonly Dictionary<Store2, UnlocatedStore> dict = [];

		public StoreGroupDescription()
		{
			var UnlocatedStore = FileCache.Data.Provider.GetTable<UnlocatedStore>();
			foreach (var record in UnlocatedStore)
			{
				var store2 = record.Store2.Instance;
				if (store2 != null) dict[store2] = record;
			}
		}

		public override object? GroupNameFromItem(object item, int level, CultureInfo culture)
		{
			if (item is Store2 store2)
			{
				if (dict.TryGetValue(store2, out var record))
				{
					var UnlocatedStoreUi = FileCache.Data.Provider.GetTable<UnlocatedStoreUi>()[(sbyte)record.UnlocatedStoreType];
					return UnlocatedStoreUi?.TitleText.GetText();
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