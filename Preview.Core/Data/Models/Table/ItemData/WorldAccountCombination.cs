using Xylia.Preview.Common.Extension;
using static Xylia.Preview.Data.Models.ItemCombination;

namespace Xylia.Preview.Data.Models;
public sealed class WorldAccountCombination : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public int Count { get; set; }

	public ItemTypeSeq ItemType { get; set; }

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

	public Ref<CostGroup> WorldAccountCombinationCostGroup { get; set; }

	public Ref<Text> RewardGroupName { get; set; }
	#endregion

	#region Methods
	public List<ItemCombinationInfo> GetInfo()
	{
		var data = new List<ItemCombinationInfo>();

		void AddGroup(ItemGroup group, short probability, string name)
		{
			if (group is null) return;

			var MemberProb = probability * 0.0001d / group.MemberItemCount;
			foreach (var item in group.MemberItem.Values())
			{
				data.Add(new()
				{
					Data = item,
					Group = name,
					Probability = MemberProb,
				});
			}
		}

		AddGroup(GreatSuccessItemGroup, GreatSuccessProbability, "great-success");
		AddGroup(SuccessItemGroup, SuccessProbability, "success");
		AddGroup(FailItemGroup, FailProbability, "fail");
		AddGroup(BigFailItemGroup, BigFailProbability, "big-fail");

		return data;
	}
	#endregion
}