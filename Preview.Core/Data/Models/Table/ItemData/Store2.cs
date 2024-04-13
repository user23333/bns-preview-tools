namespace Xylia.Preview.Data.Models;
public sealed class Store2 : ModelElement
{
	#region Attributes
	public Ref<Text> Name2 { get; set; }

	public string Icon { get; set; }

	public string NoneSelectedIcon { get; set; }

	public Ref<Faction> Faction { get; set; }

	public Ref<Item>[] Item { get; set; }

	public Ref<ItemBuyPrice>[] BuyPrice { get; set; }
	#endregion
}