using System.Windows;
using System.Windows.Controls;
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
	protected override void OnLoaded(RoutedEventArgs e)
	{
		if (DataContext is not Record record) return;

		var ImproveId = record.Attributes.Get<int>("improve-id");
		var ImproveLevel = record.Attributes.Get<sbyte>("improve-level");

		var Improvees = FileCache.Data.Provider.GetTable<ItemImprove>().Where(record => record.Id == ImproveId && record.SuccessOptionListId != 0);
		ItemGrowth2Panel_DrawImproveOption2_Before.ItemsSource = Improvees;
	}

	private void DrawImproveOption_Before_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.AddedItems[0] is not ItemImprove Improve) return;

		// search options
		var OptionList = FileCache.Data.Provider.GetTable<ItemImproveOptionList>()[Improve.SuccessOptionListId + ((long)JobSeq.JobNone >> 32)];
		
		ItemGrowth2Panel_DrawImproveOption2_After.ItemsSource = OptionList.Options;
		ItemGrowth2Panel_DrawImproveOption2_Desc.String.LabelText = string.Join("<br/>", OptionList.CreateRecipe());
	}															 
	#endregion
}