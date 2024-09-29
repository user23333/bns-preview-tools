using Xylia.Preview.Common.Attributes;

namespace Xylia.Preview.Data.Models;
public sealed class SoulBoostMission : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public MissionTypeSeq MissionType { get; set; }

	public enum MissionTypeSeq
	{
		None,
		PcLogin,
		CompleteChallengeDaily,
		CompleteChallengeWeekly,
		CompleteQuest,
		PlayTime,
		PlayTimeInZone,
		TimeBasedPlayReward,
		KillNpc,
		JoinPartyBattle,
		KillPcInPartyBattle,
		AccountLevel,
		TryTransformEquipItem,
		TransformItem,
		PcLevel,
		PcMasteryLevel,
		ExchangeItemInZone,
		LootItemInZone,
		PurchaseItemFromNpc,
		UseItem,
		ProcessGlyphFusion,
		EquipGlyph,
		JoinGuild,
		CompleteQuestGuild,
		ManipulateEnv,
		RepairItem,
		DecomposeItem,
		ImproveItem,
		EquipItem,
		SealedDungeonClear,
		ImproveItemSetBonusTotalLevel,
		DecomposeRandomBox,
		AcquireQuestReward,
		AcquireQuestBonusReward,
		COUNT
	}

	public EntityTypeSeq[] EntityType { get; set; }

	public enum EntityTypeSeq
	{
		None,
		Zone,
		Npc,
		Item,
		Quest,
		PartyBattleFieldType,
		RecipeCategory,
		EquipType,
		ItemGrade,
		GlyphColor,
		GlyphGrade,
		GlyphRewardType,
		Env,
		[Name("market-category-2")]
		MarketCategory2,
		[Name("market-category-3")]
		MarketCategory3,
		ItemGeneration,
		StageNumber,
		ImproveLevel,
		ItemImproveSetBonus,
		Dungeon,
		SealedDungeonLevel,
		Value,
		COUNT
	}

	public string[] ConditionAlias { get; set; }

	public long[] Condition { get; set; }
	#endregion
}