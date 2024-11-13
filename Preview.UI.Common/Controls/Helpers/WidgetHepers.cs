using System.Windows;
using System.Windows.Controls;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.UI.Converters;

namespace Xylia.Preview.UI.Controls.Helpers;
public static class WidgetHepers
{
	public static T Add<T>(this UIElementCollection collection, T element, FLayoutData.Anchor? anchor = default, FLayoutData.Offset? offset = default) where T : UIElement
	{
		LayoutData.SetAnchors(element, anchor);
		LayoutData.SetOffsets(element, offset);

		collection.Add(element);
		return element;
	}
}