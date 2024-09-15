using CUE4Parse.UE4;
using CUE4Parse.UE4.Assets.Utils;

namespace CUE4Parse.BNS.Assets.Exports;
[StructFallback]
public class BnsCustomResizeLink : IUStruct
{
	public bool bEnable { get; set; }
	public EBnsCustomResizeLinkType Type { get; set; }
	public string LinkWidgetName1 { get; set; }
	public string LinkWidgetName2 { get; set; }
	public float Offset1 { get; set; }
	public float Offset2 { get; set; }
}