using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.UI.GameUI.Scene.Game_ItemGrowth2;
public partial class ItemGrowth2TooltipPanel
{
	#region Constructors
	public ItemGrowth2TooltipPanel()
	{
#if DEVELOP
		// General_Emblem_RynSword_Style22_0003_108
		DataContext = Helpers.TestProvider.Provider.GetTable<Item>()["Test_N-ShopAccountShippingItem"].Source;
#endif

		InitializeComponent();
	}
	#endregion

	#region Methods
	private sbyte ImproveLevel = 0;

	protected override void OnLoaded(RoutedEventArgs e)
	{
		if (DataContext is not Record record) return;

		var ImproveId = record.Attributes.Get<int>("improve-id");
		ImproveLevel = record.Attributes.Get<sbyte>("improve-level");

		var Improves = FileCache.Data.Provider.GetTable<ItemImprove>().Where(record => record.Id == ImproveId);
		ItemGrowth2Panel_DrawImproveOption2_Before.ItemsSource = Improves.Where(record => record.SuccessOptionListId != 0);
		ItemGrowth2Panel_DrawImproveOption2_Level.ItemsSource = Improves.Select(record => record.Level);
		ItemGrowth2Panel_DrawImproveOption2_Level.SelectedItem = ImproveLevel;
	}

	private void DrawImproveOption_Level_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.AddedItems[0] is not sbyte level) return;

		ImproveLevel = level;
		DrawImproveOption_Before_SelectionChanged(ItemGrowth2Panel_DrawImproveOption2_Before, null);
	}

	private void DrawImproveOption_Before_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (sender is not Selector selector || selector.SelectedItem is not ItemImprove Improve) return;

		// search options
		var OptionList = FileCache.Data.Provider.GetTable<ItemImproveOptionList>()[Improve.SuccessOptionListId + ((long)JobSeq.JobNone << 32)];
		
		ItemGrowth2Panel_DrawImproveOption2_After.ItemsSource = OptionList.GetOptions(ImproveLevel);
		ItemGrowth2Panel_DrawImproveOption2_Desc.String.LabelText = string.Join("<br/>", OptionList.CreateRecipe());
	}
	#endregion
}