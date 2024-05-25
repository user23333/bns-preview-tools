using OfficeOpenXml;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
public sealed class QuestEpic : OutSet
{
	protected override void CreateData(ExcelWorksheet sheet)
	{
		#region Title
		sheet.SetColumn(Column++, "id", 10);
		sheet.SetColumn(Column++, "alias", 15);
		sheet.SetColumn(Column++, "name", 30);
		sheet.SetColumn(Column++, "group", 25);
		#endregion

		#region Data
		GetEpic(data =>
		{
			Row++;
			int column = 1;

			sheet.Cells[Row, column++].SetValue(data.PrimaryKey);
			sheet.Cells[Row, column++].SetValue(data);
			sheet.Cells[Row, column++].SetValue(data.Name);
			sheet.Cells[Row, column++].SetValue(data.Title);
#if DEBUG
			var rewards = data.MissionStep.SelectMany(step => step.Mission.SelectMany(mission => mission.Reward)).Where(reward => reward.HasValue);
			sheet.Cells[Row, column++].SetValue(rewards.Sum(reward => reward.Instance.BasicExp));
#endif
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