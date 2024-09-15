namespace Xylia.Preview.Data.Models;
public sealed class WorldBossReward : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public int[] AuctionPriceContributionRate { get; set; }

	public int[] AuctionPriceContributionRankTop { get; set; }

	public int[] AuctionPriceContributionRankBottom { get; set; }
	#endregion

	#region Methods
	public int GetPriceReward(int price, int rank)
	{
		var tax = (int)Math.Ceiling(price * 0.15);
		var AuctionPriceContributionTotalRate = AuctionPriceContributionRate.Sum();

		for (int i = 0; i < 20; i++)
		{
			var top = AuctionPriceContributionRankTop[i];
			var bottom = AuctionPriceContributionRankBottom[i];
			var rate = AuctionPriceContributionRate[i];

			if (rank >= top && rank <= bottom)
			{
				var member = (bottom - top + 1) * 7;  //num of jobs
				var result = (price - tax) * ((double)rate / AuctionPriceContributionTotalRate) / member;

				return (int)Math.Ceiling(result);
			}
		}

		return 0;
	}
	#endregion
}