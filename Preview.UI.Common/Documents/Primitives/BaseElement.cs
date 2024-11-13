using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Controls.Primitives;
using Xylia.Preview.UI.Documents.Primitives;

namespace Xylia.Preview.UI.Documents;
[ContentProperty("Children")]
public abstract class BaseElement : ContentElement
{
	#region Public Properties
	/// <summary>
	/// child element collection
	/// </summary>
	protected internal List<BaseElement> Children { get; set; } = [];

	public Size DesiredSize { get; protected set; }

	public Rect FinalRect { get; private set; }
	#endregion

	#region Dependency Properties
	/// <summary>
	/// DependencyProperty for <see cref="FontFamily" /> property.
	/// </summary>
	public static readonly DependencyProperty FontFamilyProperty = Control.FontFamilyProperty.AddOwner(typeof(BaseElement),
		new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily,
			FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

	/// <summary>
	/// The FontFamily property specifies the name of font family.
	/// </summary>
	[Localizability(LocalizationCategory.Font, Modifiability = Modifiability.Unmodifiable)]
	public FontFamily FontFamily
	{
		get { return (FontFamily)GetValue(FontFamilyProperty); }
		set { SetValue(FontFamilyProperty, value); }
	}

	/// <summary>
	/// DependencyProperty for <see cref="FontStyle" /> property.
	/// </summary>
	public static readonly DependencyProperty FontStyleProperty = Control.FontStyleProperty.AddOwner(typeof(BaseElement),
		new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle,
			FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

	/// <summary>
	/// The FontStyle property requests normal, italic, and oblique faces within a font family.
	/// </summary>
	public FontStyle FontStyle
	{
		get { return (FontStyle)GetValue(FontStyleProperty); }
		set { SetValue(FontStyleProperty, value); }
	}

	/// <summary>
	/// DependencyProperty for <see cref="FontWeight" /> property.
	/// </summary>
	public static readonly DependencyProperty FontWeightProperty = Control.FontWeightProperty.AddOwner(typeof(BaseElement),
		new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight,
			FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

	/// <summary>
	/// The FontWeight property specifies the weight of the font.
	/// </summary>
	public FontWeight FontWeight
	{
		get { return (FontWeight)GetValue(FontWeightProperty); }
		set { SetValue(FontWeightProperty, value); }
	}

	/// <summary>
	/// DependencyProperty for <see cref="FontStretch" /> property.
	/// </summary>
	public static readonly DependencyProperty FontStretchProperty = Control.FontStretchProperty.AddOwner(typeof(BaseElement),
		new FrameworkPropertyMetadata(FontStretches.Normal,
			FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

	/// <summary>
	/// The FontStretch property selects a normal, condensed, or extended face from a font family.
	/// </summary>
	public FontStretch FontStretch
	{
		get { return (FontStretch)GetValue(FontStretchProperty); }
		set { SetValue(FontStretchProperty, value); }
	}

	/// <summary>
	/// DependencyProperty for <see cref="FontSize" /> property.
	/// </summary>
	public static readonly DependencyProperty FontSizeProperty = Control.FontSizeProperty.AddOwner(typeof(BaseElement),
		new FrameworkPropertyMetadata(SystemFonts.MessageFontSize,
			FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

	/// <summary>
	/// The FontSize property specifies the size of the font.
	/// </summary>
	[TypeConverter(typeof(FontSizeConverter))]
	[Localizability(LocalizationCategory.None)]
	public double FontSize
	{
		get { return (double)GetValue(FontSizeProperty); }
		set { SetValue(FontSizeProperty, value); }
	}

	/// <summary>
	/// DependencyProperty for <see cref="Foreground" /> property.
	/// </summary>
	public static readonly DependencyProperty ForegroundProperty = Control.ForegroundProperty.AddOwner(typeof(BaseElement),
		new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
			FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender | FrameworkPropertyMetadataOptions.Inherits));

	/// <summary>
	/// The Foreground property specifies the foreground brush of an element's text content.
	/// </summary>
	public Brush Foreground
	{
		get { return (Brush)GetValue(ForegroundProperty); }
		set { SetValue(ForegroundProperty, value); }
	}


	public static readonly DependencyProperty StringProperty = BnsCustomBaseWidget.StringProperty.AddOwner(typeof(BaseElement),
		new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

	public StringProperty String
	{
		get { return (StringProperty)GetValue(StringProperty); }
		set { SetValue(StringProperty, value); }
	}

	public static readonly DependencyProperty ArgumentsProperty = BnsCustomLabelWidget.ArgumentsProperty.AddOwner(typeof(BaseElement),
		new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

	public TextArguments Arguments
	{
		get { return (TextArguments)GetValue(ArgumentsProperty); }
		set { SetValue(ArgumentsProperty, value); }
	}


	public static readonly DependencyProperty TimersProperty = BnsCustomLabelWidget.TimersProperty.AddOwner(typeof(BaseElement),
		new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

	public IDictionary<int, Time64> Timers
	{
		get { return (IDictionary<int, Time64>)GetValue(TimersProperty); }
		set { SetValue(TimersProperty, value); }
	}
	#endregion


	#region Protected Methods
	/// <summary>
	/// Notification that a specified property has been invalidated
	/// </summary>
	/// <param name="e">EventArgs that contains the property, metadata, old value, and new value for this change</param>
	protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
	{
		// Always call base.OnPropertyChanged, otherwise Property Engine will not work.
		base.OnPropertyChanged(e);

		bool IsValueChange = e.NewValue != e.OldValue;

		// If the modified property affects layout we have some additional
		// bookkeeping to take care of.
		if (e.Property.GetMetadata(e.Property.OwnerType) is FrameworkPropertyMetadata fmetadata)
		{
			bool affectsMeasureOrArrange = fmetadata.AffectsMeasure || fmetadata.AffectsArrange || fmetadata.AffectsParentMeasure || fmetadata.AffectsParentArrange;
			bool affectsRender = fmetadata.AffectsRender && (IsValueChange || !fmetadata.SubPropertiesDoNotAffectRender);
			if (affectsMeasureOrArrange || affectsRender)
			{
				var textContainer = EnsureTextContainer();
				if (textContainer != null)
				{
					textContainer.BeginChange();
					textContainer.EndChange();
				}
			}
		}
	}

	/// <summary>
	/// Initialize element from html
	/// </summary>
	protected internal virtual void Load(HtmlNode node)
	{
		// attribute
		foreach (var prop in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
		{
			if (prop.HasAttribute<IgnoreDataMemberAttribute>()) continue;

			var type = prop.FieldType;
			var name = (prop.GetAttribute<NameAttribute>()?.Name ?? prop.Name).ToLower();
			var value = node.GetAttributeValue(name, type, null);

			prop.SetValue(this, value);
		}

		// child
		LoadChildren(node);
	}

	/// <summary>
	/// Initialize children of element
	/// </summary>
	protected internal void LoadChildren(HtmlNode node)
	{
		Children.Clear();
		if (HtmlNode.IsClosedElement(node.Name)) return;

		foreach (var child in node.ChildNodes)
		{
			Children.Add(ElementLoad.CreateElement(child));
		}
	}

	/// <summary>
	/// Determine if use new line.
	/// </summary>
	protected internal virtual bool NewLine() => false;


	public Size Measure(Size availableSize)
	{
		if (availableSize.Width == 0) availableSize.Width = double.PositiveInfinity;
		if (availableSize.Height == 0) availableSize.Height = double.PositiveInfinity;

		return DesiredSize = MeasureCore(availableSize);
	}

	public void Arrange(Rect finalRect)
	{
		FinalRect = ArrangeCore(finalRect);
	}

	/// <summary>
	/// Measurement override. Implement your size-to-content logic here.
	/// </summary>
	/// <param name="availableSize"></param>
	/// <returns></returns>
	protected virtual Size MeasureCore(Size availableSize)
	{
		Size size = new Size();
		Size lineSize = new Size();

		foreach (var element in Children)
		{
			InheritDependency(this, element);
			Size desiredSize = element.Measure(availableSize);

			// new line
			if (element.NewLine() || (!double.IsInfinity(availableSize.Width) && lineSize.Width + desiredSize.Width > availableSize.Width))
			{
				size.Width = Math.Max(lineSize.Width, size.Width);
				size.Height += lineSize.Height;
				lineSize = desiredSize;
			}
			else
			{
				lineSize.Width += desiredSize.Width;
				lineSize.Height = Math.Max(desiredSize.Height, lineSize.Height);
			}
		}

		// full panel size
		size.Width = Math.Ceiling(Math.Max(lineSize.Width, size.Width));
		size.Height = Math.Ceiling(size.Height + lineSize.Height);
		return size;
	}

	/// <summary>
	/// ArrangeCore allows for the customization of the final sizing and positioning of children.
	/// </summary>
	/// <param name="finalRect"></param>
	/// <returns></returns>
	protected virtual Rect ArrangeCore(Rect finalRect)
	{
		#region Split
		if (Children is null) return finalRect;

		// start element in current line 
		int firstInLine = 0;
		Size lineSize = new Size();

		List<BaseElement[]> lines = [];
		for (int i = 0; i < Children.Count; i++)
		{
			var element = Children[i];
			Size desiredSize = element.DesiredSize;

			if (element.NewLine() || lineSize.Width + desiredSize.Width > finalRect.Width)  //New
			{
				lines.Add([.. Children.GetRange(firstInLine, i - firstInLine)]);
				lineSize = desiredSize;

				// single line if control width gather than line width
				if (desiredSize.Width > finalRect.Width)
				{
					lines.Add([.. Children.GetRange(i, 1)]);
					lineSize = new Size();
				}

				firstInLine = i;
			}
			else if (element.Children.LastOrDefault() is BR)  //End
			{
				lines.Add([.. Children.GetRange(firstInLine, i - firstInLine + 1)]);
				lineSize = desiredSize;
				firstInLine = i + 1;
			}
			else
			{
				lineSize.Width += desiredSize.Width;
				lineSize.Height = Math.Max(desiredSize.Height, lineSize.Height);
			}

			if (i + 1 == Children.Count && firstInLine != Children.Count)
			{
				lines.Add([.. Children.GetRange(firstInLine, i - firstInLine + 1)]);
			}
		}
		#endregion

		#region Arrange
		double y = finalRect.Y;

		foreach (var line in lines)
		{
			if (line.Length == 0) continue;

			var width = line.Sum(x => x.DesiredSize.Width);
			var height = line.Max(x => x.DesiredSize.Height);

			// draw start point
			double x = finalRect.X;

			// paragraph alignment
			if (this is P p)
			{
				var vect = p.ComputeAlignmentOffset(finalRect.Size, new Size(width, height));
				x += vect.X;
			}

			// arrange children
			foreach (var element in line)
			{
				element.Arrange(new Rect(new Point(x, y), element.DesiredSize));
				x += element.DesiredSize.Width;
			}

			y += height;
		}
		#endregion

		return finalRect;
	}

	protected internal virtual void OnRender(DrawingContext ctx)
	{
		Children?.ForEach(x => x.OnRender(ctx));
	}
	#endregion

	#region Internal Methods
	/// <summary>
	/// Inherits implement at FrameworkElement
	/// https://source.dot.net/#PresentationFramework/System/Windows/FrameworkElement.cs,1913
	/// </summary>
	internal static void InheritDependency(DependencyObject parent, DependencyObject current)
	{
		current.SetValue(StringProperty, parent.GetValue(StringProperty));
		current.SetValue(FontFamilyProperty, parent.GetValue(FontFamilyProperty));
		current.SetValue(FontStyleProperty, parent.GetValue(FontStyleProperty));
		current.SetValue(FontWeightProperty, parent.GetValue(FontWeightProperty));
		current.SetValue(FontStretchProperty, parent.GetValue(FontStretchProperty));
		current.SetValue(FontSizeProperty, parent.GetValue(FontSizeProperty));
		current.SetValue(ForegroundProperty, parent.GetValue(ForegroundProperty));
		current.SetValue(FontFamilyProperty, parent.GetValue(FontFamilyProperty));
		current.SetValue(ArgumentsProperty, parent.GetValue(ArgumentsProperty));
		current.SetValue(TimersProperty, parent.GetValue(TimersProperty));
		current.SetValue(Font.TextDecorationsProperty, parent.GetValue(Font.TextDecorationsProperty));
	}

	internal virtual IInputElement? InputHitTest(Point point)
	{
		IInputElement? element = null;

		if (Children is null) return element;

		// only Link need respond
		foreach (var child in Children)
		{
			if (child.FinalRect.Contains(point))
			{
				if (child is Link) return child;
			}

			element = child.InputHitTest(point);
		}

		return element;
	}

	internal TextContainer? EnsureTextContainer() => null;
	#endregion
}

public abstract class BaseElement<T> : BaseElement where T : HtmlElementNode, new()
{
	public T Element { get; set; } = new();

	protected internal override void Load(HtmlNode node)
	{
		base.Load(node);
		Element = (T)node;
	}
}


internal static class ElementLoad
{
	private static readonly Dictionary<string, Type> _classes = new(StringComparer.OrdinalIgnoreCase);
	private static readonly Type _run = typeof(Run);

	static ElementLoad()
	{
		var baseType = typeof(BaseElement);

		foreach (var definedType in Assembly.GetExecutingAssembly().GetTypes())
		{
			if (!definedType.IsAbstract &&
				!definedType.IsInterface &&
				baseType.IsAssignableFrom(definedType))
				_classes[definedType.Name.ToLower()] = definedType;
		}
	}

	internal static BaseElement CreateElement(HtmlNode node)
	{
		var type = _classes.GetValueOrDefault(node.Name, _run);
		var element = (BaseElement)Activator.CreateInstance(type)!;
		element.Load(node);

		return element;
	}
}