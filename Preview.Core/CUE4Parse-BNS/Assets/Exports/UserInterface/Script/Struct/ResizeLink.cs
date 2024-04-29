using CUE4Parse.UE4;
using CUE4Parse.UE4.Assets.Utils;

namespace CUE4Parse.BNS.Assets.Exports;
[StructFallback]
public class ResizeLink : IUStruct
{
	public bool bEnable { get; set; }
	public EBnsCustomResizeLinkType Type { get; set; }
	public float Offset1 { get; set; }
	public string LinkWidgetName1 { get; set; }
}

public enum EBnsCustomResizeLinkType
{
	BNS_CUSTOM_BORDER_LINK_LEFT,
	BNS_CUSTOM_BORDER_LINK_CENTER,
	BNS_CUSTOM_BORDER_LINK_RIGHT,
	BNS_CUSTOM_BORDER_LINK_RIGHT_AND_LEFT,

	BNS_CUSTOM_WIDGET_LINK_LEFT,
	BNS_CUSTOM_WIDGET_LINK_CENTER,
	BNS_CUSTOM_WIDGET_LINK_RIGHT,
	BNS_CUSTOM_WIDGET_LINK_RIGHT_AND_LEFT,
}