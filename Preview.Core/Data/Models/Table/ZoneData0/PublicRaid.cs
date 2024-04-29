namespace Xylia.Preview.Data.Models;
public sealed class PublicRaid : ModelElement, IAttration
{
	#region IAttraction
	public string Name => this.Attributes["publicraid-name2"].GetText();

	public string Description => this.Attributes["publicraid-desc"].GetText();
	#endregion
}