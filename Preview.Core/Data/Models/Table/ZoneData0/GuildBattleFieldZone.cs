using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class GuildBattleFieldZone : ModelElement, IAttraction
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<Zone> Zone { get; set; }

	public Ref<AttractionGroup> Group { get; set; }

	public Ref<Effect>[] Effect { get; set; }

	public Ref<Effect> RespawnEffect { get; set; }

	public Msec RespawnDelay { get; set; }

	public Ref<ZoneEnv2Spawn>[] Refiner { get; set; }

	public Ref<ZoneRespawn>[] RespawnByRefiner { get; set; }

	[Name("airdash-1-by-refiner")]
	public Ref<ZoneEnv2Spawn>[] Airdash1ByRefiner { get; set; }

	[Name("airdash-2-by-refiner")]
	public Ref<ZoneEnv2Spawn>[] Airdash2ByRefiner { get; set; }

	public Ref<Text> GuildBattleFieldZoneName2 { get; set; }

	public Ref<Text> GuildBattleFieldZoneDesc { get; set; }

	public string ThumbnailImage { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public Ref<Npc> LastBoss { get; set; }

	public short[] PcSpawnId { get; set; }

	public sbyte PcSpawnIdCount { get; set; }

	public Ref<ZoneEnv2Spawn>[] GateReplica { get; set; }

	public sbyte GateOpenReadyMinute { get; set; }

	public sbyte[] SunGateOpenHour { get; set; }

	public sbyte[] SunGateOpenMinute { get; set; }

	public short[] SunGateOpenDuration { get; set; }

	public sbyte SunZoneOpenHour { get; set; }

	public sbyte SunZoneOpenMinute { get; set; }

	public sbyte SunZoneCloseHour { get; set; }

	public sbyte SunZoneCloseMinute { get; set; }

	public sbyte[] MonGateOpenHour { get; set; }

	public sbyte[] MonGateOpenMinute { get; set; }

	public short[] MonGateOpenDuration { get; set; }

	public sbyte MonZoneOpenHour { get; set; }

	public sbyte MonZoneOpenMinute { get; set; }

	public sbyte MonZoneCloseHour { get; set; }

	public sbyte MonZoneCloseMinute { get; set; }

	public sbyte[] TueGateOpenHour { get; set; }

	public sbyte[] TueGateOpenMinute { get; set; }

	public short[] TueGateOpenDuration { get; set; }

	public sbyte TueZoneOpenHour { get; set; }

	public sbyte TueZoneOpenMinute { get; set; }

	public sbyte TueZoneCloseHour { get; set; }

	public sbyte TueZoneCloseMinute { get; set; }

	public sbyte[] WedGateOpenHour { get; set; }

	public sbyte[] WedGateOpenMinute { get; set; }

	public short[] WedGateOpenDuration { get; set; }

	public sbyte WedZoneOpenHour { get; set; }

	public sbyte WedZoneOpenMinute { get; set; }

	public sbyte WedZoneCloseHour { get; set; }

	public sbyte WedZoneCloseMinute { get; set; }

	public sbyte[] ThuGateOpenHour { get; set; }

	public sbyte[] ThuGateOpenMinute { get; set; }

	public short[] ThuGateOpenDuration { get; set; }

	public sbyte ThuZoneOpenHour { get; set; }

	public sbyte ThuZoneOpenMinute { get; set; }

	public sbyte ThuZoneCloseHour { get; set; }

	public sbyte ThuZoneCloseMinute { get; set; }

	public sbyte[] FriGateOpenHour { get; set; }

	public sbyte[] FriGateOpenMinute { get; set; }

	public short[] FriGateOpenDuration { get; set; }

	public sbyte FriZoneOpenHour { get; set; }

	public sbyte FriZoneOpenMinute { get; set; }

	public sbyte FriZoneCloseHour { get; set; }

	public sbyte FriZoneCloseMinute { get; set; }

	public sbyte[] SatGateOpenHour { get; set; }

	public sbyte[] SatGateOpenMinute { get; set; }

	public short[] SatGateOpenDuration { get; set; }

	public sbyte SatZoneOpenHour { get; set; }

	public sbyte SatZoneOpenMinute { get; set; }

	public sbyte SatZoneCloseHour { get; set; }

	public sbyte SatZoneCloseMinute { get; set; }

	public sbyte RequiredLevelMin { get; set; }

	public sbyte RequiredLevelMax { get; set; }

	public short RequiredFactionScore { get; set; }
	#endregion

	#region IAttraction
	public string Name => this.GuildBattleFieldZoneName2.GetText();
	public string Description => this.GuildBattleFieldZoneDesc.GetText();
	#endregion
}