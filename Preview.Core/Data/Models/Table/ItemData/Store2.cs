using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public sealed class Store2 : ModelElement, IHaveName
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<Text> Name2 { get; set; }

	public string Icon { get; set; }

	public string NoneSelectedIcon { get; set; }

	public Ref<Faction> Faction { get; set; }

	public Ref<Item>[] Item { get; set; }

	public Ref<ItemBuyPrice>[] BuyPrice { get; set; }
	#endregion

	#region Methods
	public string Name => Name2.GetText();
	#endregion
}