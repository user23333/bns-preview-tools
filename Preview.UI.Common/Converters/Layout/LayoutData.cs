using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.Core.Math;
using static CUE4Parse.BNS.Assets.Exports.FLayoutData;

namespace Xylia.Preview.UI.Converters;
public class LayoutData
{
	#region Public Properties
	/// <summary>
	/// This is the dependency property registered for the Widget' Offset attached property.
	/// </summary>
	public static readonly DependencyProperty OffsetsProperty = DependencyProperty.RegisterAttached("Offsets",
		typeof(Offset), typeof(LayoutData), new FrameworkPropertyMetadata((Offset)default,
			FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsRender));

	public static readonly DependencyProperty AnchorsProperty = DependencyProperty.RegisterAttached("Anchors",
		typeof(Anchor), typeof(LayoutData), new FrameworkPropertyMetadata((Anchor)default,
			FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsRender));

	public static readonly DependencyProperty AlignmentsProperty = DependencyProperty.RegisterAttached("Alignments",
		typeof(FVector2D), typeof(LayoutData), new FrameworkPropertyMetadata((FVector2D)default,
			FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.AffectsRender));
	#endregion

	#region Public Methods
	/// <summary>
	/// Reads the attached property Anchor from the given element.
	/// </summary>
	/// <param name="element">The element from which to read the Anchor attached property.</param>
	/// <returns>The property's value.</returns>
	/// <seealso cref="Anchor.AnchorProperty" />
	public static Anchor GetAnchors(UIElement element)
	{
		ArgumentNullException.ThrowIfNull(element);
		return (Anchor)element.GetValue(AnchorsProperty);
	}

	/// <summary>
	/// Writes the attached property Anchor to the given element.
	/// </summary>
	/// <param name="element">The element to which to write the Anchor attached property.</param>
	/// <param name="value">The Anchor to set</param>
	/// <seealso cref="Anchor.AnchorProperty" />
	public static void SetAnchors(UIElement element, Anchor? value)
	{
		ArgumentNullException.ThrowIfNull(element);
		if (value != null) element.SetValue(AnchorsProperty, value);
	}

	/// <summary>
	/// Reads the attached property Offset from the given element.
	/// </summary>
	/// <param name="element">The element from which to read the Offset attached property.</param>
	/// <returns>The property's value.</returns>
	/// <seealso cref="Anchor.OffsetProperty" />
	public static Offset GetOffsets(UIElement element)
	{
		ArgumentNullException.ThrowIfNull(element);
		return (Offset)element.GetValue(OffsetsProperty);
	}

	/// <summary>
	/// Writes the attached property Offset to the given element.
	/// </summary>
	/// <param name="element">The element to which to write the Offset attached property.</param>
	/// <param name="value">The offset to set</param>
	/// <seealso cref="Anchor.OffsetProperty" />
	public static void SetOffsets(UIElement element, Offset? value)
	{
		ArgumentNullException.ThrowIfNull(element);
		if (value != null) element.SetValue(OffsetsProperty, value);
	}

	/// <summary>
	/// Reads the attached property Alignment from the given element.
	/// </summary>
	/// <param name="element">The element from which to read the Alignment attached property.</param>
	/// <returns>The property's value.</returns>
	/// <seealso cref="AlignmentsProperty" />
	public static FVector2D GetAlignments(UIElement element)
	{
		ArgumentNullException.ThrowIfNull(element);
		return (FVector2D)element.GetValue(AlignmentsProperty);
	}

	/// <summary>
	/// Writes the attached property Alignment to the given element.
	/// </summary>
	/// <param name="element">The element to which to write the Alignment attached property.</param>
	/// <param name="value">The Alignment to set</param>
	/// <seealso cref="AlignmentsProperty" />
	public static void SetAlignments(UIElement element, FVector2D value)
	{
		ArgumentNullException.ThrowIfNull(element);
		element.SetValue(AlignmentsProperty, value);
	}


	internal static Point ComputeOffset(Size clientSize, FVector2D inkSize, EHorizontalAlignment ha = default, EVerticalAlignment va = default, FVector2D Padding = default, FVector2D Offset = default)
	{
		var offset = new Point
		{
			X = Padding.X + Offset.X + (ha switch
			{
				EHorizontalAlignment.HAlign_Right => clientSize.Width - inkSize.X,
				EHorizontalAlignment.HAlign_Center => (clientSize.Width - inkSize.X) * 0.5,
				_ => 0,
			}),

			Y = Padding.Y + Offset.Y + (va switch
			{
				EVerticalAlignment.VAlign_Bottom => clientSize.Height - inkSize.Y,
				EVerticalAlignment.VAlign_Center => (clientSize.Height - inkSize.Y) * 0.5,
				_ => 0,
			})
		};

		return offset;
	}
	#endregion
}