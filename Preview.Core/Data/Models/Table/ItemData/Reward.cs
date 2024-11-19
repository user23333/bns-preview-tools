using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.Data.Models;
public class Reward : ModelElement, IReward
{
	#region Attributes
	public string Alias { get; set; }

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
	public short Group3Probability { get; set; }

	[Name("group-3-item")]
	public Ref<Item>[] Group3Item { get; set; }

	[Name("group-3-item-prob-weight")]
	public short[] Group3ItemProbWeight { get; set; }

	[Name("group-3-item-total-prob-weight")]
	public int Group3ItemTotalProbWeight { get; set; }

	[Name("group-3-item-total-count")]
	public sbyte Group3ItemTotalCount { get; set; }

	[Name("group-4-probability")]
	public short Group4Probability { get; set; }

	[Name("group-4-item")]
	public Ref<Item>[] Group4Item { get; set; }

	[Name("group-4-selected-count")]
	public sbyte Group4SelectedCount { get; set; }   //?

	[Name("group-4-item-min")]
	public int[] Group4ItemMin { get; set; }

	[Name("group-4-item-max")]
	public int[] Group4ItemMax { get; set; }

	[Name("group-4-item-total-count")]
	public sbyte Group4ItemTotalCount { get; set; }

	[Name("group-5-probability")]
	public short Group5Probability { get; set; }

	[Name("group-5-item")]
	public Ref<Item>[] Group5Item { get; set; }

	[Name("group-5-selected-count")]
	public sbyte Group5SelectedCount { get; set; }   //?

	[Name("group-5-item-min")]
	public int[] Group5ItemMin { get; set; }

	[Name("group-5-item-max")]
	public int[] Group5ItemMax { get; set; }

	[Name("group-5-item-total-count")]
	public sbyte Group5ItemTotalCount { get; set; }

	public Ref<Item>[] RareItem { get; set; }

	public int RareItemProbWeightType { get; set; }

	public int[] RareItemProbWeight { get; set; }

	public sbyte RareItemTotalCount { get; set; }

	public short[] RareItemMin { get; set; }

	public short[] RareItemMax { get; set; }

	public Ref<Item>[] SelectedItem { get; set; }

	public short[] SelectedItemCount { get; set; }

	public sbyte SelectedItemAssuredCount { get; set; }

	public Ref<Item>[] RandomItem { get; set; }

	public Ref<SmartDropReward>[] SmartFixedReward { get; set; }

	[Name("smart-group-1-2-probability")]
	public short SmartGroup12Probability { get; set; }

	[Name("smart-group-1-prob-weight")]
	public short SmartGroup1ProbWeight { get; set; }

	[Name("smart-group-1-reward")]
	public Ref<SmartDropReward>[] SmartGroup1Reward { get; set; }

	[Name("smart-group-1-assured-count")]
	public sbyte SmartGroup1AssuredCount { get; set; }

	[Name("smart-group-1-reward-total-count")]
	public sbyte SmartGroup1RewardTotalCount { get; set; }

	[Name("smart-group-2-prob-weight")]
	public short SmartGroup2ProbWeight { get; set; }

	[Name("smart-group-2-reward")]
	public Ref<SmartDropReward>[] SmartGroup2Reward { get; set; }

	[Name("smart-group-2-assured-count")]
	public sbyte SmartGroup2AssuredCount { get; set; }

	[Name("smart-group-2-reward-total-count")]
	public sbyte SmartGroup2RewardTotalCount { get; set; }

	[Name("smart-group-1-2-total-prob-weight")]
	public int SmartGroup12TotalProbWeight { get; set; }

	[Name("smart-group-3-probability")]
	public short SmartGroup3Probability { get; set; }

	[Name("smart-group-3-reward")]
	public Ref<SmartDropReward>[] SmartGroup3Reward { get; set; }

	[Name("smart-group-3-reward-prob-weight")]
	public short[] SmartGroup3RewardProbWeight { get; set; }

	[Name("smart-group-3-reward-total-prob-weight")]
	public int SmartGroup3RewardTotalProbWeight { get; set; }

	[Name("smart-group-3-reward-total-count")]
	public sbyte SmartGroup3RewardTotalCount { get; set; }

	[Name("smart-group-4-probability")]
	public short SmartGroup4Probability { get; set; }

	[Name("smart-group-4-reward")]
	public Ref<SmartDropReward>[] SmartGroup4Reward { get; set; }

	[Name("smart-group-4-selected-count")]
	public sbyte SmartGroup4SelectedCount { get; set; }

	[Name("smart-group-4-reward-total-count")]
	public sbyte SmartGroup4RewardTotalCount { get; set; }

