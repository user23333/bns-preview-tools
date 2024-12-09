using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public abstract class NpcTalkMessage : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<Text> Name2 { get; set; }

	public Ref<Faction> RequiredFaction { get; set; }

	public Ref<Quest> RequiredCompleteQuest { get; set; }

	public Ref<Text>[] StepText { get; set; }

	public Ref<Text>[] StepSubtext { get; set; }

	public Ref<Text>[] StepNext { get; set; }

	public string[] StepKismet { get; set; }

	public Ref<Cinematic>[] StepCinematic { get; set; }

	public ObjectPath[] StepShow { get; set; }

	public ObjectPath[] StepCameraShow { get; set; }

	public sbyte FunctionStep { get; set; }

	public Ref<Social> EndTalkSocial { get; set; }

	public string EndTalkSound { get; set; }

	public sealed class Branch : NpcTalkMessage
	{
		public bool InitialBranch { get; set; }

		[Name("branch-msg")]
		public Ref<NpcTalkMessage>[] BranchMsg { get; set; }
	}

	public sealed class Questmessage : NpcTalkMessage
	{
		public Ref<Social> EndTalkSocialQuestOk { get; set; }

		public string EndTalkSoundQuestOk { get; set; }
	}

	public sealed class Teleport : NpcTalkMessage { }

	public sealed class Craft : NpcTalkMessage
	{
		public CraftMessageTypeSeq CraftMessageType { get; set; }

		public enum CraftMessageTypeSeq
		{
			None,
			Join,
			Ask,
			Busy,
			Receive,
			NeedLevel,
			FullCraft,
			COUNT
		}
	}

	public sealed class FactionCoinExchange : NpcTalkMessage { }

	public sealed class Store : NpcTalkMessage
	{
		public StoreMessageTypeSeq StoreMessageType { get; set; }

		public enum StoreMessageTypeSeq
		{
			Sale,
			NotAuthority,
			COUNT
		}
	}

	public sealed class Warehouse : NpcTalkMessage { }

	public sealed class Auction : NpcTalkMessage { }

	public sealed class Delivery : NpcTalkMessage { }

	public sealed class MakeSummoned : NpcTalkMessage
	{
		public Ref<Social> EndTalkSocialOk { get; set; }
	}

	public sealed class SummonedBeautyShop : NpcTalkMessage { }

	public sealed class SummonedNameChange : NpcTalkMessage
	{
		public string EndTalkShowOk { get; set; }

		public string EndTalkSoundOk { get; set; }
	}

	public sealed class CreateGuild : NpcTalkMessage
	{
		public Ref<Social> EndTalkSocialOk { get; set; }

		public string EndTalkSoundOk { get; set; }
	}

	public sealed class JoinFaction : NpcTalkMessage
	{
		public PopulationStatisticsSeq PopulationStatistics { get; set; }

		public enum PopulationStatisticsSeq
		{
			None,
			Faction1High,
			Equal,
			Faction1Low,
			JoinedGuild,
			TransferCooltime,
			COUNT
		}

		public Ref<Social> EndTalkSocialOk { get; set; }

		public Ref<NpcTalkMessage> FailPopulationMessage { get; set; }
	}

	public sealed class TransferFaction : NpcTalkMessage
	{
		public PopulationStatisticsSeq PopulationStatistics { get; set; }

		public enum PopulationStatisticsSeq
		{
			None,
			Faction1High,
			Equal,
			Faction1Low,
			JoinedGuild,
			TransferCooltime,
			COUNT
		}

		public string EndTalkSoundOk { get; set; }

		public Ref<NpcTalkMessage> FailPopulationMessage { get; set; }
	}

	public sealed class ContributeGuildReputation : NpcTalkMessage { }

	public sealed class DungeonProgress : NpcTalkMessage
	{
		public Ref<Dungeon> Dungeon { get; set; }
	}

	public sealed class SelectJoinFaction : NpcTalkMessage
	{
		public PopulationStatisticsSeq PopulationStatistics { get; set; }

		public enum PopulationStatisticsSeq
		{
			None,
			Faction1High,
			Equal,
			Faction1Low,
			JoinedGuild,
			TransferCooltime,
			COUNT
		}

		public Ref<NpcTalkMessage>[] Msg { get; set; }

		public Ref<Faction>[] Faction { get; set; }
	}

	public sealed class GuildCustomize : NpcTalkMessage
	{
		public GuildCustomizeMessageTypeSeq GuildCustomizeMessageType { get; set; }

		public enum GuildCustomizeMessageTypeSeq
		{
			None,
			EnterCustomize,
			NotAuthority,
			ActivateFaction,
			WaitingArena,
			COUNT
		}
	}

	public sealed class JobChangeJoin : NpcTalkMessage { }

	public sealed class JobChangeShow : NpcTalkMessage { }

	public sealed class RandomoptionReset : NpcTalkMessage { }
	#endregion

	#region Helpers
	public struct NpcTalkMessageStep
	{
		public Ref<Text> Text { get; set; }

		public Ref<Text> Subtext { get; set; }

		public Ref<Text> Next { get; set; }

		public string Kismet { get; set; }

		public Ref<Cinematic> Cinematic { get; set; }

		public ObjectPath Show { get; set; }

		public ObjectPath CameraShow { get; set; }

		public readonly bool IsValid => Text.HasValue;
	}

	public NpcTalkMessageStep[] GetSteps()
	{
		var steps = new NpcTalkMessageStep[30];

		for (int i = 0; i < 30; i++)
		{
			steps[i] = new NpcTalkMessageStep()
			{
				Text = StepText[i],
				Subtext = StepSubtext[i],
				Next = StepNext[i],
				Kismet = StepKismet[i],
				Cinematic = StepCinematic[i],
				Show = StepShow[i],
				CameraShow = StepCameraShow[i]
			};
		}

		return steps;
	}
	#endregion
}