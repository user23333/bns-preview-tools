using OfficeOpenXml;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal class ZoneNpcSpawnOut : OutSet
{
	public override bool Visible => false;

	protected override void CreateData(ExcelPackage package)
	{
		#region Title
		var sheet = CreateSheet(package);
		int column = 1, row = 1;
		sheet.SetColumn(column++, "zone", 10);
		sheet.SetColumn(column++, "alias", 35);
		sheet.SetColumn(column++, "name", 20);
		sheet.SetColumn(column++, "max-hp", 20);
		sheet.SetColumn(column++, "attack-power-creature-min", 20);
		sheet.SetColumn(column++, "attack-power-creature-max", 20);
		sheet.SetColumn(column++, "defend-power-creature-value", 20);
		sheet.SetColumn(column++, "berserk-sequence-invoke-time", 20);
		#endregion

		#region Data
		var provider = new BnsDatabase(new FolderProvider(@"D:\Tencent\BnsData\GameData_Tencent\20241121"), Globals.Definition);
		foreach (var record in provider.Provider.GetTable<ZoneNpcSpawn>())
		{
			var npc = record.Npc.Value;
			if (npc is null) continue;

			row++;
			column = 1;
			sheet.Cells[row, column++].SetValue(record.Zone);
			sheet.Cells[row, column++].SetValue(npc.Alias);
			sheet.Cells[row, column++].SetValue(npc.GetName());
			sheet.Cells[row, column++].SetValue(npc.MaxHp);
			sheet.Cells[row, column++].SetValue(npc.AttackPowerCreatureMin);
			sheet.Cells[row, column++].SetValue(npc.AttackPowerCreatureMax);
			sheet.Cells[row, column++].SetValue(npc.DefendPowerCreatureValue);
			sheet.Cells[row, column++].SetValue(npc.BossNpc.Value?.BerserkSequenceInvokeTime);
		}
		#endregion
	}
}