using System.Windows;
using System.Windows.Controls;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Common.Interactivity;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Extensions;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;

namespace Xylia.Preview.UI.GameUI.Scene.Game_ItemStore;
public partial class LegacyItemStorePanel
{
	#region Fields
	private readonly ItemStorePanelViewModel _viewModel;
	private ContextMenu? ItemMenu;
	#endregion

	#region OnInitialize
	public LegacyItemStorePanel()
	{
		InitializeComponent();

		DataContext = _viewModel = new ItemStorePanelViewModel();
		TreeView.ItemsSource = _viewModel.Source.Groups;
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
			IconImage.ExpansionComponentList["BackGroundFrameImage"]?.SetValue(item.BackgroundImage);
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
					ItemBrand.ExpansionComponentList["IconImage"]?.SetValue(itemBuyPrice.ItemBrand?.Icon);

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
		_viewModel.Source?.Refresh();
	}
	#endregion
}