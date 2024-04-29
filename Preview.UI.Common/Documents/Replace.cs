using System.Windows;
using HtmlAgilityPack;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Documents;
public class Replace : BaseElement
{
	#region Fields
	public string? P { get; set; }
	#endregion

	#region Methods
	protected internal override void Load(HtmlNode node)
	{
		this.P = node.GetAttributeValue("p", null);
	}

	protected override Size MeasureCore(Size availableSize)
	{
		Children = [new Paragraph(P.GetText())];
		return base.MeasureCore(availableSize);
	}
	#endregion
}