namespace Xylia.Preview.UI.Documents.Links;
public class Emoticon : LinkId
{
	public string? alias;

	internal override void Load(string text)
	{
		alias = text;
	}
}