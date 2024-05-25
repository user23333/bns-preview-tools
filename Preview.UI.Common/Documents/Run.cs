using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using HtmlAgilityPack;

namespace Xylia.Preview.UI.Documents;
/// <summary>
/// A terminal element in text flow hierarchy - contains a uniformatted run of unicode characters
/// </summary>
[ContentProperty("Text")]
public class Run : BaseElement
{
	#region Constructors
	/// <summary>
	/// Initializes an instance of Run class.
	/// </summary>
	public Run()
	{

	}

	/// <summary>
	/// Initializes an instance of Run class specifying its text content.
	/// </summary>
	/// <param name="text">Text content assigned to the Run.</param>
	public Run(string text)
	{
		Text = text;
	}
	#endregion

	#region Properties
	public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Run),
		  new FrameworkPropertyMetadata(string.Empty,
			   FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

	public string Text
	{
		get { return (string)GetValue(TextProperty); }
		set { SetValue(TextProperty, value); }
	}


	internal static readonly DependencyProperty TextDecorationsProperty = Font.TextDecorationsProperty.AddOwner(typeof(Run),
		   new FrameworkPropertyMetadata(new TextDecorationCollection(),
			   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

	internal TextDecorationCollection TextDecorations
	{
		get => (TextDecorationCollection)GetValue(TextDecorationsProperty);
		set => SetValue(TextDecorationsProperty, value);
	}
	#endregion

	#region Methods
	protected internal override void Load(HtmlNode node)
	{
		Text = WebUtility.HtmlDecode(node.OuterHtml);
	}

	protected override Size MeasureCore(Size availableSize)
	{
		_format = new FormattedText(Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
			new(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Foreground, 96)
		{
			MaxTextWidth = double.IsInfinity(availableSize.Width) ? 0 : availableSize.Width,
			//TextAlignment = this.String?.HorizontalAlignment switch
			//{
			//	HAlignment.HAlign_Center => TextAlignment.Center,
			//	HAlignment.HAlign_Right => TextAlignment.Right,
			//	_ => TextAlignment.Left,
			//}
		};

		_format.SetTextDecorations(TextDecorations);

		return new Size(_format.WidthIncludingTrailingWhitespace, _format.Height);
	}

	protected internal override void OnRender(DrawingContext ctx)
	{
		// Draw the formatted text string to the DrawingContext of the control.
		if (_format != null) ctx.DrawText(_format, FinalRect.TopLeft);

		// WARNING: the point is related to TextAlignment, I don't know how to switch to absolute coordinates
		// temporary logic in ArrangeCore, alignment of wrapping line may exist issue.
	}

	public override string ToString()
	{
		return GetType() + $" text:{Text}";
	}
	#endregion


	#region Private Fields
	private FormattedText? _format;
	#endregion
}