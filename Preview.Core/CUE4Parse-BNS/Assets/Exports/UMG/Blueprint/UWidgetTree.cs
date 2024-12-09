using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports;
public class UWidgetTree : USerializeObject, INamedSlotInterface
{
	[UPROPERTY]
	public FPackageIndex RootWidget;   //UWidget
}