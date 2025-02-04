using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using CUE4Parse.Compression;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Configuration;

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

	[TestMethod]
	[DataRow(@"C:\Users\Xylia\Pictures\BnS\CharacterCustomize\外形_灵_女00000.jpg")]
	public void Screenshot(string FilePath)
	{
		var bitmap = new Bitmap(FilePath);

		//https://exiftool.org/TagNames/EXIF.html
		//foreach (PropertyItem property in bitmap.PropertyItems)
		//	Console.WriteLine($"ID:0x{property.Id:X2}, Type:{property.Type}, Length:{property.Len}");

		var xmlstring = Encoding.UTF8.GetString(bitmap.GetPropertyItem(0x02bc).Value);
		var serializer = new XmlSerializer(typeof(ScreenShot));
		var screenshot = serializer.Deserialize(new StringReader(xmlstring)) as ScreenShot;

		var param = new Param8(screenshot.appearance.data);
	}
}