using System.Windows;
using HtmlAgilityPack;

namespace Xylia.Preview.UI.Documents;
public class BR : BaseElement
{
	public const string Tag = "<br/>";

	protected internal override void Load(HtmlNode node)
	{

	}

	protected override Size MeasureCore(Size availableSize) => new(0, FontSize);
}