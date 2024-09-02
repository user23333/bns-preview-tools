namespace Xylia.Preview.UI.Documents.Links;
public class Glyph : LinkId
{
	public string? alias;

	internal override void Load(string text)
	{
		alias = text;
	}
}