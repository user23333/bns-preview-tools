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
	public string Alias { get; set; }

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

	public sbyte RareItemSelectCount { get; set; }

	public short[] RareItemStackCount { get; set; }

	public short NormalItemSuccessProbability { get; set; }

	public Ref<ModelElement>[] NormalItem { get; set; }

	public sbyte NormalItemTotalCount { get; set; }

	public sbyte NormalItemSelectCount { get; set; }

	public short[] NormalItemStackCount { get; set; }

	public Ref<ModelElement>[] PremiumItem { get; set; }

	public sbyte PremiumItemTotalCount { get; set; }

	public sbyte PremiumItemSelectCount { get; set; }

	public short[] PremiumItemStackCount { get; set; }

	public short RandomItemSuccessProbability { get; set; }

	public Ref<ModelElement>[] RandomItem { get; set; }

	public sbyte RandomItemTotalCount { get; set; }

	public short[] RandomItemSelectPropWeight { get; set; }

	public bool RandomFailureMileageSave { get; set; }

	public Ref<RandomDistribution> RandomFailureMileageDistributionType { get; set; }

	public Ref<ItemTransformRecipe>[] RandomFailureMileageInfluenceRecipe { get; set; }

	public Ref<ItemTransformRetryCost> RandomRetryCost { get; set; }

	public WeaponGemTypeSeq MainIngredientWeaponGemType { get; set; }

	public enum WeaponGemTypeSeq
	{
		None,
		Ruby,
		Topaz,
		Sapphire,
		Jade,
		Amethyst,
		Emerald,
		Diamond,
		Obsidian,
		Amber,
		Garnet,
		Aquamarine,
		RubyTopaz,
		RubySapphire,
		RubyJade,
		RubyAmethyst,
		RubyEmerald,
		RubyDiamond,
		TopazSapphire,
		TopazJade,
		TopazAmethyst,
		TopazEmerald,
		TopazDiamond,
		SapphireJade,
		SapphireAmethyst,
		SapphireEmerald,
		SapphireDiamond,
		JadeAmethyst,
		JadeEmerald,
		JadeDiamond,
		AmethystEmerald,
		AmethystDiamond,
		EmeraldDiamond,
		AquamarineDiamond,
		AmberDiamond,
		ObsidianGarnet,
		CorundumWhite,
		CorundumBlack,
		CorundumPink,
		CorundumYellow,
		CorundumBluegreen,
		CorundumBlue,
		CorundumAquamarine,
		CorundumAmber,
		CorundumRuby,
		CorundumAmethyst,
		CorundumJade,
		AquamarineAmber,
		COUNT
	}

	public short MainIngredientWeaponGemLevel { get; set; }

	public sbyte MainIngredientWeaponGemGrade { get; set; }

	public WeaponGemTypeSeq[] SubIngredientWeaponGemType { get; set; }

	public short[] SubIngredientWeaponGemLevel { get; set; }

	public sbyte[] SubIngredientWeaponGemGrade { get; set; }

	public short WeaponGemTransformFailProbability { get; set; }

	public Ref<Item> TitleItem { get; set; }

	public Ref<Text> TitleName { get; set; }

	public Ref<RandomboxPreview> TitleReward { get; set; }

	public UpperCategorySeq UpperCategory { get; set; }

	public enum UpperCategorySeq
	{
		None,
		General,
		WeaponGem,
		PetGem,
		Event,
		COUNT
	}

	public CategorySeq Category { get; set; }

	public enum CategorySeq
	{
		None,
		Event,
		Material,
		Costume,
		Weapon,
		LegendaryWeapon,
		Accessory,
		WeaponGemAdder,
		WeaponGem2,
		Piece,
		Purification,
		Special,
		Pet,
		PetLegend,
		PetChange,
		TaijiGem,
		Division,
		WeaponEnchantGem,
		Sewing,
		WeaponTransform,
		AccessoryTransform,
		EquipGem,
		Card,
		Spirit,
		Etc,
		PetGem,
		[Name("common-1")]
		Common1,
		[Name("common-2")]
		Common2,
		[Name("common-3")]
		Common3,
		[Name("common-4")]
		Common4,
		[Name("common-5")]
		Common5,
		COUNT
	}

	public bool UseRandom { get; set; }

	public Ref<Effect> FailEffect { get; set; }

	public Ref<Quest> Quest { get; set; }

	public bool BmIngredientRecipe { get; set; }

	public WarningSeq Warning { get; set; }

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

	public sbyte EventIndex { get; set; }

	public int RecipeScore { get; set; }

	public RecipeSeriesSeq RecipeSeries { get; set; }

	public enum RecipeSeriesSeq
	{
		None,
		[Name("normal-1")]
		Normal1,
		[Name("normal-2")]
		Normal2,
		[Name("normal-3")]
		Normal3,
		[Name("normal-4")]
		Normal4,
		[Name("normal-5")]
		Normal5,
		[Name("normal-6")]
		Normal6,
		[Name("normal-7")]
		Normal7,
		[Name("normal-8")]
		Normal8,
		[Name("normal-9")]
		Normal9,
		[Name("normal-11")]
		Normal11,
		[Name("normal-12")]
		Normal12,
		[Name("normal-13")]
		Normal13,
		[Name("normal-14")]
		Normal14,
		[Name("normal-15")]
		Normal15,
		[Name("normal-16")]
		Normal16,
		[Name("normal-17")]
		Normal17,
		[Name("normal-18")]
		Normal18,
		[Name("normal-19")]
		Normal19,
		[Name("normal-20")]
		Normal20,
		[Name("bm-1")]
		Bm1,
		[Name("bm-2")]
		Bm2,
		[Name("bm-3")]
		Bm3,
		[Name("bm-4")]
		Bm4,
		[Name("bm-5")]
		Bm5,
		[Name("bm-6")]
		Bm6,
		[Name("bm-7")]
		Bm7,
		[Name("bm-8")]
		Bm8,
		[Name("bm-9")]
		Bm9,
		[Name("bm-11")]
		Bm11,
		[Name("bm-12")]
		Bm12,
		[Name("bm-13")]
		Bm13,
		[Name("bm-14")]
		Bm14,
		[Name("bm-15")]
		Bm15,
		[Name("bm-16")]
		Bm16,
		[Name("bm-17")]
		Bm17,
		[Name("bm-18")]
		Bm18,
		[Name("bm-19")]
		Bm19,
		[Name("bm-20")]
		Bm20,
		COUNT
	}

	public sbyte DefiniteDiscountMinimumValue { get; set; }
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