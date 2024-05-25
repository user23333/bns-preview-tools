using System.Windows;
using System.Windows.Media;
using CUE4Parse.BNS.Assets.Exports;
using HtmlAgilityPack;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.UI.Documents.Primitives;

namespace Xylia.Preview.UI.Documents;
public class Font : BaseElement
{
	#region Constructors
	public Font()
	{

	}

	internal Font(string Name, params BaseElement[] elements)
	{
		this.Name = Name;
		this.Children = [.. elements];
	}
	#endregion

	#region Public Properties
	/// <summary>
	/// fontset path
	/// </summary>
	public string? Name;


	public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register("TextDecorations",
		typeof(TextDecorationCollection), typeof(Font), new FrameworkPropertyMetadata(new TextDecorationCollection(),
			FrameworkPropertyMetadataOptions.AffectsRender));

	public TextDecorationCollection TextDecorations
	{
		get => (TextDecorationCollection) GetValue(TextDecorationsProperty);
		set => SetValue(TextDecorationsProperty, value);
	}
	#endregion


	#region Override Methods
	protected internal override void Load(HtmlNode node)
	{
		Children = TextContainer.Load(node.ChildNodes);
		Name = node.Attributes["name"]?.Value;
	}

	protected override Size MeasureCore(Size availableSize)
	{
		GetFont(FileCache.Provider.LoadObject<UFontSet>(Name));
		return base.MeasureCore(availableSize);
	}
	#endregion

	#region Private Methods
	private void GetFont(UFontSet fontset)
	{
		if (fontset is null) return;

		var FontColor = fontset.FontColors?.Load<UFontColor>();
		if (FontColor != null && !SkipColor())
		{
			var f = FontColor.FontColor.ToFColor(true);
			Foreground = new SolidColorBrush(Color.FromArgb(f.A, f.R, f.G, f.B));
		}

		var FontAttribute = fontset.FontAttribute?.Load<UFontAttribute>();
		if (FontAttribute != null)
		{
			if (FontAttribute.Italic) FontStyle = FontStyles.Italic;
			//if (FontAttribute.Shadow) ;
			if (FontAttribute.Strokeout) TextDecorations = System.Windows.TextDecorations.Strikethrough;
			if (FontAttribute.Underline) TextDecorations = System.Windows.TextDecorations.Underline;
		}

		var FontFace = fontset.FontFace?.Load<UBNSFontFace>();
		if (FontFace != null)
		{
			FontSize = FontFace.Height;
		}
	}

	/// <summary>
	/// Indicates whether to follow the application theme color
	/// </summary>
	/// <returns></returns>
	private bool SkipColor()
	{
		return Name
			is "00008130.Program.Fontset_ItemGrade_1"
			or "00008130.Program.Fontset_ItemGrade_2"
			or "00008130.UI.Normal_12";
	}
	#endregion
}