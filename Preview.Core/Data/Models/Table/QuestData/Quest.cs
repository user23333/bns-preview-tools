using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.Data.Models.Sequence.Combat;
using static Xylia.Preview.Data.Models.PartyBattleFieldZone;

namespace Xylia.Preview.Data.Models;
public sealed class Quest : ModelElement, IHaveName
{
	#region Attributes
	public LazyList<Acquisition> Acquisition { get; set; }

	public LazyList<Mission_Step> MissionStep { get; set; }

	public LazyList<Completion> Completion { get; set; }

	public LazyList<Transit> Transit { get; set; }


	[Side(ReleaseSide.Server)]
	public BroadcastCategorySeq BroadcastCategory { get; set; }
	public enum BroadcastCategorySeq
	{
		None,
		Field,
		Always,
		SoloQuartet,
		Sextet,
	}

	//[Side(ReleaseSide.Server), Repeat(3)]
	public Ref<Achievement>[] ExtraQuestCompleteAchievement { get; set; }

	[Side(ReleaseSide.Server)]
	public Ref<Cinematic> ReplayEpicZoneLeaveCinematic { get; set; }


	public enum CategorySeq
	{
		Epic,
		Normal,
		Job,
		Dungeon,
		Attraction,
		TendencySimple,
		TendencyTendency,
		Mentoring,
		Hunting,
		COUNT
	}

	public enum ContentTypeSeq
	{
		None,
		Gather,
		Production,
		PvpReward,
		Festival,
		EliteSkill,
		Duel,
		PartyBattle,
		Special,
		SideEpisode,
		Hidden,
		COUNT
	}

	public enum SaveType
	{
		All,

		/// <summary>
		/// 25000~25500
		/// </summary>
		Nothing,

		/// <summary>
		/// 20000~23000
		/// </summary>
		[Name("except-completion")]
		ExceptCompletion,

		/// <summary>
		/// 28000~
		/// </summary>
		[Name("except-completion-and-logout-save")]
		ExceptCompletionAndLogoutSave,
	}

	public CategorySeq Category => Attributes.Get<CategorySeq>("category");
	public ContentTypeSeq ContentType => Attributes.Get<ContentTypeSeq>("content-type");
	public ResetType ResetType => Attributes.Get<ResetType>("reset-type");
	#endregion


	#region Methods
	public string Name => Attributes["name2"]?.GetText();

	public string Title => Attributes["group2"]?.GetText();

	public ImageProperty FrontIcon => new() { BaseImageTexture = GetImageTexture() };

	public ImageProperty FrontIconOver => new() { BaseImageTexture = GetImageTexture(true) };

	private FPackageIndex GetImageTexture(bool over = false)
	{
		string name;

		bool repeat = ResetType != ResetType.None;
		switch (Category)
		{
			case CategorySeq.Epic: name = "Map_Epic_Start"; break;
			case CategorySeq.Job: name = "Map_Job_Start"; break;
			case CategorySeq.Dungeon: return null;
			case CategorySeq.Attraction: name = "Map_attraction_start"; break;
			case CategorySeq.TendencySimple: name = "Map_System_start"; break;
			case CategorySeq.TendencyTendency: name = "Map_System_start"; break;
			case CategorySeq.Mentoring: name = "mento_mentoring_start"; break;
			case CategorySeq.Hunting: name = repeat ? "Map_Hunting_repeat_start" : "Map_Hunting_start"; break;
			case CategorySeq.Normal:
			{
				// faction quest
				if (Attributes["main-faction"] != null)
				{
					name = repeat ? "Map_Faction_repeat_start" : "Map_Faction_start";
				}
				else
				{
					name = ContentType switch
					{
						ContentTypeSeq.Festival => repeat ? "Map_Festival_repeat_start" : "Map_Festival_start",
						ContentTypeSeq.Duel or ContentTypeSeq.PartyBattle => repeat ? "Map_Faction_repeat_start" : "Map_Faction_start",
						ContentTypeSeq.SideEpisode => "Map_side_episode_start",
						ContentTypeSeq.Special => "Map_Job_Start",
						ContentTypeSeq.Hidden => "Map_Hidden_Start",
						_ => repeat ? "Map_Repeat_start" : "Map_Normal_Start",
					};
				}
			}
			break;

			default: throw new NotImplementedException();
		}

		if (over) name += "_over";
		return new MyFPackageIndex($"00009499.{name}");
	}


