using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class ItemGraph : ModelElement
{
	public sealed class Seed : ItemGraph
	{
		#region Attributes
		public Ref<Item>[] SeedItem { get; set; }
		public Ref<ItemGraphSeedGroup> SeedItemGroup { get; set; }
		public SeedItemSubGroupSeq[] SeedItemSubGroup { get; set; }
		public NodeTypeSeq NodeType { get; set; }
		public enum NodeTypeSeq
		{
			SeedNormal,
			SeedBlackSky,
			COUNT
		}
		public AttributeGroupSeq AttributeGroup { get; set; }
		public EquipType ItemEquipType { get; set; }
		public GrowthCategorySeq GrowthCategory { get; set; }
		public enum GrowthCategorySeq
		{
			None,
			Dungeon,
			Raid,
			Pvp,
			Attribute,
			Etc1,
			Etc2,
			COUNT
		}
		public short Row { get; set; }
		public short Column { get; set; }
		#endregion

		public void CreateEdgeHelper(GameDataTable<ItemGraph> table)
		{
			if (Attributes.Get<BnsBoolean>("use-improve"))
			{
				var item = this.SeedItem.FirstOrDefault().Value;
				if (item is null) return;

				var improve = this.Provider.GetTable<ItemImprove>().FirstOrDefault(x => x.Id == item.ImproveId && x.Level == item.ImproveLevel);
				if (improve != null)
				{
					var NextItem = item.Attributes.Get<Record>("improve-next-item");
					foreach (var recipe in improve.GetRecipes())
					{
						table.Elements.Add(new Edge()
						{
							StartItem = item,
							EndItem = NextItem,
							SuccessProbability = recipe.SuccessProbability == 1000 ? SuccessProbabilitySeq.Definite : SuccessProbabilitySeq.Stochastic,

							Recipe = recipe
						});
					}
				}

				//var succession = ItemImproveSuccession.FindByFeed(Provider, item, ImproveSuccessionSeed);
				//if (succession != null)
				//{
				//	foreach (var recipe in succession.CreateRecipe(ImproveSuccessionSeed, out var NextItem))
				//	{
				//		table.Elements.Add(new Edge()
				//		{
				//			StartItem = item,
				//			EndItem = NextItem,
				//			SuccessProbability = SuccessProbabilitySeq.Definite,
				//			StartOrientation = OrientationSeq.Horizontal,
				//			EndOrientation = OrientationSeq.Horizontal,

				//			Recipe = recipe
				//		});
				//	}
				//}
			}
		}
	}

	public sealed class Edge : ItemGraph
	{
		#region Attributes
		public EdgeTypeSeq EdgeType { get; set; }
		public AttributeGroupSeq AttributeGroup { get; set; }
		public SeedItemSubGroupSeq SeedItemSubGroup { get; set; }
		public Ref<Item> FeedItem { get; set; }
		public Ref<ItemTransformRecipe> FeedRecipe { get; set; }
		public Ref<Item> StartItem { get; set; }
		public OrientationSeq StartOrientation { get; set; } = OrientationSeq.Vertical;
		public Ref<Item> EndItem { get; set; }
		public OrientationSeq EndOrientation { get; set; } = OrientationSeq.Vertical;
		public SuccessProbabilitySeq SuccessProbability { get; set; }
		public bool HasArrow { get; set; }
		#endregion

		#region Helper
		public string Title => $"{StartItem.Value?.ItemNameOnly} ➠ {EndItem.Value?.ItemNameOnly}";

		public RecipeHelper Recipe { get; internal set; }

		public void CreateRecipeHelper()
		{
			Recipe = FeedRecipe.Value?.GetRecipe();
		}
		#endregion
	}

	#region Sequence
	public enum SeedItemSubGroupSeq
	{
		SubGroup1,
		SubGroup2,
	}

	public enum AttributeGroupSeq
	{
		None,
		AttributeGroup1,
		AttributeGroup2,
	}

	public enum EdgeTypeSeq
	{
		Growth,

		Awaken,

		Transform,

		JumpTransform,

		Purification,
	}

	public enum OrientationSeq
	{
		Horizontal,
		Vertical,
	}

	public enum SuccessProbabilitySeq
	{
		Definite,
		Stochastic,
	}
	#endregion
}