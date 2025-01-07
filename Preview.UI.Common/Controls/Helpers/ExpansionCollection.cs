using System.Collections.ObjectModel;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.UI.Controls.Primitives;

namespace Xylia.Preview.UI.Controls.Helpers;
public class ExpansionCollection : ObservableCollection<UBnsCustomExpansionComponent>
{
	#region Constructors
	public ExpansionCollection(BnsCustomBaseWidget owner)
	{
		Owner = owner;
		CollectionChanged += (s, e) => owner.InvalidateVisual();
	}

	internal ExpansionCollection(BnsCustomBaseWidget owner, IEnumerable<UBnsCustomExpansionComponent> collections) : this(owner)
	{
		// use cloned instead of themselves
		foreach (var component in collections)
		{
			this.Add(component.Clone());
		}
	}
	#endregion

	#region Protected Methods
	protected override void InsertItem(int index, UBnsCustomExpansionComponent item)
	{
		base.InsertItem(index, item);
		dic[item.ExpansionName.Text] = item;
		Owner.InvalidateVisual();
	}

	protected override void RemoveItem(int index)
	{
		var removedItem = this[index];

		base.RemoveItem(index);
		dic.Remove(removedItem.ExpansionName.Text);
		Owner.InvalidateVisual();
	}

	protected override void SetItem(int index, UBnsCustomExpansionComponent item)
	{
		var originalItem = this[index];
		base.SetItem(index, item);

		dic.Remove(originalItem.ExpansionName.Text);
		dic[item.ExpansionName.Text] = item;
		Owner.InvalidateVisual();
	}

	protected override void ClearItems()
	{
		base.ClearItems();
		dic.Clear();
		Owner.InvalidateVisual();
	}

	public UBnsCustomExpansionComponent? this[string name] => dic!.GetValueOrDefault(name);
	#endregion

	#region Private Fields
	private readonly BnsCustomBaseWidget Owner;
	private readonly Dictionary<string, UBnsCustomExpansionComponent> dic = [];
	#endregion
}