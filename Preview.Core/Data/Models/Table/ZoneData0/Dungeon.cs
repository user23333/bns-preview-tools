namespace Xylia.Preview.Data.Models;
public sealed class Dungeon : ModelElement, IAttration
{
	#region IAttraction
	public string Name => this.Attributes["dungeon-name2"].GetText();

	public string Description => this.Attributes["dungeon-desc"].GetText();
	#endregion
}