namespace Xylia.Preview.Data.Models;
public sealed class RaidDungeon : ModelElement, IAttration
{
	#region IAttraction
	public string Name => this.Attributes["name2"].GetText();

	public string Description => this.Attributes["raid-dungeon-desc"].GetText();
	#endregion
}