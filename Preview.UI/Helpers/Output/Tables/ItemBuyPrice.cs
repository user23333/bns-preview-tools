﻿using OfficeOpenXml;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal sealed class ItemBuyPriceOut : OutSet
{
	protected override void CreateData(ExcelPackage package)
	{
		#region Title
		var sheet = CreateSheet(package);
		int column = 1, row = 1;
		sheet.SetColumn(column++, "alias", 70);
		sheet.SetColumn(column++, "钱币", 15);
		sheet.SetColumn(column++, "物品组", 20);
		sheet.SetColumn(column++, "物品1", 25);
		sheet.SetColumn(column++, "物品2", 25);
		sheet.SetColumn(column++, "物品3", 25);
		sheet.SetColumn(column++, "物品4", 25);
		sheet.SetColumn(column++, "灵气");
		sheet.SetColumn(column++, "仙豆");
		sheet.SetColumn(column++, "龙果");
		sheet.SetColumn(column++, "仙桃");
		sheet.SetColumn(column++, "珍珠");
		sheet.SetColumn(column++, "满足成就点数");
		sheet.SetColumn(column++, "满足完成成就");
		sheet.SetColumn(column++, "满足势力等级");
		sheet.SetColumn(column++, "满足个人战比武等级");
		sheet.SetColumn(column++, "满足车轮战比武等级");
		sheet.SetColumn(column++, "满足升龙谷等级");
		sheet.SetColumn(column++, "满足白鲸湖等级");
		sheet.SetColumn(column++, "满足银河遗迹等级");
		sheet.SetColumn(column++, "限购设置");
		#endregion

		#region Data
		foreach (var record in Source.Provider.GetTable<ItemBuyPrice>())
		{
			row++;
			column = 1;

			sheet.Cells[row, column++].SetValue(record);
			sheet.Cells[row, column++].SetValue(record.Money);
			sheet.Cells[row, column++].SetValue(record.RequiredItemBrand?.Name);

			for (int i = 0; i < 4; i++)
			{
				var item = record.RequiredItem[i].Value;
				var count = record.RequiredItemCount[i];

				sheet.Cells[row, column++].SetValue(item is null ? "" : (item.Name + " " + count));
			}

			sheet.Cells[row, column++].SetValue(record.RequiredFactionScore);
			sheet.Cells[row, column++].SetValue(record.RequiredDuelPoint);
			sheet.Cells[row, column++].SetValue(record.RequiredPartyBattlePoint);
			sheet.Cells[row, column++].SetValue(record.RequiredFieldPlayPoint);
			sheet.Cells[row, column++].SetValue(record.RequiredLifeContentsPoint);
			sheet.Cells[row, column++].SetValue(record.RequiredAchievementScore);
			sheet.Cells[row, column++].SetValue(record.RequiredAchievement?.Name2.GetText());

			sheet.Cells[row, column++].SetValue(record.FactionLevel);
			sheet.Cells[row, column++].SetValue(record.CheckSoloDuelGrade);
			sheet.Cells[row, column++].SetValue(record.CheckTeamDuelGrade);
			sheet.Cells[row, column++].SetValue(record.CheckBattleFieldGradeOccupationWar);
			sheet.Cells[row, column++].SetValue(record.CheckBattleFieldGradeCaptureTheFlag);
			sheet.Cells[row, column++].SetValue(record.CheckBattleFieldGradeLeadTheBall);
			sheet.Cells[row, column++].SetValue(record.CheckContentQuota.Value);
		}
		#endregion
	}
}