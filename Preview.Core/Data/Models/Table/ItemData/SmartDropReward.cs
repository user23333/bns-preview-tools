using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.Properties;

namespace Xylia.Preview.Data.Models;
/// <summary>
/// SmartDropReward 
/// </summary>
/// <remarks>
/// Only get one of the items. drop-rate will increase the probability of player job.
/// </remarks>
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
	public static implicit operator Item(SmartDropReward reward) => reward.GetItem(Settings.Default.Job);

	public Item GetItem(params JobSeq[] job)
	{
		if (job.Length == 0) job = [JobSeq.JobNone];

		// No need to deal with drop-rate in our project.
		var items = Item.Values().Where(x =>
		{
			foreach (var j in job)
			{
				if (x.EquipJobCheck.CheckSeq(j)) return true;
			}

			return false;
		});

		// Only get one of the items.
		return items.FirstOrDefault();
	}
	#endregion
}