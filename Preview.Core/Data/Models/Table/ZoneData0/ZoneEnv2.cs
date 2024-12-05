using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using static Xylia.Preview.Data.Models.WorldAccountExpedition;

namespace Xylia.Preview.Data.Models;
public class ZoneEnv2 : ModelElement, IHaveName
{
	#region Attributes
	public string Alias { get; set; }

	public bool InitEnable { get; set; }

	public Msec InitEnableDuration { get; set; }

	public Ref<Text> Name2 { get; set; }

	public bool HideDisable { get; set; }

	// public Category Category { get; set; }

	//public Ref<FieldItem> RequiredFielditem { get; set; }

	//public Ref<Item> ManipulateByItem { get; set; }

	//public short ManipulateByItemCount { get; set; }

	//public bool ShowRequireManipulateByItemMessage { get; set; }

	//public bool ConsumeItemByManipulate { get; set; }

	//public Ref<Cinematic> BroadcastManipulateCinematic { get; set; }

	//public ShowConfirmType ShowConfirmType { get; set; }

	//public ManipulateNotificationRange ManipulateNotificationRange { get; set; }

	//public int RequiredFactionScore { get; set; }

	//public int RequiredFactionScoreMax { get; set; }

	//public bool ConsumeFactionScore { get; set; }

	//public Msec ManipulateDuration { get; set; }

	//public Ref<Effect> ManipulatedEffect { get; set; }

	//public Ref<Effect> MainFactionEffect { get; set; }

	//public Distance MainFactionEffectDistance { get; set; }

	//public Ref<Effect> CannotManipulateEffect { get; set; }

	//public bool CannotManipulateInCombat { get; set; }

	//public bool CannotManipulateInMaxInstantHeartCount { get; set; }

	//public bool RequiredJoinedMainFaction { get; set; }

	//public bool RequiredActivatedMainFaction { get; set; }

	//public Ref<Faction> RequiredActivatedFaction { get; set; }

	//public Msec RespawnDuration { get; set; }

	//public bool Rollback { get; set; }

	//public short MaxHp { get; set; }

	//public Ref<Skill>[] AttackSkill { get; set; }

	//public Ref<Skill3>[] AttackSkill3 { get; set; }

	//public bool DiceResultBroadcast { get; set; }

	//public bool SaveAuthorizer { get; set; }

	//public bool OccurrenceEventInRespawn { get; set; }

	//public bool BroadcastPickupRewardToWorld { get; set; }

	//public bool AcquireRewardToInventory { get; set; }

	//public int RewardFactionScore { get; set; }

	//public string GainFactionScoreShowname { get; set; }

	//public string CasterAnimname { get; set; }

	//public string CasterShowname { get; set; }

	//public string CasterPickingAnimname { get; set; }

	//public string CasterManipulateFinishShowname { get; set; }

	//public string EnvAnimname { get; set; }

	//public string EnvSoundName { get; set; }

	//public Ref<Text> ActionName2 { get; set; }

	//public Ref<Text> ActionDesc2 { get; set; }

	//public bool HideClose { get; set; }

	//public bool HideEmpty { get; set; }

	//public bool TooltipOption { get; set; }

	//public bool ShowQuestIndicator { get; set; }

	//public string DefaultIndicatorImage { get; set; }

	//public ForwardingType[] ForwardingTypes { get; set; }

	//public Ref<Quest>[] Quests { get; set; }

	//public sbyte[] Missions { get; set; }

	//public sbyte[] Cases { get; set; }

	//public short[] CaseSubtypes { get; set; }

	//public ForwardingType[] LootForwardingTypes { get; set; }

	//public Ref<Quest>[] LootQuests { get; set; }

	//public sbyte[] LootMissions { get; set; }

	//public sbyte[] LootCases { get; set; }

	//public Ref<Item>[] LootItem { get; set; }

	//public bool Lootable { get; set; }

	//public Ref<ModelElement> Attraction { get; set; }

	//public string CasterReactionAttach { get; set; }

	//public string SpawnMesh { get; set; }

	//public string SpawnAnimset { get; set; }

	//public Ref<Text> ImageText { get; set; }

	public string MapunitImageEnableCloseTrueImageset { get; set; }

	public string MapunitImageEnableCloseTrueOverImageset { get; set; }

	public short MapunitImageEnableCloseTrueSizeX { get; set; }

	public short MapunitImageEnableCloseTrueSizeY { get; set; }

	public string MapunitImageEnableCloseFalseImageset { get; set; }

	public string MapunitImageEnableCloseFalseOverImageset { get; set; }

