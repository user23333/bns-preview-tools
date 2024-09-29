using OfficeOpenXml;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal sealed class QuestEpic : OutSet
{
	protected override void CreateData(ExcelPackage package)
	{
		#region Title
		var sheet = CreateSheet(package);
		int column = 1, row = 1;
		sheet.SetColumn(column++, "id", 10);
		sheet.SetColumn(column++, "alias", 15);
		sheet.SetColumn(column++, "name", 30);
		sheet.SetColumn(column++, "group", 25);
		#endregion

		#region Data
		GetEpic(data =>
		{
			row++;
			column = 1;

			sheet.Cells[row, column++].SetValue(data.PrimaryKey);
			sheet.Cells[row, column++].SetValue(data);
			sheet.Cells[row, column++].SetValue(data.Name);
			sheet.Cells[row, column++].SetValue(data.Title);
		});
		#endregion
	}


	public static void GetEpic(Action<Quest> act, JobSeq? job = null) => GetEpic(FileCache.Data.Provider.GetTable<Quest>()["q_epic_221"], act, job ?? UserSettings.Default.Job);

	public static void GetEpic(Quest quest, Action<Quest> act, JobSeq job)
	{
		if (quest is null) return;

		// act
		act(quest);

		// get next
		GetEpic(quest.GetNextQuest(job), act, job);
	}
}