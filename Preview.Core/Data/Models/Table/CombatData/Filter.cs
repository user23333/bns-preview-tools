using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public class Filter : ModelElement
{
	#region Attribute
	public Script_obj Subject { get; set; }
	public Script_obj Target { get; set; }
	public Script_obj Subject2 { get; set; }
	public Script_obj Target2 { get; set; }
	public Ref<Text> Description { get; set; }
	#endregion
}