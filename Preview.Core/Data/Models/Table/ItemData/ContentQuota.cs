using System.Text;
using Xylia.Preview.Data.Common;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class ContentQuota : ModelElement
{
	#region Attributes
	public long MinValue { get; set; }

	public long MaxValue { get; set; }

	public short Version { get; set; }

	public TargetTypeSeq TargetType { get; set; }
	public enum TargetTypeSeq
	{
		Character,
		Account,
		COUNT
	}

	public Time64 ExpirationTime { get; set; }

	public ChargeIntervalSeq ChargeInterval { get; set; }
	public enum ChargeIntervalSeq
	{
		None,
		Hourly,
		Daily,
		Weekly,
		COUNT
	}

	public DayOfWeekSeq ChargeDayOfWeek { get; set; }

	public sbyte ChargeTime { get; set; }

	public long ChargeAmountPerInterval { get; set; }

	public bool ConsumeKeyRecord { get; set; }

	public Ref<ContentQuota>[] ConsumeOrder { get; set; }
	#endregion

	#region Methods
	public string ItemStoreText
	{
		get
		{
			var builder = new StringBuilder();
			builder.Append(TargetType switch
			{
				TargetTypeSeq.Character => "UI.ItemStore.ContentQuota.Character".GetText() + " ",
				TargetTypeSeq.Account => "UI.ItemStore.ContentQuota.Account".GetText() + " ",
				_ => null
			});

			if (ExpirationTime != default) builder.Append("UI.ItemStore.ContentQuota.ExpirationTime".GetText([ExpirationTime]));
			if (ChargeInterval != ChargeIntervalSeq.None) builder.Append(ChargeInterval.ToString());

			builder.Append("UI.ItemStore.ContentQuota.ItemBuyCount".GetText([ChargeAmountPerInterval, MaxValue]));

			return builder.ToString();
		}
	}

	public string ItemStoreDesc
	{
		get
		{
			if (ExpirationTime != default)
				return "UI.ItemStore.BuyConfirm.Expiration.QuotaDesc".GetText([ExpirationTime, ExpirationTime.Hour.AMPM(), ExpirationTime.Hour]);

			return (ChargeInterval switch
			{
				ChargeIntervalSeq.None => "UI.ItemStore.BuyConfirm.None.QuotaDesc",
				ChargeIntervalSeq.Hourly => "UI.ItemStore.BuyConfirm.EveryHour.QuotaDesc",
				ChargeIntervalSeq.Daily => "UI.ItemStore.BuyConfirm.Daily.QuotaDesc",
				ChargeIntervalSeq.Weekly => ChargeDayOfWeek switch
				{
					DayOfWeekSeq.Sun => "UI.ItemStore.BuyConfirm.Sun.QuotaDesc",
					DayOfWeekSeq.Mon => "UI.ItemStore.BuyConfirm.Mon.QuotaDesc",
					DayOfWeekSeq.Tue => "UI.ItemStore.BuyConfirm.Tue.QuotaDesc",
					DayOfWeekSeq.Wed => "UI.ItemStore.BuyConfirm.Wed.QuotaDesc",
					DayOfWeekSeq.Thu => "UI.ItemStore.BuyConfirm.Thu.QuotaDesc",
					DayOfWeekSeq.Fri => "UI.ItemStore.BuyConfirm.Fri.QuotaDesc",
					DayOfWeekSeq.Sat => "UI.ItemStore.BuyConfirm.Sat.QuotaDesc",
					_ => null
				},
				_ => null
			}).GetText([ChargeDayOfWeek, ChargeTime, ChargeTime.AMPM()]);
		}
	}

	public string DungeonBonusRewardDesc
	{
		get
		{
			return (ChargeInterval switch
			{
				ChargeIntervalSeq.Daily => "UI.DungeonBonusReward.Guide.QuotaDesc.Daily",
				ChargeIntervalSeq.Weekly => ChargeDayOfWeek switch
				{
					DayOfWeekSeq.Sun => "UI.DungeonBonusReward.Guide.QuotaDesc.Sun",
					DayOfWeekSeq.Mon => "UI.DungeonBonusReward.Guide.QuotaDesc.Mon",
					DayOfWeekSeq.Tue => "UI.DungeonBonusReward.Guide.QuotaDesc.Tue",
					DayOfWeekSeq.Wed => "UI.DungeonBonusReward.Guide.QuotaDesc.Wed",
					DayOfWeekSeq.Thu => "UI.DungeonBonusReward.Guide.QuotaDesc.Thu",
					DayOfWeekSeq.Fri => "UI.DungeonBonusReward.Guide.QuotaDesc.Fri",
					DayOfWeekSeq.Sat => "UI.DungeonBonusReward.Guide.QuotaDesc.Sat",
					_ => null
				},
				_ => null
			}).GetText([ChargeTime.AMPM(), ChargeTime]);
		}
	}
	#endregion
}