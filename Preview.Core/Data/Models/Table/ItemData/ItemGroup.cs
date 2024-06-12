namespace Xylia.Preview.Data.Models;
public sealed class ItemGroup : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<Item>[] Item { get; set; }

	public sbyte ItemTotalCount { get; set; }
	#endregion
}