using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
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

	private void ItemStore_ItemList_Column_1_1_Initialized(object sender, EventArgs e)
	{
		var width = (BnsCustomImageWidget)sender;
		var data = (Tuple<Item, ItemBuyPrice>)width.DataContext;
		var itemBuyPrice = data.Item2;

		var Name = width.GetChild<BnsCustomLabelWidget>("Name");
		if (Name != null) Name.String.LabelText = data.Item1.ItemName;

		var IconImage = width.GetChild<BnsCustomImageWidget>("IconImage");
		if (IconImage != null)
		{
			var item = data.Item1;
			IconImage.ToolTip = new ItemTooltipPanel() { DataContext = item };
			IconImage.ExpansionComponentList["BackGroundFrameImage"]?.SetValue(item.BackIcon);
			IconImage.ExpansionComponentList["IconImage"]?.SetValue(item.FrontIcon);
			IconImage.ExpansionComponentList["Grade_Image"]?.SetValue(null);
			IconImage.ExpansionComponentList["UnusableImage"]?.SetValue(null);
			IconImage.ExpansionComponentList["CanSaleItem"]?.SetValue(item.CanSaleItemImage);
		}

		var PriceHoler = width.GetChild<BnsCustomImageWidget>("PriceHoler");
		if (PriceHoler != null)
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

		var Limit = width.GetChild<BnsCustomLabelWidget>("Limit");
		if (Limit != null)
		{
			var ContentQuota = itemBuyPrice?.CheckContentQuota.Instance;
			if (ContentQuota is null) Limit.Visibility = Visibility.Hidden;
			else
			{
				Limit.Visibility = Visibility.Visible;
				Limit.String.LabelText = ContentQuota.Text;
				Limit.ToolTip = ContentQuota.Describe;
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
		widget.ExpansionComponentList["CanSaleItem"]!.bShow = item.Auctionable;
		widget.ExpansionComponentList["Count"]?.SetValue(count.ToString());
	}
	#endregion

	#region Methods 
	private void SelectedStoreChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.OldValue == e.NewValue || e.NewValue is not Store2 record) return;

		// update source
		ItemStore_ItemList.ItemsSource = LinqExtensions.Combine(
			record.Item.Select(x => x.Instance).ToArray(),
			record.BuyPrice.Select(x => x.Instance).ToArray());
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


	private void ExtractPrice_Click(object sender, RoutedEventArgs e) => OutSet.Start<ItemBuyPriceOut>();

	private void ExtractCloset_Click(object sender, RoutedEventArgs e) => OutSet.Start<ItemCloset>();
	#endregion


	#region Helpers
	private ICollectionView? source;

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