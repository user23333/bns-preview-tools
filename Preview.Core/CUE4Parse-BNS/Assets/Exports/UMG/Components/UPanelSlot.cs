using CUE4Parse.UE4.Objects.Core;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.UE4.Assets.Exports;
public abstract class UPanelSlot : UVisual
{
	[UPROPERTY] public FPackageIndex Content;
	[UPROPERTY] public FPackageIndex Parent;
	[UPROPERTY] public FLayout LayoutData;
	// Padding
}

public class UBackgroundBlurSlot : UPanelSlot
{

}
public class UBorderSlot : UPanelSlot
{

}
public class UButtonSlot : UPanelSlot
{

}
public class UCanvasPanelSlot : UPanelSlot
{

}
public class UGridSlot : UPanelSlot
{

}
public class UHorizontalBoxSlot : UPanelSlot
{

}
public class ULoadGuardSlot : UPanelSlot
{

}
public class UOverlaySlot : UPanelSlot
{

}
public class UCommonVisibilitySwitcherSlot : UPanelSlot
{

}
public class USafeZoneSlot : UPanelSlot
{

}
public class UScaleBoxSlot : UPanelSlot
{

}
public class UScrollBox : UPanelSlot
{

}
public class USizeBoxSlot : UPanelSlot
{

}
public class UStackBoxSlot : UPanelSlot
{

}
public class UUniformGridSlot : UPanelSlot
{

}
public class UVerticalBoxSlot  : UPanelSlot
{

}
public class UWidgetSwitcherSlot : UPanelSlot
{

}
public class UWindowTitleBarAreaSlot : UPanelSlot
{

}
public class UWrapBox : UPanelSlot
{

}