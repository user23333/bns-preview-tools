using OfficeOpenXml;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal sealed class ChallengeListOut : OutSet
{
	protected override void CreateData(ExcelPackage package)
	{
		#region Title
		var sheet = CreateSheet(package);
		int column = 1, row = 1;
		sheet.SetColumn(column++, "Sun", 40);
		sheet.SetColumn(column++, "Mon", 40);
		sheet.SetColumn(column++, "Tue", 40);
		sheet.SetColumn(column++, "Wed", 40);
		sheet.SetColumn(column++, "Thu", 40);
		sheet.SetColumn(column++, "Fri", 40);
		sheet.SetColumn(column++, "Sat", 40);
		#endregion

		column = 0;
		foreach (var record in Source!.Provider.GetTable<ChallengeList>().Where(x =>
			x.ChallengeType > ChallengeList.ChallengeTypeSeq.None && 
			x.ChallengeType <= ChallengeList.ChallengeTypeSeq.Sat))
		{
			column++;
			sheet.Cells[row = 2, column].SetValue(record.ChallengeType);

			// ChallengeQuestBasic
			for (int i = 0; i < record.ChallengeQuestBasic.Length; i++)
			{
				var quest = record.ChallengeQuestBasic[i].Instance;
				if (quest is null) continue;

				var grade = record.ChallengeQuestGrade[i];
				var attraction = record.ChallengeQuestAttraction[i].Instance;
				var expansion = record.ChallengeQuestExpansion[i].Instance;

				sheet.Cells[row++, column].SetValue(GradeText(grade) + quest.Name);
			}

			// ChallengeNpcKill
			for (int i = 0; i < record.ChallengeNpcKill.Length; i++)
			{
				var npc = record.ChallengeNpcKill[i].Instance;
				if (npc is null) continue;

				var grade = record.ChallengeNpcGrade[i];
				var difficulty = record.ChallengeNpcDifficulty[i];
				var attraction = record.ChallengeNpcAttraction[i].Instance;
				var quest = record.ChallengeNpcQuest[i];

				sheet.Cells[row++, column].SetValue(GradeText(grade) + $"{difficulty} {npc.Name2.GetText()}");
			}

			// Reward
			for (int i = 0; i < record.Reward.Length; i++)
			{
				var reward = record.Reward[i].Instance;
				if (reward is null) continue;

				sheet.Cells[row++, column].SetValue(
					"UI.ChallengeToday.TomorrowQuestCountGuide".GetText([null, null, record.ChallengeCountForReward[i]]) + "\n" +
					reward);
			}
		}
	}

	private static string GradeText(ChallengeList.Grade grade) => grade switch
	{
		ChallengeList.Grade.Grade1 => "1☆ ",
		ChallengeList.Grade.Grade2 => "2☆ ",
		ChallengeList.Grade.Grade3 => "3☆ ",
		_ => string.Empty
	};
}