using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
[Side(ReleaseSide.Server)]
public sealed class ZoneNpcSpawn : ModelElement
{
	#region Attributes
	public int Zone { get; set; }
	public short Id { get; set; }
	public string Alias { get; set; }
	public Ref<Npc> Npc { get; set; }

	public bool Respawn { get; set; }
	public Msec RespawnDelayMin { get; set; }
	public Msec RespawnDelayMax { get; set; }

	public enum ZoneDetect
	{
		None,
		Both182,
		Both360,
		Npc180,
		Npc360,
		Pc180,
		Pc360,
		COUNT
	}
	#endregion

	#region Helpers
	public class ZoneNpcSpawnChannel
	{
		internal ZoneNpcSpawn Owner;
		public ushort Channel { get; set; }
		public Time64 LastTime { get; set; }

		public Time64 NextTime
		{
			get
			{
				var delay = Owner!.RespawnDelayMin;
				var count = (DateTime.Now - LastTime) / delay + 1;

				return LastTime + delay * count;
			}
		}
	}

	public List<ZoneNpcSpawnChannel> Channels { get; } = [];

	public void AddChannel(ZoneNpcSpawnChannel channel)
	{
		channel.Owner = this;
		Channels.Add(channel);
	}
	#endregion
}