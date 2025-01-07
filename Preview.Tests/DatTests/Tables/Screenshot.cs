using System.Data;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using CUE4Parse.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Configuration;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Tests.DatTests;
public partial class TableTests
{
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

	[TestMethod]
	public void ParamTest()
	{
		foreach (var record in Database.Provider.GetTable<CustomizingUiMatchParam>()
			.Where(x => x.ParamIndex > -1 && x.Race == RaceSeq.린 && x.Sex == SexSeq.남)
			.OrderBy(x => x.ParamIndex))
		{
			Console.WriteLine("// {0} = {1}",
				record.ParamIndex,
				record.SubName.ToString()?.SubstringAfter("UI.CustomizeCharacter."));
		}
	}
}