using System.Diagnostics;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class SoulBoostSeason : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public Ref<SoulBoostEvent> SoulBoostEvent { get; set; }

	public sbyte MaxPcCount { get; set; }

	public TimeUniversal StartTime { get; set; }

	public TimeUniversal EndTime { get; set; }

	public TimeUniversal ResultExpirationTime { get; set; }

	public sbyte SeasonSlotId { get; set; }

	public string SeasonName { get; set; }

	public string SeasonNameEn { get; set; }

	public string SeasonNameFr { get; set; }

	public string SeasonNameDe { get; set; }

	public string SeasonNamePt { get; set; }

	public string SeasonNameTh { get; set; }

	public string SeasonNameVn { get; set; }

	public Ref<Text> SeasonNameText { get; set; }

	public string SeasonBannerImageRef { get; set; }

	public bool IsBattlePass { get; set; }

	public sbyte RequiredLevel { get; set; }

	public sbyte RequiredMasteryLevel { get; set; }

	public Ref<Quest>[] RequiredPrecedingQuest { get; set; }

	public Ref<Item>[] PurchaseGradeItem { get; set; }
	#endregion

	#region Methods
	public void TestMethod()
	{
		SoulBoostEvent Event = this.SoulBoostEvent;

		foreach (var MissionStep in Event.MissionStep.Values().OrderBy(x => x.StepNumber))
		{
			Debug.WriteLine(MissionStep.StepNumber);

			foreach (var MissionTask in MissionStep.MissionTask.Values())
			{
				Debug.WriteLine($"{MissionTask.MissionPoint}  {MissionTask.NameText.GetText()}");
			}
		}

		for (int i = 0; i < Event.GradePoint.Length; i++)
		{
			var GradePoint = Event.GradePoint[i];
			var GradeReward = Event.GradeReward[i].Instance;
			var BmGradeReward = Event.BmGradeReward[i].Instance;
			if (GradePoint == 0) break;

			Debug.WriteLine($"grade {i + 1}");

			LinqExtensions.Create(GradeReward?.Item, GradeReward?.ItemCount)
				.Where(x => x.Item1.HasValue)
				.ForEach(x => Debug.WriteLine($" {x.Item1.Instance.Name} {x.Item2}"));

			LinqExtensions.Create(BmGradeReward?.Item, BmGradeReward?.ItemCount)
				.Where(x => x.Item1.HasValue)
				.ForEach(x => Debug.WriteLine($" {x.Item1.Instance.Name} {x.Item2}"));
		}
	}
	#endregion
}