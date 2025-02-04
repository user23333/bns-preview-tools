using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;
public partial class TableTests
{
	[TestMethod]
	public void ZoneTest()
	{
		var output = new XmlDocument();
		var _table = (XmlElement)output.AppendChild(output.CreateElement("table"));
		_table.SetAttribute("type", "zone");

		foreach (var record in Database.Provider.GetTable<Zone>())
		{
			var _record = output.CreateElement("record");
			_record.SetAttribute("id", record.Id.ToString());
			_record.SetAttribute("name", record.Name);
			_record.SetAttribute("zone-type2", record.ZoneType2.ToString());
			_table.AppendChild(_record);
		}

		output.Save("\\zone.xml");
	}

	[TestMethod]
	public void ZoneNpcSpawnTest()
	{
		// ME_CrocodileSharkTribeC2_0002
		var spawn = new ZoneNpcSpawn()
		{
			RespawnDelayMin = 150000 + 6000,
		};
		spawn.AddChannel(new() { Channel = 1, LastTime = Time64.Parse("2024/10/15 12:57:54", EPublisher.ZTx) });
		spawn.AddChannel(new() { Channel = 2, LastTime = Time64.Parse("2024/10/15 12:59:54", EPublisher.ZTx) });
		spawn.AddChannel(new() { Channel = 3, LastTime = Time64.Parse("2024/10/15 13:01:15", EPublisher.ZTx) });
		spawn.AddChannel(new() { Channel = 4, LastTime = Time64.Parse("2024/10/15 13:04:27", EPublisher.ZTx) });

		foreach (var channel in spawn.Channels.OrderBy(x => x.NextTime))
		{
			Console.WriteLine($"{channel.Channel} {channel.NextTime}");
		}
	}
}