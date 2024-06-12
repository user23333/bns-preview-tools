using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;

[TestClass]
public partial class Tables
{
	readonly BnsDatabase Database = new(new FolderProvider(@"D:\Tencent\BnsData\data"));

	[TestMethod]
	public void SerializeTest()
	{
		using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
		var PrivateKey = rsa.ToXmlString(true);
		var PublicKey = rsa.ToXmlString(false);
		var Parameter = rsa.ExportParameters(true);

		Console.WriteLine(PrivateKey);
		Console.WriteLine(PublicKey);
	}


	[TestMethod]
	[DataRow("MH_MusinTower_0002")]
	public void NpcTest(string alias)
	{
		var record = Database.Provider.GetTable<Npc>()[alias];

		Console.WriteLine(record.Attributes["defend-power-creature-value"]);
		Console.WriteLine(record.Attributes["defend-parry-value-modify"]);
		Console.WriteLine(record.Attributes["defend-dodge-value-modify"]);
	}

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
	[DataRow("Combination_Card_0009")]
	public void CombinationTest(string alias)
	{
		var record = Database.Provider.GetTable<WorldAccountCombination>()[alias];
		Console.WriteLine(record.MaterialGroupName.GetText());

		var FailItemGroup = record.FailItemGroup.Instance;
		Console.WriteLine((record.FailProbability * 0.0001d / FailItemGroup.ItemTotalCount).ToString("P"));
		FailItemGroup.Item.ForEach(x => Console.WriteLine(x.Name));

		var SuccessItemGroup = record.SuccessItemGroup.Instance;
		Console.WriteLine();
		Console.WriteLine((record.SuccessProbability * 0.0001d / SuccessItemGroup.ItemTotalCount).ToString("P"));
		SuccessItemGroup.Item.ForEach(x => Console.WriteLine(x.Name));
	}
}