	public Quest GetNextQuest(JobSeq job = JobSeq.검사)
	{
		var completion = Completion?.FirstOrDefault();
		if (completion is null) return null;

		foreach (var NextQuest in completion.NextQuest)
		{
			var jobs = NextQuest.Job;
			if (jobs is null || jobs.CheckSeq(job)) return NextQuest.Quest.Instance;
		}

		return null;
	}

	public IEnumerable<QuestReward> GetRewards() => MissionStep.SelectMany(step => step.Mission.SelectMany(mission => mission.Reward)).Values();
	#endregion
}


public class Acquisition : ModelElement
{
	public List<Case> Case { get; set; }

	public List<TutorialCase> TutorialCase { get; set; }

	public Ref<QuestReward>[] Reward { get; set; }
}

public class Mission_Step : ModelElement
{
	[Name("mission")]
	public List<Mission> Mission { get; set; }

	[Name("mission-step-success")]
	public List<Mission_Step_Success> MissionStepSuccess { get; set; }

	[Name("mission-step-fail")]
	public List<Mission_Step_Fail> MissionStepFail { get; set; }


	public string Text => Attributes["desc"].GetText();
}

public abstract class Case : ModelElement
{
	#region Base
	public List<FilterSet> FilterSet { get; set; }
	public List<ReactionSet> ReactionSet { get; set; }


	//[Side(ReleaseSide.Client)]
	//public Indicator Indicator { get; set; }

	[Side(ReleaseSide.Client)]
	public bool ShowInTooltip { get; set; }

	[Side(ReleaseSide.Client)]
	public bool VisibleObject { get; set; }

	[Side(ReleaseSide.Client)]
	public Ref<TalkSocial> CaseTalksocial { get; set; }

	[Side(ReleaseSide.Client)]
	public float CaseTalksocialDelay { get; set; }



	[Side(ReleaseSide.Server)]
	public Ref<Zone> Zone { get; set; }

	//[Side(ReleaseSide.Server)]
	//public Ref<QuestDecision> QuestDecision { get; set; }

	//[Side(ReleaseSide.Server)]
	//public Ref<QuestDecision> FailQuestDecision { get; set; }

	[Side(ReleaseSide.Server)]
	public Ref<FieldItem> DropGadget { get; set; }

	[Side(ReleaseSide.Server)]
	public bool PartyBroadcast { get; set; }

	[Side(ReleaseSide.Server)]
	public bool TeamBroadcast { get; set; }


	public virtual List<Record> Attractions { get; }
	#endregion

	#region Sub
	public sealed class Talk : Case
	{
		public Ref<ModelElement> Object { get; set; }
	}

	public sealed class TalkToItem : Case
	{
		public Ref<Item> Item { get; set; }

		public override List<Record> Attractions => new() { Item.Instance?.Source };
	}

	public sealed class TalkToSelf : Case
	{

	}

	public sealed class Manipulate : Case
	{
		public Ref<ModelElement> Object2 { get; set; }
		public Ref<ModelElement>[] MultiObject { get; set; }


		public override List<Record> Attractions
		{
			get
			{
				var result = new List<Record>();
				result.Add(Object2.Instance?.Source);
				MultiObject.ForEach(x => result.Add(x.Instance?.Source));

				return result;
			}
		}
	}

	public sealed class NpcManipulate : Case
	{
		public Ref<ModelElement> Object { get; set; }
		public Ref<ModelElement>[] MultiObject { get; set; }

		public override List<Record> Attractions
		{
			get
			{
				var result = new List<Record>();
				MultiObject.ForEach(x => result.Add(x.Instance?.Source));

				return result;
			}
		}
	}

	public sealed class Approach : Case
	{
	}

	public sealed class Skill : Case
	{
		[Side(ReleaseSide.Client)]
		public Ref<NpcResponse> NpcResponse { get; set; }

		public Ref<ModelElement>[] Object2 { get; set; }

		public Ref<Models.Skill> skill { get; set; }

		public Ref<Models.Skill3> skill3 { get; set; }
	}

	public sealed class Loot : Case
	{
		public Ref<ModelElement> Object2 { get; set; }
		public Ref<ModelElement>[] MultiObject { get; set; }

		[Side(ReleaseSide.Client)]
		public Ref<VirtualItem> Looting { get; set; }

		public override List<Record> Attractions
		{
			get
			{
				var result = new List<Record>();
				result.Add(Object2.Instance?.Source);
				MultiObject.ForEach(x => result.Add(x.Instance?.Source));

				return result;
			}
		}
	}

