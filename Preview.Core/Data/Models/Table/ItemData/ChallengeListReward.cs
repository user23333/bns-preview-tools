using System.Text;

namespace Xylia.Preview.Data.Models;
public class ChallengeListReward : ModelElement 
{
	#region Attributes
	public Ref<Item>[] RewardItem { get; set; }

	public short[] RewardItemCount { get; set; }

	public int RewardMoney { get; set; }

	public int RewarAccountExp { get; set; }
	#endregion


	#region Methods
	public override string ToString()
	{
		var builder = new StringBuilder();

		for (int i = 0; i < RewardItem.Length; i++)
		{
			var item = RewardItem[i].Instance;
			if (item is null) continue;

			builder.Append($"\n{item.Name} x{RewardItemCount[i]}");
		}

		if (RewardMoney > 0) builder.Append("Money: " + RewardMoney);
		if (RewarAccountExp > 0) builder.Append("AExp: " + RewarAccountExp);

		return builder.ToString();
	}
	#endregion
}