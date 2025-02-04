using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Tests.DatTests;
public partial class TableTests
{
	[TestMethod]
	[DataRow("ItemSpirit_0215")]
	public void DistributionTest(string alias)
	{
		var record = Database.Provider.GetTable<ItemSpirit>()[alias];

		var array1 = record.DistributionType.Value.Do(record.AbilityMin[0], record.AbilityMax[0]);
		var array2 = record.DistributionType.Value.Do(record.AbilityMin[1], record.AbilityMax[1]);

		//Console.WriteLine(record.AttachAbility[0].GetText());
		//for (int i = 0; i < array1.Length; i++) Console.WriteLine($"{array1[i].Item1} {array1[i].Item2:P5}");

		Console.WriteLine(record.AttachAbility[1].GetText());
		for (int i = 0; i < array2.Length; i++) Console.WriteLine($"{array2[i].Item1} {array2[i].Item2:P5}");
	}

	[TestMethod]
	[DataRow("RacoonStoreTest_03", 1)]
	public void RacoonStoreTest(string alias, int slot)
	{
		var group = Database.Provider.GetTable<RacoonStore>()[alias].Attributes.Get<int>("slot-group-" + slot);
		var items = Database.Provider.GetTable<RacoonStoreItem>().Where(x => x.SlotGroup == group);
		var TotalItemProbWeight = items.Sum(x => x.ItemProbWeight);

		foreach (var record in items)
		{
			Console.WriteLine(string.Format("{0} {1:P3}  {2} {3}",
				record.Item.Value?.Name,
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

	}

	[TestMethod]
	[DataRow("SoulBoost_Season_0018")]
	public void SoulBoostTest(string alias)
	{
		Console.WriteLine(TimeUniversal.Parse("2024/12/25 8:00:00").Ticks);

		Database.Provider.GetTable<SoulBoostSeason>()[alias].TestMethod();
	}

	[TestMethod]
	public void WorldBossRewardTest(int total, int rank, double arg3)
	{
		var record = Database.Provider.GetTable<WorldBossReward>()["ME_BC_DesertDragon_0001_reward"];
		var result = record.GetPriceReward(total, rank);

		Console.WriteLine($"{total} 排名{rank}的分配结果是 {result}  {arg3 / result:P3}");
	}
}