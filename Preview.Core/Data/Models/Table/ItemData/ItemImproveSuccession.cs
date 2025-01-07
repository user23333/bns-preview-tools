using System.Diagnostics;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Data.Models;
public sealed class ItemImproveSuccession : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public int SeedImproveId { get; set; }

	public sbyte SeedImproveLevel { get; set; }

	public int ResultImproveId { get; set; }

	public sbyte ResultImproveLevel { get; set; }

	public int FeedMainImproveId { get; set; }

	public sbyte FeedMainImproveLevel { get; set; }

	public Ref<Item> FeedMainIngredient { get; set; }

	public short FeedMainIngredientCount { get; set; }

	public Ref<Item>[] FeedSubIngredient { get; set; }

	public short[] FeedSubIngredientCount { get; set; }

	public int CostMoney { get; set; }

	public sbyte[] FeedSuccessionOptionStep { get; set; }

	public sbyte[] ResultSuccessionOptionStep { get; set; }

	public bool KeepMainIngredientSpirit { get; set; }
	#endregion

	#region Methods
	internal static IEnumerable<ItemImproveSuccession> FindBySeed(IDataProvider provider, Item seed)
	{
		return provider.GetTable<ItemImproveSuccession>().Where(record => 
			seed.ImproveId == record.SeedImproveId && seed.ImproveLevel == record.SeedImproveLevel);
	}

	internal static ItemImproveSuccession FindByFeed(IDataProvider provider, Item feed, Item seed = null)
	{
		return provider.GetTable<ItemImproveSuccession>().FirstOrDefault(record =>
			feed.ImproveId == record.FeedMainImproveId && feed.ImproveLevel == record.FeedMainImproveLevel &&
		   (seed is null || seed.ImproveId == record.ResultImproveId));
	}

	internal IEnumerable<RecipeHelper> CreateRecipe(Item seed, out Item result)
	{
		// This method is missing the seed, the result is inaccurate
		if (seed is null)
		{
			result = Provider.GetTable<Item>().FirstOrDefault(item => item.ImproveId == ResultImproveId && item.ImproveLevel == ResultImproveLevel);
			seed = Provider.GetTable<Item>().FirstOrDefault(item => item.ImproveId == SeedImproveId && item.ImproveLevel == SeedImproveLevel);
		}
		else
		{
			Debug.Assert(seed.ImproveId == SeedImproveId);
			Debug.Assert(seed.ImproveLevel == SeedImproveLevel);
			result = ItemImprove.GetResultItem(seed, ResultImproveLevel);
		}

		// NOTE:
		// If pass in is the feed-item , use the seed-item as the MainItem
		// Otherwise , use the feed-item as the MainItem 
		var FeedMainIngredient = Attributes.Get<Item>("feed-main-ingredient");
		var FeedMainIngredientCount = Attributes.Get<short>("feed-main-ingredient-count");
		var FeedSubIngredient = Attributes.Get<Item[]>($"feed-sub-ingredient");
		var FeedSubIngredientCount = Attributes.Get<short[]>($"feed-sub-ingredient-count");
		var CostMoney = Attributes.Get<int>("cost-money");

		var recipe = new RecipeHelper
		{
			MainItem = seed,
			MainItemCount = 1,
			SubItem = [FeedMainIngredient, .. FeedSubIngredient],
			SubItemCount = [FeedMainIngredientCount, .. FeedSubIngredientCount],
			Money = CostMoney,
			SuccessProbability = 1000,

			Guide = "UI.ItemGrowth2.ImproveSuccession.Warning.seed".GetText(),
		};

		return [recipe];
	}
	#endregion
}