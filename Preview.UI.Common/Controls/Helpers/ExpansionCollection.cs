using System.Collections.ObjectModel;
using CUE4Parse.BNS.Assets.Exports;

namespace Xylia.Preview.UI.Controls.Helpers;
public class ExpansionCollection : Collection<ExpansionComponent>
{
	#region Constructors
	public ExpansionCollection()
	{

	}

	internal ExpansionCollection(IEnumerable<ExpansionComponent> collections)
	{
		// use cloned instead of themselves
		foreach (var component in collections)
		{
			this.Add(component.Clone());
		}
	}
	#endregion

	#region Protected Methods
	protected override void InsertItem(int index, ExpansionComponent item)
	{
		base.InsertItem(index, item);
		dic[item.ExpansionName.Text] = item;
	}

	protected override void RemoveItem(int index)
	{
		var removedItem = this[index];

		base.RemoveItem(index);
		dic.Remove(removedItem.ExpansionName.Text);
	}

	protected override void SetItem(int index, ExpansionComponent item)
	{
		var originalItem = this[index];
		base.SetItem(index, item);

		dic.Remove(originalItem.ExpansionName.Text);
		dic[item.ExpansionName.Text] = item;
	}

	protected override void ClearItems()
	{
		base.ClearItems();
		dic.Clear();
	}

	public ExpansionComponent? this[string name] => dic!.GetValueOrDefault(name);
	#endregion

	#region Private Fields
	private readonly Dictionary<string, ExpansionComponent> dic = [];
	#endregion
}