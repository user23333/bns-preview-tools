﻿using System.Collections;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class ItemImproveOptionList : ModelElement, IRecipeHelper
{
	#region Attributes
	public JobSeq Job { get; set; }

	public Ref<ItemImproveOption>[] Option { get; set; }

	public short[] OptionWeight { get; set; }

	public int OptionWeightTotal { get; set; }

	public int[] DrawCostMoney { get; set; }

	public Ref<Item>[] DrawCostMainItem { get; set; }

	public short[] DrawCostMainItemCount { get; set; }

	[Name("draw-cost-sub-item-1")]
	public Ref<Item>[] DrawCostSubItem1 { get; set; }

	[Name("draw-cost-sub-item-count-1")]
	public short[] DrawCostSubItemCount1 { get; set; }

	[Name("draw-cost-sub-item-2")]
	public Ref<Item>[] DrawCostSubItem2 { get; set; }

	[Name("draw-cost-sub-item-count-2")]
	public short[] DrawCostSubItemCount2 { get; set; }

	[Name("draw-cost-sub-item-3")]
	public Ref<Item>[] DrawCostSubItem3 { get; set; }

	[Name("draw-cost-sub-item-count-3")]
	public short[] DrawCostSubItemCount3 { get; set; }

	[Name("draw-cost-sub-item-4")]
	public Ref<Item>[] DrawCostSubItem4 { get; set; }

	[Name("draw-cost-sub-item-count-4")]
	public short[] DrawCostSubItemCount4 { get; set; }

	public Ref<ItemImproveOption>[] SuccessionRandomOption { get; set; }

	public short[] SuccessionRandomOptionWeight { get; set; }

	public int SuccessionRandomOptionWeightTotal { get; set; }
	#endregion

	#region Methods
	public IEnumerable GetOptions(sbyte level)
	{
		var options = new List<Tuple<ItemImproveOption, double>>();

		for (int i = 0; i < Option.Length; i++)
		{
			var option = Option[i].Value;
			if (option is null) continue;

			option = this.Provider.GetTable<ItemImproveOption>()[option.Id + ((long)level << 32)];
			options.Add(new(option, (double)OptionWeight[i] / OptionWeightTotal));
		}

		return options;
	}

	public IEnumerable<RecipeHelper> GetRecipes()
	{
		var recipes = new List<RecipeHelper>();

		for (sbyte i = 1; i <= 4; i++)
		{
			var CostMoney = Attributes.Get<int>("draw-cost-money-" + i);
			var CostMainItem = Attributes.Get<Item>("draw-cost-main-item-" + i);
			if (CostMainItem is null) continue;

			var CostMainItemCount = Attributes.Get<short>("draw-cost-main-item-count-" + i);
			var CostSubItem = Attributes.Get<Item[]>("draw-cost-sub-item-" + i);
			var CostSubItemCount = Attributes.Get<short[]>("draw-cost-sub-item-count-" + i);

			recipes.Add(new RecipeHelper()
			{
				MainItem = CostMainItem,
				MainItemCount = CostMainItemCount,
				SubItem = CostSubItem,
				SubItemCount = CostSubItemCount,
				Money = CostMoney
			});
		}

		return recipes;
	}
	#endregion
}