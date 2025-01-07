using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;
public partial class TableTests
{
	[TestMethod]
	public void WorldAccountExpeditionTest()
	{
		foreach (var record in Database.Provider.GetTable<WorldAccountExpedition>()
			.Where(x => x.ExpeditionType == WorldAccountExpedition.ExpeditionTypeSeq.Story))
		{
			Console.WriteLine(record.Name.GetText());
			record.Target.Values().ForEach(t => Console.WriteLine("	{0}", t.GetName()));
		}
	}
}