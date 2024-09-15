using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Tests.DatTests;
public partial class Tables
{
	[TestMethod]
	[DataRow("Spirit_0002")]
	public void DistributionTest(string alias)
	{
		var record = Database.Provider.GetTable<RandomDistribution>()[alias];

		var total = record.Weight.Sum(x => x);
		var count = 155 - 78;

		// 最大值应用 weight-101，然后均分剩下的值
		// 如果有余数则分配给最小值
	}

	[TestMethod]
	[DataRow("Accessory_Pirate_Grade5_Personal_002")]
	public void SmartDropRewardTest(string alias)
	{
		var record = Database.Provider.GetTable<SmartDropReward>()[alias];

		var item = record.GetItem(JobSeq.소환사);
		Console.WriteLine(item.ItemName);
	}

	[TestMethod]
	[DataRow(10008)]
	public void RacoonStoreTest(int group)
	{
		var table = Database.Provider.GetTable<RacoonStoreItem>();
		var records = table.Where(x => x.SlotGroup == group);
		var TotalItemProbWeight = records.Sum(x => x.ItemProbWeight);

		foreach (var record in records)
		{
			Console.WriteLine(string.Format("{0} {1:P3}",
				record.Item.Instance.Name,
				(double)record.ItemProbWeight / TotalItemProbWeight));
		}
	}



	[TestMethod]
	[DataRow(180000, 183, 76)]
	[DataRow(180000, 90, 107)]
	[DataRow(240000, 1, 285)]
	[DataRow(600000, 1, 714)]
	public void TestData(int total, int rank, double arg3)
	{
		var record = Database.Provider.GetTable<WorldBossReward>()["ME_BC_DesertDragon_0001_reward"];
		var result = record.GetPriceReward(total, rank);

		Console.WriteLine($"{total} 排名{rank}的分配结果是 {result}  {arg3 / result:P3}");
	}
}