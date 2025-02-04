using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
/// <summary>
/// Provides statistical recipe information
/// </summary>
public class RecipeHelper
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

    #region Properties
    public static float DiscountRate = 0.2F;

    public Tuple<Item, short>[] SubItemList => LinqExtensions.Tuple(SubItem, SubItemCount);

    public string Price => Money.Money;

    public string DiscountPrice => new Integer((int)(Money * (1 - DiscountRate))).Money;

    public string Guide { get; internal set; }
    #endregion


    #region Methods
    public override string ToString()
    {
        return string.Format("{0} {1} {2}",
            Money.Money,
            MainItem?.ItemNameOnly + MainItemCount,
            string.Join(",", SubItemList.Where(x => x.Item1 != null).Select(x => x.Item1.ItemNameOnly + x.Item2)));
    }
    #endregion
}

internal interface IRecipeHelper
{
    IEnumerable<RecipeHelper> GetRecipes();
}