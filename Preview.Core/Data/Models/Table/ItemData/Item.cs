using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Data.Models.Sequence;
using static Xylia.Preview.Data.Models.Item;
using static Xylia.Preview.Data.Models.Item.Grocery;

namespace Xylia.Preview.Data.Models;
public abstract class Item : ModelElement, IHaveName
{
	#region Attributes
	public Ref<ItemCombat>[] ItemCombat { get; set; }
	public Ref<ItemBrand> Brand { get; set; }

	public GameCategory3Seq GameCategory3 => Attributes.Get<string>("game-category-3").ToEnum<GameCategory3Seq>();

	public bool CannotDispose => Attributes.Get<BnsBoolean>("cannot-dispose");
	public bool CannotSell => Attributes.Get<BnsBoolean>("cannot-sell");
	public bool CannotTrade => Attributes.Get<BnsBoolean>("cannot-trade");
	public bool CannotDepot => Attributes.Get<BnsBoolean>("cannot-depot");
	public bool ConsumeDurability => Attributes.Get<BnsBoolean>("consume-durability");
	public bool Auctionable => Attributes.Get<BnsBoolean>("auctionable");
	public bool WorldBossAuctionable => Attributes.Get<BnsBoolean>("world-boss-auctionable");
	public bool SealRenewalAuctionable => Attributes.Get<BnsBoolean>("seal-renewal-auctionable");
	public bool PartyAuctionExclusion => Attributes.Get<BnsBoolean>("party-auction-exclusion");
	public bool AcquireUsed => Attributes.Get<BnsBoolean>("acquire-used");
	public bool EquipUsed => Attributes.Get<BnsBoolean>("equip-used");
	public bool AccountUsed => Attributes.Get<BnsBoolean>("account-used");


	public JobSeq[] EquipJobCheck { get; set; }
	public SexSeq2 EquipSex => Attributes["equip-sex"].ToEnum<SexSeq2>();
	public enum SexSeq2
	{
		SexNone,
		All,
		Male,
		Female,
	}

	public Race EquipRace => Race.Get(Attributes["equip-race"].ToEnum<RaceSeq2>());
	public enum RaceSeq2
	{
		RaceNone,
		All,
		Jin,
		Gon,
		Lyn,
		Kun,
		SummonedAll,
		SummonedCat,
	}

	public EquipType EquipType => Attributes["equip-type"].ToEnum<EquipType>();

	public sbyte ItemGrade => Attributes.Get<sbyte>("item-grade");

	public LegendGradeBackgroundParticleTypeSeq LegendGradeBackgroundParticleType => Attributes["legend-grade-background-particle-type"].ToEnum<LegendGradeBackgroundParticleTypeSeq>();
	public enum LegendGradeBackgroundParticleTypeSeq
	{
		None,
		TypeGold,
		TypeRedup,
		TypeGoldup,
		COUNT
	}

	public ItemDecomposeInfo DecomposeInfo => new(this);

	public Ref<SetItem> SetItem { get; set; }


	public int RandomOptionGroupId => Attributes.Get<int>("random-option-group-id");

	public int ImproveId => Attributes.Get<int>("improve-id");
	public sbyte ImproveLevel => Attributes.Get<sbyte>("improve-level");

