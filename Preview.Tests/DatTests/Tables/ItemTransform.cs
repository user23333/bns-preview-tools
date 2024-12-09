using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Tests.DatTests;
public partial class TableTests
{
	[TestMethod]
	public void ItemTransformTest()
	{
		foreach (var record in Database.Provider.GetTable<ItemTransformRecipe>())
		{
			Console.WriteLine("{0} -> {1}",
				record.MainIngredient.Instance.GetName(),
				record.NormalItem[0].Instance.GetName());
		}
	}

	[TestMethod]
	public void ItemImproveSuccessionTest()
	{
		var item = Database.Provider.GetTable<Item>()["General_SeasonWpn_01_WarDagger_0003_00"];
		Debug.WriteLine(item.GetName());

		foreach (var succession in ItemImproveSuccession.FindBySeed(Database.Provider, item))
		{
			Debug.WriteLine(succession.Attributes);
			Debug.WriteLine(ItemImprove.GetResultItem(item, succession.ResultImproveLevel).GetName());
		}
	}
}
