using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Documents.Primitives;
using HorizontalAlignment = Xylia.Preview.Data.Models.Document.HorizontalAlignment;
using VerticalAlignment = Xylia.Preview.Data.Models.Document.VerticalAlignment;

namespace Xylia.Preview.UI.Documents;
/// <summary>
/// Paragraph element 
/// </summary>
public class P : BaseElement<Data.Models.Document.P>, IMetaData
{
	#region Constructors
	public P()
	{
		
	}

	internal P(string? InnerText, FPackageIndex? fontset = null) : this()
	{
		Element = new();
		UpdateString(new StringProperty() { LabelText = InnerText, fontset = fontset });
	}
	#endregion

	#region Protected Methods
	protected internal override void Load(HtmlNode node)
	{
		base.Load(node);
		ArgumentNullException.ThrowIfNull(Element);

		if (Element.Bullets != null)
			Children.Insert(0, new Font(Element.bulletsfontset, [new Run(Element.Bullets)]));
	}

	protected override Size MeasureCore(Size availableSize)
	{
		var size = base.MeasureCore(availableSize);

		if (Element != null)
		{
			size.Height += Element.TopMargin + Element.BottomMargin;
			size.Width += Element.LeftMargin + Element.RightMargin;
		}

		return size;
	}

	public Vector ComputeAlignmentOffset(Size clientSize, Size inkSize)
	{
		var ha = Element?.HorizontalAlignment ?? default;
		var va = Element?.VerticalAlignment ?? default;

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
	public void UpdateString(StringProperty property)
	{
		Children.Clear();

		var text = property.LabelText?.Text;
		if (text is null) return;

		var doc = new HtmlDocument();
		doc.LoadHtml(text);

		var elements = TextContainer.Load(doc.DocumentNode.ChildNodes);
		Children = property.fontset is null ? elements : [new Font(property.fontset.GetPathName(), elements)];
	}

	public void UpdateTooltip(StringProperty property)
	{
		//throw new System.NotImplementedException();
	}
	#endregion
}