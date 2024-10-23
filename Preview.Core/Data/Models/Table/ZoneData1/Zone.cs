using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public class Zone : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public enum ZoneType2Seq
	{
		None,
		Persistent,
		Single,
		Instant,
	}

	public ZoneType2Seq ZoneType2 { get; set; }


	public short Terrain { get; set; }

	public short Region { get; set; }

	public Ref<MapInfo> Map { get; set; }

	public Ref<MapArea> Area { get; set; }
	#endregion
}

public interface IAttraction : IHaveName
{
	string Description { get; }

	Ref<AttractionRewardSummary> RewardSummary { get; }
}