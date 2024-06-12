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
}