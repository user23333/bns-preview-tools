using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public sealed class ItemImprove : ModelElement, IItemRecipeHelper
{
	#region Attributes
	public int Id { get; set; }

	public sbyte Level { get; set; }

	public int SuccessOptionListId { get; set; }

	public sbyte SuccessOptionSlotNumber { get; set; }
	#endregion

	#region Methods
	public string ImproveOptionAcquireLevel => SuccessOptionListId > 0 ? "UI.EquipmentGuide.ItemInfo.ImproveOptionAcquireLevel.Desc".GetText([Level]) : null;


	internal static Item GetResultItem(Item item, sbyte improvelevel)
	{
		ArgumentNullException.ThrowIfNull(item);

		if (item.ImproveLevel == improvelevel) return item;
		else if (item.ImproveLevel > improvelevel)
		{
			item = item.Attributes.Get<Item>("improve-prev-item");
			return GetResultItem(item, improvelevel);
		}
		else if (item.ImproveLevel < improvelevel)
		{
			item = item.Attributes.Get<Item>("improve-next-item");
			return GetResultItem(item, improvelevel);
		}
		else throw new NotSupportedException();
	}

	public IEnumerable<ItemRecipeHelper> GetRecipes()
	{
		var recipes = new List<ItemRecipeHelper>();

		for (sbyte i = 1; i <= 5; i++)
		{
			var CostMoney = Attributes.Get<int>("cost-money-" + i);
			var CostMainItem = Attributes.Get<Item>("cost-main-item-" + i);
			if (CostMainItem is null) continue;

			var CostMainItemCount = Attributes.Get<short>("cost-main-item-count-" + i);
			var CostSubItem = LinqExtensions.For(8, (id) => Attributes.Get<Item>($"cost-sub-item-{i}-{id}"));
			var CostSubItemCount = LinqExtensions.For(8, (id) => Attributes.Get<short>($"cost-sub-item-count-{i}-{id}"));
			var SuccessProbability = Attributes.Get<short>("success-probability-" + i);
			var UseSuccessProbability = Attributes.Get<bool>("use-success-probability-" + i);
			var FailLevelDiff = Attributes.Get<sbyte>("fail-level-diff-" + i);

			string Warning = !UseSuccessProbability ? null :
				FailLevelDiff > 0 ? "UI.ItemGrowth.Warning.Improvement.FailProbabilityWithLevelDown" :
				"UI.ItemGrowth.Warning.Improvement.FailProbabilityWithoutLevelDown";

			recipes.Add(new ItemRecipeHelper()
			{
				MainItem = CostMainItem,
				MainItemCount = CostMainItemCount,
				SubItem = CostSubItem,
				SubItemCount = CostSubItemCount,
				Money = CostMoney,
				SuccessProbability = UseSuccessProbability ? SuccessProbability : (short)1000,

				Guide = Warning.GetText(),
			});
		}

		return recipes;
	}
	#endregion
}