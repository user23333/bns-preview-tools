namespace Xylia.Preview.Data.Models;
public sealed class FactionBattleFieldZone : ModelElement, IAttration
{
	#region IAttraction
	public string Name => this.Attributes["faction-battle-field-zone-name2"].GetText();

	public string Description => this.Attributes["faction-battle-field-zone-desc"].GetText();
	#endregion
}