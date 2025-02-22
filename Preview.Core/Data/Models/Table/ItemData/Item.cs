﻿using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.Properties;
using static Xylia.Preview.Data.Models.Item;
using static Xylia.Preview.Data.Models.Item.Grocery;

namespace Xylia.Preview.Data.Models;
public abstract class Item : ModelElement, IHaveName
{
	#region Attributes
	public Ref<ItemCombat>[] ItemCombat { get; set; }
	public Ref<ItemBrand> Brand { get; set; }

	public GameCategorySeq GameCategory1 => Attributes.Get<GameCategorySeq>("game-category-1");
	public GameCategory2Seq GameCategory2 => Attributes.Get<GameCategory2Seq>("game-category-2");
	public GameCategory3Seq GameCategory3 => Attributes.Get<GameCategory3Seq>("game-category-3");
	public MarketCategorySeq MarketCategory => Attributes.Get<MarketCategorySeq>("market-category-1");
	public MarketCategory2Seq MarketCategory2 => Attributes.Get<MarketCategory2Seq>("market-category-2");
	public MarketCategory3Seq MarketCategory3 => Attributes.Get<MarketCategory3Seq>("market-category-3");

	public bool CannotDispose => Attributes.Get<bool>("cannot-dispose");
	public bool CannotSell => Attributes.Get<bool>("cannot-sell");
	public bool CannotTrade => Attributes.Get<bool>("cannot-trade");
	public bool CannotDepot => Attributes.Get<bool>("cannot-depot");
	public bool ConsumeDurability => Attributes.Get<bool>("consume-durability");
	public bool Auctionable => Attributes.Get<bool>("auctionable");
	public bool WorldBossAuctionable => Attributes.Get<bool>("world-boss-auctionable");
	public bool SealRenewalAuctionable => Attributes.Get<bool>("seal-renewal-auctionable");
	public bool PartyAuctionExclusion => Attributes.Get<bool>("party-auction-exclusion");
	public bool AcquireUsed => Attributes.Get<bool>("acquire-used");
	public bool EquipUsed => Attributes.Get<bool>("equip-used");
	public bool AccountUsed => Attributes.Get<bool>("account-used");


	public JobSeq[] EquipJobCheck { get; set; }
	public SexSeq2 EquipSex => Attributes.Get<SexSeq2>("equip-sex");
	public enum SexSeq2
	{
		SexNone,
		[Text(null)] All,
		[Text("Name.sex.male")] Male,
		[Text("Name.sex.female")] Female,
	}

	public Race EquipRace => Attributes.Get<RaceSeq2>("equip-race").To<Race>();
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

	public EquipType EquipType => Attributes.Get<EquipType>("equip-type");

	public sbyte ItemGrade => Attributes.Get<sbyte>("item-grade");

	public LegendGradeBackgroundParticleTypeSeq LegendGradeBackgroundParticleType => Attributes.Get<LegendGradeBackgroundParticleTypeSeq>("legend-grade-background-particle-type");
	public enum LegendGradeBackgroundParticleTypeSeq
	{
		None,
		TypeGold,
		TypeRedup,
		TypeGoldup,
		COUNT
	}

	public SetItem SetItem => Attributes.Get<SetItem>("set-item");
	public Faction Faction => Attributes.Get<Faction>("faction");
	public Faction EquipFaction => Attributes.Get<Faction>("equip-faction");


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

			var text = Attributes["name2"].GetText();
			if (Settings.Default.UseDebugMode) text ??= ToString();

