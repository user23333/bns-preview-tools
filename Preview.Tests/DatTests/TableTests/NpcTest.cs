using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;
public partial class Tables
{
	[TestMethod]
	[DataRow("MH_MusinTower_0002")]
	public void NpcTest(string alias)
	{
		var record = Database.Provider.GetTable<Npc>()[alias];

		record.AbilityTest();
	}
}