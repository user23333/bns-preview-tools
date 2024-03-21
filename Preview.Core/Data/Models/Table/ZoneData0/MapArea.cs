using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public class MapArea : ModelElement, IHaveName
{
	#region Attributes
	public short Id { get; set; }
	public string Alias { get; set; }
	public Ref<Text> Name2 { get; set; }
	#endregion

	#region Methods
	public string Text => Name2.GetText();
	#endregion
}