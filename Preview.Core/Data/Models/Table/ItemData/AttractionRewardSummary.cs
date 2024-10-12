using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class AttractionRewardSummary : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<ModelElement>[] RewardItemCommon { get; set; }

	public enum ItemCategorySeq
	{
		None,
		Costume,
		Weapon,
		EquipGem,
		Accessory,
		Etc,
		COUNT
	}

	public ItemCategorySeq[] RewardItemCommonCategory { get; set; }

	public ItemConditionType[] RewardItemCommonConditionType { get; set; }

	public Ref<ModelElement>[] RewardItemNormal { get; set; }

	public ItemCategorySeq[] RewardItemNormalCategory { get; set; }

	public ItemConditionType[] RewardItemNormalConditionType { get; set; }

	public Ref<ModelElement>[] RewardItemHard { get; set; }

	public ItemCategorySeq[] RewardItemHardCategory { get; set; }

	public ItemConditionType[] RewardItemHardConditionType { get; set; }

	public Ref<ModelElement>[] RewardItemEasy { get; set; }

	public ItemCategorySeq[] RewardItemEasyCategory { get; set; }

	public ItemConditionType[] RewardItemEasyConditionType { get; set; }

	public Ref<ModelElement>[] MainRewardItemNormal { get; set; }

	public Ref<ModelElement>[] MainRewardItemHard { get; set; }

	public Ref<ModelElement>[] MainRewardItemEasy { get; set; }

	public Ref<ModelElement>[] AdditionalRewardItem { get; set; }

	public ItemCategorySeq[] AdditionalRewardItemCategory { get; set; }

	public ItemConditionType[] AdditionalRewardItemConditionType { get; set; }

	public Ref<ModelElement>[] BonusRewardItemCommon { get; set; }

	public ItemCategorySeq[] BonusRewardItemCommonCategory { get; set; }

	public ItemConditionType[] BonusRewardItemCommonConditionType { get; set; }

	public Ref<ModelElement>[] BonusRewardItemEasy { get; set; }

	public ItemCategorySeq[] BonusRewardItemEasyCategory { get; set; }

	public ItemConditionType[] BonusRewardItemEasyConditionType { get; set; }

	public Ref<ModelElement>[] BonusRewardItemNormal { get; set; }

	public ItemCategorySeq[] BonusRewardItemNormalCategory { get; set; }

	public ItemConditionType[] BonusRewardItemNormalConditionType { get; set; }

	public Ref<ModelElement>[] BonusRewardItemHard { get; set; }

	public ItemCategorySeq[] BonusRewardItemHardCategory { get; set; }

	public ItemConditionType[] BonusRewardItemHardConditionType { get; set; }

	public BonusRewardDifficultyTypeSeq[] BonusRewardDifficultyType { get; set; }

	public enum BonusRewardDifficultyTypeSeq
	{
		None,
		Easy,
		Normal,
		Hard,
		COUNT
	}

	public Ref<ContentQuota>[] BonusRewardQuota { get; set; }
	#endregion
}