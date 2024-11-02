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

			AdditionalGlyph.Values().ForEach((glyph, idx) =>
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
			ResultGlyph.Values().ForEach((glyph, i) =>
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
			var glyphs = new List<(Glyph, double)>();
			foreach (var glyph in Provider.GetTable<Glyph>())
			{
				double prob = 1;

				// Only return first ability
				if (RewardType == RewardTypeSeq.Upgrade)
				{
					if (glyph.AbilityId != 1) continue;
				}

				if (GradeProbWeightTotal > 0)
				{
					var w = GradeProbWeight[glyph.Grade - 1];
					if (w == 0) continue;

					prob *= (double)w / GradeProbWeightTotal;
				}

				if (TierProbWeightTotal > 0)
				{
					var w = TierProbWeight[glyph.RewardTier - 1];
					if (w == 0) continue;

					prob *= (double)w / TierProbWeightTotal;
				}

				if (GroupProbWeightTotal > 0)
				{
					if (glyph.GroupId == 0 || !ResultGroupId.Any(x => x == glyph.GroupId)) continue;

					for (int i = 0; i < 8; i++)
					{
						if (ResultGroupId[i] == glyph.GroupId)
						{
							prob *= (double)GroupProbWeight[i] / GroupProbWeightTotal;
						}
					}
				}

				glyphs.Add((glyph, prob));
			}

			foreach (var group in glyphs.GroupBy(x => x.Item2))
			{
				var probability = group.Key / group.Count();

				data.AddRange(group.Select(x => new GlyphRewardInfo()
				{
					Data = x.Item1,
					Probability = probability,
				}));
			}
		}
		#endregion

		return data;
	}

	public class GlyphRewardInfo : IReward
	{
		public Glyph Data;
		public string Group;
		internal double Probability;

		public string ProbabilityInfo => Probability.ToString("P2");
	}
	#endregion
}