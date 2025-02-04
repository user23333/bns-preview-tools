using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class AchievementRegister : ModelElement, IHaveName
{
	#region Attributes
	public string Alias { get; set; }

	public short Id { get; set; }

	public JobSeq Job { get; set; }

	public short Version { get; set; }

	public int MaxValue { get; set; }

	public bool ForNewbieCare { get; set; }

	public bool Persistant { get; set; }

	public bool IncrementFromClient { get; set; }

	public Ref<Achievement>[] Achievement { get; set; }

	public Ref<Text> RegisterName { get; set; }

	public sealed class Null : AchievementRegister { }

	public sealed class KillBossNpc : AchievementRegister
	{
		public Ref<Npc>[] BossNpc { get; set; }

		public bool FirstAttacker { get; set; }

		public bool LastAttacker { get; set; }

		public sbyte MaxLevel { get; set; }

		public bool TeamBroadcast { get; set; }

		public DifficultyTypeSeq DifficultyType { get; set; }

		public sbyte PartyMemberMinCount { get; set; }

		public sbyte PartyMemberMaxCount { get; set; }

		public bool PartySameJob { get; set; }

		public sbyte MinSealedLevel { get; set; }

		public SealedLevelOpSeq MinSealedLevelOp { get; set; }

		public sbyte MaxSealedLevel { get; set; }

		public SealedLevelOpSeq MaxSealedLevelOp { get; set; }

		public enum SealedLevelOpSeq
		{
			None,
			OrMore,
			OrLess,
			Over,
			Under,
			Equal,
			COUNT
		}


		protected override IEnumerable<object> Object => BossNpc.Values();
	}

	public sealed class KillNpc : AchievementRegister
	{
		public Ref<Npc>[] Npc { get; set; }

		public Ref<Faction> Faction { get; set; }

		protected override IEnumerable<object> Object => Npc.Values();
	}

	public sealed class KillPc : AchievementRegister
	{
		public Ref<Faction> Faction { get; set; }

		public RaceSeq Race { get; set; }

		public JobSeq TargetJob { get; set; }

		public sbyte AboveLevel { get; set; }

		public sbyte BelowLevel { get; set; }
	}

	public sealed class EquipItemBrandN : AchievementRegister
	{
		public Ref<ItemBrand>[] ItemBrand { get; set; }

		public EquipType[] EquipType { get; set; }

		public sbyte[] ItemMinLevel { get; set; }

		public Ref<Text>[] SlotName { get; set; }
	}

	public sealed class EquipItemN : AchievementRegister
	{
		public Ref<Item>[] Item { get; set; }

		public Ref<Text>[] SlotName { get; set; }
	}

	public sealed class UseGrocery : AchievementRegister
	{
		public Ref<Item>[] Item { get; set; }

		protected override IEnumerable<object> Object => Item.Values();
	}

	public sealed class UseGroceryN : AchievementRegister
	{
		public Ref<Item>[] Item { get; set; }

		public Ref<Text>[] SlotName { get; set; }
	}

	public sealed class UseGroceryBrandN : AchievementRegister
	{
		public Ref<ItemBrand>[] ItemBrand { get; set; }

		public Ref<Text>[] SlotName { get; set; }
	}

	public sealed class Time : AchievementRegister { }

	public sealed class UseSkillToNpc : AchievementRegister
	{
		public int[] SkillId { get; set; }

		public Ref<Npc>[] Npc { get; set; }
	}

	public sealed class DefendNpcSkill : AchievementRegister
	{
		public Ref<Skill3> NpcSkill { get; set; }

		public int[] SkillId { get; set; }
	}

	public sealed class PcExhaustion : AchievementRegister { }

	public sealed class PcDead : AchievementRegister { }

	public sealed class UseGadgetToNpc : AchievementRegister
	{
		public Ref<FieldItem> Gadget { get; set; }

		public Ref<Npc> Npc { get; set; }

		protected override IEnumerable<object> Object => [Gadget.Value, Npc.Value];
	}

	public sealed class QuestComplete : AchievementRegister
	{
		public Ref<Quest> Quest { get; set; }
	}

	public sealed class QuestCompleteCount : AchievementRegister
	{
		public Quest.ResetTypeSeq ResetType { get; set; }

		public ResetByAcquireTimeSeq ResetByAcquireTime { get; set; }

		public enum ResetByAcquireTimeSeq
		{
			None,
			Daily,
			Weekly,
			COUNT
		}
	}

	public sealed class TendencyQuestCompleteCount : AchievementRegister
	{
		public Ref<ModelElement> RequiredAttraction { get; set; }

		public sbyte TendencyId { get; set; }
	}

	public sealed class DuelWinCount : AchievementRegister
	{
		public DuelTypeSeq DuelType { get; set; }

		public enum DuelTypeSeq
		{
			None,
			Solo,
			Team,
			COUNT
		}
	}

	public sealed class DuelGrade : AchievementRegister
	{
		public DuelTypeSeq DuelType { get; set; }

		public enum DuelTypeSeq
		{
			None,
			Solo,
			Team,
			COUNT
		}
	}

	public sealed class ManipulateEnv : AchievementRegister
	{
		public Ref<ZoneEnv2>[] Env2 { get; set; }

		public Env2StateSeq[] Env2State { get; set; }

		public enum Env2StateSeq
		{
			None,
			Open,
			Close,
			Empty,
			[Name("step-1")] Step1,
			[Name("step-2")] Step2,
			[Name("step-3")] Step3,
			[Name("step-4")] Step4,
			[Name("step-5")] Step5,
			[Name("step-6")] Step6,
			[Name("step-7")] Step7,
			COUNT
		}
	}

	public sealed class ExchangeFactionScoreCount : AchievementRegister
	{
		public Ref<Npc> Npc { get; set; }

		public sbyte MinExchangeScore { get; set; }
	}

	public sealed class TeleportCount : AchievementRegister
	{
		public Ref<Teleport> Teleport { get; set; }
	}

	public sealed class HelpRestoration : AchievementRegister { }

	public sealed class HelpResurrect : AchievementRegister { }

	public sealed class GetContributionCount : AchievementRegister
	{
		public Ref<Npc>[] Npc { get; set; }
	}

	public sealed class GetContributionScore : AchievementRegister
	{
		public Ref<Npc>[] Npc { get; set; }
	}

	public sealed class GiveFactionScoreToRefiner : AchievementRegister { }

	public sealed class DuelBotChallengeFinishedFloor : AchievementRegister { }

	public sealed class AccumulateFieldPlayPointBySimpleTendencyQuest : AchievementRegister { }

	public sealed class AccumulateFactionScore : AchievementRegister { }

	public sealed class GrowthItem : AchievementRegister
	{
		public Ref<ItemBrand> SeedItemBrand { get; set; }

		public Ref<ItemBrand> FeedItemBrand { get; set; }

		public sbyte SeedItemBeforeLevel { get; set; }

		public sbyte SeedItemGrowthLevel { get; set; }

		public sbyte FeedItemLevel { get; set; }
	}

	public sealed class AttachEquipGemPiece : AchievementRegister
	{
		public Ref<ItemBrand> PrimaryItemBrand { get; set; }

		public Ref<ItemBrand> SecondaryItemBrand { get; set; }

		public sbyte PrimaryItemGrade { get; set; }

		public sbyte SecondaryItemGrade { get; set; }
	}

	public sealed class DecomposeItem : AchievementRegister
	{
		public Ref<ItemBrand> ItemBrand { get; set; }

		public sbyte ItemGrade { get; set; }

		public sbyte ItemLevel { get; set; }

		public EquipType EquipType { get; set; }

		public bool EquipGemOnly { get; set; }

		public sbyte WeaponGemLevel { get; set; }
	}

	public sealed class DecomposeItemN : AchievementRegister
	{
		public Ref<Item>[] Item { get; set; }

		public Ref<Text>[] SlotName { get; set; }
	}

	public sealed class DecomposeItemEquipTypeN : AchievementRegister
	{
		public Ref<ItemBrand> ItemBrand { get; set; }

		public sbyte ItemGrade { get; set; }

		public sbyte ItemLevel { get; set; }

		public EquipType[] EquipType { get; set; }

		public Ref<Text>[] SlotName { get; set; }
	}

	public sealed class DecomposeItemWeaponGemLevelN : AchievementRegister
	{
		public Ref<ItemBrand> WeaponGemBrand { get; set; }

		public sbyte WeaponGemGrade { get; set; }

		public sbyte[] WeaponGemLevel { get; set; }

		public Ref<Text>[] SlotName { get; set; }
	}

	public sealed class AttachGemToWeapon : AchievementRegister
	{
		public Ref<ItemBrand> WeaponBrand { get; set; }

		public sbyte WeaponGrade { get; set; }

		public Ref<ItemBrand> WeaponGemBrand { get; set; }

		public sbyte WeaponGemGrade { get; set; }

		public sbyte WeaponGemLevel { get; set; }
	}

	public sealed class AttachGemToWeaponN : AchievementRegister
	{
		public Ref<ItemBrand> WeaponBrand { get; set; }

		public sbyte WeaponGrade { get; set; }

		public Ref<Item>[] WeaponGem { get; set; }

		public Ref<Text>[] SlotName { get; set; }
	}

	public sealed class AwakeningItem : AchievementRegister
	{
		public Ref<ItemBrand> ItemBrand { get; set; }

		public EquipType EquipType { get; set; }
	}

	public sealed class TransformItemBrand : AchievementRegister
	{
		public Ref<ItemBrand> ItemBrand { get; set; }

		public sbyte ItemGrade { get; set; }

		public EquipType EquipType { get; set; }

		public ResultSeq Result { get; set; }

		public enum ResultSeq
		{
			None,
			Blank,
			Normal,
			Rare,
			Premium,
			COUNT
		}
	}

	public sealed class TransformItem : AchievementRegister
	{
		public Ref<Item> Item { get; set; }

		public ResultSeq Result { get; set; }

		public enum ResultSeq
		{
			None,
			Blank,
			Normal,
			Rare,
			Premium,
			COUNT
		}
	}

	public sealed class ZeroDurability : AchievementRegister
	{
		public Ref<ItemBrand> ItemBrand { get; set; }

		public sbyte ItemGrade { get; set; }
	}

	public sealed class RepairItem : AchievementRegister
	{
		public Ref<ItemBrand> ItemBrand { get; set; }

		public sbyte ItemGrade { get; set; }
	}

	public sealed class CheckCombinationCount : AchievementRegister { }

	public sealed class CompletitionStarWords : AchievementRegister { }

	public sealed class DetachPostAttachmentCount : AchievementRegister { }

	public sealed class DetachPostAttachmentMoney : AchievementRegister { }

	public sealed class DetachPostAttachmentItem : AchievementRegister
	{
		public Ref<Item> Item { get; set; }
	}

	public sealed class DetachPostAttachmentItemN : AchievementRegister
	{
		public Ref<Item>[] Item { get; set; }

		public Ref<Text>[] SlotName { get; set; }
	}

	public sealed class TakeCraftCount : AchievementRegister { }

	public sealed class TakeCraftItem : AchievementRegister
	{
		public Ref<Item> Item { get; set; }
	}

	public sealed class TakeCraftItemN : AchievementRegister
	{
		public Ref<Item>[] Item { get; set; }

		public Ref<Text>[] SlotName { get; set; }
	}

	public sealed class InventorySize : AchievementRegister { }

	public sealed class WardrobeSize : AchievementRegister { }

	public sealed class DepotSize : AchievementRegister { }

	public sealed class Depository2Size : AchievementRegister { }

	public sealed class PcLevel : AchievementRegister { }

	public sealed class PcMasteryLevel : AchievementRegister { }

	public sealed class ClientOnly : AchievementRegister { }

	public sealed class PartyBattleWinCount : AchievementRegister
	{
		public PartyBattleFieldZone.PartyBattleFieldZoneType PartyBattleType { get; set; }
	}

	public sealed class PartyBattleChallengeCount : AchievementRegister
	{
		public PartyBattleFieldZone.PartyBattleFieldZoneType PartyBattleType { get; set; }
	}

	public sealed class PartyBattleGrade : AchievementRegister
	{
		public PartyBattleFieldZone.PartyBattleFieldZoneType PartyBattleType { get; set; }
	}

	public sealed class LeadTheBallGoalInCount : AchievementRegister { }

	public sealed class SkillTrainingSubjectComplete : AchievementRegister
	{
		public Ref<SkillTrainingRoomGroup> SkillTrainingRoomGroup { get; set; }
	}

	public sealed class DisposeItemBuyPriceRequiredPoint : AchievementRegister { }

	public sealed class AccumulatePartyBattlePointByPartyBattleField : AchievementRegister { }

	public sealed class AcquireFishCount : AchievementRegister { }

	public sealed class AcquireSpecificFishCount : AchievementRegister
	{
		public Ref<Fish> Fish { get; set; }
	}

	public sealed class AcquireFishSizeCount : AchievementRegister
	{
		public int FishSize { get; set; }
	}

	public sealed class AcquireFishGradeCount : AchievementRegister
	{
		public sbyte FishGrade { get; set; }
	}

	public sealed class AcquireFishSizeGradeCount : AchievementRegister
	{
		public FishGrade.GradeSeq FishSizeGrade { get; set; }
	}

	public sealed class AccumulateLifeContentsPointByFishing : AchievementRegister { }
	public sealed class HyperRacingGameParticipation : AchievementRegister { }
	public sealed class HyperRacingGameFinish : AchievementRegister { }
	public sealed class HyperRacingGameRecord : AchievementRegister { }
	public sealed class FinishFeedback : AchievementRegister { }
	public sealed class DuelNpcChallengeFinishedFloor : AchievementRegister { }
	#endregion

	#region Methods
	protected virtual IEnumerable<object> Object { get; } = [];

	public string Name => RegisterName.GetText([null, .. Object]);
	#endregion
}