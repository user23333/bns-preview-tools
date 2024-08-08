using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Tests.DatTests;

[TestClass]
public partial class Tables
{
	readonly BnsDatabase Database = new(new FolderProvider(@"D:\Tencent\BnsData\data_ztx\20240805"));

	[TestMethod]
	public void SerializeTest()
	{
		using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
		var PrivateKey = rsa.ToXmlString(true);
		var PublicKey = rsa.ToXmlString(false);
		var Parameter = rsa.ExportParameters(true);

		Console.WriteLine(PrivateKey);
		Console.WriteLine(PublicKey);

		Console.WriteLine(TimeUniversal.Parse("2024/7/24 8:00:00").Ticks);
	}
}