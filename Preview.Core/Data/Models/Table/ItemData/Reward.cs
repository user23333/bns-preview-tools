using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.Data.Models;
public class Reward : ModelElement
{
	#region Attributes
	public Ref<Item>[] FixedItem { get; set; }

	public short[] FixedItemMin { get; set; }

	public short[] FixedItemMax { get; set; }

	[Name("group-1-2-probability")]
	public short Group12Probability { get; set; }

	[Name("group-1-prob-weight")]
	public short Group1ProbWeight { get; set; }

	[Name("group-1-item")]
	public Ref<Item>[] Group1Item { get; set; }

	[Name("group-1-assured-count")]
	public sbyte Group1AssuredCount { get; set; }

	[Name("group-1-item-total-count")]
	public sbyte Group1ItemTotalCount { get; set; }

	[Name("group-2-prob-weight")]
	public short Group2ProbWeight { get; set; }

	[Name("group-2-item")]
	public Ref<Item>[] Group2Item { get; set; }

	[Name("group-2-assured-count")]
	public sbyte Group2AssuredCount { get; set; }

	[Name("group-2-item-total-count")]
	public sbyte Group2ItemTotalCount { get; set; }

	[Name("group-1-2-total-prob-weight")]
	public int Group12TotalProbWeight { get; set; }

	[Name("group-3-probability")]
	public int Group3Probability { get; set; }

	[Name("group-3-item")]
	public Ref<Item>[] Group3Item { get; set; }

	[Name("group-3-item-prob-weight")]
	public short[] Group3ItemProbWeight { get; set; }

	[Name("group-3-total-prob-weight")]
	public int Group3TotalProbWeight { get; set; }

	[Name("group-3-item-total-count")]
	public short Group3ItemTotalCount { get; set; }

	[Name("group-4-probability")]
	public short Group4Probability { get; set; }

	[Name("group-4-item")]
	public Ref<Item>[] Group4Item { get; set; }

	[Name("group-4-selected-count")]
	public short Group4SelectedCount { get; set; }

	[Name("group-4-item-min")]
	public int[] Group4ItemMin { get; set; }

	[Name("group-4-item-max")]
	public int[] Group4ItemMax { get; set; }

	[Name("group-4-item-total-count")]
	public short Group4ItemTotalCount { get; set; }

	[Name("group-5-probability")]
	public short Group5Probability { get; set; }

	[Name("group-5-item")]
	public Ref<Item>[] Group5Item { get; set; }

	[Name("group-5-selected-count")]
	public short Group5SelectedCount { get; set; }

	[Name("group-5-item-min")]
	public int[] Group5ItemMin { get; set; }

	[Name("group-5-item-max")]
	public int[] Group5ItemMax { get; set; }

	[Name("group-5-item-total-count")]
	public short Group5ItemTotalCount { get; set; }

	public Ref<Item>[] RareItem { get; set; }

	public int RareItemProbWeightType { get; set; }

	public int[] RareItemProbWeight { get; set; }

	public short RareItemTotalCount { get; set; }

	public short[] RareItemMin { get; set; }

	public short[] RareItemMax { get; set; }

	public Ref<Item>[] SelectedItem { get; set; }

	public short[] SelectedItemCount { get; set; }

	public sbyte SelectedItemAssuredCount { get; set; }

	public Ref<Item>[] RandomItem { get; set; }
	#endregion

