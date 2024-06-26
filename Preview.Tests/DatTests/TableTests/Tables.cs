using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Common.DataStruct;
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

		Console.WriteLine(TimeUniversal.Parse("2024/6/26 8:00:00").Ticks);
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
}