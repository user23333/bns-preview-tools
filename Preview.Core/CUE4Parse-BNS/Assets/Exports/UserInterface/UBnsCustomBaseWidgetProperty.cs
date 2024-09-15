using CUE4Parse.UE4.Assets.Exports;

namespace CUE4Parse.BNS.Assets.Exports;
public abstract class UBnsCustomBaseWidgetProperty : USerializeObject
{

}


public class UBnsCustomCaptionProperty : UBnsCustomBaseWidgetProperty { }

public class UBnsCustomImageProperty : UBnsCustomBaseWidgetProperty { }

public class UBnsCustomRadioButtonProperty  : UBnsCustomBaseWidgetProperty { }

public class UBnsCustomWindowProperty : UBnsCustomBaseWidgetProperty { }