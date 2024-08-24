namespace Xylia.Preview.Data.Models;
public sealed class ItemGroup : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public Ref<Item>[] MemberItem { get; set; }

	public sbyte MemberItemCount { get; set; }
	#endregion
}