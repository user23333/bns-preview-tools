namespace Xylia.Preview.Data.Models;
public class Zone : ModelElement
{
	#region Attributes
	public Ref<MapInfo> Map { get; set; }
	public Ref<MapArea> Area { get; set; }
	#endregion
}