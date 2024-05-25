using HtmlAgilityPack;

namespace Xylia.Preview.UI.Documents;
public class SelFont : BaseElement
{
	#region Fields
	public string? Name1 { get; set; }
	public string? Name2 { get; set; }
	public string? P { get; set; }
	#endregion

	#region Methods
	protected internal override void Load(HtmlNode node)
	{
		P = node.GetAttributeValue("p", null);
		Name1 = node.GetAttributeValue("name-1", null);
		Name2 = node.GetAttributeValue("name-2", null);
	}
	#endregion
}