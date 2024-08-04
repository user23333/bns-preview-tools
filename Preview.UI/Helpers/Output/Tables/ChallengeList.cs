using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal sealed class ChallengeListOut : OutSet
{
	protected override void CreateData()
	{
		#region Title
		var sheet = CreateSheet();
		//sheet.SetColumn(Column++, "任务序号", 10);
		//sheet.SetColumn(Column++, "任务别名", 15);
		//sheet.SetColumn(Column++, "任务名称", 30);
		//sheet.SetColumn(Column++, "group", 25);
		//sheet.SetColumn(Column++, "category", 10);
		//sheet.SetColumn(Column++, "content-type", 10);
		//sheet.SetColumn(Column++, "reset-type", 10);
		//sheet.SetColumn(Column++, "retired", 10);
		//sheet.SetColumn(Column++, "tutorial", 10);
		#endregion

		int column = 0, row = 1;
		foreach (var record in Source.Provider.GetTable<ChallengeList>().Where(x =>
			x.ChallengeType >= ChallengeList.ChallengeTypeSeq.Mon &&
			x.ChallengeType <= ChallengeList.ChallengeTypeSeq.Sat))
		{
			column++;
			sheet.Cells[row = 1, column].SetValue(record.ChallengeType);


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

				var count = record.ChallengeCountForReward[i];
				sheet.Cells[row++, column].SetValue($"完成{count}个可获得 {reward}");
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