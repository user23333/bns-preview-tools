namespace Xylia.Preview.UI.Documents.Links;
public class Skill : LinkId
{
	public string alias;

	#region Methods
	internal override void Load(string text)
	{
		alias = text;
	}
	#endregion
}