using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models.Sequence;
using static Xylia.Preview.Data.Models.Item;
using static Xylia.Preview.Data.Models.Item.Accessory;
using static Xylia.Preview.Data.Models.Item.Weapon;

namespace Xylia.Preview.Data.Models;
public sealed class ItemTransformRecipe : ModelElement
{
	#region Attributes
	public Ref<ItemTransformUpgradeItem> UpgradeGrocery { get; set; }

	public sbyte RequiredInvenCapacity { get; set; }

	public int MoneyCost { get; set; }

	public Ref<ModelElement> MainIngredient { get; set; }

	public ItemConditionType MainIngredientConditionType { get; set; }

	public sbyte MainIngredientMinLevel { get; set; }

	public short MainIngredientStackCount { get; set; }

	public Ref<Text> MainIngredientTitleName { get; set; }

	public Ref<Item> MainIngredientTitleItem { get; set; }

	public bool KeepMainIngredientWeaponGemSlot { get; set; }

	public bool KeepMainIngredientWeaponAppearance { get; set; }

	public bool KeepMainIngredientSpirit { get; set; }

	public bool ConsumeMainIngredient { get; set; }

	public Ref<ModelElement>[] SubIngredient { get; set; }

	public ItemConditionType[] SubIngredientConditionType { get; set; }

	public sbyte[] SubIngredientMinLevel { get; set; }

	public short[] SubIngredientStackCount { get; set; }

	public Ref<Text>[] SubIngredientTitleName { get; set; }

	public Ref<Item>[] SubIngredientTitleItem { get; set; }

	public bool ConsumeSubIngredient { get; set; }

	public Ref<Item>[] FixedIngredient { get; set; }

	public short[] FixedIngredientStackCount { get; set; }

	public bool ConsumeFixedIngredient { get; set; }

	public sbyte SpecialFixedIndex { get; set; }

	public bool EnableBatchTransform { get; set; }

	public bool IsFixedResultRecipe { get; set; }

	public short RareItemSuccessProbability { get; set; }

	public Ref<ModelElement>[] RareItem { get; set; }

	public sbyte RareItemTotalCount { get; set; }

	public sbyte[] RareItemSelectCount { get; set; }

	public sbyte[] RareItemStackCount { get; set; }

	public short NormalItemSuccessProbability { get; set; }

	public Ref<ModelElement>[] NormalItem { get; set; }

	public sbyte NormalItemTotalCount { get; set; }

	public sbyte[] NormalItemSelectCount { get; set; }

	public sbyte[] NormalItemStackCount { get; set; }

	public Ref<ModelElement>[] PremiumItem { get; set; }

	public sbyte PremiumItemTotalCount { get; set; }

	public sbyte[] PremiumItemSelectCount { get; set; }

	public sbyte[] PremiumItemStackCount { get; set; }

	public short RandomItemSuccessProbability { get; set; }

	public Ref<ModelElement>[] RandomItem { get; set; }

	public sbyte RandomItemTotalCount { get; set; }

	public short[] RandomItemSelectPropWeight { get; set; }

	public bool RandomFailureMileageSave { get; set; }

	public Ref<ItemTransformRecipe>[] RandomFailureMileageInfluenceRecipe { get; set; }

	public Ref<ItemTransformRetryCost> RandomRetryCost { get; set; }

	//public WeaponGemType MainIngredientWeaponGemType { get; set; }

	public short MainIngredientWeaponGemLevel { get; set; }

	public sbyte MainIngredientWeaponGemGrade { get; set; }

	//public WeaponGemType[] SubIngredientWeaponGemType { get; set; }

	public short[] SubIngredientWeaponGemLevel { get; set; }

	public sbyte[] SubIngredientWeaponGemGrade { get; set; }

	public Ref<Item> TitleItem { get; set; }

	public Ref<Text> TitleName { get; set; }

	public Ref<RandomboxPreview> TitleReward { get; set; }

	public bool UseRandom { get; set; }

	public WarningSeq Warning { get; set; }
	#endregion

	#region Sequence
	public enum WarningSeq
	{
		None,

		[Text("Transform.Warning.fail")]
		Fail,

		[Text("Transform.Warning.stuck")]
		Stuck,

		[Text("Transform.Warning.gemslotreset")]
		Gemslotreset,

		[Text("Transform.Warning.fail-gemslotreset")]
		FailGemslotreset,

		[Text("Transform.Warning.stuck-gemslotreset")]
		StuckGemslotreset,

		[Text("Transform.Warning.change")]
		Change,

		[Text("Transform.Warning.lower")]
		Lower,

		[Text("Transform.Warning.lower-gemslotreset")]
		LowerGemslotreset,

		[Text("Transform.Warning.partialfail")]
		Partialfail,

