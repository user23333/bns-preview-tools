using System.Data;
using CUE4Parse.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Tests.DatTests;
public partial class TableTests
{
	[TestMethod]
	public void CustomizingParamTest()
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