	public string ItemName => $"<link id='item:{ToString()}'>{ItemNameOnly}</link>";
	public string ItemNameOnly
	{
		get
		{
			var TagIconGrade = Attributes.Get<Icon>("tag-icon-grade")?.GetImage();
			if (TagIconGrade != null)
			{
				TagIconGrade.TintColor = new TintColor()
				{
					SpecifiedColor = ItemGrade switch
					{
						1 => new FLinearColor(0.325037f, 0.325037f, 0.325037f, 1f),
						2 => new FLinearColor(1f, 1f, 1f, 1f),
						3 => new FLinearColor(0.096266f, 1f, 0.186989f, 1f),
						4 => new FLinearColor(0f, 0.694081f, 1f, 1f),
						5 => new FLinearColor(0.68703f, 0.037029f, 1f, 1f),
						6 => new FLinearColor(0.88318f, 0.442323f, 0.03423f, 1f),
						7 => new FLinearColor(1f, 0.186989f, 0.000805f, 1f),
						8 => new FLinearColor(1f, 0.000000f, 0.234895f, 1f),
						9 => new FLinearColor(1f, 0.846873f, 0.016807f, 1f),
						_ => default,
					}
				};
			}

			var text = Attributes["name2"].GetText() ?? ToString();
			return $"<font name='00008130.Program.Fontset_ItemGrade_{ItemGrade}'>{text}</font>" + TagIconGrade?.Tag;
		}
	}

	public int ClosetGroupId => Attributes.Get<int>("closet-group-id");
	#endregion

	#region Sub
	public sealed class Weapon : Item
	{
		public WeaponTypeSeq WeaponType => Attributes["weapon-type"].ToEnum<WeaponTypeSeq>();
		public enum WeaponTypeSeq
		{
			None,
			BareHand,
			Sword,
			Gauntlet,
			AuraBangle,
			Pistol,
			Rifle,
			TwoHandedAxe,
			Bow,
			Staff,
			Dagger,
			Pet1,
			Pet2,
			Gun,
			GreatSword,
			LongBow,
			Spear,
			Orb,
			DualBlade,
			COUNT
		}
	}

	public sealed class Costume : Item
	{

	}

	public sealed class Grocery : Item
	{
		public GroceryTypeSeq GroceryType => Attributes["grocery-type"].ToEnum<GroceryTypeSeq>();
		public enum GroceryTypeSeq
		{
			Other,
			Repair,
			Seal,
			RandomBox,
			CaveEscape,
			Key,
			WeaponGemSlotExpander,
			Sealed,
			WeaponGemSlotAdder,
			Messenger,
			QuestReplayEpic,
			BaseCampWarp,
			PetFood,
			ResetDungeon,
			SkillBook,
			FishingPaste,
			Badge,
			Scroll,
			FusionSubitem,
			Card,
			Glyph,
			SoulBoost,

			COUNT
		}
	}

	public sealed class Gem : Item
	{
		public short WeaponGemLevel { get; set; }

		public bool CannotTransform { get; set; }


		public WeaponEnchantGemSlotTypeSeq WeaponEnchantGemSlotType { get; set; }
		public enum WeaponEnchantGemSlotTypeSeq
		{
			None,
			First,
			Second,
			COUNT
		}

		public AccessoryEnchantGemEquipAccessoryTypeSeq AccessoryEnchantGemEquipAccessoryType { get; set; }
		public enum AccessoryEnchantGemEquipAccessoryTypeSeq
		{
			None,
			Ring,
			Earring,
			Necklace,
			Belt,
			Bracelet,
			Gloves,
			COUNT
		}
	}

	public sealed class Accessory : Item
	{
		public AccessoryTypeSeq AccessoryType => Attributes["accessory-type"].ToEnum<AccessoryTypeSeq>();
		public enum AccessoryTypeSeq
		{
			Accessory,
			CostumeAttach,
			Ring,
			Earring,
			Necklace,
			Belt,
			Bracelet,
			Soul,
			Soul2,
			Gloves,
			Rune1,
			Rune2,
			Nova,
			Vehicle,
			AppearanceNormalState,
			AppearanceIdleState,
			AppearanceChatting,
			AppearancePortrait,
			AppearanceHypermove,
			AppearanceNamePlate,
			AppearanceSpeechBubble,

			COUNT
		}
	}

	public sealed class Enchant : Item
	{

	}
	#endregion


	#region Methods
	public string Name => Attributes["name2"].GetText() ?? ToString();

	public ImageProperty BackIcon => IconTexture.GetBackground(ItemGrade);

