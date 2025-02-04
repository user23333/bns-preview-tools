namespace Xylia.Preview.Data.Models.Document;
public class P : HtmlElementNode
{
	//bulletimage
	//backgroundimagesetpath

	public HorizontalAlignment HorizontalAlignment;
	public VerticalAlignment VerticalAlignment;
	public HorizontalAlignment FirstLineHorizontalAlignment;
	public JustificationType JustificationType;

	public float RightMargin;
	public float TopMargin;
	public float LeftMargin;
	public float BottomMargin;

	//idt
	//spacebetweenlines
	//BackgroundImagePaddingX
	//BackgroundImagePaddingY
	//wordwrap
	public bool Justification;

	public bool DisableParagraphBreak;

	public HorizontalAlignment BulletHorizontalAlignment;
	public string BulletsFontset;
	public string BulletsFontset2;
	public string Bullets;

	//bulletimage
	//backgroundimagesetpath
}


public enum HorizontalAlignment
{
	Left,
	Center,
	Right,
	Stretch
}

public enum VerticalAlignment
{
	Top,
	Center,
	Bottom,
	Stretch
}

public enum JustificationType
{
	Default,
	LineFeedByWidgetArea,
	LineFeedByLineArea,
}