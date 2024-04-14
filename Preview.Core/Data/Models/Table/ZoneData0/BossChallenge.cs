using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public sealed class BossChallenge : ModelElement, IAttraction
{
	#region Attributes
	public sbyte UiTextGrade { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }
	#endregion

	#region IAttraction
	public string Name => this.Attributes["boss-challenge-name2"].GetText();

	public string Description => this.Attributes["boss-challenge-desc"].GetText();
	#endregion
}