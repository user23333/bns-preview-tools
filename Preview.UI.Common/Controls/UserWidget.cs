using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Converters;

namespace Xylia.Preview.UI.Controls;
internal interface IUserWidget
{
	/// <summary>
	/// collection of child element
	/// All widget have child elements
	/// </summary>
	UIElementCollection Children { get; }
}

[ContentProperty("Children")]
public abstract class UserWidget : Control, IUserWidget
{
	#region Constructorss
	public UserWidget()
	{
		Children = new(this, this);
	}
	#endregion

	#region Children
	public UIElementCollection Children { get; init; }

	protected override IEnumerator LogicalChildren => Children.GetEnumerator();

	protected override int VisualChildrenCount => Children.Count;

	protected override Visual GetVisualChild(int index)
	{
		if (IsZStateDirty) { RecomputeZState(); }
		int visualIndex = _zLut != null ? _zLut[index] : index;
		return Children[visualIndex];
	}

	/// <summary>
	/// Finds an child element that has the provided identifier name.
	/// </summary>
	/// <param name="name">The name of the requested element.</param>
	/// <returns> The requested element. This can be null if no matching element was found.</returns>
	public T? GetChild<T>(string name) where T : FrameworkElement => FindName(Name + "_" + name) as T;
	#endregion

	#region Layout
	internal FLayoutData.Offset Offsets
	{
		get => LayoutData.GetOffsets(this);
		set => LayoutData.SetOffsets(this, value);
	}

	internal Vector ScrollOffset { get; set; }

	public float Top
	{
		get => Offsets.Top;
		set => Offsets = Offsets with { Top = value };
	}

	public float Left
	{
		get => Offsets.Left;
		set => Offsets = Offsets with { Left = value };
	}

	public float Bottom
	{
		get => Offsets.Top + (float)RenderSize.Height;
	}

	public float Right
	{
		get => Offsets.Left + (float)RenderSize.Width;
	}
	#endregion


	#region Protected Methods
	/// <summary>
	/// Update the current visual state of the control using transitions
	/// </summary>
	internal void UpdateVisualState()
	{
		ChangeVisualState(false);
	}

	/// <summary>
	///     Change to the correct visual state for the Control.
	/// </summary>
	/// <param name="useTransitions">
	///     true to use transitions when updating the visual state, false to
	///     snap directly to the new visual state.
	/// </param>
	internal virtual void ChangeVisualState(bool useTransitions)
	{
		if (Validation.GetHasError(this))
		{
			if (IsKeyboardFocused)
			{
				VisualStateManager.GoToState(this, VisualStates.StateInvalidFocused, useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, VisualStates.StateInvalidUnfocused, useTransitions);
			}
		}
		else
		{
			VisualStateManager.GoToState(this, VisualStates.StateValid, useTransitions);
		}
	}

