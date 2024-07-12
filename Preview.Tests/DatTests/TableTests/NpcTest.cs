using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Creature;

namespace Xylia.Preview.Tests.DatTests;
public partial class Tables
{
	[TestMethod]
	[DataRow("MH_MusinTower_0002")]
	public void NpcTest(string alias)
	{
		var record = Database.Provider.GetTable<Npc>()[alias];

		var BossNpc = ((Record)record.Attributes["boss-npc"])?.As<BossNpc>();
		//Console.WriteLine(BossNpc?.BerserkSequenceInvokeTime);

		var level = (sbyte)record.Attributes["level"];
		var masterylevel = (sbyte)record.Attributes["mastery-level"];

		Console.WriteLine(record.Attributes["name2"].GetText() + $" ({level})");

		var DefendPowerCreatureValue = (int)record.Attributes["defend-power-creature-value"];
		Console.WriteLine($"defend {DefendPowerCreatureValue} → {AbilityFunction.DefendPower.GetPercent(DefendPowerCreatureValue, level):P3}");
	}
}