using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Xylia.Preview.UI.Extensions;
public static class ControlHelpers
{
	#region Common
	// UIElement.PreviousArrangeRect property is internal
	static readonly FieldInfo PreviousArrangeRect = typeof(UIElement).GetField("_finalRect", BindingFlags.NonPublic | BindingFlags.Instance)!;

	public static Rect GetFinalRect(this UIElement element)
	{
		ArgumentNullException.ThrowIfNull(element);
		return (Rect)PreviousArrangeRect.GetValue(element)!;
	}

	public static void SetFinalRect(this UIElement element, Rect value)
	{
		ArgumentNullException.ThrowIfNull(element);
		PreviousArrangeRect.SetValue(element, value);
	}

	public static T? GetParent<T>(this DependencyObject reference) where T : Visual
	{
		while (true)
		{
			reference = VisualTreeHelper.GetParent(reference);

			if (reference is null) return default;
			if (reference is T) return reference as T;
		}
	}

	public static void SetVisiable(this FrameworkElement element, bool visible)
	{
		element.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
	}

	public static byte[] Snapshot(this FrameworkElement element)
	{
		var bounds = new Rect(element.DesiredSize);
		element.Arrange(bounds);

		// If you need to create a bitmap with a specified resolution, you could directly
		// pass the specified dpiX and dpiY values to RenderTargetBitmap constructor.
		var bmp = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96d, 96d, PixelFormats.Pbgra32);
		bmp.Render(element);

		return bmp.AsData();
	}
	#endregion

	#region Property
	public static DependencyProperty Register<T>(this Type ownerType, string name, T? defaultValue = default,
		FrameworkPropertyMetadataOptions flags = FrameworkPropertyMetadataOptions.AffectsRender,
		PropertyChangedCallback? callback = null)
	{
		return Register<T>(ownerType, name, new FrameworkPropertyMetadata(defaultValue, flags, callback));
	}

	public static DependencyProperty Register<T>(this Type ownerType, string name, FrameworkPropertyMetadata metadata)
	{
		return DependencyProperty.Register(name, typeof(T), ownerType, metadata);
	}
	#endregion

	#region Valid
	/// <summary>
	/// Validate input value
	/// </summary>
	/// <param name="value"></param>
	/// <returns>Returns False if value is NaN or NegativeInfinity or PositiveInfinity. Otherwise, returns True.</returns>
	public static bool IsValidDoubleValue(object value)
	{
		double d = (double)value;

		return !(double.IsNaN(d) || double.IsInfinity(d));
	}

	/// <summary>
	/// Validate input value
	/// </summary>
	/// <param name="value"></param>
	/// <returns>Returns False if value is NaN or NegativeInfinity or PositiveInfinity or negative. Otherwise, returns True.</returns>
	public static bool IsValidChange(object value)
	{
		double d = (double)value;

		return IsValidDoubleValue(value) && d >= 0.0;
	}

	// A version of Object.Equals with paranoia for mismatched types, to avoid problems
	// with classes that implement Object.Equals poorly
	internal static bool EqualsEx(object o1, object o2)
	{
		try
		{
			return Object.Equals(o1, o2);
		}
		catch (System.InvalidCastException)
		{
			// A common programming error: the type of o1 overrides Equals(object o2)
			// but mistakenly assumes that o2 has the same type as o1:
			//     MyType x = (MyType)o2;
			// This throws InvalidCastException when o2 is a sentinel object,
			// e.g. UnsetValue, DisconnectedItem, NewItemPlaceholder, etc.
			// Rather than crash, just return false - the objects are clearly unequal.
			return false;
		}
	}
	#endregion
}