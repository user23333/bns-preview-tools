using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class QuestBonusRewardSetting : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<Quest> Quest { get; set; }

	public Ref<QuestBonusReward> Reward { get; set; }

	public Ref<ContentQuota> BasicQuota { get; set; }

	public Ref<ContentsReset>[] ContentsReset { get; set; }


	public sealed class SealedLevel : QuestBonusRewardSetting
	{
		public sbyte sealedLevel { get; set; }
	}

	public sealed class DifficultyType : QuestBonusRewardSetting
	{
		public DifficultyTypeSeq difficultyType { get; set; }
	}

	public sealed class IgnoreDifficulty : QuestBonusRewardSetting
	{

	}
	#endregion
}