	public sealed class Killed : Case
	{
		public Ref<ModelElement> Object2 { get; set; }
		public Ref<ModelElement>[] MultiObject { get; set; }


		public override List<Record> Attractions
		{
			get
			{
				var result = new List<Record>();
				result.Add(Object2.Instance?.Source);
				MultiObject.ForEach(x => result.Add(x.Instance?.Source));

				return result;
			}
		}
	}

	public sealed class FinishBlow : Case
	{
		public Ref<Npc> Npc { get; set; }

		public override List<Record> Attractions => new() { Npc.Instance?.Source };
	}

	public sealed class EnvEntered : Case
	{
		public Ref<ModelElement> Object2 { get; set; }

		public Ref<EnvResponse> EnvResponse { get; set; }


		public override List<Record> Attractions => new() { Object2.Instance?.Source };
	}

	public sealed class EnterZone : Case
	{
		public Ref<ModelElement> Object { get; set; }

		public override List<Record> Attractions => [Object.Instance?.Source];
	}

	public sealed class ConvoyArrived : Case
	{
		public Ref<ModelElement> Object { get; set; }

		[Side(ReleaseSide.Server)]
		public Ref<ZoneConvoy> Convoy { get; set; }

		public override List<Record> Attractions => [Object.Instance?.Source];
	}

	public sealed class ConvoyFailed : Case
	{
		public Ref<ModelElement> Object { get; set; }

		public Ref<ZoneConvoy> Convoy { get; set; }

		public override List<Record> Attractions => [Object.Instance?.Source];
	}

	public sealed class NpcBleedingOccured : Case
	{
		public Ref<ModelElement> Object { get; set; }

		[Side(ReleaseSide.Server)]
		public sbyte idx { get; set; }

		public override List<Record> Attractions => [Object.Instance?.Source];
	}

	public sealed class EnterPortal : Case
	{
		public Ref<ModelElement> Object2 { get; set; }

		public override List<Record> Attractions => [Object2.Instance?.Source];
	}

	public sealed class AcquireSummoned : Case
	{
		public Ref<ModelElement> Object { get; set; }

		public Ref<SummonedPreset> SummonedPreset { get; set; }

		public Ref<NpcResponse> NpcResponse { get; set; }

		public Ref<Text> ButtonTextAccept { get; set; }

		public Ref<Text> ButtonTextCancel { get; set; }

		public override List<Record> Attractions => [Object.Instance?.Source];
	}

	public sealed class PcSocial : Case
	{
		public Ref<ModelElement> Object2 { get; set; }

		[Side(ReleaseSide.Client)]
		public Ref<TalkSocial> Social { get; set; }

		[Side(ReleaseSide.Client)]
		public Ref<StateSocial> StateSocial { get; set; }

		[Side(ReleaseSide.Client)]
		public Ref<NpcResponse> NpcResponse { get; set; }


		public override List<Record> Attractions => [Object2.Instance?.Source];
	}

	public sealed class JoinFaction : Case
	{
		public Ref<Faction> Faction { get; set; }

		public Ref<ModelElement> Object { get; set; }

		public Ref<NpcResponse>[] NpcResponse { get; set; }

		public Ref<Text> ButtonTextAccept { get; set; }

		public Ref<Text> ButtonTextCancel { get; set; }

		public Ref<Item> Grocery { get; set; }

		public short GroceryCount { get; set; }

		public bool RemoveGrocery { get; set; }


		public override List<Record> Attractions => [Object.Instance?.Source];
	}

	public sealed class DuelFinish : Case
	{
		public ResultSeq DuelResult { get; set; }
		public enum ResultSeq
		{
			None,
			All,
			Win,
			Lose,
		}

		public DuelTypeSeq DuelType { get; set; }
		public enum DuelTypeSeq
		{
			None,
			DeathMatch1VS1,
			TagMatch3VS3,
			SuddenDeath3VS3,
		}

		public ArenaMatchingRuleDetail ArenaMatchingRuleDetail { get; set; }

		public int DuelStraightWin { get; set; }

		public sbyte DuelGrade { get; set; }
	}

	public sealed class PartyBattle : Case
	{
		public PartyBattleFieldZoneType PartyBattleType { get; set; }

		public DuelFinish.ResultSeq PartyBattleResult { get; set; }
	}

	public sealed class PartyBattleAction : Case
	{
		public PartyBattleFieldZoneType PartyBattleType { get; set; }

