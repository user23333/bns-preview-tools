using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
[Side(ReleaseSide.Client)]
public class NpcTalkMessage : ModelElement
{
	#region Attributes	
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

		public Ref<NpcTalkMessage>[] BranchMsg { get; set; }
	}

	public sealed class Questmessage : NpcTalkMessage
	{
		public Ref<Social> EndTalkSocialQuestOk { get; set; }

		public string EndTalkSoundQuestOk { get; set; }

	}

	public sealed class Teleport : NpcTalkMessage
	{
	}

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

	public sealed class FactionCoinExchange : NpcTalkMessage
	{
	}

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

	public sealed class Warehouse : NpcTalkMessage
	{
	}

	public sealed class Auction : NpcTalkMessage
	{
	}

	public sealed class Delivery : NpcTalkMessage
	{
	}

	public sealed class MakeSummoned : NpcTalkMessage
	{
		public Ref<Social> EndTalkSocialOk { get; set; }
	}

	public sealed class SummonedBeautyShop : NpcTalkMessage
	{
	}

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
		//public PopulationStatistics PopulationStatistics { get; set; }

		public Ref<Social> EndTalkSocialOk { get; set; }

		public Ref<NpcTalkMessage> FailPopulationMessage { get; set; }
	}

	public sealed class TransferFaction : NpcTalkMessage
	{
		//public PopulationStatistics PopulationStatistics { get; set; }

		public string EndTalkSoundOk { get; set; }

		public Ref<NpcTalkMessage> FailPopulationMessage { get; set; }
	}

	public sealed class ContributeGuildReputation : NpcTalkMessage
	{
	}

	public sealed class DungeonProgress : NpcTalkMessage
	{
		public Ref<Dungeon> Dungeon { get; set; }
	}

	public sealed class SelectJoinFaction : NpcTalkMessage
	{
		//public PopulationStatistics PopulationStatistics { get; set; }

		public Ref<NpcTalkMessage>[] Msg { get; set; }

		public Ref<Faction>[] Faction { get; set; }
	}

	public sealed class GuildCustomize : NpcTalkMessage
	{
		//public GuildCustomizeMessageType GuildCustomizeMessageType { get; set; }
	}
	#endregion
}