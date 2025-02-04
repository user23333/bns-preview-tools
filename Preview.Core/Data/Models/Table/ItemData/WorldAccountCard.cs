namespace Xylia.Preview.Data.Models;
public sealed class WorldAccountCard : ModelElement
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public Ref<Item> Item { get; set; }

	public bool Disabled { get; set; }

	public short SortNo { get; set; }

	public string CardImage { get; set; }

	public string[] CardOriginalImage { get; set; }

	public Ref<Text>[] CardOriginalImageDesc { get; set; }

	public bool SetCardOriginalImage { get; set; }

	public bool SpecialEffect { get; set; }

	public short Season { get; set; }

	public Ref<WorldAccountExpedition>[] Expedition { get; set; }
	#endregion

	#region Methods
	public IEnumerable<WorldAccountCardCollection> CanUseCollection => Provider.GetTable<WorldAccountCardCollection>().Where(x => x.CollectionCard.Any(c => c == this));
	#endregion
}