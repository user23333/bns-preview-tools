using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class GlyphReward : ModelElement
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public Ref<Text> Name { get; set; }

	public Icon ShuffleRewardIcon { get; set; }

	public Ref<Text> ShuffleRewardTooltip { get; set; }

	public UpgradeRewardPreviewSeq UpgradeRewardPreview { get; set; }
	public enum UpgradeRewardPreviewSeq
	{
		FixedScore,
		RandomScore,
		COUNT
	}

	public bool UpgradeRewardWarningMessage { get; set; }

	public RewardTypeSeq RewardType { get; set; }
	public enum RewardTypeSeq
	{
		Acquire,
		Upgrade,
		Shuffle,
		COUNT
	}

	public sbyte Grade { get; set; }

	public Glyph.ColorSeq Color { get; set; }

	public Glyph.GlyphTypeSeq GlyphType { get; set; }

	public int CostMoney { get; set; }

	public Ref<Item>[] CostItem { get; set; }

	public short[] CostItemCount { get; set; }

	public sbyte TierPickProbability { get; set; }

	public sbyte AdditionalGlyphPickProbability { get; set; }

	public short[] GradeProbWeight { get; set; }

	public short GradeProbWeightTotal { get; set; }

	public short[] TierProbWeight { get; set; }

	public short TierProbWeightTotal { get; set; }

	public short[] ResultGlyphProbWeight { get; set; }

	public short ResultGlyphProbWeightTotal { get; set; }

	public Ref<Glyph>[] ResultGlyph { get; set; }

	public short[] AdditionalGlyphProbWeight { get; set; }

	public short AdditionalGlyphProbWeightTotal { get; set; }

	public Ref<Glyph>[] AdditionalGlyph { get; set; }

	public short[] CostGroupId { get; set; }

	public short[] ResultGroupId { get; set; }

	public short[] GroupProbWeight { get; set; }

	public short GroupProbWeightTotal { get; set; }
	#endregion
}