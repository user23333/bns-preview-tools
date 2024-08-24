using Xylia.Preview.Common.Extension;
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

	#region Methods
	public List<GlyphRewardInfo> GetInfo()
	{
		var data = new List<GlyphRewardInfo>();

		#region Additional
		if (AdditionalGlyphPickProbability > 0)
		{
			var group = string.Format("{0}% ", AdditionalGlyphPickProbability) + "UI.GlyphProbability.Title.AcquireAddGlyph".GetText();

			AdditionalGlyph.Select(x => x.Instance).ForEach((glyph, idx) =>
			{
				data.Add(new GlyphRewardInfo()
				{
					Data = glyph,
					Group = group,
					Probability = (double)AdditionalGlyphProbWeight[idx] / AdditionalGlyphProbWeightTotal
				});
			});
		}
		#endregion

		#region General
		if (ResultGlyphProbWeightTotal > 0)
		{
			ResultGlyph.Select(x => x.Instance).ForEach((glyph, i) =>
			{
				data.Add(new GlyphRewardInfo()
				{
					Data = glyph,
					Probability = (double)ResultGlyphProbWeight[i] / ResultGlyphProbWeightTotal
				});
			});
		}
		else
		{
			var glyphs = Provider.GetTable<Glyph>();

			for (int i = 0; i < GradeProbWeight.Length; i++)
			{
				var w = GradeProbWeight[i];
				if (w == 0) continue;

				var _ = glyphs.Where(glyph => glyph.Grade == i + 1);
			}

			for (int i = 0; i < TierProbWeight.Length; i++)
			{
				var w = TierProbWeight[i];
				if (w == 0) continue;

				var _ = glyphs.Where(glyph => glyph.RewardTier == i + 1);
			}
		}

		//if (GroupProbWeightTotal > 0)
		//{
		//	for (int i = 0; i < GroupProbWeight.Length; i++)
		//	{
		//		//var CostGroupId = this.CostGroupId[i];
		//		var GroupProbWeight = this.GroupProbWeight[i];
		//		if (GroupProbWeight == 0) break;

		//		var glyphs = Provider.GetTable<Glyph>().Where(record => record.GroupId == ResultGroupId[i]);
		//		var prob = (double)GroupProbWeight / GroupProbWeightTotal / glyphs.Count();

		//		data.AddRange(glyphs.Select(glyph => new GlyphRewardInfo() { Data = glyph, Probability = prob }));
		//	}
		//}
		#endregion

		return data;
	}

	public class GlyphRewardInfo : IReward
	{
		public Glyph Data;
		public string Group;
		internal double Probability;

		public string ProbabilityInfo => Probability.ToString("P4");
	}
	#endregion
}