	#region Methods
	public List<RewardInfo> GetInfos()
	{
		var FormatText = "获得其中{0}种";
		var datas = new List<RewardInfo>();

		for (int i = 0; i < FixedItem.Length; i++)
		{
			var item = FixedItem[i].Instance;
			if (item is null) continue;

			datas.Add(new RewardInfo()
			{
				Item = item,
				Group = new("fixed", "固定获得"),
				Min = FixedItemMin[i],
				Max = FixedItemMax[i],
			});
		}

		if (Group12Probability > 0)
		{
			var probability = (double)Group12Probability / Group12TotalProbWeight;

			for (int i = 0; i < Group1Item.Length; i++)
			{
				var item = Group1Item[i].Instance;
				if (item is null) continue;

				datas.Add(new RewardInfo()
				{
					Item = item,
					Group = new("group-1", string.Format("{1}% " + FormatText, Group1AssuredCount, Group1ProbWeight * probability)),
					Min = 1,
					Max = 1,
				});
			}

			for (int i = 0; i < Group2Item.Length; i++)
			{
				var item = Group2Item[i].Instance;
				if (item is null) continue;

				datas.Add(new RewardInfo()
				{
					Item = item,
					Group = new("group-2", string.Format("{1}% " + FormatText, Group2AssuredCount, Group2ProbWeight * probability)),
					Min = 1,
					Max = 1,
				});
			}
		}

		if (Group3Probability > 0)
		{
			for (int i = 0; i < Group3Item.Length; i++)
			{
				var item = Group3Item[i].Instance;
				if (item is null) continue;

				datas.Add(new RewardInfo()
				{
					Item = item,
					Group = new("group-3", string.Format("{1}% " + FormatText, 1, Group3Probability)),
					Probability = Group3ItemProbWeight[i],
					ProbabilityType = Group3TotalProbWeight,
				});
			}
		}

		if (Group4Probability > 0)
		{
			var TotalProbWeight = Group4ItemMax.Sum();
			for (int i = 0; i < Group4Item.Length; i++)
			{
				var item = Group4Item[i].Instance;
				if (item is null) continue;

				var min = Group4ItemMin[i];
				var max = Group4ItemMax[i];

				datas.Add(new RewardInfo()
				{
					Item = item,
					Group = new("group-4", string.Format("{1}% " + FormatText, Group4SelectedCount, Group4Probability)),
					Probability = max,
					ProbabilityType = TotalProbWeight,
				});
			}
		}

		if (Group5Probability > 0)
		{
			var TotalProbWeight = Group5ItemMax.Sum();
			for (int i = 0; i < Group5Item.Length; i++)
			{
				var item = Group5Item[i].Instance;
				if (item is null) continue;

				var min = Group5ItemMin[i];
				var max = Group5ItemMax[i];

				datas.Add(new RewardInfo()
				{
					Item = item,
					Group = new("group-5", string.Format("{1}% " + FormatText, Group4SelectedCount, Group4Probability)),
					Probability = max,
					ProbabilityType = TotalProbWeight,
				});
			}
		}

		var RareItemTotalProbWeight = RareItemProbWeight.Sum();
		if (RareItemTotalProbWeight > 0)
		{
			for (int i = 0; i < RareItem.Length; i++)
			{
				var item = RareItem[i].Instance;
				if (item is null) continue;

				datas.Add(new RewardInfo()
				{
					Item = item,
					Group = new("rare", string.Format(FormatText, 1)),
					Min = RareItemMin[i],
					Max = RareItemMax[i],
					Probability = RareItemProbWeight[i],
					ProbabilityType = RareItemProbWeightType,
				});
			}
		}

		if (SelectedItemAssuredCount > 0)
		{
			for (int i = 0; i < SelectedItem.Length; i++)
			{
				var item = SelectedItem[i].Instance;
				if (item is null) continue;

				datas.Add(new RewardInfo()
				{
					Item = item,
					Group = new("selected", string.Format("选择" + FormatText, SelectedItemAssuredCount)),
					Min = SelectedItemCount[i],
					Max = SelectedItemCount[i],
				});
			}
		}

		return datas;
	}

	public class RewardInfo
	{
		public Item Item;
		public (string, string) Group;
		public short Min = 1;
		public short Max = 1;
		public int Probability;
		public int ProbabilityType;

		public string CountInfo => Min != Max ? string.Format("{0}-{1}", Min, Max) : string.Format("{0}", Min);
		public string ProbabilityInfo => Probability == 0 ? string.Empty : ((double)Probability / ProbabilityType).ToString("P" + ProbabilityType.GetPercentLength());
	}
	#endregion
}