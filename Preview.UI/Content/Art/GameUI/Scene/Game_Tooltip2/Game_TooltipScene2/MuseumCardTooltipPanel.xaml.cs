using System.Windows;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Document;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip2;
public partial class MuseumCardTooltipPanel
{
	public MuseumCardTooltipPanel()
	{
		InitializeComponent();
#if DEVELOP
		DataContext = Xylia.Preview.Common.Globals.GameData.Provider.GetTable<WorldAccountCard>()["WorldAccountCard_0150"];
#endif
	}

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not WorldAccountCard record) return;

		var item = record.Item.Value;

		MuseumCardTooltipPanel_Card.ExpansionComponentList["Icon"]!.SetValue(new MyFPackageIndex(record.CardImage));
		MuseumCardTooltipPanel_Card.ExpansionComponentList["CardName"]!.SetValue(item?.ItemNameOnly);
		MuseumCardTooltipPanel_Card.ExpansionComponentList["CardName"].StringProperty.ClippingBound = new FVector2D(0, -155); // need fix padding...
		MuseumCardTooltipPanel_Card.ExpansionComponentList["Grade"]!.SetValue(new MyFPackageIndex($"BNSR/Content/Art/UI/GameUI/Resource/GameUI_ImageSet/CollectionCard_Preview_{item.ItemGrade}.CollectionCard_Preview_{item.ItemGrade}"));
		MuseumCardTooltipPanel_Card.ExpansionComponentList["UnusableImage"]!.SetValue(item?.UnusableImage);

		MuseumCardTooltipPanel_Collection.String.LabelText = string.Join(BR.Tag, record.CanUseCollection.Select(x => x.CollectionName.GetText()));
	}
	#endregion
}
