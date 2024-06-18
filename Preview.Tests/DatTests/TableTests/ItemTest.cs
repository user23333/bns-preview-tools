using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Tests.DatTests;
public partial class Tables
{
	[TestMethod]
	[DataRow("Accessory_Pirate_Grade5_Personal_002")]
	public void SmartDropRewardTest(string alias)
	{
		var record = Database.Provider.GetTable<SmartDropReward>()[alias];

		var item = record.GetItem(JobSeq.소환사);
		Console.WriteLine(item.ItemName);
	}
}