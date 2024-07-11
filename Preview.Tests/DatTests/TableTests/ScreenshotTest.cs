﻿using System.Data;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models.Config;

namespace Xylia.Preview.Tests.DatTests;
public partial class Tables
{
	[TestMethod]
	[DataRow(@"C:\Users\Xylia\Pictures\BnS\截图00000.jpg")]
	public void Screenshot(string FilePath)
	{
		var bitmap = new Bitmap(FilePath);
		//var item = bitmap.GetPropertyItem(0x02bd);
		//bitmap.SetPropertyItem(item);

		//https://exiftool.org/TagNames/EXIF.html
		//foreach (PropertyItem property in bitmap.PropertyItems)
		//	Console.WriteLine($"ID:0x{property.Id:X2}, Type:{property.Type}, Length:{property.Len}");

		var xmlstring = Encoding.UTF8.GetString(bitmap.GetPropertyItem(0x02bc).Value);

		var serializer = new XmlSerializer(typeof(ScreenShot));
		var screenshot = serializer.Deserialize(new StringReader(xmlstring)) as ScreenShot;

		Console.WriteLine(xmlstring);


		//Param8 p1 = "01010106010c050403010201020102020b0102043500000000000a00000000050a00000000000f000000000000000000000000ec2800000000000000000000000000000000000000000000000000000000000000000a000000000000";
		//Param8 p2 = "01010106010c050403010201020105020b0102013500000000000a00000000050a00000000000f000000000000000000000000ec2800000000000000000000000000000000000000000000000000000000000000000a000000000000";
		//if (p1 != p2)
		//{

		//}
	}
}