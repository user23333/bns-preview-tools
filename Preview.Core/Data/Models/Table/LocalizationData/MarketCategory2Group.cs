using System.Collections;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.Properties;

namespace Xylia.Preview.Data.Models;
public abstract class MarketCategory2Group : ModelElement, IHaveName
{
	#region Attributes
	public int Id { get; set; }

	public int SortNo { get; set; }

	public bool Visible { get; set; }

	public Ref<Text> Name2 { get; set; }

	public sealed class Favorite : MarketCategory2Group
	{
		private static HashSet<Common.DataStruct.Ref> _favorites;
		public static HashSet<Common.DataStruct.Ref> Favorites
		{
			get => Settings.Default.GetValue(ref _favorites, "Preview_FavoriteItems") ?? [];
			set => Settings.Default.SetValue(_favorites = value, "Preview_FavoriteItems");
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

	public sealed class MarketCategory2 : MarketCategory2Group, IEnumerable
	{
		[Name("market-category-2")] public MarketCategory2Seq Marketcategory2 { get; set; }
		[Name("market-category-3-group")] public Ref<MarketCategory3Group>[] MarketCategory3Group { get; set; }

		public override bool Filter(Record record)
		{
			if (Marketcategory2 != record.Attributes.Get<MarketCategory2Seq>("market-category-2")) return false;

			return true;
		}

		IEnumerator IEnumerable.GetEnumerator() => MarketCategory3Group.Values().GetEnumerator();
	}

	public sealed class All : MarketCategory2Group
	{

	}
	#endregion

	#region Methods
	private string _name;
	public string Name { get => _name ?? Name2.GetText(); set => _name = value; }

	public virtual bool Filter(Record record) => true;
	#endregion
}