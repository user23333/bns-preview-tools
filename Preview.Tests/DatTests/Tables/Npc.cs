using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;
public partial class TableTests
{
	[TestMethod]
	[DataRow("q_801_1")]
	public void NpctTalkMessageTest(string alias)
	{
		var record = Database.Provider.GetTable<NpcTalkMessage>()[alias];
		record.Test();
	}
}