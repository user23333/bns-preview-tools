using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Controls.Primitives;
using Xylia.Preview.UI.Converters;

namespace Xylia.Preview.UI.Controls;
public class BnsCustomColumnListWidget : BnsCustomBaseWidget
{
	#region Constructors  		
	static BnsCustomColumnListWidget()
	{
		// Register PropertyTypeMetadata
		ColumnDefinition.WidthProperty.OverrideMetadata(Owner, new FrameworkPropertyMetadata(new PropertyChangedCallback(OnUserSizePropertyChanged)));
		RowDefinition.HeightProperty.OverrideMetadata(Owner, new FrameworkPropertyMetadata(new PropertyChangedCallback(OnUserSizePropertyChanged)));
	}
	#endregion

	#region Properties & Events
	private static readonly Type Owner = typeof(BnsCustomColumnListWidget);

	public static readonly DependencyProperty RowProperty = DependencyProperty.RegisterAttached("Row",
		typeof(int), Owner, new FrameworkPropertyMetadata(-1,
			FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange));

	public static readonly DependencyProperty ColumnProperty = DependencyProperty.RegisterAttached("Column",
		typeof(int), Owner, new FrameworkPropertyMetadata(-1,
			FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange));

	public static readonly DependencyProperty RowSpanProperty = DependencyProperty.RegisterAttached("RowSpan",
		typeof(int), Owner, new FrameworkPropertyMetadata(1,
			FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange));

	public static readonly DependencyProperty ColumnSpanProperty = DependencyProperty.RegisterAttached("ColumnSpan",
		typeof(int), Owner, new FrameworkPropertyMetadata(1,
			FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsParentArrange));

	public List<RowDefinition> RowDefinitions { get; } = [];

	public List<ColumnDefinition> ColumnDefinitions { get; } = [];


	public static readonly DependencyProperty ShowLinesProperty = DependencyProperty.Register("ShowLines",
		typeof(bool), Owner, new FrameworkPropertyMetadata(BooleanBoxes.FalseBox,
			FrameworkPropertyMetadataOptions.AffectsRender));

	public bool ShowLines
	{
		get => (bool)GetValue(ShowLinesProperty);
		set => SetValue(ShowLinesProperty, BooleanBoxes.Box(value));
	}


	public event EventHandler<MergeEventArgs>? CellMerge;
	#endregion

	#region Dependency Helpers
	public static int GetRow(UIElement element)
	{
		ArgumentNullException.ThrowIfNull(element);
		return (int)element.GetValue(RowProperty);
	}

	public static void SetRow(UIElement element, int value)
	{
		ArgumentNullException.ThrowIfNull(element);
		element.SetValue(RowProperty, value);
	}

	public static int GetColumn(UIElement element)
	{
		ArgumentNullException.ThrowIfNull(element);
		return (int)element.GetValue(ColumnProperty);
	}

	public static void SetColumn(UIElement element, int value)
	{
		ArgumentNullException.ThrowIfNull(element);
		element.SetValue(ColumnProperty, value);
	}

	public static int GetRowSpan(UIElement element)
	{
		ArgumentNullException.ThrowIfNull(element);
		return (int)element.GetValue(RowSpanProperty);
	}

	public static void SetRowSpan(UIElement element, int value)
	{
		ArgumentNullException.ThrowIfNull(element);
		element.SetValue(RowSpanProperty, value);
	}

	public static int GetColumnSpan(UIElement element)
	{
		ArgumentNullException.ThrowIfNull(element);
		return (int)element.GetValue(ColumnSpanProperty);
	}

	public static void SetColumnSpan(UIElement element, int value)
	{
		ArgumentNullException.ThrowIfNull(element);
		element.SetValue(ColumnSpanProperty, value);
	}
	#endregion


	#region Override Methods
	protected override Size MeasureOverride(Size constraint)
	{
		var children = Children.OfType<UIElement>();
		if (children.Any())
		{
			CheckDefinition(children);

			if (CellMerge != null)
			{
				// Compare cell one by one, and merge them if the callback value is true
				var cols = children.GroupBy(GetColumn);
				foreach (var group in cols) CheckMerge(group.GetEnumerator());
			}
		}

		// measure
		if (double.IsInfinity(constraint.Width)) constraint.Width = ColumnDefinitions.Sum(d => d.Width.Value);
		if (double.IsInfinity(constraint.Height)) constraint.Height = RowDefinitions.Sum(d => d.Height.Value);
		return base.MeasureOverride(constraint);
	}

	protected override Size ArrangeOverride(Size constraint)
	{
		var offsetU = GetOffset(ColumnDefinitions.ToArray(), constraint.Width, false);
		var offsetV = GetOffset(RowDefinitions.ToArray(), constraint.Height, true);

		foreach (UIElement child in Children)
		{
			if (child == null) continue;

			var column = GetColumn(child);
			var row = GetRow(child);
			var columnspan = GetColumnSpan(child);
			var rowspan = GetRowSpan(child);

			// base layout
			if (column == -1 && row == -1)
			{
				child.Arrange(ArrangeChild(child, constraint));
			}
			else
			{
				var c1 = offsetU.ElementAtOrDefault(column);
				var r1 = offsetV.ElementAtOrDefault(row);
				var c2 = offsetU.ElementAtOrDefault(column + columnspan - 1);
				var r2 = offsetV.ElementAtOrDefault(row + rowspan - 1);

				var rect = new Rect(c1.X, r1.X,
					c2.X + c2.Y - c1.X,
					r2.X + r2.Y - r1.X);

				child.Arrange(rect);
				LayoutData.SetFinalRect(child, rect);
			}
		}

		return constraint;
	}

