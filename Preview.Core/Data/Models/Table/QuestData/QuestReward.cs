using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class QuestReward : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public enum QuestFirstProgressSeq
	{
		None,
		Y,
		N,
		COUNT,
	}

	public QuestFirstProgressSeq QuestFirstProgress { get; set; }

	public sbyte QuestCompletionCount { get; set; }

	public Op QuestCompletionCountOp { get; set; }

	public int BasicMoney { get; set; }

	public int BasicExp { get; set; }

	public int BasicAccountExp { get; set; }

	public sbyte BasicMasteryLevel { get; set; }

	public short BasicProductionExp { get; set; }

	public short BasicFactionReputation { get; set; }

	public short BasicGuildReputation { get; set; }

	public int BasicDuelPoint { get; set; }

	public int BasicPartyBattlePoint { get; set; }

	public int BasicFieldPlayPoint { get; set; }

	public Ref<Skill3>[] FixedSkill3 { get; set; }

	public Ref<ModelElement>[] FixedCommonSlot { get; set; }

	public sbyte[] FixedCommonItemCount { get; set; }

	public sbyte[] FixedCommonSkillVarIdx { get; set; }

	public Ref<ModelElement>[] OptionalCommonSlot { get; set; }

	public sbyte[] OptionalCommonItemCount { get; set; }


	public Ref<Item>[] DayofweekSunFixedItem { get; set; }

	public sbyte[] DayofweekSunFixedItemCount { get; set; }

	public Ref<Item>[] DayofweekMonFixedItem { get; set; }

	public sbyte[] DayofweekMonFixedItemCount { get; set; }

	public Ref<Item>[] DayofweekTueFixedItem { get; set; }

	public sbyte[] DayofweekTueFixedItemCount { get; set; }

	public Ref<Item>[] DayofweekWedFixedItem { get; set; }

	public sbyte[] DayofweekWedFixedItemCount { get; set; }

	public Ref<Item>[] DayofweekThuFixedItem { get; set; }

	public sbyte[] DayofweekThuFixedItemCount { get; set; }

	public Ref<Item>[] DayofweekFriFixedItem { get; set; }

	public sbyte[] DayofweekFriFixedItemCount { get; set; }

	public Ref<Item>[] DayofweekSatFixedItem { get; set; }

	public sbyte[] DayofweekSatFixedItemCount { get; set; }

	public int DayofweekSunBonusMoney { get; set; }

	public int DayofweekMonBonusMoney { get; set; }

	public int DayofweekTueBonusMoney { get; set; }

	public int DayofweekWedBonusMoney { get; set; }

	public int DayofweekThuBonusMoney { get; set; }

	public int DayofweekFriBonusMoney { get; set; }

	public int DayofweekSatBonusMoney { get; set; }

	public Ref<Item>[] NewbiePartyFixedItem { get; set; }

	public sbyte[] NewbiePartyFixedItemCount { get; set; }

	public Ref<Item>[] OldbiePartyFixedItem { get; set; }

	public sbyte[] OldbiePartyFixedItemCount { get; set; }

	public Ref<Item>[] MentorFixedItem { get; set; }

	public sbyte[] MentorFixedItemCount { get; set; }

	public Ref<Item>[] HongmoonItem { get; set; }

	public short[] HongmoonItemCount { get; set; }

	public Ref<Item>[] MembershipItem { get; set; }

	public short[] MembershipItemCount { get; set; }

	public Ref<Item>[] PccafeItem { get; set; }

	public short[] PccafeItemCount { get; set; }

	public Ref<Item>[] DungeonAdditionalRewardItem { get; set; }

	public sbyte[] DungeonAdditionalRewardItemCount { get; set; }

	public Ref<CostGroup>[] DungeonAdditionalRewardCostA { get; set; }

	public Ref<CostGroup>[] DungeonAdditionalRewardCostB { get; set; }

	public Ref<QuestSealedDungeonReward> SealedDungeonReward { get; set; }

	public int SealedDungeonRewardDataId { get; set; }
	#endregion

	#region Methods
	public Integer Money => new((BasicMoney + DayofweekBonusMoney) * 1.0F);

	public Ref<Item>[] DayofweekFixedItem => DateTime.Now.DayOfWeek switch
	{
		DayOfWeek.Sunday => DayofweekSunFixedItem,
		DayOfWeek.Monday => DayofweekMonFixedItem,
		DayOfWeek.Tuesday => DayofweekTueFixedItem,
		DayOfWeek.Wednesday => DayofweekWedFixedItem,
		DayOfWeek.Thursday => DayofweekThuFixedItem,
		DayOfWeek.Friday => DayofweekFriFixedItem,
		DayOfWeek.Saturday => DayofweekSatFixedItem,
		_ => throw new ArgumentOutOfRangeException()
	};

	public sbyte[] DayofweekFixedItemCount => DateTime.Now.DayOfWeek switch
	{
		DayOfWeek.Sunday => DayofweekSunFixedItemCount,
		DayOfWeek.Monday => DayofweekMonFixedItemCount,
		DayOfWeek.Tuesday => DayofweekTueFixedItemCount,
		DayOfWeek.Wednesday => DayofweekWedFixedItemCount,
		DayOfWeek.Thursday => DayofweekThuFixedItemCount,
		DayOfWeek.Friday => DayofweekFriFixedItemCount,
		DayOfWeek.Saturday => DayofweekSatFixedItemCount,
		_ => throw new ArgumentOutOfRangeException()
	};

	public int DayofweekBonusMoney => DateTime.Now.DayOfWeek switch
	{
		DayOfWeek.Sunday => DayofweekSunBonusMoney,
		DayOfWeek.Monday => DayofweekMonBonusMoney,
		DayOfWeek.Tuesday => DayofweekTueBonusMoney,
		DayOfWeek.Wednesday => DayofweekWedBonusMoney,
		DayOfWeek.Thursday => DayofweekThuBonusMoney,
		DayOfWeek.Friday => DayofweekFriBonusMoney,
		DayOfWeek.Saturday => DayofweekSatBonusMoney,
		_ => throw new ArgumentOutOfRangeException()
	};
	#endregion
}