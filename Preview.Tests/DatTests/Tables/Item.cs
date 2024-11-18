using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Tests.DatTests;
public partial class TableTests
{
	[TestMethod]
	[DataRow("ItemSpirit_0180")]
	public void DistributionTest(string alias)
	{
		var record = Database.Provider.GetTable<ItemSpirit>()[alias];
		var array1 = record.DistributionType.Instance.Do(record.AbilityMin[0], record.AbilityMax[0]);
		var array2 = record.DistributionType.Instance.Do(record.AbilityMin[1], record.AbilityMax[1]);

		for (int i = 0; i < array1.Length; i++) Console.WriteLine($"{array1[i].Item1} {array1[i].Item2:P5}");
		for (int i = 0; i < array2.Length; i++) Console.WriteLine($"{array2[i].Item1} {array2[i].Item2:P5}");
	}

	[TestMethod]
	[DataRow(80)]
	public void RacoonStoreTest(int group)
	{
		var table = Database.Provider.GetTable<RacoonStoreItem>();
		var records = table.Where(x => x.SlotGroup == group);
		var TotalItemProbWeight = records.Sum(x => x.ItemProbWeight);

		foreach (var record in records)
		{
			Console.WriteLine(string.Format("{0} {1:P3}  {2} {3}",
				record.Item.Instance?.Name,
				(double)record.ItemProbWeight / TotalItemProbWeight,
				record.CostType,
				record.ItemCost
			));
		}
	}

	[DataRow("Accessory_Pirate_Grade5_Personal_002")]
	public void SmartDropRewardTest(string alias)
	{
		var record = Database.Provider.GetTable<SmartDropReward>()[alias];

		var item = record.GetItem(JobSeq.소환사);
		Console.WriteLine(item.ItemName);
	}

	[TestMethod]
	[DataRow("SoulBoost_Season_0014")]
	public void SoulBoostTest(string alias)
	{
		var record = Database.Provider.GetTable<SoulBoostSeason>()[alias];
		record.TestMethod();
	}

	[TestMethod]
	[DataRow(180000, 183, 76)]
	[DataRow(180000, 90, 107)]
	[DataRow(240000, 1, 285)]
	[DataRow(600000, 1, 714)]
	public void WorldBossRewardTest(int total, int rank, double arg3)
	{
		var record = Database.Provider.GetTable<WorldBossReward>()["ME_BC_DesertDragon_0001_reward"];
		var result = record.GetPriceReward(total, rank);

		Console.WriteLine($"{total} 排名{rank}的分配结果是 {result}  {arg3 / result:P3}");
	}
}