			return $"<font name='00008130.Program.Fontset_ItemGrade_{ItemGrade}'>{text}</font>" + TagIconGrade?.Tag;
		}
	}

	public int ClosetGroupId => Attributes.Get<int>("closet-group-id");

	public sealed class Weapon : Item
	{
		public WeaponTypeSeq WeaponType => Attributes.Get<WeaponTypeSeq>("weapon-type");
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
			[Name("pet-1")] Pet1,
			[Name("pet-2")] Pet2,
			Gun,
			GreatSword,
			LongBow,
			Spear,
			Orb,
			DualBlade,
			Instrument,
			COUNT
		}
	}

	public sealed class Costume : Item
	{

	}

	public sealed class Grocery : Item
	{
		public GroceryTypeSeq GroceryType => Attributes.Get<GroceryTypeSeq>("grocery-type");
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
			BaseCampWarp,
			PetFood,
			ResetDungeon,
			SkillBook,
			FishingPaste,
			Badge,
			Scroll,
			FusionSubitem,
			Card,
			Relic,
			RelicMaterial,
			StarStone,
			Voucher,
			CapitalTeleport,
			SkillTrainByItemExtract,
			COUNT
		}

		public sbyte SkillLimitLevel => Attributes.Get<sbyte>("skill-limit-level");
		public sbyte SkillLimitLevelMax => Attributes.Get<sbyte>("skill-limit-level-max");
		public sbyte SkillLimitMasteryLevel => Attributes.Get<sbyte>("skill-limit-mastery-level");
		public sbyte SkillLimitMasteryLevelMax => Attributes.Get<sbyte>("skill-limit-mastery-level-max");
		public JobSeq SkillLimitJob => Attributes.Get<JobSeq>("skill-limit-job");
		public Faction SkillLimitFaction => Attributes.Get<Faction>("skill-limit-faction");
		public short SkillLimitFactionLevel => Attributes.Get<short>("skill-limit-faction-level");
		public Faction SkillLimitActivatedFaction => Attributes.Get<Faction>("skill-limit-activated-faction");
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
		public AccessoryTypeSeq AccessoryType => Attributes.Get<AccessoryTypeSeq>("accessory-type");
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
			NormalStateAppearance,
			IdleStateAppearance,
			ChattingSymbol,
			PortraitAppearance,
			HypermoveAppearance,
			NamePlateAppearance,
			SpeechBubbleAppearance,
			TalkSocial,
			Armlet1,
			Armlet2,
			COUNT
		}
	}

	public sealed class Enchant : Item
	{

	}
	#endregion

	#region Properties
	public ItemDecomposeInfo DecomposeInfo => new(this);

	public string Name => Attributes["name2"].GetText() ?? ToString();

	public Icon FrontIcon => Attributes.Get<Icon>("icon");

	public ImageProperty BackgroundImage => IconTexture.GetBackground(ItemGrade);

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
	#endregion
}

public class ItemDecomposeInfo
{
	#region Fields
	public bool DecomposeRewardByConsumeIndex;
	public Reward[] DecomposeReward;
	public Reward DecomposeEventReward;
	public Dictionary<JobSeq, Reward> DecomposeJobRewards = [];
	public int DecomposeMax = 1;
	public int DecomposeMoneyCost;
	public Item[] DecomposeByItem2;
	public short[] DecomposeByItem2StackCount;
	public Item[] JobDecomposeByItem2;
	public short[] JobDecomposeByItem2StackCount;
	#endregion

	#region Constructor
	internal ItemDecomposeInfo(Item data)
	{
		var attributes = data.Attributes;

		DecomposeRewardByConsumeIndex = attributes.Get<bool>("decompose-reward-by-consume-index");
		DecomposeReward = attributes.Get<Reward[]>("decompose-reward");
		DecomposeEventReward = attributes.Get<Reward>("decompose-event-reward");
		Job.PcJobs.ForEach(job => DecomposeJobRewards[job] = attributes.Get<Reward>("decompose-job-reward-" + job.GetDescription()));
		DecomposeMax = attributes.Get<sbyte>("decompose-max");
		DecomposeMoneyCost = attributes.Get<int>("decompose-money-cost");
		DecomposeByItem2 = attributes.Get<Item[]>("decompose-by-item2");
		DecomposeByItem2StackCount = attributes.Get<short[]>("decompose-by-item2-stack-count");
		JobDecomposeByItem2 = attributes.Get<Item[]>("job-decompose-by-item2");
		JobDecomposeByItem2StackCount = attributes.Get<short[]>("job-decompose-by-item2-stack-count");
	}
	#endregion


	#region Methods
	public FPackageIndex GetImage()
	{
		var image = GetImage(DecomposeByItem2[0]);
		image ??= GetImage(JobDecomposeByItem2[0]);
		image ??= DecomposeMoneyCost == 0 ? null : new MyFPackageIndex("BNSR/Content/Art/UI/GameUI/Resource/GameUI_Icon/Weapon_Lock_04.Weapon_Lock_04");

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