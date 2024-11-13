using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.Properties;
using REF = Xylia.Preview.Data.Common.DataStruct.Ref;

namespace Xylia.Preview.Data.Models;
public class MarketCategory2Group : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public int SortNo { get; set; }

	public bool Visible { get; set; }

	public Ref<Text> Name2 { get; set; }

	public virtual bool Filter(Record record) => true;

	public sealed class Favorite : MarketCategory2Group
	{
		private static HashSet<REF> _favorites;
		public static HashSet<REF> Favorites
		{
			get => Settings.Default.GetValue(ref _favorites, "Preview", "FavoriteItems") ?? [];
			set => Settings.Default.SetValue(_favorites = value, "Preview", "FavoriteItems");
		}

		public override bool Filter(Record record) => Favorites.Contains(record.PrimaryKey);
	}

	public sealed class WorldBoss : MarketCategory2Group
	{
		public override bool Filter(Record record)
		{
			return record.Attributes.Get<bool>("world-boss-auctionable");
		}
	}

	public sealed class MarketCategory2 : MarketCategory2Group
	{
		[Name("market-category-2")]
		public MarketCategory2Seq Marketcategory2 { get; set; }

		[Name("market-category-3-group")]
		public Ref<MarketCategory3Group>[] MarketCategory3Group { get; set; }

		public override bool Filter(Record record)
		{
			if (Marketcategory2 != record.Attributes.Get<MarketCategory2Seq>("market-category-2")) return false;

			return true;
		}
	}
	#endregion
}