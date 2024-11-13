namespace Xylia.Preview.Data.Models;
public sealed class SoulBoostGradeReward : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public Ref<Item>[] Item { get; set; }

	public short[] ItemCount { get; set; }

	public int Contribution { get; set; }
	#endregion
}