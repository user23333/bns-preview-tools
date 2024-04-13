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
		if (FontColor != null)
		{
			var f = FontColor.FontColor.ToFColor(true);
			Foreground = new SolidColorBrush(Color.FromArgb(f.A, f.R, f.G, f.B));
		}

		var FontAttribute = fontset.FontAttribute?.Load<UFontAttribute>();
		if (FontAttribute != null)
		{
			var style = System.Drawing.FontStyle.Regular;

			if (FontAttribute.Italic) style |= System.Drawing.FontStyle.Italic;
			//if (FontAttribute.Shadow) style |= System.Drawing.FontStyle.Shadow;
			if (FontAttribute.Strokeout) style |= System.Drawing.FontStyle.Strikeout;
			if (FontAttribute.Underline) style |= System.Drawing.FontStyle.Underline;
		}

		var FontFace = fontset.FontFace?.Load<UBNSFontFace>();
		if (FontFace != null)
		{
			FontSize = FontFace.Height;
		}
	}
	#endregion
}