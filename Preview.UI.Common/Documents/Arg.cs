using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using HtmlAgilityPack;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Documents;
public class Arg : BaseElement
{
	#region Fields
	public string? P { get; set; }
	public string? Id { get; set; }
	public string? Seq { get; set; }
	public bool Link { get; set; }

	TextArguments.Argument? Argument;
	#endregion


	#region Methods
	protected internal override void Load(HtmlNode node)
	{
		P = node.GetAttributeValue("p", null);
		Id = node.GetAttributeValue("id", null);
		Seq = node.GetAttributeValue("seq", null);
		Link = node.GetAttributeValue("link", false);
		Argument = new TextArguments.Argument(P, Id, Seq);
	}

	protected override Size MeasureCore(Size availableSize)
	{
		this.Children = [];

		var result = Argument!.GetObject(Arguments);
		if (result is null) return new Size();
		else if (result is ImageProperty property) Children.Add(new Image(property));
		else if (result is int @int) Children.Add(new Run() { Text = @int.ToString("N0") });
		else if (result is not null) Children.Add(new Paragraph(result.ToString()));

		return base.MeasureCore(availableSize);
	}
	#endregion
}