	internal static void OnVisualStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		// Due to inherited properties, its safer not to cast to control because this might get fired for
		// non-controls.
		if (d is UserWidget control)
		{
			control.UpdateVisualState();
		}
	}


	/// <summary>
	/// Updates DesiredSize of the Widget. This is the first pass of layout.
	/// </summary>
	/// <param name="constraint">Constraint size is an "upper limit" that Widget should not exceed.</param>
	/// <returns>Widget' desired size.</returns>
	protected override Size MeasureOverride(Size constraint)
	{
		foreach (UIElement child in Children)
		{
			if (child == null) continue;

			// Compute size of the child
			double w, h;
			var anchor = LayoutData.GetAnchors(child);
			var offset = LayoutData.GetOffsets(child);

			// point
			if (anchor.Minimum.X == anchor.Maximum.X && anchor.Minimum.Y == anchor.Maximum.Y)
			{
				w = offset.Right;
				h = offset.Bottom;
			}
			// horizontal line
			else if (anchor.Minimum.Y == anchor.Maximum.Y)
			{
				//OffsetLeft, PosY, OffsetRight, SizeY
				w = constraint.Width * (anchor.Maximum.X - anchor.Minimum.X) - offset.Left - offset.Right;
				h = offset.Bottom;
			}
			// vertical line
			else if (anchor.Minimum.X == anchor.Maximum.X)
			{
				//PosX, OffsetTop, SizeX, OffsetBottom
				w = offset.Right;
				h = constraint.Height * (anchor.Maximum.Y - anchor.Minimum.Y) - offset.Top - offset.Bottom;
			}
			// area
			else
			{
				w = constraint.Width * (anchor.Maximum.X - anchor.Minimum.X);
				h = constraint.Height * (anchor.Maximum.Y - anchor.Minimum.Y);
			}


			// Measure widget if invalid value
			// NOTE: WPF only use result that less than or equal to input value in MeasureCore
			if (h == 0) h = double.PositiveInfinity;
			if (w == 0) w = double.PositiveInfinity;

			if (child is FrameworkElement fe)
			{
				h += fe.Margin.Top + fe.Margin.Bottom;
				w += fe.Margin.Left + fe.Margin.Right;
			}

			child.Measure(new Size(w, h));
		}

		// auto size
		var children = Children.OfType<UIElement>();
		if (double.IsInfinity(constraint.Width)) constraint.Width = children.Select(x => LayoutData.GetOffsets(x).Left + x.DesiredSize.Width).DefaultIfEmpty().Max();
		if (double.IsInfinity(constraint.Height)) constraint.Height = children.Select(x => LayoutData.GetOffsets(x).Top + x.DesiredSize.Height).DefaultIfEmpty().Max();

		return constraint;
	}

	/// <summary>
	/// Widget computes a position for each of its children taking into account their margin and attached Widget properties: Top, Offset.  
	/// Widget will also arrange each of its children.
	/// </summary>
	/// <param name="constraint">Size that Widget will assume to position children.</param>
	protected override Size ArrangeOverride(Size constraint)
	{
		foreach (UIElement child in Children)
		{
			if (child == null) continue;

			child.Arrange(ArrangeChild(child, constraint));
		}

		return constraint;
	}

	protected virtual Rect ArrangeChild(UIElement child, Size constraint)
	{
		var anchor = LayoutData.GetAnchors(child);
		var offset = LayoutData.GetOffsets(child);
		var alignments = LayoutData.GetAlignments(child);

		// Compute pos of the child	
		var x = anchor.Minimum.X * (constraint.Width - child.DesiredSize.Width) - (alignments.X * child.DesiredSize.Width) + offset.Left;
		var y = anchor.Minimum.Y * (constraint.Height - child.DesiredSize.Height) - (alignments.Y * child.DesiredSize.Height) + offset.Top;

		// extra support
		if (child is FrameworkElement fe)
		{
			x += fe.Margin.Left;
			y += fe.Margin.Top;
		}

		return new Rect(new Point(x, y), child.DesiredSize);
	}
	#endregion

	#region ZOrder Support
	private int[]? _zLut;                                //  look up table for converting from logical to visual indices
	private bool IsZStateDirty => _zLut is null || _zLut.Length != Children.Count;

	/// <summary>
	/// ZOrder property is an attached property. Panel reads it to alter the order
	/// of children rendering. Children with greater values will be rendered on top of
	/// children with lesser values.
	/// In case of two children with the same ZOrder property value, order of rendering
	/// is determined by their order in Panel.Children collection.
	/// </summary>
	public static readonly DependencyProperty ZOrderProperty = DependencyProperty.RegisterAttached("ZOrder",
		typeof(int), typeof(UserWidget), new FrameworkPropertyMetadata(0,
			new PropertyChangedCallback(OnZOrderPropertyChanged)));

	/// <summary>
	/// Helper for setting ZOrder property on a UIElement.
	/// </summary>
	/// <param name="element">UIElement to set ZOrder property on.</param>
	/// <param name="value">ZOrder property value.</param>
	public static void SetZOrder(UIElement element, int value)
	{
		ArgumentNullException.ThrowIfNull(element);
		element.SetValue(ZOrderProperty, value);
	}

	/// <summary>
	/// Helper for reading ZOrder property from a UIElement.
	/// </summary>
	/// <param name="element">UIElement to read ZOrder property from.</param>
	/// <returns>ZOrder property value.</returns>
	public static int GetZOrder(UIElement element)
	{
		ArgumentNullException.ThrowIfNull(element);
		return ((int)element.GetValue(ZOrderProperty));
	}

	/// <summary>
	/// <see cref="PropertyMetadata.PropertyChangedCallback"/>
	/// </summary>
	private static void OnZOrderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		int oldValue = (int)e.OldValue;
		int newValue = (int)e.NewValue;

		if (oldValue == newValue)
			return;

		if (VisualTreeHelper.GetParent(d) is UserWidget widget)
		{
			widget._zLut = null;
		}
	}

	private void RecomputeZState()
	{
		int count = (Children != null) ? Children.Count : 0;
		bool isDiverse = false;
		bool lutRequired = false;
		int consonant = (int)ZOrderProperty.DefaultMetadata.DefaultValue;
		System.Collections.Generic.List<Int64> stableKeyValues = null;

		if (count > 0)
		{
			if (Children![0] != null)
			{
				consonant = (int)Children[0].GetValue(ZOrderProperty);
			}

			if (count > 1)
			{
				stableKeyValues = new List<Int64>(count)
				{
					(Int64)consonant << 32
				};

				int prevZ = consonant;

				int i = 1;
				do
				{
					int z = Children[i] != null
						? (int)Children[i].GetValue(ZOrderProperty)
						: consonant;

					//  this way of calculating values of stableKeyValues required to
					//  1)  workaround the fact that Array.Sort is not stable (does not preserve the original
					//      order of elements if the keys are equal)
					//  2)  avoid O(N^2) performance of Array.Sort, which is QuickSort, which is known to become O(N^2)
					//      on sorting N eqial keys
					stableKeyValues.Add(((Int64)z << 32) + i);
					//  look-up-table is required iff z's are not monotonically increasing function of index.
					//  in other words if stableKeyValues[i] >= stableKeyValues[i-1] then calculated look-up-table
					//  is guaranteed to be degenerated...
					lutRequired |= z < prevZ;
					prevZ = z;

					isDiverse |= (z != consonant);
				} while (++i < count);
			}
		}

		if (lutRequired)
		{
			stableKeyValues.Sort();

			if (_zLut == null || _zLut.Length != count)
			{
				_zLut = new int[count];
			}

			for (int i = 0; i < count; ++i)
			{
				_zLut[i] = (int)(stableKeyValues[i] & 0xffffffff);
			}
		}
		else
		{
			_zLut = null;
		}
	}
	#endregion
}