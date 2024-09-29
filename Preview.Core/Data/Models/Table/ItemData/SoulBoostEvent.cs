using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class SoulBoostEvent : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public Ref<Text> EventNameText { get; set; }

	public string EventName { get; set; }

	public string EventNameEn { get; set; }

	public string EventNameFr { get; set; }

	public string EventNameDe { get; set; }

	public string EventNamePt { get; set; }

	public string EventNameTh { get; set; }

	public string EventNameVn { get; set; }

	public MissionStepNameTypeSeq MissionStepNameType { get; set; }

	public enum MissionStepNameTypeSeq
	{
		None,
		[Name("type-1")]
		Type1,
		[Name("type-2")]
		Type2,
		[Name("type-3")]
		Type3,
		[Name("type-4")]
		Type4,
		[Name("type-5")]
		Type5,
		[Name("type-6")]
		Type6,
		[Name("type-7")]
		Type7,
		[Name("type-8")]
		Type8,
		COUNT
	}

	public ObjectPath FrontImageset { get; set; }

	public ObjectPath BackImageset { get; set; }

	public ObjectPath FootImageset { get; set; }

	public ObjectPath SlotImageset { get; set; }

	public int MaxPoint { get; set; }

	public int MaxItemPoint { get; set; }

	public int[] GradePoint { get; set; }

	public Ref<SoulBoostGradeReward>[] GradeReward { get; set; }

	public Ref<SoulBoostGradeReward>[] BmGradeReward { get; set; }

	public int ExchangeRewardPoint { get; set; }

	public Ref<Item> ExchangeRewardItem { get; set; }

	public short ExchangeRewardItemCount { get; set; }

	public int ExchangeRewardContribution { get; set; }

	public int BmExchangeRewardPoint { get; set; }

	public Ref<Item> BmExchangeRewardItem { get; set; }

	public short BmExchangeRewardItemCount { get; set; }

	public int BmExchangeRewardContribution { get; set; }

	public sbyte BmAccumulateRewardIntervalDay { get; set; }

	public Ref<Item> BmAccumulateRewardItem { get; set; }

	public short BmAccumulateRewardItemCount { get; set; }

	public BmAccumulateRewardStartTimeTypeSeq BmAccumulateRewardStartTimeType { get; set; }

	public enum BmAccumulateRewardStartTimeTypeSeq
	{
		None,
		SeasonStartTime,
		ParticipationTime,
		COUNT
	}

	public Ref<Item> BmActivateItem { get; set; }

	public short BmActivateItemCount { get; set; }

	public UnlocatedStoreTypeSeq UnlocatedStoreType { get; set; }

	public enum UnlocatedStoreTypeSeq
	{
		UnlocatedNone,
		UnlocatedStore,
		AccountStore,
		[Name("soul-boost-store-1")]
		SoulBoostStore1,
		[Name("soul-boost-store-2")]
		SoulBoostStore2,
		[Name("soul-boost-store-3")]
		SoulBoostStore3,
		[Name("soul-boost-store-4")]
		SoulBoostStore4,
		[Name("soul-boost-store-5")]
		SoulBoostStore5,
		[Name("soul-boost-store-6")]
		SoulBoostStore6,
		EventStore,
		COUNT
	}

	public Ref<SoulBoostMissionStep>[] MissionStep { get; set; }

	public Ref<Item>[] CoreRewardItem { get; set; }

	public bool PurchaseGrade { get; set; }
	#endregion
}