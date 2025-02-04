using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.Properties;
using static Xylia.Preview.Data.Models.Reward;

namespace Xylia.Preview.Data.Models;
public class SmartDropReward : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<Item>[] Item { get; set; }

	public sbyte ItemTotalCount { get; set; }

	public short CommonPouchDropRate { get; set; }

	public short PersonalPouchDropRate { get; set; }
	#endregion

	#region Methods
	public IEnumerable<RewardInfo> GetRewards(string group, string text)
	{
		// Warning: if no item for the job, a random item will be selected.
		JobSeq[] job = [Settings.Default.Job];
		var data = new List<RewardInfo>();

		// deal with drop rate
		var items = Item.Values().Where(x =>
		{
			foreach (var j in job)
			{
				if (x.EquipJobCheck.CheckSeq(j)) return true;
			}

			return false;
		}).ToArray();

		return items.Select(item => new RewardInfo()
		{
			Data = item,
			Probability = 1,
			ProbabilityType = items.Length,
			Group = group,
			GroupText = text,
		});
	}
	#endregion
}