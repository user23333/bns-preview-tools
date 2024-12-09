using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.BinData.Serialization;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;
[TestClass]
public partial class TableTests
{
	private BnsDatabase Database { get; } = new(new FolderProvider(
		new DirectoryInfo(@"D:\Tencent\BnsData\GameData_ZTx").GetDirectories()[^1].FullName, EPublisher.ZTx), Globals.Definition);

	[TestMethod]
	public void NewTest()
	{
		var table = Globals.GameData.Provider.GetTable("worldbossspawn");
		var records = table.Records;

		// check required
		var record = new Record(table, null);
		record.Attributes["id"] = 7;
		records.Add(record);
		var settings = new TableWriterSettings() { Encoding = Encoding.UTF8 };
		Debug.WriteLine(settings.Encoding.GetString(table.WriteXml(settings)));
	}
}