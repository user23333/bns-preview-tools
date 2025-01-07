namespace Xylia.Preview.Data.Models.Document;
public class P : HtmlElementNode
{
	public HorizontalAlignment HorizontalAlignment { get => GetAttributeValue<HorizontalAlignment>(); set => SetAttributeValue(value); }
	public VerticalAlignment VerticalAlignment { get => GetAttributeValue<VerticalAlignment>(); set => SetAttributeValue(value); }
	public HorizontalAlignment FirstLineHorizontalAlignment { get => GetAttributeValue<HorizontalAlignment>(); set => SetAttributeValue(value); }
	public JustificationType JustificationType { get => GetAttributeValue<JustificationType>(); set => SetAttributeValue(value); }

	public float RightMargin { get => GetAttributeValue<float>(); set => SetAttributeValue(value); }
	public float TopMargin { get => GetAttributeValue<float>(); set => SetAttributeValue(value); }
	public float LeftMargin { get => GetAttributeValue<float>(); set => SetAttributeValue(value); }
	public float BottomMargin { get => GetAttributeValue<float>(); set => SetAttributeValue(value); }


	//idt
	//spacebetweenlines
	//BackgroundImagePaddingX
	//BackgroundImagePaddingY
	//wordwrap
	public bool Justification { get => GetAttributeValue<bool>(); set => SetAttributeValue(value); }
	//disableparagraphbreak


	//bullethorizontalalignment
	public string bulletsfontset { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
	public string bulletsfontset2 { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
	public string Bullets { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }

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