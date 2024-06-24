using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;
public partial class Tables
{
	[TestMethod]
	public void QuestTest()
	{
		var record = Database.Provider.GetTable<Quest>()[1732];
		Console.WriteLine(record.Name);
	}
}