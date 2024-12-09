using System.Security.Cryptography;
using CUE4Parse.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Tests.DatTests;
[TestClass]
public partial class DatTests
{
    [TestMethod]
    public void Package()
    {
        var param = new PackageParam(@"D:\Tencent\BnsData\Test\data.pak")
        {
            FolderPath = @"D:\Tencent\BnsData\Test\data",
            CompressionLevel = CompressionLevel.Fast,
            BinaryXmlVersion = BinaryXmlVersion.None,
            CompressionMethod = CompressionMethod.Oodle,
        };

        BNSDat.CreateFromDirectory(param);
    }

	[TestMethod]
	public void SerializeTest()
	{
		using var rsa = new RSACryptoServiceProvider(1024);
		var PrivateKey = rsa.ToXmlString(true);
		var PublicKey = rsa.ToXmlString(false);
		var Parameter = rsa.ExportParameters(true);

		Console.WriteLine(PrivateKey);
		Console.WriteLine(PublicKey);
	}
}