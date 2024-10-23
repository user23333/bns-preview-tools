using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;
public partial class TableTests
{
	[TestMethod]
	public void SkillTest()
	{
		foreach (var record in Database.Provider.GetTable<ExtractSkillTrainByItem>())
		{
			Console.WriteLine("{0} {1}", 
				record.extractSkillTrainByItem.Instance?.Name,
				record.SkillTrainByItem.Instance.MainChangeSkill.Instance?.Name);
		}
	}
}