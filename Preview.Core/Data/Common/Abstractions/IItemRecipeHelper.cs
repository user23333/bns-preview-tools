using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Common.Abstractions;
internal interface IItemRecipeHelper
{
	IEnumerable<ItemRecipeHelper> CreateRecipe();
}

/// <summary>
/// Provides statistical recipe information
/// </summary>
public class ItemRecipeHelper
{
	#region Fields
	public Item MainItem { get; set; }

	public short MainItemCount { get; set; }

	public Item[] SubItem { get; set; }

	public short[] SubItemCount { get; set; }

	public Integer Money { get; set; }

	/// <summary>
	/// range 0 - 1000
	/// the client does not display probability information in CN
	/// </summary>
	public short SuccessProbability { get; set; }
	#endregion

	#region Properies
	public static float DiscountRate = 0.2F;

	public Tuple<Item, short>[] SubItemList => LinqExtensions.Create(SubItem, SubItemCount);

	public string Price => Money.Money;

	public string DiscountPrice => new Integer(Money * (1 - DiscountRate)).Money;

	public string Guide { get; internal set; }
	#endregion


	#region Methods
	public override string ToString()
	{
		return string.Format("{0} {1} {2}",
			Money.Money,
			MainItem?.ItemName + MainItemCount,
			string.Join(",", SubItemList.Select(x => x.Item1.ItemName + x.Item2)));
	}
	#endregion
}