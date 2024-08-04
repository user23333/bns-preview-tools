using System.Diagnostics;
using System.Windows.Input;
using Xylia.Preview.Data.Models.Document;

namespace Xylia.Preview.UI.Documents;
public class Link : BaseElement<Data.Models.Document.Link>
{
	#region Fields
	public bool IgnoreInput;
	public bool Editable;
	public LinkId? Id { get; set; }
	#endregion

	protected internal override void Load(HtmlNode node)
	{
		base.Load(node);

		var id = Element.Id;
		if (string.IsNullOrWhiteSpace(id) || id == "none") return;

		// split
		var tmp = id.Split(':', 2);
		var type = tmp[0]?.Trim();
		switch (type)
		{
			case "item": Id = new Links.Item(); break;
			case "item-name": Id = new Links.ItemName(); break;
			case "tooltip": Id = new Links.Tooltip(); break;

			default: Debug.WriteLine($"link type `{type}` not supported!"); return;
		}

		Id.Load(tmp[1]);

		// events
		this.MouseEnter += Id.OnMouseEnter;
		this.MouseLeave += Id.OnMouseLeave;
		this.MouseLeftButtonDown += Id.OnMouseLeftButtonDown;
		this.MouseRightButtonDown += Id.OnMouseRightButtonDown;
	}
}

public abstract class LinkId
{
	internal abstract void Load(string text);

	internal virtual void OnMouseEnter(object sender, MouseEventArgs e)
	{

	}

	internal virtual void OnMouseLeave(object sender, MouseEventArgs e)
	{

	}

	internal virtual void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		Debug.WriteLine("MouseLeftButtonDown: " + this);
	}

	internal virtual void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
	{

	}
}