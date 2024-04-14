using System.Windows;
using CUE4Parse.UE4.Objects.UObject;
using HtmlAgilityPack;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Documents.Primitives;

namespace Xylia.Preview.UI.Documents;
/// <summary>
/// Paragraph element 
/// </summary>
public class Paragraph : BaseElement, IMetaData
{
	#region Constructors
	public Paragraph()
	{
		this.Children = [];
	}

	internal Paragraph(string? InnerText, FPackageIndex? fontset = null) : this()
	{
		FontSet = fontset;
		UpdateString(InnerText);
	}
	#endregion

	#region Public Properties
	//bullethorizontalalignment
	public HorizontalAlignment HorizontalAlignment { get; set; }
	public VerticalAlignment VerticalAlignment { get; set; }
	//firstlinehorizontalalignment

	public JustificationTypeSeq JustificationType { get; set; }
	public enum JustificationTypeSeq
	{
		Default,
		LineFeedByWidgetArea,
		LineFeedByLineArea,
	}

	public float RightMargin { get; set; }
	public float TopMargin { get; set; }
	public float LeftMargin { get; set; }
	public float BottomMargin { get; set; }



	//idt
	//spacebetweenlines
	//BackgroundImagePaddingX
	//BackgroundImagePaddingY
	//wordwrap
	public bool Justification { get; set; }
	//disableparagraphbreak
	public string? bulletsfontset { get; set; }
	public string? bulletsfontset2 { get; set; }
	public string? Bullets { get; set; }
	//bulletimage
	//backgroundimagesetpath
	#endregion


	#region Protected Methods
	protected internal override void Load(HtmlNode node)
	{
		Children = TextContainer.Load(node.ChildNodes);

		TopMargin = node.GetAttributeValue("topmargin", 0f);
		LeftMargin = node.GetAttributeValue("leftmargin", 0f);
		RightMargin = node.GetAttributeValue("rightmargin", 0f);
		BottomMargin = node.GetAttributeValue("bottommargin", 0f);

		Justification = node.GetAttributeValue("justification", false);
		JustificationType = node.GetAttributeValue("justificationtype", JustificationTypeSeq.Default);
		HorizontalAlignment = node.GetAttributeValue("horizontalalignment", HorizontalAlignment.Left);
		VerticalAlignment = node.GetAttributeValue("verticalalignment", VerticalAlignment.Top);

		Bullets = node.Attributes["bullets"]?.Value;
		bulletsfontset = node.Attributes["bulletsfontset"]?.Value;

		if (Bullets != null) Children.Insert(0, new Font(bulletsfontset, new Run(Bullets)));
	}

	private void Load(string? InnerText, string? fontset = null)
	{
		if (InnerText is null) return;

		var doc = new HtmlDocument();
		doc.LoadHtml(InnerText.Replace("\n", "<br/>"));

		var elements = TextContainer.Load(doc.DocumentNode.ChildNodes);
		this.Children = fontset is null ? [.. elements] : [new Font(fontset, elements.ToArray())];
	}

	protected override Size MeasureCore(Size availableSize)
	{
		var size = base.MeasureCore(availableSize);
		size.Height += TopMargin + BottomMargin;
		size.Width += LeftMargin + RightMargin;

		return size;
	}

	public Vector ComputeAlignmentOffset(Size clientSize, Size inkSize)
	{
		var ha = this.HorizontalAlignment;
		var va = this.VerticalAlignment;

		Vector offset = new Vector();

		//this is to degenerate Stretch to Top-Left in case when clipping is about to occur
		//if we need it to be Center instead, simply remove these 2 ifs
		if (ha == HorizontalAlignment.Stretch && inkSize.Width > clientSize.Width)
		{
			ha = HorizontalAlignment.Left;
		}

		if (va == VerticalAlignment.Stretch && inkSize.Height > clientSize.Height)
		{
			va = VerticalAlignment.Top;
		}
		//end of degeneration of Stretch to Top-Left

		if (ha == HorizontalAlignment.Center || ha == HorizontalAlignment.Stretch)
		{
			offset.X = (clientSize.Width - inkSize.Width) * 0.5;
		}
		else if (ha == HorizontalAlignment.Right)
		{
			offset.X = clientSize.Width - inkSize.Width;
		}
		else
		{
			offset.X = 0;
		}

		if (va == VerticalAlignment.Center || va == VerticalAlignment.Stretch)
		{
			offset.Y = (clientSize.Height - inkSize.Height) * 0.5;
		}
		else if (va == VerticalAlignment.Bottom)
		{
			offset.Y = clientSize.Height - inkSize.Height;
		}
		else
		{
			offset.Y = 0;
		}

		return offset;
	}
	#endregion


	#region IMetaData
	internal FPackageIndex? FontSet;

	public void UpdateString(string? text)
	{
		this.Load(text, FontSet?.GetPathName());
	}

	public void UpdateTooltip(string? title)
	{
		//throw new System.NotImplementedException();
	}
	#endregion
}