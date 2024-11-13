using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class ItemBrand : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public long[] TransformItemByJob { get; set; }

	#endregion

	#region Methods
	public ItemBrandTooltip GetTooltip(ItemConditionType ConditionType)
	{
		return Provider.GetTable<ItemBrandTooltip>()[this.Id + ((long)ConditionType << 32)];
	}
	#endregion
}