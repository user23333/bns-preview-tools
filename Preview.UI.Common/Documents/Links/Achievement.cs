namespace Xylia.Preview.UI.Documents.Links;
public sealed class Achievement : LinkId
{
	public string alias;

	#region Methods
	internal override void Load(string text)
	{
		// achievement:291_event_SoulEvent_Extreme_0004_step1:123.3.0.1.1.1.626f57f5.1.0.0.0.1
		var data = text.Split('.');

		alias = data[0];
	}
	#endregion
}