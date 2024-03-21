using Xylia.Preview.Common.Attributes;

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

	public short Unk2504 { get; set; }

	public short[] RareItemMin { get; set; }

	public short[] RareItemMax { get; set; }

	public Ref<Item>[] SelectedItem { get; set; }

	public short[] SelectedItemCount { get; set; }

	public sbyte SelectedItemAssuredCount { get; set; }

	public Ref<Item>[] RandomItem { get; set; }

	#endregion

	#region Methods
	protected internal override void LoadHiddenField()
	{
		if (Attributes["rare-item-1"] != null && Attributes["rare-item-1-max"] is null)
		{
			this.Attributes["rare-item-1-max"] = 1;
			this.Attributes["rare-item-1-min"] = 1;

			for (int i = 1; i <= 40; i++)
			{
				var item = this.Attributes["rare-item-" + i];
				if (item != null) this.Attributes["rare-item-prob-weight-" + i] = 50;
			}
		}

		if (Attributes["group-3-item-1"] != null && Attributes["group-3-probability"] is null)
		{
			Attributes["group-3-probability"] = 1000;

			for (int i = 1; i <= 35; i++)
			{
				if (Attributes["group-3-item-" + i] != null)
				{
					Attributes["group-3-item-prob-weight-" + i] = 3;
				}
			}
		}

		if (Attributes["group-1-2-probability"] is null)
		{
			bool UseGroup1 = false;
			bool UseGroup2 = false;

			if (Attributes["group-2-item-1"] != null)
			{
				Attributes["group-2-assured-count"] = 1;
				UseGroup2 = true;
			}

			if (Attributes["group-1-item-1"] != null)
			{
				Attributes["group-1-assured-count"] = 1;
				UseGroup1 = true;
			}

			if (UseGroup1 || UseGroup2)
			{
				Attributes["group-1-2-probability"] = 100;

				if (UseGroup1) Attributes["group-1-prob-weight"] = UseGroup1 && UseGroup2 ? 50 : 100;
				if (UseGroup2) Attributes["group-2-prob-weight"] = UseGroup1 && UseGroup2 ? 50 : 100;
			}
		}
	}

	public void TestMethods()
	{
		for (int i = 0; i < FixedItem.Length; i++)
		{
			var item = FixedItem[i].Instance;
			var min = FixedItemMin[i];
			var max = FixedItemMax[i];
		}


		if (Group12Probability > 0)
		{
			Console.WriteLine(" == Group 1-2 == " + Group12Probability + "%");
			Console.WriteLine(" == Group 1 == " + Divide(Group1ProbWeight, Group12TotalProbWeight) + $"	count: {Group1AssuredCount}  total:{Group1ItemTotalCount}");
			for (int i = 0; i < Group1Item.Length; i++)
			{
				var item = Group1Item[i].Instance;
				if (item != null) Console.WriteLine($"{item.ItemNameOnly}");
			}


			Console.WriteLine(" == Group 2 == " + Divide(Group2ProbWeight, Group12TotalProbWeight) + $"	count: {Group2AssuredCount}  total:{Group2ItemTotalCount}");
			for (int i = 0; i < Group2Item.Length; i++)
			{
				var item = Group2Item[i].Instance;
				if (item != null) Console.WriteLine($"{item.ItemNameOnly}");
			}
		}

		if (Group3Probability > 0)
		{
			Console.WriteLine(" == Group3 == " + Group3Probability + "%");
			for (int i = 0; i < Group3Item.Length; i++)
			{
				var item = Group3Item[i].Instance;
				var weight = Group3ItemProbWeight[i];

				if (item != null) Console.WriteLine($"{item.ItemNameOnly} {(double)weight / Group3TotalProbWeight:P5}");
			}
		}

		if (Group4Probability > 0)
		{
			Console.WriteLine(" == Group4 == " + Group4Probability + "%");
			for (int i = 0; i < Group4Item.Length; i++)
			{
				var item = Group4Item[i].Instance;
				var min = Group4ItemMin[i];
				var max = Group4ItemMax[i];

				if (item != null) Console.WriteLine($"{item.ItemNameOnly}");
			}
		}

		var RareItemTotalProbWeight = RareItemProbWeight.Sum();
		if (RareItemTotalProbWeight > 0)
		{
			Console.WriteLine(" == Rare == " + Divide(RareItemTotalProbWeight, RareItemProbWeightType));
			for (int i = 0; i < RareItem.Length; i++)
			{
				var item = RareItem[i].Instance;
				var weight = RareItemProbWeight[i];
				var min = RareItemMin[i];
				var max = RareItemMax[i];

				if (item != null) Console.WriteLine($"{item.ItemNameOnly} {min}-{max} {Divide(weight, RareItemProbWeightType, 5)}");
			}
		}
	}

	private static string Divide(int x, int y, int length = 2)
	{
		return ((double)x / y).ToString("P" + length);
	}
	#endregion
}