		public PartyBattleActionTypeSeq PartyBattleActionType { get; set; }
		public enum PartyBattleActionTypeSeq
		{
			None,
			Occupy,
		}
	}

	public sealed class CompleteQuest : Case
	{
		public Ref<Quest> completeQuest { get; set; }
	}

	public sealed class PickUpFielditem : Case
	{
		public Ref<FieldItem> Fielditem { get; set; }
	}

	public sealed class BattleRoyal : Case
	{
		public Ref<BattleRoyalField> BattleRoyalField { get; set; }
	}

	public sealed class AttractionPopup : Case
	{
		public Ref<ZoneEnv2> AttractionPopupEnv { get; set; }
	}

	public sealed class PublicRaid : Case
	{

	}
	#endregion
}

public abstract class TutorialCase : ModelElement
{
	#region Base
	public List<FilterSet> FilterSet { get; set; }

	public List<ReactionSet> ReactionSet { get; set; }


	[Side(ReleaseSide.Server)]
	public Ref<Zone> Zone { get; set; }

	public virtual List<Record> Attractions { get; }
	#endregion

	#region Sub
	public sealed class AcquireItem : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Item> Item { get; set; }
	}

	public sealed class EquipItem : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public GameCategory2Seq ItemCategory { get; set; }

		[Side(ReleaseSide.Client)]
		public Ref<Item> Item { get; set; }
	}

	public sealed class UnequipItem : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public GameCategory2Seq ItemCategory { get; set; }

		[Side(ReleaseSide.Client)]
		public Ref<Item> Item { get; set; }
	}

	public sealed class UseItem : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Item> Item { get; set; }
	}

	public sealed class GrowItem : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Item> MaterialItem { get; set; }

		[Side(ReleaseSide.Client)]
		public Ref<Item> PrimaryItem { get; set; }
	}

	public sealed class TransformItem : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<ItemTransformRecipe> ItemTransformRecipe { get; set; }
	}

	public sealed class PickUpFielditem : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<FieldItem> Fielditem { get; set; }
	}

	public sealed class PickDownFielditem : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<FieldItem> Fielditem { get; set; }
	}

	public sealed class Targeting : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Npc> Npc { get; set; }
	}

	public sealed class TalkStart : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Npc> Npc { get; set; }
	}

	public sealed class WindowOpen : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public WindowTypeSeq WindowType { get; set; }

		[Side(ReleaseSide.Client)]
		public WindowOpenWaySeq WindowOpenWay { get; set; }

		public enum WindowTypeSeq
		{
			Inverntory,

			QuestJournal,

			Skill,

			Sandbox,

			Auction,

			CashShop,

			Wardrobe,

			AccountContents,
		}

		public enum WindowOpenWaySeq
		{
			None,
			ByNpcSellerButton,
		}
	}

	public sealed class CompleteSelfRevival : TutorialCase
	{

	}

	public sealed class NpcBleeding : TutorialCase
	{
		public Ref<Npc> Npc { get; set; }

		public sbyte Percent { get; set; }
	}

	public sealed class PcBleeding : TutorialCase
	{
		public sbyte Percent { get; set; }
	}

	public sealed class Exhausted : TutorialCase
	{

	}

	public sealed class Attacked : TutorialCase
	{

	}

	public sealed class AcquireSp : TutorialCase
	{
		public sbyte Sp { get; set; }
	}


	public sealed class Skill : TutorialCase
	{
		public Ref<ModelElement>[] Object2 { get; set; }

		public SkillCheckTypeSeq SkillCheckType { get; set; }
		public enum SkillCheckTypeSeq
		{
			SkillKey,
			SkillId,
		}

		public Ref<Models.Skill> skill { get; set; }

		public Ref<Models.Skill3> Skill3 { get; set; }

		public int Skill3Id { get; set; }

		public Ref<Effect> TargetRequiredEffect { get; set; }

		public sbyte TargetEffectCount { get; set; }
	}

	public sealed class SkillSequence : TutorialCase
	{
		//[Repeat(16)]
		public Ref<ModelElement>[] Object2 { get; set; }

		public Ref<TutorialSkillSequence> skillSequence { get; set; }
	}

	public sealed class SkillTraining : TutorialCase
	{

	}

	public sealed class QuestTrackingPosition : TutorialCase
	{

	}

	public sealed class RepairWithCampfire : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Item> Item { get; set; }
	}

	public sealed class Teleport : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Item> Item { get; set; }
	}

	public sealed class ExpandInventory : TutorialCase
	{

	}

	public sealed class GemCompose : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Item> PrimaryItem { get; set; }

		[Side(ReleaseSide.Client)]
		public Ref<Item> MaterialItem { get; set; }
	}

	public sealed class GemDecompose : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Item> Item { get; set; }
	}

	public sealed class WeaponGem : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Item> Weapon { get; set; }

		[Side(ReleaseSide.Client)]
		public Ref<Item> Gem { get; set; }
	}

	public sealed class DetachWeaponGem : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Item> Weapon { get; set; }

		[Side(ReleaseSide.Client)]
		public Ref<Item> Gem { get; set; }
	}

	public sealed class Airdash : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<ModelElement> Object2 { get; set; }

		[Side(ReleaseSide.Client)]
		public Ref<EnvResponse> EnvResponse { get; set; }


		public override List<Record> Attractions => [Object2.Instance?.Source];
	}

	public sealed class EnlargeMiniMap : TutorialCase
	{

	}

	public sealed class TransparentMiniMap : TutorialCase
	{

	}

	public sealed class ResurrectingSummoned : TutorialCase
	{

	}

	public sealed class MoveToPosition : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Npc> LinkNpc { get; set; }

		[Side(ReleaseSide.Client)]
		public float LocationX { get; set; }

		[Side(ReleaseSide.Client)]
		public float LocationY { get; set; }

		[Side(ReleaseSide.Client)]
		public float ApproachRange { get; set; }
	}

	public sealed class UseHeartCount : TutorialCase
	{

	}

	public sealed class ChargeHeartCount : TutorialCase
	{

	}

	public sealed class TeleportZone : TutorialCase
	{
		[Side(ReleaseSide.Client)]
		public Ref<Teleport> TeleportID { get; set; }
	}

	public sealed class WeaponStarstone : TutorialCase
	{

	}

	public sealed class WeaponStarwords : TutorialCase
	{

	}

	public sealed class WeaponSoulgem : TutorialCase
	{

	}

	public sealed class ActivateWorldAccountCardCollection : TutorialCase
	{

	}
	#endregion
}

