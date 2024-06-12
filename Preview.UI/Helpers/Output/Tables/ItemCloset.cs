using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using static Xylia.Preview.Data.Models.Item;
using static Xylia.Preview.Data.Models.Item.Accessory;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal sealed class ItemCloset : OutSet
{
    protected override void CreateData()
    {
		#region Title
		var sheet = CreateSheet();
		int column = 1, row = 1;
		sheet.SetColumn(column++, "id", 15);
        sheet.SetColumn(column++, "alias", 40);
        sheet.SetColumn(column++, "name", 25);
        sheet.SetColumn(column++, "equip-type", 15);
        sheet.SetColumn(column++, "sex", 15);
        sheet.SetColumn(column++, "race", 15);
        sheet.SetColumn(column++, "closet-group-id", 20);
        #endregion

        foreach (var item in Source.Provider.GetTable<Item>())
        {
            #region Check
            bool flag = false;
            if (item is Costume) flag = true;
            else if (item is Weapon && item.ClosetGroupId != 0) flag = true;
            else if (item is Accessory accessory && accessory.AccessoryType is AccessoryTypeSeq.CostumeAttach or AccessoryTypeSeq.Vehicle) flag = true;

            if (!flag) continue;
            else if (item.Attributes.Get<int>("usable-duration") != 0) continue;
			#endregion


			row++;
            column = 1;

            sheet.Cells[row, column++].SetValue(item.PrimaryKey);
            sheet.Cells[row, column++].SetValue(item.ToString());
            sheet.Cells[row, column++].SetValue(item.Name);
            sheet.Cells[row, column++].SetValue(item.EquipType.GetText());
            sheet.Cells[row, column++].SetValue(item.EquipSex.GetText());
            sheet.Cells[row, column++].SetValue(item.EquipRace);
            sheet.Cells[row, column++].SetValue(item.ClosetGroupId);
        }
    }
}