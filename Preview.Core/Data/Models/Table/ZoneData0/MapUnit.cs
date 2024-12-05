using CUE4Parse.UE4.Objects.Core.Math;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public class MapUnit : ModelElement, IHaveName
{
	#region Attributes
	public short Mapid { get; set; }

	public int ZoneId { get; set; }

	public bool IsPhasingUnit { get; set; }

	public float PositionX { get; set; }

	public float PositionY { get; set; }

	public float PositionZ { get; set; }

	public enum CategorySeq
	{
		[Text("Name.mapunit.category.none")] None,
		[Text("Name.mapunit.category.player")] Player,
		[Text("Name.mapunit.category.party")] Party,
		[Text("Name.mapunit.category.team")] Team,
		[Text("Name.mapunit.category.guild")] Guild,
		[Text("Name.mapunit.category.friend")] Friend,
		RevengeEnemy,
		[Text("Name.mapunit.category.faction")] Faction,
		[Text("Name.mapunit.category.duel-enemy")] DuelEnemy,
		[Text("Name.mapunit.category.quest")] Quest,
		[Text("UI.MapInfoOption.NPC")] Npc,
		Env,
		[Text("Name.mapunit.category.teleport")] Teleport,
		[Text("Name.mapunit.category.airdash")] Airdash,
		Link,
		[Text("Name.mapunit.category.convoy")] Convoy,
		SpawnedEnv,
		Static,
		[Text("Name.mapunit.category.auction")] Auction,
		[Text("Name.mapunit.category.store")] Store,
		[Text("Name.mapunit.category.camp")] Camp,
		[Text("Name.mapunit.category.party-camp")] PartyCamp,
		[Text("Name.mapunit.category.roulette")] Roulette,
		[Text("Name.mapunit.category.field-boss")] FieldBoss,
		[Text("Name.mapunit.category.gather")] Gather,
		[Text("Name.mapunit.category.craft")] Craft,
		[Text("Name.mapunit.category.gather-env")] GatherEnv,
		[Text("Name.mapunit.category.heart")] Heart,
		[Text("Name.mapunit.category.enter-arena")] EnterArena,
		[Text("Name.mapunit.category.weapon-box")] WeaponBox,
		[Text("Name.mapunit.category.refiner")] Refiner,
		[Text("Name.mapunit.category.dungeon-3")] Dungeon3,
		[Text("Name.mapunit.category.dungeon-4")] Dungeon4,
		[Text("Name.mapunit.category.dungeon-5")] Dungeon5,
		[Text("Name.mapunit.category.raid-dungeon")] RaidDungeon,
		[Text("Name.mapunit.category.classic-field")] ClassicField,
		[Text("Name.mapunit.category.faction-battle-field")] FactionBattleField,
		[Text("Name.mapunit.category.guild-battle-field")] GuildBattleField,
		PartyBattleStartpoint,
		PartyBattleEnemy,
		[Text("Name.mapunit.category.fishing-field")] FishingField,
		[Text("Name.mapunit.category.expedition-env")] ExpeditionEnv,
		[Text("Name.mapunit.category.wandering-npc")] WanderingNpc,

		[Text("UI.Expedition.Main.Collection")] ExpeditionEnv_Collection, //append 
		COUNT
	}

	public CategorySeq Category { get; set; }

	public enum MapDepthSeq : byte
	{
		N1,
		N2,
		N3,
		N4,
		N5,
	}

	public MapDepthSeq MapDepth { get; set; }

	public MapDepthSeq ArenaDungeonMapDepth { get; set; }

	public bool Zoom { get; set; }

	public bool Rotate { get; set; }

	public bool Click { get; set; }

	public bool Front { get; set; }

	public bool ShowTooltip { get; set; }

	public Ref<Text> Name2 { get; set; }

	public short Opacity { get; set; }

	public short SizeX { get; set; }

	public short SizeY { get; set; }

	public short OufofsightSizeX { get; set; }

	public short OufofsightSizeY { get; set; }

	public ObjectPath Imageset { get; set; }

	public ObjectPath OverImageset { get; set; }

	public ObjectPath PressedImageset { get; set; }

	public ObjectPath OutofsightImageset { get; set; }

	public ObjectPath OutofsightOverImageset { get; set; }

	public ObjectPath OutofsightPressedImageset { get; set; }

	public float CenterPosX { get; set; }

	public float CenterPosY { get; set; }
	#endregion

	#region Sub
	public sealed class Static : MapUnit
	{

	}

	public sealed class Quest : MapUnit
	{

	}

	public sealed class Link : MapUnit
	{

	}

	public sealed class Npc : MapUnit
	{

	}

	public sealed class Boss : MapUnit
	{

	}

	public sealed class Airdash : MapUnit
	{

	}

	public sealed class Env : MapUnit
	{

	}

	public sealed class Attraction : MapUnit
	{

	}

	public sealed class NpcGroup : MapUnit
	{

	}

	public sealed class GuildBattleFieldPortal : MapUnit
	{

	}

	public sealed class PartyBattleStartpointAlpha : MapUnit
	{

	}

	public sealed class PartyBattleStartpointBeta : MapUnit
	{

	}

	public sealed class FishingField : MapUnit
	{

	}
	#endregion


	#region Methods
	public string Name => Name2.GetText() ?? base.ToString();

	public FVector Position => new(PositionX, PositionY, PositionZ);

	public FVector2D Size => new(SizeX, SizeY);
	#endregion
}