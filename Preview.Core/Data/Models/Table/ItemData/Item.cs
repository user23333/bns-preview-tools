using System.Text;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models.Creature;
using Xylia.Preview.Data.Models.Sequence;
using static Xylia.Preview.Data.Models.Item;
using static Xylia.Preview.Data.Models.Item.Grocery;

namespace Xylia.Preview.Data.Models;
public abstract class Item : ModelElement, IHaveName
{
	#region Attributes
	public Ref<ItemCombat>[] ItemCombat { get; set; }
	public Ref<ItemBrand> Brand { get; set; }

	public bool Auctionable => Attributes.Get<BnsBoolean>("auctionable");
	public bool WorldBossAuctionable => Attributes.Get<BnsBoolean>("world-boss-auctionable");
	public bool SealRenewalAuctionable => Attributes.Get<BnsBoolean>("seal-renewal-auctionable");

	public bool AccountUsed => Attributes.Get<BnsBoolean>("account-used");

	public GameCategory3Seq GameCategory3 => Attributes["game-category-3"].ToEnum<GameCategory3Seq>();

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

	public string ItemName => ItemNameOnly;
	public string ItemNameOnly => $"<font name=\"00008130.Program.Fontset_ItemGrade_{ItemGrade}\">{Attributes["name2"].GetText()}</font>";

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
	public string Name => Attributes["name2"].GetText() ?? base.ToString();

	public ImageProperty BackIcon => IconTexture.GetBackground(ItemGrade);

	public ImageProperty FrontIcon => IconTexture.Parse(Attributes.Get<string>("icon"));

	public FPackageIndex CanSaleItemImage => new MyFPackageIndex(
		Auctionable ? "BNSR/Content/Art/UI/GameUI/Resource/GameUI_Icon/SlotItem_marketBusiness.SlotItem_marketBusiness" :
		AccountUsed ? "BNSR/Content/Art/UI/GameUI/Resource/GameUI_Icon/SlotItem_privateSale.SlotItem_privateSale" : null);

	public FPackageIndex UnusableImage => DecomposeInfo.GetImage();

	public Tuple<string, string> CollectionSubstitute
	{
		get
		{
			StringBuilder Substitute1 = new(), Substitute2 = new();

			#region Info
			var MainInfo = Attributes.Get<Record>("main-info").GetText();
			var SubInfo = Attributes.Get<Record>("sub-info").GetText();
			if (MainInfo != null) Substitute1.AppendLine(MainInfo);
			if (SubInfo != null) Substitute2.AppendLine(SubInfo);
			#endregion

			#region Ability
			var data = new Dictionary<MainAbility, long>();

			var AttackPowerEquipMin = Attributes.Get<short>("attack-power-equip-min");
			var AttackPowerEquipMax = Attributes.Get<short>("attack-power-equip-max");
			data[MainAbility.AttackPowerEquipMinAndMax] = (AttackPowerEquipMin + AttackPowerEquipMax) / 2;

			var PveBossLevelNpcAttackPowerEquipMin = Attributes.Get<short>("pve-boss-level-npc-attack-power-equip-min");
			var PveBossLevelNpcAttackPowerEquipMax = Attributes.Get<short>("pve-boss-level-npc-attack-power-equip-max");
			data[MainAbility.PveBossLevelNpcAttackPowerEquipMinAndMax] = (PveBossLevelNpcAttackPowerEquipMin + PveBossLevelNpcAttackPowerEquipMax) / 2;

			var PvpAttackPowerEquipMin = Attributes.Get<short>("pvp-attack-power-equip-min");
			var PvpAttackPowerEquipMax = Attributes.Get<short>("pvp-attack-power-equip-max");
			data[MainAbility.PvpAttackPowerEquipMinAndMax] = (PvpAttackPowerEquipMin + PvpAttackPowerEquipMax) / 2;

			// HACK: Actually, the ability value is single get
			foreach (var seq in Enum.GetValues<MainAbility>())
			{
				if (seq == MainAbility.None) continue;

				var name = seq.ToString().TitleLowerCase();
				var value = Convert.ToInt32(this.Attributes[name]);
				if (value != 0) data[seq] = value;
				else if (seq != MainAbility.AttackAttributeValue)
				{
					var value2 = Convert.ToInt32(this.Attributes[name + "-equip"]);
					if (value2 != 0) data[seq] = value2;
				}
			}

			// HACK: Actually, the MainAbility is not this sequence
			var MainAbility1 = Attributes["main-ability-1"].ToEnum<MainAbility>();
			var MainAbility2 = Attributes["main-ability-2"].ToEnum<MainAbility>();

			foreach (var ability in data)
			{
				if (ability.Value == 0) continue;

				var text = ability.Key.GetText(ability.Value);
				if (ability.Key == MainAbility1 || ability.Key == MainAbility2) Substitute1.AppendLine(text);
				else Substitute2.AppendLine(text);
			}


			if (this is Gem)
			{
				var MainAbilityFixed = Attributes.Get<Record>("main-ability-fixed")?.As<ItemRandomAbilitySlot>();
				var SubAbilityFixed = Attributes.Get<Record>("sub-ability-fixed")?.As<ItemRandomAbilitySlot>();
				var SubAbilityRandomCount = Attributes.Get<sbyte>("sub-ability-random-count");
				var SubAbilityRandom = LinqExtensions.For(8, (id) => Attributes.Get<Record>("sub-ability-random-" + id)?.As<ItemRandomAbilitySlot>());

				if (MainAbilityFixed != null) Substitute1.AppendLine(MainAbilityFixed.Description);
				if (SubAbilityFixed != null) Substitute2.AppendLine(SubAbilityFixed.Description);
				if (SubAbilityRandomCount > 0)
				{
					Substitute2.AppendLine(TextHelper.RandomNum.Replace([SubAbilityRandomCount]));
					SubAbilityRandom.ForEach(x => Substitute2.AppendLine(x.Description + " <Image imagesetpath=\"00015590.Tag_Random\" enablescale=\"true\" scalerate=\"1.2\"/>"), true);
				}
			}
			#endregion

			#region Equip
			for (int i = 1; i <= 4; i++)
			{
				var EffectEquip = Attributes.Get<Record>("effect-equip-" + i);
				if (EffectEquip is null) continue;

				var Name3 = EffectEquip.Attributes.Get<Record>("name3").GetText();
				var Description3 = EffectEquip.Attributes.Get<Record>("description3").GetText();

				if (Name3 != null) Substitute1.AppendLine(Name3);
				if (Description3 != null) Substitute2.AppendLine(Description3);
			}
			#endregion

			return new(
				Substitute1.ToString().TrimEnd('\n'),
				Substitute2.ToString().TrimEnd('\n'));
		}
	}

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