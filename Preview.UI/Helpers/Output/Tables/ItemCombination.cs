using OfficeOpenXml;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal class ItemCombinationOut : OutSet
{
	protected override void CreateData()
	{
		var provider = Source.Provider;

		#region Data
		{
			var sheet = CreateSheet("ItemCombination");
			int column = 1, row = 1;
			foreach (var record in provider.GetTable<ItemCombination>())
			{
				sheet.Cells[row, 1].SetValue(record.MaterialGroupName.GetText());

				WriteGroup(sheet, ref row, record.GreatSuccessItemGroup, record.GreatSuccessProbability);
				WriteGroup(sheet, ref row, record.SuccessItemGroup, record.SuccessProbability);
				WriteGroup(sheet, ref row, record.FailItemGroup, record.FailProbability);
				WriteGroup(sheet, ref row, record.BigFailItemGroup, record.BigFailProbability);
				row += 2;
			}
		}

		{
			var sheet = CreateSheet("WorldAccountCombination");
			int column = 1, row = 1;
			foreach (var record in provider.GetTable<WorldAccountCombination>())
			{
				sheet.Cells[row, 1].SetValue(record.MaterialGroupName.GetText());

				WriteGroup(sheet, ref row, record.GreatSuccessItemGroup, record.GreatSuccessProbability);
				WriteGroup(sheet, ref row, record.SuccessItemGroup, record.SuccessProbability);
				WriteGroup(sheet, ref row, record.FailItemGroup, record.FailProbability);
				WriteGroup(sheet, ref row, record.BigFailItemGroup, record.BigFailProbability);
				row += 2;
			}
		}
		#endregion
	}

	private static void WriteGroup(ExcelWorksheet sheet, ref int row, ItemGroup group, double probability)
	{
		if (group is null) return;

		var ItemProbability = probability * 0.0001d / group.ItemTotalCount;
		foreach (var item in group.Item.SelectNotNull(x => x.Instance))
		{
			row++;
			sheet.Cells[row, 1].SetValue(item.Name);
			sheet.Cells[row, 2].SetValue(ItemProbability);
			sheet.Cells[row, 2].Style.Numberformat.Format = "0.000%";
		}
	}
}