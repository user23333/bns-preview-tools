using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Xylia.Preview.UI.Controls.Primitives;

namespace Xylia.Preview.UI.Controls.Helpers;
public class WidgetCollection(UserWidget parent) : UIElementCollection(parent, parent)
{
	public override IEnumerator GetEnumerator()
	{
		IEnumerator itor = base.GetEnumerator();

		// sort to implement widget link method
		List<object> elements = [];
		while (itor.MoveNext()) elements.Add(itor.Current);

		itor.Reset();
		while (itor.MoveNext())
		{
			if (itor.Current is BnsCustomBaseWidget widget)
			{
				var index = elements.IndexOf(widget);

				ChangeIndex(elements, index, parent.GetChild<UIElement>(widget.HorizontalResizeLink?.LinkWidgetName1, true));
				ChangeIndex(elements, index, parent.GetChild<UIElement>(widget.HorizontalResizeLink?.LinkWidgetName2, true));
				ChangeIndex(elements, index, parent.GetChild<UIElement>(widget.VerticalResizeLink?.LinkWidgetName1, true));
				ChangeIndex(elements, index, parent.GetChild<UIElement>(widget.VerticalResizeLink?.LinkWidgetName2, true));
			}
		}

		return elements.GetEnumerator();
	}

	static void ChangeIndex(List<object> elements, int index, object? element)
	{
		if (element is null) return;

		var index2 = elements.IndexOf(element);
		if (index < index2)
		{
			elements.RemoveAt(index2);
			elements.Insert(index, element);
		}
	}
}