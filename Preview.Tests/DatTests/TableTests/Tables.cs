using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Tests.DatTests;

[TestClass]
public partial class Tables
{
	private BnsDatabase Database { get; } = new(new FolderProvider(
		new DirectoryInfo(@"D:\Tencent\BnsData\GameData_ZTx").GetDirectories()[^1].FullName));

	[TestMethod]
	public void SerializeTest()
	{
		using var rsa = new RSACryptoServiceProvider(1024);
		var PrivateKey = rsa.ToXmlString(true);
		var PublicKey = rsa.ToXmlString(false);
		var Parameter = rsa.ExportParameters(true);

		Console.WriteLine(PrivateKey);
		Console.WriteLine(PublicKey);

		Console.WriteLine(TimeUniversal.Parse("2024/8/20 7:59:00").Ticks);
		Console.WriteLine(TimeUniversal.Parse("2024/8/21 8:00:00").Ticks);
	}
}