	public ImageProperty FrontIcon => Attributes.Get<Icon>("icon")?.GetImage();

	public FPackageIndex CanSaleItemImage => new MyFPackageIndex(
		Auctionable ? "BNSR/Content/Art/UI/GameUI_BNSR/Resource/GameUI_Icon3_R/SlotItem_marketBusiness.SlotItem_marketBusiness" :
		AccountUsed ? "BNSR/Content/Art/UI/GameUI_BNSR/Resource/GameUI_Icon3_R/SlotItem_privateSale.SlotItem_privateSale" : null);

	public FPackageIndex UnusableImage => DecomposeInfo.GetImage();

	public string AcquireRoute
	{
		get
		{
			// the original method is a little stupid
			// I want to retrieve the desc6 text

			// Item.DescTitle.0001
			return this.Attributes["description6"]?.GetText();
		}
	}

	public bool IsExpiration
	{
		get
		{
			var time = Attributes.Get<Record>("event-info")?.Attributes.Get<Time64>("event-expiration-time");
			if (time is null) return false;

			return time.Value < DateTimeOffset.Now.ToUnixTimeSeconds();
		}
	}
	#endregion
}

public class ItemDecomposeInfo
{
	#region Fields
	public bool DecomposeRewardByConsumeIndex;
	public int DecomposeMax = 1;
	public int DecomposeMoneyCost;

	public Reward[] DecomposeReward;
	public Reward DecomposeEventReward;
	public Dictionary<JobSeq, Reward> DecomposeJobRewards = [];

	public Tuple<Item, short>[] Decompose_By_Item2;
	public Tuple<Item, short>[] Job_Decompose_By_Item2;
	#endregion

	#region Constructor
	internal ItemDecomposeInfo(Item data)
	{
		var attributes = data.Attributes;

		DecomposeMax = attributes.Get<sbyte>("decompose-max");
		DecomposeMoneyCost = attributes.Get<int>("decompose-money-cost");
		DecomposeRewardByConsumeIndex = attributes.Get<BnsBoolean>("decompose-reward-by-consume-index");

		LinqExtensions.For(ref DecomposeReward, 7, (id) => attributes.Get<Record>("decompose-reward-" + id)?.As<Reward>());
		Job.GetPcJob().ForEach(job => DecomposeJobRewards[job] = attributes.Get<Record>("decompose-job-reward-" + job.GetDescription())?.As<Reward>());

		LinqExtensions.For(ref Decompose_By_Item2, 7, (id) => new(attributes.Get<Record>("decompose-by-item2-" + id)?.As<Item>(), attributes.Get<short>("decompose-by-item2-stack-count-" + id)));
		LinqExtensions.For(ref Job_Decompose_By_Item2, 7, (id) => new(attributes.Get<Record>("job-decompose-by-item2-" + id)?.As<Item>(), attributes.Get<short>("job-decompose-by-item2-stack-count-" + id)));
	}
	#endregion


	#region Methods
	public FPackageIndex GetImage()
	{
		var image = GetImage(this.Decompose_By_Item2[0].Item1);
		image ??= GetImage(this.Job_Decompose_By_Item2[0].Item1);
		image ??= this.DecomposeMoneyCost == 0 ? null : new MyFPackageIndex("BNSR/Content/Art/UI/GameUI/Resource/GameUI_Icon/Weapon_Lock_04.Weapon_Lock_04");

		return image;
	}

	private static FPackageIndex GetImage(Item item2)
	{
		if (item2 is null) return null;
		else if (item2 is Grocery grocery && grocery.GroceryType == GroceryTypeSeq.Key) return new MyFPackageIndex("BNSR/Content/Art/UI/GameUI/Resource/GameUI_Icon/unuseable_KeyLock.unuseable_KeyLock");
		else return new MyFPackageIndex("BNSR/Content/Art/UI/GameUI/Resource/GameUI_Icon/Weapon_Lock_04.Weapon_Lock_04");
	}
	#endregion
}