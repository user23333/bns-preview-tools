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

		Console.WriteLine(record.Attributes["defend-power-creature-value"]);
		Console.WriteLine(record.Attributes["defend-parry-value-modify"]);
		Console.WriteLine(record.Attributes["defend-dodge-value-modify"]);
	}
}