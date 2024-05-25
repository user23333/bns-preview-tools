using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;

[TestClass]
public partial class Tables
{
	readonly BnsDatabase Database = new(new FolderProvider(@"D:\资源\客户端相关\Auto\data"));

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
}