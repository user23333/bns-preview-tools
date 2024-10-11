using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class MarketCategory2Group : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public int SortNo { get; set; }

	public bool Visible { get; set; }

	public Ref<Text> Name2 { get; set; }

	public sealed class Favorite : MarketCategory2Group
	{
	}

	public sealed class WorldBoss : MarketCategory2Group
	{
	}

	public sealed class MarketCategory2 : MarketCategory2Group
	{
		public MarketCategory2Seq Marketcategory2 { get; set; }

		public Ref<MarketCategory3Group>[] MarketCategory3Group { get; set; }
	}
	#endregion
}