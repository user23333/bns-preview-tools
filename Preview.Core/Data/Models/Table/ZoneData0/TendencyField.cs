namespace Xylia.Preview.Data.Models;
public abstract class TendencyField : ModelElement, IAttraction
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public Ref<Text> TendencyFieldName2 { get; set; }

	public Ref<Text> TendencyFieldDesc { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public sbyte UiTextGrade { get; set; }

	public Ref<GameMessage>[] TendencyStandbyMsg { get; set; }

	public Ref<GameMessage>[] TendencyStartMsg { get; set; }

	public Ref<GameMessage>[] TendencyTimeupAlarmMsg { get; set; }

	public Ref<GameMessage>[] TendencyEndMsg { get; set; }

	public Ref<GameMessage>[] GuideStandbyMsg { get; set; }

	public Ref<GameMessage>[] GuideStartMsg { get; set; }

	public Ref<GameMessage>[] GuideTimeupAlarmMsg { get; set; }

	public Ref<GameMessage>[] GuideEndMsg { get; set; }

	public sbyte RecommandLevelMin { get; set; }

	public sbyte RecommandLevelMax { get; set; }

	public sbyte RecommandMasteryLevelMin { get; set; }

	public sbyte RecommandMasteryLevelMax { get; set; }

	public short RecommendAttackPower { get; set; }

	public Ref<Item> StandardGearWeapon { get; set; }

	public Ref<Quest>[] DisplayQuests { get; set; }

	public Ref<Text> Tactic { get; set; }

	public Ref<ContentsJournalRecommendItem> RecommendAlias { get; set; }

	public sealed class Normal : TendencyField
	{
	}

	public sealed class Buyudo : TendencyField
	{
	}
	#endregion

	#region IAttraction
	public string Name => this.TendencyFieldName2.GetText();

	public string Description => this.TendencyFieldDesc.GetText();
	#endregion
}