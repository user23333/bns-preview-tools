namespace Xylia.Preview.Data.Models;
public sealed class PartyBattleFieldZone : ModelElement, IAttration
{
	#region Attributes
	public enum PartyBattleFieldZoneType
	{
		None,
		OccupationWar,
		CaptureTheFlag,
		LeadTheBall,

		COUNT
	}

	public Ref<Text> ZoneName2;

	public Ref<Text> ZoneDesc;
	#endregion

	#region Methods
	public string Name => this.ZoneName2.GetText();

	public string Description => this.ZoneDesc.GetText();
	#endregion
}