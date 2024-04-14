using System.Windows;
using System.Windows.Controls;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Extensions;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
using Xylia.Preview.UI.Helpers.Output;
using Xylia.Preview.UI.Helpers.Output.Tables;
using static Xylia.Preview.Data.Models.UnlocatedStore;

namespace Xylia.Preview.UI.GameUI.Scene.Game_ItemStore;
public partial class LegacyItemStorePanel
{
	#region OnInitialize
	protected override void OnLoading()
	{
		InitializeComponent();

		#region type
		var UnlocatedStore = FileCache.Data.Provider.GetTable<UnlocatedStore>();
		var UnlocatedStoreUi = FileCache.Data.Provider.GetTable<UnlocatedStoreUi>();

		var group = new Dictionary<UnlocatedStoreTypeSeq, TreeViewItem>();
		foreach (var record in UnlocatedStoreUi.Append(new UnlocatedStoreUi()
		{
			UnlocatedStoreType = UnlocatedStoreTypeSeq.UnlocatedNone,
			TitleText = new("UI.ItemStore.Title"),
		}))
		{
			if (record.UnlocatedStoreType > UnlocatedStoreTypeSeq.SoulBoostStore1 &&
				record.UnlocatedStoreType <= UnlocatedStoreTypeSeq.SoulBoostStore6) continue;

			this.TreeView.Items.Add(group[record.UnlocatedStoreType] = new TreeViewImageItem()
			{
				Header = record.TitleText.GetText(),
				//Image = FileCache.Provider.LoadObject(record.TitleIcon)?.GetImage()?.ToImageSource(),
			});
		}
		#endregion

		#region Store
		foreach (var store2 in FileCache.Data.Provider.GetTable<Store2>())
		{
			var type = UnlocatedStore.FirstOrDefault(x => store2.Equals(x.Store2))?.UnlocatedStoreType ?? UnlocatedStoreTypeSeq.UnlocatedNone;
			if (type > UnlocatedStoreTypeSeq.SoulBoostStore1 && type <= UnlocatedStoreTypeSeq.SoulBoostStore6)
				type = UnlocatedStoreTypeSeq.SoulBoostStore1;

			group[type].Items.Add(new TreeViewItem()
			{
				Tag = store2,
				Header = $"[{store2.Name2.GetText()}] {store2}"
			});
		}
		#endregion
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
			IconImage.ToolTip = new ItemTooltipPanel() { DataContext = data.Item1 };
			IconImage.ExpansionComponentList["BackGroundFrameImage"]?.SetValue(data.Item1.BackIcon);
			IconImage.ExpansionComponentList["IconImage"]?.SetValue(data.Item1.FrontIcon);
			IconImage.ExpansionComponentList["Grade_Image"]?.SetValue(null);
			IconImage.ExpansionComponentList["UnusableImage"]?.SetValue(null);
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
				ItemBrand.SetVisibility(itemBuyPrice.ItemBrand != null);
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
	private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.OldValue == e.NewValue) return;
		if (e.NewValue is not Control element || element.Tag is not Store2 record) return;

		// update source
		ItemStore_ItemList.ItemsSource = LinqExtensions.Combine(
			record.Item.Select(x => x.Instance).ToArray(),
			record.BuyPrice.Select(x => x.Instance).ToArray());
	}

	private void ExtractPrice_Click(object sender, RoutedEventArgs e) => OutSet.Start<ItemBuyPriceOut>();

	private void ExtractCloset_Click(object sender, RoutedEventArgs e) => OutSet.Start<ItemCloset>();
	#endregion
}