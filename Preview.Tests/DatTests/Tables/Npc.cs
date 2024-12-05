using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;
public partial class TableTests
{
	[TestMethod]
	[DataRow("qrsp_2276_1")]
	public void NpcResponseTest(string alias)
	{
		var record = Database.Provider.GetTable<NpcResponse>()[alias];
		record.TalkMessage.Instance?.Test();
	}
}