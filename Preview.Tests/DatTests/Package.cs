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
}