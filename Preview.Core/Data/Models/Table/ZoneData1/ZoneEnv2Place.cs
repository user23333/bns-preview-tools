using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public abstract class ZoneEnv2Place : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public Vector32[] RegionPoint { get; set; }

	public Distance Height { get; set; }

	public Vector32 ActionPoint { get; set; }

	public Distance ActionRadius { get; set; }

	public bool UseManipulationPoint { get; set; }

	public Vector32 ManipulationPoint { get; set; }

	public Distance ManipulationRadius { get; set; }

	public Distance ManipulationHeightUpper { get; set; }

	public Distance ManipulationHeightLower { get; set; }

	public string EnvActorname { get; set; }

	public bool SpawnEnv { get; set; }

	public sealed class Button : ZoneEnv2Place { }
	public sealed class Chest : ZoneEnv2Place { }
	public sealed class Pot : ZoneEnv2Place { }
	public sealed class Gate : ZoneEnv2Place { }
	public sealed class Wall : ZoneEnv2Place { }
	public sealed class Refiner : ZoneEnv2Place { }
	public sealed class ControlPoint : ZoneEnv2Place { }
	public sealed class Portal : ZoneEnv2Place { }
	public sealed class FootSwitch : ZoneEnv2Place { }
	public sealed class EffectRegion : ZoneEnv2Place { }

	public sealed class Airdash : ZoneEnv2Place
	{
		public Msec Duration { get; set; }
		public Vector32 EndPos { get; set; }
		public Vector32 SummonEndPos { get; set; }
	}

	public sealed class AirdashLeave : ZoneEnv2Place { }
	public sealed class OceanicRegion : ZoneEnv2Place { }
	public sealed class FallDeath : ZoneEnv2Place { }
	public sealed class MultipleLoop : ZoneEnv2Place { }
	public sealed class Deck : ZoneEnv2Place { }
	public sealed class FishingPoint : ZoneEnv2Place { }
	public sealed class AttractionPopup : ZoneEnv2Place { }
	public sealed class EnterArenaDungeonlobby : ZoneEnv2Place { }
	public sealed class Board : ZoneEnv2Place { }
	#endregion
}