	[Name("smart-group-5-probability")]
	public short SmartGroup5Probability { get; set; }

	[Name("smart-group-5-reward")]
	public Ref<SmartDropReward>[] SmartGroup5Reward { get; set; }

	[Name("smart-group-5-selected-count")]
	public sbyte SmartGroup5SelectedCount { get; set; }

	[Name("smart-group-5-reward-total-count")]
	public sbyte SmartGroup5RewardTotalCount { get; set; }

	public Ref<SmartDropReward>[] SmartRareReward { get; set; }

	public int SmartRareRewardProbWeightType { get; set; }

	public int[] SmartRareRewardProbWeight { get; set; }

	public sbyte SmartRareRewardTotalCount { get; set; }
	#endregion

	#region Methods
	IEnumerable<IRewardHelper> IReward.GetRewards() => GetRewards();

	public IEnumerable<RewardInfo> GetRewards()
	{
		var data = new List<RewardInfo>();

		#region Normal		
		FixedItem.Values().ForEach((item, idx) =>
		{
			data.Add(new RewardInfo()
			{
				Data = item,
				Group = "fixed",
				GroupText = "UI.RandomBox.Probability.MiddleCategory.FixedReward".GetText(),
				Min = FixedItemMin[idx],
				Max = FixedItemMax[idx],
			});
		});

		if (Group12Probability > 0)
		{
			var probability = (double)Group12Probability / Group12TotalProbWeight;

			Group1Item.Values().ForEach(item =>
			{
				data.Add(new RewardInfo()
				{
					Data = item,
					Group = "group-1",
					GroupText = string.Format("{0}% ", Group1ProbWeight * probability) + "UI.RandomBox.Probability.MiddleCategory.GroupReward1".GetText([Group1AssuredCount]),
				});
			});

			Group2Item.Values().ForEach(item =>
			{
				data.Add(new RewardInfo()
				{
					Data = item,
					Group = "group-2",
					GroupText = string.Format("{0}% ", Group2ProbWeight * probability) + "UI.RandomBox.Probability.MiddleCategory.GroupReward2".GetText([Group2AssuredCount]),
				});
			});
		}

		if (Group3ItemTotalCount > 0)
		{
			Group3Item.Values().ForEach((item, idx) =>
			{
				data.Add(new RewardInfo()
				{
					Data = item,
					Group = "group-3",
					GroupText = string.Format("{0}% ", Group3Probability) + "UI.RandomBox.Probability.MiddleCategory.GroupReward3".GetText([1]),
					Probability = Group3ItemProbWeight[idx],
					ProbabilityType = Group3ItemTotalProbWeight,
				});
			});
		}

		// For group4 and group5, the integer part affects count, the decimal part affects probability.
		// WARNING: When the probability of quantity cannot be viewed, the probability is corrected to the quantity is greater than 0.
		if (Group4ItemTotalCount > 0)
		{
			Group4Item.Values().ForEach((item, idx) =>
			{
				int min = Group4ItemMin[idx], max = Group4ItemMax[idx];

				data.Add(new RewardInfo()
				{
					Data = item,
					Group = "group-4",
					GroupText = string.Format("{0}% ", Group4Probability) + "UI.RandomBox.Probability.MiddleCategory.GroupReward4".GetText([1]),
					Min = (short)Math.Floor(min * 0.01),
					Max = (short)Math.Ceiling(max * 0.01),
					Probability = Math.Min(100d, min) / Group4ItemTotalCount,
					ProbabilityType = 100,
				});
			});
		}

		if (Group5ItemTotalCount > 0)
		{
			Group5Item.Values().ForEach((item, idx) =>
			{
				int min = Group5ItemMin[idx], max = Group5ItemMax[idx];

				data.Add(new RewardInfo()
				{
					Data = item,
					Group = "group-5",
					GroupText = string.Format("{0}% ", Group5Probability) + "UI.RandomBox.Probability.MiddleCategory.GroupReward5".GetText([1]),
					Min = (short)Math.Floor(min * 0.01),
					Max = (short)Math.Ceiling(max * 0.01),
					Probability = Math.Min(100d, min) / Group5ItemTotalCount,
					ProbabilityType = 100,
				});
			});
		}

		if (RareItemTotalCount > 0)
		{
			RareItem.Values().ForEach((item, idx) =>
			{
				data.Add(new RewardInfo()
				{
					Data = item,
					Group = "rare",
					GroupText = "UI.RandomBox.Probability.MiddleCategory.RareReward".GetText([1]),
					Min = RareItemMin[idx],
					Max = RareItemMax[idx],
					Probability = RareItemProbWeight[idx],
					ProbabilityType = RareItemProbWeightType,
				});
			});
		}

		if (SelectedItemAssuredCount > 0)
		{
			SelectedItem.Values().ForEach((item, idx) =>
			{
				data.Add(new RewardInfo()
				{
					Data = item,
					Group = "selected",
					GroupText = "UI.RandomBox.Probability.MiddleCategory.SelectedReward".GetText([SelectedItemAssuredCount]),
					Min = SelectedItemCount[idx],
					Max = SelectedItemCount[idx],
				});
			});
		}
		#endregion

		#region SmartDrop
		SmartFixedReward.Values().ForEach(reward =>
		{
			data.Add(new RewardInfo()
			{
				Data = reward,
				Group = "smart-fixed-reward",
				GroupText = "UI.RandomBox.Probability.MiddleCategory.SmartDropFixedReward".GetText(),
			});
		});

		if (SmartGroup12Probability > 0)
		{
			var probability = (double)SmartGroup12Probability / SmartGroup12TotalProbWeight;

			SmartGroup1Reward.Values().ForEach(reward =>
			{
				data.Add(new RewardInfo()
				{
					Data = reward,
					Group = "smart-group-1-reward",
					GroupText = string.Format("{0}% ", SmartGroup1ProbWeight * probability) + "UI.RandomBox.Probability.MiddleCategory.SmartDropGroupReward1".GetText([SmartGroup1AssuredCount]),
				});
			});

			SmartGroup2Reward.Values().ForEach(reward =>
			{
				data.Add(new RewardInfo()
				{
					Data = reward,
					Group = "smart-group-2-reward",
					GroupText = string.Format("{0}% ", SmartGroup2ProbWeight * probability) + "UI.RandomBox.Probability.MiddleCategory.SmartDropGroupReward2".GetText([SmartGroup2AssuredCount]),
				});
			});
		}

		if (SmartGroup3RewardTotalCount > 0)
		{
			SmartGroup3Reward.Values().ForEach((reward, idx) =>
			{
				data.Add(new RewardInfo()
				{
					Data = reward,
					Group = "smart-group-3-reward",
					GroupText = string.Format("{0}% ", SmartGroup3Probability) + "UI.RandomBox.Probability.MiddleCategory.SmartDropGroupReward3".GetText([1]),
					Probability = SmartGroup3RewardProbWeight[idx],
					ProbabilityType = SmartGroup3RewardTotalProbWeight,
				});
			});
		}

		if (SmartGroup4RewardTotalCount > 0)
		{
			SmartGroup4Reward.Values().ForEach((reward, idx) =>
			{
				data.Add(new RewardInfo()
				{
					Data = reward,
					Group = "smart-group-4-reward",
					GroupText = string.Format("{0}% ", SmartGroup4Probability) + "UI.RandomBox.Probability.MiddleCategory.SmartDropGroupReward4".GetText([1]),
					Probability = 1,
					ProbabilityType = SmartGroup4RewardTotalCount,
				});
			});
		}

		if (SmartGroup5RewardTotalCount > 0)
		{
			SmartGroup5Reward.Values().ForEach((reward, idx) =>
			{
				data.Add(new RewardInfo()
				{
					Data = reward,
					Group = "smart-group-5-reward",
					GroupText = string.Format("{0}% ", SmartGroup5Probability) + "UI.RandomBox.Probability.MiddleCategory.SmartDropGroupReward5".GetText([1]),
					Probability = 1,
					ProbabilityType = SmartGroup5RewardTotalCount,
				});
			});
		}

		if (SmartRareRewardTotalCount > 0)
		{
			SmartRareReward.Values().ForEach((reward, idx) =>
			{
				data.Add(new RewardInfo()
				{
					Data = reward,
					Group = "smart-rare-reward",
					GroupText = "UI.RandomBox.Probability.MiddleCategory.SmartDropRareReward".GetText([1]),
					Probability = SmartRareRewardProbWeight[idx],
					ProbabilityType = SmartRareRewardProbWeightType,
				});
			});
		}
		#endregion

		return data;
	}

	public class RewardInfo : IRewardHelper
	{
		public Item Data;
		public short Min = 1;
		public short Max = 1;
		internal double Probability;
		internal int ProbabilityType;

		object IRewardHelper.Data => Data;
		public string Group { get; set; }
		public string GroupText { get; set; }
		public string Text => (Min == Max ? "UI.RandomBox.Probability.MiddleCategory.Element.Fixed" : "UI.RandomBox.Probability.MiddleCategory.Element.Range").GetText([Data, Min, Max]);
		public string ProbabilityInfo => Probability == 0 ? string.Empty : (Probability / ProbabilityType).ToString("P" + ProbabilityType.GetPercentLength());
	}
	#endregion
}