	public short MapunitImageEnableCloseFalseSizeX { get; set; }

	public short MapunitImageEnableCloseFalseSizeY { get; set; }

	public string MapunitImageEnableOpenImageset { get; set; }

	public string MapunitImageEnableOpenOverImageset { get; set; }

	public short MapunitImageEnableOpenSizeX { get; set; }

	public short MapunitImageEnableOpenSizeY { get; set; }

	public string MapunitImageDisableImageset { get; set; }

	public string MapunitImageDisableOverImageset { get; set; }

	public short MapunitImageDisableSizeX { get; set; }

	public short MapunitImageDisableSizeY { get; set; }

	public string MapunitImageUnconfirmedImageset { get; set; }

	public string MapunitImageUnconfirmedOverImageset { get; set; }

	public short MapunitImageUnconfirmedSizeX { get; set; }

	public short MapunitImageUnconfirmedSizeY { get; set; }

	public bool ForceShowNameplate { get; set; }


	public sealed class Button : ZoneEnv2
	{
		public bool EnableCreateSoloParty { get; set; }
	}

	public sealed class Chest : ZoneEnv2
	{
		public ExpeditionTypeSeq ExpeditionType { get; set; }

		public Ref<WorldAccountExpedition> Expedition { get; set; }

		#region Methods
		public override MapUnit.CategorySeq MapUnitCategory => ExpeditionType switch
		{
			ExpeditionTypeSeq.ViewPoint => MapUnit.CategorySeq.ExpeditionEnv,
			ExpeditionTypeSeq.Collection => MapUnit.CategorySeq.ExpeditionEnv_Collection,
			_ => base.MapUnitCategory
		};

		protected override void LoadHiddenField()
		{
			base.LoadHiddenField();

			// append mapunit display
			if (ExpeditionType == ExpeditionTypeSeq.Collection && string.IsNullOrEmpty(MapunitImageDisableImageset))
			{
				var alias = this.Attributes.Get<string>("alias");
				if (alias.EndsWith("_collectA", StringComparison.OrdinalIgnoreCase))
				{
					MapunitImageDisableImageset		= "00009499.field_wantedflag";
					MapunitImageDisableOverImageset = "00009499.field_wantedflag_over";
				}
				else if (alias.EndsWith("_collectB", StringComparison.OrdinalIgnoreCase))
				{
					MapunitImageDisableImageset		= "00009499.dungeon_cheonmugungsoulball_active";
					MapunitImageDisableOverImageset = "00009499.dungeon_cheonmugungsoulball_active_over";
				}
				else if (alias.EndsWith("_collectC", StringComparison.OrdinalIgnoreCase))
				{
					MapunitImageDisableImageset		= "00009499.dungeon_frozenark_pylon";
					MapunitImageDisableOverImageset = "00009499.dungeon_frozenark_pylon_over";
				}
				else if (alias.EndsWith("_collectD", StringComparison.OrdinalIgnoreCase))
				{
					MapunitImageDisableImageset		= "00009499.dungeon_ancientmechamonk_spawnblock";
					MapunitImageDisableOverImageset = "00009499.dungeon_ancientmechamonk_spawnblock_over";
				}
				else
				{
					MapunitImageDisableImageset		= "00009499.conflict_soulstone";
					MapunitImageDisableOverImageset = "00009499.conflict_soulstone_over";
				}

				MapunitImageDisableSizeX = MapunitImageDisableSizeY = 25;
			}
		}
		#endregion
	}

	public sealed class Pot : ZoneEnv2
	{
	}

	public sealed class Gate : ZoneEnv2
	{
	}

	public sealed class Wall : ZoneEnv2
	{
	}

	public sealed class Refiner : ZoneEnv2
	{
		public Msec OccupationDuration { get; set; }

		public Msec TryRefiningDuration { get; set; }

		public Msec BaseRefiningDuration { get; set; }

		public Msec AccroachDuration { get; set; }

		public Msec GiveFactionScoreDuration { get; set; }

		public Msec RespawnDurationInRefined { get; set; }

		public int RefineFactionScore { get; set; }

		public Ref<Item> RefinedRewardItem { get; set; }

		public short RefinedRewardItemBaseCount { get; set; }

		public Ref<Effect> RefinedEffect { get; set; }

		public Distance RefinedEffectDistance { get; set; }

		public bool RefiningDisable { get; set; }

		public Ref<Effect> OccputationEffect { get; set; }

		public Distance OccputationEffectDistance { get; set; }

		public string OccupyActionIcon { get; set; }

		public Ref<Text> OccupyActionName { get; set; }

		public string OccupyCasterShowname { get; set; }

