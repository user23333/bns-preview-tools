using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class Teleport : ModelElement	, IHaveName
{
	#region Attributes
	public string Alias { get; set; }

	public bool Retired { get; set; }

	public Ref<Text> Name2 { get; set; }

	public Ref<ZoneTeleportPosition> TeleportPosition { get; set; }

	public Distance DistanceFromStartTeleport { get; set; }

	public int PricePercent { get; set; }

	public Ref<Text> Description2 { get; set; }

	public Ref<Faction> ActivatedFaction { get; set; }

	public bool JoinedFaction { get; set; }

	public sbyte RequiredPcLevel { get; set; }

	public sbyte RequiredPcMasteryLevel { get; set; }

	public sbyte RequiredFactionLevel { get; set; }

	public Ref<Quest> UiPrecedingQuest { get; set; }
	#endregion

	#region Methods
	public string Name => Name2.GetText() ?? ToString();
	#endregion
}