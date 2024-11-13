using Xylia.Preview.Common.Attributes;

namespace Xylia.Preview.Data.Models;
public sealed class RaidDungeon : ModelElement, IAttraction
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public Ref<Text> Name2 { get; set; }

	public DungeonTypeSeq DungeonType { get; set; }

	public enum DungeonTypeSeq
	{
		Unbind,
		Bind,
		COUNT
	}

	public Ref<AttractionGroup> Group { get; set; }

	public sbyte MaxUnbindCount { get; set; }

	public int ResetMoney { get; set; }

	public Ref<Item>[] ResetItem { get; set; }

	public sbyte[] ResetItemCount { get; set; }

	public bool UsePersonalBinding { get; set; }

	public PersonalBindingSlotSeq PersonalBindingSlot { get; set; }

	public enum PersonalBindingSlotSeq
	{
		None,
		[Name("slot-1")]
		Slot1,
		[Name("slot-2")]
		Slot2,
		[Name("slot-3")]
		Slot3,
		COUNT
	}

	public short PcMax { get; set; }

	public sbyte RequiredLevel { get; set; }

	public sbyte RequiredMasteryLevel { get; set; }

	public Ref<Quest>[] RequiredPrecedingQuest { get; set; }

	public RequiredPrecedingQuestCheckSeq RequiredPrecedingQuestCheck { get; set; }

	public enum RequiredPrecedingQuestCheckSeq
	{
		And,
		Or,
		COUNT
	}

	public Ref<Quest>[] AttractionQuest { get; set; }

	public bool EnableHeartCount { get; set; }

	public sbyte MaxInstantHeartCount { get; set; }

	public Ref<Effect>[] Effect { get; set; }

	public sbyte StepCount { get; set; }

	public Ref<Zone>[] Zone { get; set; }

	public Ref<Npc>[] BossNpc { get; set; }

	public sbyte[] ZoneIndex { get; set; }

	public Ref<Text> DungeonTapName2 { get; set; }

	public Ref<Text>[] StepName2 { get; set; }

	public string[] StepImage { get; set; }

	public string ArenaMinimap { get; set; }

	public Ref<Text> RaidDungeonDesc { get; set; }

	public sbyte UiTextGrade { get; set; }

	public Ref<AttractionRewardSummary> RewardSummary { get; set; }

	public sbyte RecommandLevelMin { get; set; }

	public sbyte RecommandLevelMax { get; set; }

	public sbyte RecommandMasteryLevelMin { get; set; }

	public sbyte RecommandMasteryLevelMax { get; set; }

	public short RecommendAttackPower { get; set; }

	public Ref<Item> StandardGearWeapon { get; set; }

	public Ref<Quest>[] DisplayQuests { get; set; }

	public Ref<Text> Tactic { get; set; }

	public Ref<ContentsJournalRecommendItem> RecommendAlias { get; set; }

	public Ref<ContentsReset> ContentsReset { get; set; }

	public Ref<Npc>[] BossNpcAlias { get; set; }

	public Ref<Text>[] BossNpcSection { get; set; }
	#endregion

	#region IAttraction
	public string Name => this.Name2.GetText();

	public string Description => this.RaidDungeonDesc.GetText();
	#endregion
}