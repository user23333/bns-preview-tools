using System.Windows;
using System.Windows.Controls;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Common.Interactivity;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Extensions;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;

namespace Xylia.Preview.UI.GameUI.Scene.Game_ItemStore;
public partial class ItemStorePanel
{
	#region Fields
	private readonly ItemStorePanelViewModel _viewModel;
	private ContextMenu? ItemMenu;
	#endregion

	#region OnInitialize
	public ItemStorePanel()
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

	private void ItemStore_ItemList_Column_Initialized(object sender, EventArgs e)
	{
		var widget = (BnsCustomImageWidget)sender;
		widget.MouseEnter += (_, _) => widget.ExpansionComponentList["MouseOver"]!.SetExpansionShow(true);
		widget.MouseLeave += (_, _) => widget.ExpansionComponentList["MouseOver"]!.SetExpansionShow(false);
	}

	private void ItemStore_ItemList_Column_InitializeData(object sender, DependencyPropertyChangedEventArgs e)
	{
		var data = (Tuple<Item, ItemBuyPrice>)e.NewValue;
		var item = data.Item1;
		var itemBuyPrice = data.Item2;

		// update
		var widget = (BnsCustomImageWidget)sender;
		widget.ToolTip = new ItemTooltipPanel() { DataContext = item };

		var Name = widget.GetChild<BnsCustomLabelWidget>("Name");
		if (Name != null) Name.String.LabelText = item.ItemName;

		var IconImage = widget.GetChild<BnsCustomImageWidget>("IconImage");
		if (IconImage != null)
		{
			IconImage.DataContext = item;
			IconImage.ContextMenu = ItemMenu;
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
			var visiable = PriceHoler.SetVisiable(itemBuyPrice != null);
			if (visiable)
			{
				PriceHoler.GetChild<BnsCustomLabelWidget>("money")!.String.LabelText = itemBuyPrice.money;
				PriceHoler.GetChild<BnsCustomLabelWidget>("Coin")!.String.LabelText = itemBuyPrice.Coin;

				var ExtraCost = PriceHoler.GetChild<BnsCustomImageWidget>("ExtraCost");
				if (ExtraCost != null)
				{
					var ItemBrand = ExtraCost.GetChild<BnsCustomImageWidget>("ItemBrand")!;
					ItemBrand.SetVisiable(itemBuyPrice.RequiredItemBrand != null);
					ItemBrand.ExpansionComponentList["IconImage"]?.SetValue(itemBuyPrice.RequiredItemBrand?.Icon);

					for (int i = 0; i < 4; i++)
					{
						var _widget = ExtraCost.GetChild<BnsCustomImageWidget>("DisposeItem_" + (i + 1))!;
						var _item = itemBuyPrice.RequiredItem[i].Value;
						var _count = itemBuyPrice.RequiredItemCount[i];

						if (_widget.SetVisiable(_item != null))
						{
							_widget.DataContext = _item;
							_widget.ToolTip = new BnsTooltipHolder();
							_widget.ExpansionComponentList["IconImage"]?.SetValue(_item.FrontIcon);
							_widget.ExpansionComponentList["Count"]?.SetValue(_count.ToString());
							_widget.ExpansionComponentList["CanSaleItem"]!.bShow = _item.Auctionable;
						}
					}
				}
			}
		}

		var Limit = widget.GetChild<BnsCustomLabelWidget>("Limit");
		if (Limit != null)
		{
			var ContentQuota = itemBuyPrice?.CheckContentQuota.Value;
			if (ContentQuota is null) Limit.Visibility = Visibility.Hidden;
			else
			{
				Limit.Visibility = Visibility.Visible;
				Limit.String.LabelText = ContentQuota.ItemStoreText;
				Limit.ToolTip = ContentQuota.ItemStoreDesc;
			}
		}
	}
	#endregion

	#region Methods 
	private void OnSelectStore(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.OldValue == e.NewValue || e.NewValue is not Store2 record) return;

		// update source
		ItemStore_ItemList_Holder.ScrollToTop();
		ItemStore_ItemList.ItemsSource = LinqExtensions.Tuple(
			record.Item.Select(x => x.Value).ToArray(),
			record.BuyPrice.Select(x => x.Value).ToArray())
			.Where(x => x.Item1 != null);
	}

	private void SearchStarted(object sender, TextChangedEventArgs e)
	{
		_viewModel.Filter = SearcherRule.Text;
		_viewModel.Source?.Refresh();
	}
	#endregion
}