﻿using System.Windows;
using System.Windows.Controls;
using Xylia.Preview.UI.Converters;
using static CUE4Parse.UE4.Objects.Core.FLayout;

namespace Xylia.Preview.UI.Controls.Helpers;
public static class WidgetHelpers
{
	public static T Add<T>(this UIElementCollection collection, T element, Anchor? anchor = default, Offset? offset = default) where T : UIElement
	{
		LayoutData.SetAnchors(element, anchor);
		LayoutData.SetOffsets(element, offset);

		collection.Add(element);
		return element;
	}

	internal static bool HasDefaultValue(DependencyObject d, DependencyProperty dp)
	{
		return dp.GetMetadata(d).DefaultValue != null;
	}
}