	protected override void OnRender(DrawingContext ctx)
	{
		base.OnRender(ctx);

		if (this.ShowLines)
		{
			// outer border
			ctx.DrawRectangle(new SolidColorBrush(), new Pen(Foreground, 2),
				 new Rect(new Point(-ScrollOffset.X, -ScrollOffset.Y), DesiredSize));

			// Draw border using arrange data
			foreach (UIElement child in Children)
			{
				if (child == null) continue;

				var rect = LayoutData.GetFinalRect(child);
				if (rect != default)
				{
					rect.X -= ScrollOffset.X;
					rect.Y -= ScrollOffset.Y;
					ctx.DrawRectangle(new SolidColorBrush(), new Pen(Foreground, 1), rect);
				}
			}
		}
	}
	#endregion

	#region Private Methods
	private void CheckDefinition(IEnumerable<UIElement> elements)
	{
		var rows = elements.Max(GetRow) + 1 - RowDefinitions.Count;
		if (rows < 0) RowDefinitions.RemoveRange(RowDefinitions.Count + rows, -rows);
		else for (int i = 0; i < rows; i++) RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30) });
	}

	private void CheckMerge(IEnumerator<UIElement> enumerator)
	{
		UIElement? prev = null;

		while (enumerator.MoveNext())
		{
			var current = enumerator.Current;
			if (current.Visibility != Visibility.Visible) continue;

			if (prev != null)
			{
				var arg = new MergeEventArgs(prev, current);
				CellMerge.Invoke(this, arg);

				if (arg.Handled)
				{
					SetRowSpan(prev, GetRowSpan(prev) + 1);
					current.Visibility = Visibility.Collapsed;
					continue;
				}
			}

			prev = current;
		}
	}


	/// <summary>
	/// Calculates and sets offset for all definitions in the given array.
	/// </summary>
	/// <param name="definitions">Array of definitions to process.</param>
	/// <param name="finalSize">Final size to lay out to.</param>
	/// <param name="columns">True if sizing row definitions, false for columns</param>
	/// <returns></returns>
	private Vector[] GetOffset(DefinitionBase[] definitions, double finalSize, bool columns)
	{
		var sizes = SetFinalSizeLegacy(definitions, finalSize, columns);
		var vects = new Vector[definitions.Length];

		double offset = 0;
		for (int i = 0; i < sizes.Length; i++)
		{
			var size = sizes[i];

			vects[i] = new Vector(offset, size);
			offset += size;
		}

		return vects;
	}

	/// <summary>
	/// Calculates and sets final size for all definitions in the given array.
	/// </summary>
	/// <param name="definitions">Array of definitions to process.</param>
	/// <param name="finalSize">Final size to lay out to.</param>
	/// <param name="columns">True if sizing row definitions, false for columns</param>
	private double[] SetFinalSizeLegacy(DefinitionBase[] definitions, double finalSize, bool columns)
	{
		double allStarWeights = 0;
		var useLayoutRounding = this.UseLayoutRounding;
		var sizes = new double[definitions.Length];

		// If using layout rounding, check whether rounding needs to compensate for high DPI
		//double dpi = 1.0;

		//if (useLayoutRounding)
		//{
		//	DpiScale dpiScale = GetDpi();
		//	dpi = columns ? dpiScale.DpiScaleX : dpiScale.DpiScaleY;
		//	roundingErrors = RoundingErrors;
		//}

		for (int i = 0; i < definitions.Length; i++)
		{
			//  if definition is shared then is cannot be star

			var definition = definitions[i];
			var size = definition is ColumnDefinition c ? c.Width :
				definition is RowDefinition r ? r.Height :
				throw new NotSupportedException();

			if (size.IsAbsolute)
			{
				sizes[i] = size.Value;
			}
			else if (size.IsAuto)
			{

			}
			else if (size.IsStar)
			{
				sizes[i] = -size.Value;
				allStarWeights += size.Value;
			}
		}

		if (allStarWeights > 0)
		{
			var resolvedSize = (finalSize - sizes.Where(s => s > 0).Sum()) / allStarWeights;

			for (int i = 0; i < sizes.Length; i++)
			{
				var size = sizes[i];
				if (size < 0) sizes[i] = -size * resolvedSize;
			}
		}

		return sizes;
	}

	/// <summary>
	/// <see cref="PropertyMetadata.PropertyChangedCallback"/>
	/// </summary>
	/// <remarks>
	/// This method needs to be internal to be accessable from derived classes.
	/// </remarks>
	private static void OnUserSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		DefinitionBase definition = (DefinitionBase)d;

		var parent = (FrameworkElement)definition.Parent;
		parent.InvalidateMeasure();
	}
	#endregion
}

public class MergeEventArgs(object item1, object item2) : EventArgs
{
	public object Item1 { get; init; } = item1;

	public object Item2 { get; init; } = item2;

	/// <summary>
	/// Gets or sets whether the event has been handled.
	/// </summary>
	public bool Handled { get; set; }
}