		[Text("Transform.Warning.tradeimpossible")]
		Tradeimpossible,

		[Text("UI.Sewing.Warning.DeleteParticle")]
		DeleteParticle,

		[Text("UI.Sewing.Warning.DeleteDesign")]
		DeleteDesign,

		[Text("Transform.Warning.spiritreset")]
		Spiritreset,

		[Text("Transform.Warning.fail-spiritreset")]
		FailSpiritreset,

		[Text("Transform.Warning.gemslotreset-spiritreset")]
		GemslotresetSpiritreset,

		[Text("Transform.Warning.fail-gemslotreset-spiritreset")]
		FailGemslotresetSpiritreset,

		[Text("Transform.Warning.lower-spiritreset")]
		LowerSpiritreset,

		[Text("Transform.Warning.lower-gemslotreset-spiritreset")]
		LowerGemslotresetSpiritreset,

		[Text("Transform.Warning.partialfail-spiritreset")]
		PartialfailSpiritreset,

		[Text("Transform.Warning.cannot-division")]
		CannotDivision,

		[Text("Transform.Warning.fail-cannot-division")]
		FailCannotDivision,

		COUNT
	}
	#endregion

	#region Methods
	public RecipeHelper GetRecipe()
	{
		var MainItem = SubIngredient.Values().FirstOrDefault() as Item;
		var MainItemCount = SubIngredientStackCount.FirstOrDefault();

		return new RecipeHelper
		{
			MainItem = MainItem,
			MainItemCount = MainItemCount,
			SubItem = FixedIngredient.Values().ToArray(),
			SubItemCount = FixedIngredientStackCount,
			Money = MoneyCost,
			Guide = Warning.GetText(),
		};
	}

	public static IEnumerable<ItemTransformRecipe> QueryRecipe(IDataProvider provider, Item Item) => provider.GetTable<ItemTransformRecipe>().Where(o =>
	{
		var MainIngredient = o.MainIngredient.Instance;
		if (MainIngredient is Item item) return item == Item;
		else if (MainIngredient is ItemBrand itembrand)
		{
			if (itembrand != Item.Brand.Instance) return false;

			var type = o.MainIngredientConditionType;
			if (type == ItemConditionType.All || type == ItemConditionType.None) return true;
			else if (Item is Item.Weapon Weapon)
			{
				var WeaponType = Weapon.WeaponType;
				if (type == ItemConditionType.Weapon && WeaponType != WeaponTypeSeq.Pet1 && WeaponType != WeaponTypeSeq.Pet2) return true;

				return type == WeaponType switch
				{
					WeaponTypeSeq.Sword => ItemConditionType.Sword,
					WeaponTypeSeq.Gauntlet => ItemConditionType.Gauntlet,
					WeaponTypeSeq.AuraBangle => ItemConditionType.AuraBangle,
					WeaponTypeSeq.TwoHandedAxe => ItemConditionType.Axe,
					WeaponTypeSeq.Staff => ItemConditionType.Staff,
					WeaponTypeSeq.Dagger => ItemConditionType.Dagger,
					WeaponTypeSeq.Pet1 => ItemConditionType.Pet1,
					WeaponTypeSeq.Pet2 => ItemConditionType.Pet2,
					WeaponTypeSeq.Gun => ItemConditionType.ShooterGun,
					WeaponTypeSeq.GreatSword => ItemConditionType.GreatSword,
					WeaponTypeSeq.LongBow => ItemConditionType.LongBow,
					WeaponTypeSeq.Spear => ItemConditionType.Spear,
					WeaponTypeSeq.Orb => ItemConditionType.Orb,

					_ => ItemConditionType.None,
				};
			}
			else if (Item is Item.Accessory Accessory)
			{
				return type == Accessory.AccessoryType switch
				{
					AccessoryTypeSeq.Ring => ItemConditionType.Ring,
					AccessoryTypeSeq.Earring => ItemConditionType.Earring,
					AccessoryTypeSeq.Necklace => ItemConditionType.Necklace,
					AccessoryTypeSeq.Belt => ItemConditionType.Belt,
					AccessoryTypeSeq.Bracelet => ItemConditionType.Bracelet,
					AccessoryTypeSeq.Soul => ItemConditionType.Soul,
					AccessoryTypeSeq.Soul2 => ItemConditionType.Soul2,
					AccessoryTypeSeq.Gloves => ItemConditionType.Gloves,
					AccessoryTypeSeq.Rune1 => ItemConditionType.Rune1,
					AccessoryTypeSeq.Rune2 => ItemConditionType.Rune2,
					AccessoryTypeSeq.Nova => ItemConditionType.Nova,

					_ => ItemConditionType.None,
				};
			}
		}

		return false;
	});
	#endregion
}