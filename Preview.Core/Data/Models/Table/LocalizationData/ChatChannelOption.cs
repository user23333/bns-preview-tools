namespace Xylia.Preview.Data.Models;
public sealed class ChatChannelOption : ModelElement
{
	#region Attributes
	public long Id { get; set; }

	public CategorySeq Category { get; set; }

	public enum CategorySeq
	{
		None,
		ChatSight,
		ChatParty,
		ChatTeam,
		ChatTeamLeader,
		ChatOne,
		ChatInDomain,
		ChatWorld,
		ChatFaction,
		ChatGuild,
		ChatGuildManager,
		ChatPartySearch,
		ChatNpc,
		ChatGodsayNormal,
		ChatGodsayCampaign,
		ChatGodsayEmergency,
		ChatFieldDungeon,
		ChatQqC2c,
		ChatQqGroup,
		ChatGuildSearch,
		ChatWatch,
		ChatFriends,
		Default,
		Warning,
		Info,
		Party,
		PartyMatch,
		Team,
		Faction,
		Guild,
		GuildMatch,
		Exhaustion,
		ExhaustionPc,
		ExpAcquisition,
		ExpLoss,
		Levelup,
		MoneyAcquisition,
		MoneyLoss,
		ItemAcquisition,
		ItemLoss,
		SkillBuildUpPointAcquisition,
		QuestAcquisition,
		QuestComplete,
		TalkSocial,
		FieldDungeon,
		Qq,
		CombatNormal,
		CombatCritical,
		CombatHeal,
		CombatDefend,
		CombatParry,
		CombatAbnormal,
		CombatAttackedNormal,
		CombatAttackedCritical,
		CombatTargetHeal,
		CombatTargetDefend,
		CombatTargetAbnormal,
		CombatOtherNormal,
		CombatOtherCritical,
		CombatOtherHeal,
		CombatOtherDefend,
		CombatOtherAbnormal,
		CombatPartyNormal,
		CombatPartyCritical,
		CombatPartyHeal,
		CombatPartyDefend,
		CombatPartyAbnormal,
		CombatPartyAttackedNormal,
		CombatPartyAttackedCritical,
		CombatPartyTargetDefend,
		Mentoring,
		Skill,
		COUNT
	}

	public sbyte FontColorIndex { get; set; }

	public GroupSeq Group { get; set; }

	public enum GroupSeq
	{
		None,
		Chat,
		Chat2,
		System,
		Combat,
		COUNT
	}

	public bool Modifiable { get; set; }
	#endregion
}