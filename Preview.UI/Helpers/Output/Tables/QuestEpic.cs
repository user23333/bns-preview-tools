using OfficeOpenXml;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.UI.Helpers.Output.Tables;
internal sealed class QuestEpic : OutSet
{
    protected override void CreateData(ExcelWorksheet sheet)
    {
        #region Title
        sheet.SetColumn(Column++, "id", 10);
        sheet.SetColumn(Column++, "alias", 15);
        sheet.SetColumn(Column++, "name", 30);
        sheet.SetColumn(Column++, "group", 25);
        #endregion

        GetEpic(data =>
        {
            Row++;
            int column = 1;

            sheet.Cells[Row, column++].SetValue(data.PrimaryKey);
            sheet.Cells[Row, column++].SetValue(data);
            sheet.Cells[Row, column++].SetValue(data.Text);
            sheet.Cells[Row, column++].SetValue(data.Title);
        });
    }


    public static void GetEpic(Action<Quest> act, JobSeq TargetJob = JobSeq.소환사) => GetEpic(FileCache.Data.Provider.GetTable<Quest>()["q_epic_221"], act, TargetJob);

    public static void GetEpic(Quest quest, Action<Quest> act, JobSeq TargetJob = JobSeq.소환사)
    {
        if (quest is null) return;

        // act
        act(quest);

        // get next
        var Completion = quest.Completion?.FirstOrDefault();
        if (Completion is null) return;
        foreach (var NextQuest in Completion.NextQuest)
        {
            var jobs = NextQuest.Job;
            if (jobs is null || jobs[0] == JobSeq.JobNone || jobs.FirstOrDefault(job => job == TargetJob) != JobSeq.JobNone)
                GetEpic(NextQuest.Quest.Instance, act, TargetJob);
        }
    }
}