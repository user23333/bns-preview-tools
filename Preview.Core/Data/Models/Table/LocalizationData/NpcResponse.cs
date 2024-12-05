namespace Xylia.Preview.Data.Models;
public sealed class NpcResponse : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public FactionCheckTypeSeq FactionCheckType { get; set; }

	public enum FactionCheckTypeSeq
	{
		Is,
		IsNot,
		IsNone,
		COUNT
	}

	public Ref<Faction>[] Faction { get; set; }

	public Ref<Quest> RequiredCompleteQuest { get; set; }

	public FactionLevelCheckTypeSeq FactionLevelCheckType { get; set; }

	public enum FactionLevelCheckTypeSeq
	{
		None,
		CheckForSuccess,
		CheckForFail,
		COUNT
	}

	public Ref<NpcTalkMessage> TalkMessage { get; set; }

	public Ref<IndicatorSocial> IndicatorSocial { get; set; }

	public Ref<Social> ApproachSocial { get; set; }

	public Ref<IndicatorIdle> Idle { get; set; }

	public bool IdleVisible { get; set; }

	public Ref<Social> IdleStart { get; set; }

	public Ref<Social> IdleEnd { get; set; }
	#endregion
}