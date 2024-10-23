using System.IO;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xylia.Preview.Tests.DatTests;
public partial class TableTests
{
	[TestMethod]
	public void TextTest()
	{
		#region Create dict
		var dict1 = new Dictionary<string, string>();
		var dict2 = new Dictionary<string, string>();

		var table1 = new XmlDocument();
		table1.Load("C:\\Users\\Xylia\\Desktop\\TextData.SkillTrain.x16");
		foreach (XmlElement record in table1.SelectNodes("table/record"))
		{
			var alias = record.GetAttribute("alias");
			var text = record.InnerXml;

			dict1[alias] = text;
		}

		var table2 = new XmlDocument();
		table1.Load("C:\\Users\\Xylia\\Desktop\\TextData.SkillTrain_cn.x16");
		foreach (XmlElement record in table1.SelectNodes("table/record"))
		{
			var alias = record.GetAttribute("alias");
			var text = record.InnerXml;

			dict2[alias] = text;
		}

		var replace = new Dictionary<string, string>();
		foreach (var pair1 in dict1)
		{
			replace[pair1.Value] = dict2[pair1.Key];
		}
		#endregion

		var temp = File.ReadAllText(@"D:\Tencent\BnsData\GameData_ZNcs\20241016\TextData.2.x16");
		foreach (var pair in replace) temp = temp.Replace(pair.Key, pair.Value);

		File.WriteAllText(@"D:\Tencent\BnsData\GameData_ZNcs\20241016\TextData.3.x16", temp, Encoding.Unicode);
	}
}