		public string TryRefineActionIcon { get; set; }

		public Ref<Text> TryRefineActionName { get; set; }

		public string TryRefineCasterShowname { get; set; }

		public string GiveScoreActionIcon { get; set; }

		public Ref<Text> GiveScoreActionName { get; set; }

		public Ref<Social> GiveScoreSocial { get; set; }

		public string GiveScoreCasterShowname { get; set; }

		public string AccroachActionIcon { get; set; }

		public Ref<Text> AccroachActionName { get; set; }

		public string AccroachCasterShowname { get; set; }

		public string AccroachStartKismet { get; set; }

		public string AccroachEndKismet { get; set; }

		public string FriendGateStartKismet { get; set; }

		public string FriendGateEndKismet { get; set; }

		public string EnemyGateStartKismet { get; set; }

		public string EnemyGateEndKismet { get; set; }

		public sbyte RefinerUiIndex { get; set; }
	}

	public sealed class ControlPoint : ZoneEnv2
	{
		public Ref<FieldItem> ManipulationRequiredFieldItem { get; set; }

		public Ref<FieldItemDrop> ManipulationDropFieldItem { get; set; }

		public short OccupationZoneScore { get; set; }

		public short OccupationBonusZoneScorePerSec { get; set; }

		public ObjectPath FriendOccupationAdditiveEffect { get; set; }

		public ObjectPath EnemyOccupationAdditiveEffect { get; set; }

		public ObjectPath FriendOccupationAdditiveSound { get; set; }

		public ObjectPath EnemyOccupationAdditiveSound { get; set; }
	}

	public sealed class Portal : ZoneEnv2
	{
		//public PortalType PortalType { get; set; }

		public Ref<Zone> TransitZone { get; set; }

		public Ref<Dungeon> TransitDungeon { get; set; }

		public bool TransitReentrancePcspawn { get; set; }

		public short TransitPcSpawn { get; set; }

		public Ref<ZonePcSpawn> TransitPcSpawnInArena { get; set; }

		public Ref<Cinematic> TransitLeaveCinematic { get; set; }

		public Ref<Cinematic> TransitEnterCinematic { get; set; }

		public Ref<Effect>[] Effect { get; set; }

		public Ref<Social> TransitLeaveSocial { get; set; }

		public Ref<Social> TransitEnterSocial { get; set; }

		public sbyte RequiredLevel { get; set; }
	}

	public sealed class PortalList : ZoneEnv2
	{
		public Ref<ZoneEnv2>[] PortalId { get; set; }
	}

	public sealed class FootSwitch : ZoneEnv2
	{
	}

	public sealed class EffectRegion : ZoneEnv2
	{
		public Ref<Effect>[] Effect { get; set; }
	}

	public sealed class Airdash : ZoneEnv2
	{
		public sbyte AirdashLevel { get; set; }

		public string Kismet { get; set; }
	}

	public sealed class AirdashLeave : ZoneEnv2
	{
		public sbyte AirdashLevel { get; set; }

		public Ref<Zone> TransitZone { get; set; }

		public short TransitPcSpawn { get; set; }

		public Ref<Social> TransitLeaveSocial { get; set; }

		public Ref<Social> TransitEnterSocial { get; set; }
	}

	public sealed class OceanicRegion : ZoneEnv2
	{
	}

	public sealed class FallDeath : ZoneEnv2
	{
	}

	public sealed class MultipleLoop : ZoneEnv2
	{
		public Ref<Effect> LoopRestrictedEffect { get; set; }
	}

	public sealed class Deck : ZoneEnv2
	{
	}

	public sealed class FishingPoint : ZoneEnv2
	{
		public Ref<FishingField> FishingField { get; set; }
	}

	public sealed class AttractionPopup : ZoneEnv2
	{
		//public Ref<EnvEntrance> EnvEntrance { get; set; }
	}

	public sealed class EnterArenaDungeonlobby : ZoneEnv2
	{
		public bool EnterSealeddungeon { get; set; }
	}
	#endregion

	#region Methods
	public string Name => Name2.GetText();

	public virtual MapUnit.CategorySeq MapUnitCategory => MapUnit.CategorySeq.Env;

	protected override void LoadHiddenField()
	{
		if (Attributes["script"] is null)
		{
			var alias = Attributes.Get<string>("alias");
			var type = Attributes.Get<string>("type");

			if (!(type is "portal" or "oceanic-region" or "fall-death" or "attraction-popup" or "enter-arena-dungeonlobby"))
			{
				Attributes["script"] = alias + "_ai";
			}
		}
	}
	#endregion
}