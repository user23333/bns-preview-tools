using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class QuestReward : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public enum QuestFirstProgressSeq
	{
		None,
		Y,
		N,
		COUNT,
	}

	public QuestFirstProgressSeq QuestFirstProgress { get; set; }

	public sbyte QuestCompletionCount { get; set; }

	public Op QuestCompletionCountOp { get; set; }

	public int BasicMoney { get; set; }

	public int BasicExp { get; set; }

	public int BasicAccountExp { get; set; }

	public sbyte BasicMasteryLevel { get; set; }

	public short BasicProductionExp { get; set; }

	public short BasicFactionReputation { get; set; }

	public short BasicGuildReputation { get; set; }

	public int BasicDuelPoint { get; set; }

	public int BasicPartyBattlePoint { get; set; }

	public int BasicFieldPlayPoint { get; set; }

	public Ref<Skill3>[] FixedSkill3 { get; set; }

	public Ref<ModelElement>[] FixedCommonSlot { get; set; }

	public sbyte[] FixedCommonItemCount { get; set; }

	public sbyte[] FixedCommonSkillVarIdx { get; set; }

	public Ref<ModelElement>[] OptionalCommonSlot { get; set; }

	public sbyte[] OptionalCommonItemCount { get; set; }
	#endregion
}