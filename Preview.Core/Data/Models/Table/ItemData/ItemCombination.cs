namespace Xylia.Preview.Data.Models;
public sealed class ItemCombination : ModelElement
{
	#region Attributes
	public string Alias { get; set; }


	public Ref<ItemGroup> MaterialGroup { get; set; }

	public Ref<Text> MaterialGroupName { get; set; }

	public short GreatSuccessProbability { get; set; }

	public Ref<ItemGroup> GreatSuccessItemGroup { get; set; }

	public short SuccessProbability { get; set; }

	public Ref<ItemGroup> SuccessItemGroup { get; set; }

	public short FailProbability { get; set; }

	public Ref<ItemGroup> FailItemGroup { get; set; }

	public short BigFailProbability { get; set; }

	public Ref<ItemGroup> BigFailItemGroup { get; set; }

	public Ref<CostGroup> ItemCombinationCostGroup { get; set; }

	public Ref<Text> RewardGroupName { get; set; }
	#endregion
}