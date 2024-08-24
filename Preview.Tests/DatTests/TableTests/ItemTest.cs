using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Document;
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
}