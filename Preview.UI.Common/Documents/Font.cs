using System.Windows;
using System.Windows.Media;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Models.Document;

namespace Xylia.Preview.UI.Documents;
public class Font : BaseElement<Data.Models.Document.Font>
{
	#region Constructors
	public Font()
	{

	}

	internal Font(string name, params BaseElement[] elements)
	{
		this.Name = name;
		this.Children = [.. elements];
	}
	#endregion

	#region Public Properties
	public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name",
		typeof(string), typeof(Font), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender, OnFontChanged));

	public string Name
	{
		get => (string)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}

	internal static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register("TextDecorations",
		typeof(TextDecorationCollection), typeof(Font), new FrameworkPropertyMetadata(new TextDecorationCollection(),
			FrameworkPropertyMetadataOptions.AffectsRender));

	internal TextDecorationCollection TextDecorations
	{
		get => (TextDecorationCollection)GetValue(TextDecorationsProperty);
		set => SetValue(TextDecorationsProperty, value);
	}
	#endregion

	#region Override Methods
	protected internal override void Load(HtmlNode node)
	{
		Name = node.GetAttributeValue<string>("name");
		LoadChildren(node);
	}
	#endregion

	#region Private Methods
	private static void OnFontChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (e.OldValue == e.NewValue) return;

		var font = (Font)d;
		font.OnFontChanged(Globals.GameProvider.LoadObject<UFontSet>((string)e.NewValue));
	}

	private void OnFontChanged(UFontSet fontset)
	{
		if (fontset is null) return;

		var FontFace = fontset.FontFace?.Load<UBNSFontFace>();
		if (FontFace != null)
		{
			FontSize = FontFace.Height;
		}

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
			if (FontAttribute.Strokeout) TextDecorations = System.Windows.TextDecorations.Strikethrough;
			if (FontAttribute.Underline) TextDecorations = System.Windows.TextDecorations.Underline;
		}

		// why miss attribute Strokeout 
		if (Name == "00008130.UI.Vital_LightGray_ShaStroke") TextDecorations = System.Windows.TextDecorations.Strikethrough;
	}

	/// <summary>
	/// Indicates whether to follow the application theme color
	/// </summary>
	private bool SkipColor()
	{
		if (Name is null) return false;

		return Name.Contains("Normal_", StringComparison.OrdinalIgnoreCase) || Name
			is "00008130.Program.Fontset_ItemGrade_1"
			or "00008130.Program.Fontset_ItemGrade_2";
	}
	#endregion
}