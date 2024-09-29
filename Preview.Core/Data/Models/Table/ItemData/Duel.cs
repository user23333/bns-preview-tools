using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class Duel : ModelElement, IAttraction
{
	#region	Attributes
	public string Alias { get; set; }

	public Ref<AttractionGroup> Group { get; set; }

	public DuelTypeSeq DuelType { get; set; }

	public enum DuelTypeSeq
	{
		None,
		DeathMatch1vs1,
		TagMatch3vs3,
		COUNT
	}

	public sbyte MaxRoundCount { get; set; }

	public sbyte RoundWinCount { get; set; }

	public sbyte RoundCount { get; set; }

	public Msec RoundStartCountdownDuration { get; set; }

	public Msec RoundPreparationDuration { get; set; }

	public Msec RoundDuration { get; set; }

	public Msec RoundRestartDuration { get; set; }

	public Ref<Zone> Zone { get; set; }

	public Ref<ZonePcSpawn>[] ArenaOutsideAlphaSidePcSpawn { get; set; }

	public Ref<ZonePcSpawn>[] ArenaOutsideBetaSidePcSpawn { get; set; }

	public Ref<Effect> Effect { get; set; }

	public bool IsUnratedMatch { get; set; }

	[Name("loading-description-1")]
	public Ref<Text> LoadingDescription1 { get; set; }

	[Name("loading-description-2")]
	public Ref<Text> LoadingDescription2 { get; set; }

	public Icon LoadingIcon { get; set; }

	//public Ref<Boast> Boast { get; set; }

	public Ref<Quest>[] AttractionQuest { get; set; }

	public bool UiFilterAttractionQuestOnly { get; set; }

	public Ref<Text> DuelName2 { get; set; }

	public Ref<Text> DuelDesc { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public Ref<WeeklyTimeTable>[] WeeklyTimeTableForAddedReward { get; set; }

	public int[] BonusPointPercent { get; set; }

	public int[] BonusExpPercent { get; set; }

	public sbyte CameraWorldPosIndex { get; set; }

	public Ref<WeeklyTimeTable> DisableCalcRatingScoreWeeklyTime { get; set; }

	public Ref<WeeklyTimeTable> AvailableNormalMatchingWeeklyTime { get; set; }

	public Ref<WeeklyTimeTable> AvailableIngameChampionshipMatchingWeeklyTime { get; set; }

	public bool IsChampionship { get; set; }
	#endregion

	#region IAttraction
	public string Name => this.DuelName2.GetText();

	public string Description => this.DuelDesc.GetText();
	#endregion
}