public class Mission : ModelElement
{
	public List<Case> Case { get; set; }

	public List<TutorialCase> TutorialCase { get; set; }

	#region Properties
	public sbyte Id => Attributes.Get<sbyte>("id");

	public string Text => Attributes["name2"]?.GetText();

	public Ref<QuestReward>[] Reward { get; set; }

	public short CurrentRegisterValue => 0;

	public string TagName => null;
	#endregion
}

public class Mission_Step_Success : ModelElement
{
	public List<Case> Case { get; set; }

	public List<TutorialCase> TutorialCase { get; set; }
}

public class Mission_Step_Fail : ModelElement
{
	public List<Case> Case { get; set; }

	public List<TutorialCase> TutorialCase { get; set; }


	[Side(ReleaseSide.Client)]
	public Ref<TalkSocial> FailTalksocial { get; set; }

	[Side(ReleaseSide.Client)]
	public float FailTalksocialDelay { get; set; }

	//[Side(ReleaseSide.Server)]
	//public Ref<QuestDecision> QuestDecision { get; set; }

	//[Side(ReleaseSide.Server), Repeat(2)]
	public Ref<Zone>[] Zone { get; set; }
}

public class Completion : ModelElement
{
	public List<NextQuest> NextQuest { get; set; }
}

public class NextQuest : ModelElement
{
	public Ref<Faction> Faction { get; set; }

	public JobSeq[] Job { get; set; }

	public Ref<Quest> Quest { get; set; }
}

public class Transit : ModelElement
{
	public List<Destination> Destination { get; set; }
	public List<Complete> Complete { get; set; }
	public List<NotAcquire> NotAcquire { get; set; }
}

public class Destination : ModelElement
{
	public sbyte MissionStepId { get; set; }

	public sbyte ZoneIndex { get; set; }

	[Side(ReleaseSide.Client)]
	public string Kismet { get; set; }
}

public class Complete : ModelElement
{
	public sbyte ZoneIndex { get; set; }

	[Side(ReleaseSide.Client)]
	public string Kismet { get; set; }
}

public class NotAcquire : ModelElement
{
	public sbyte ZoneIndex { get; set; }

	[Side(ReleaseSide.Client)]
	public string Kismet { get; set; }
}