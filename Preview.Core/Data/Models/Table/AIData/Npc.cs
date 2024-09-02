using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class Npc : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public string Name { get; set; }

	public string Title { get; set; }

	public Ref<Text> Name2 { get; set; }

	public Ref<Text> Title2 { get; set; }

	public Ref<BossNpc> BossNpc { get; set; }

	public bool SoulNpc { get; set; }

	//public BossType BossType { get; set; }

	public bool WriteCombatLog { get; set; }

	public Distance Radius { get; set; }

	public short Scale { get; set; }

	public Velocity WalkSpeed { get; set; }

	public Velocity RunSpeed { get; set; }

	//public NeutralHostile NeutralHostile { get; set; }

	public bool Offensive { get; set; }

	//public SkillAttribute[] ImmuneSkillAttribute { get; set; }

	public EffectAttributeSeq[] ImmuneEffectAttribute { get; set; }

	public EffectAttributeSeq[] ImmuneBreakerAttribute { get; set; }

	public sbyte ImmuneBreakerCount { get; set; }

	public Msec ImmuneBreakerInitDuration { get; set; }

	public ObjectPath[] ImmuneBreakerShow { get; set; }

	public Ref<Store> Store { get; set; }

	public Ref<StoreByItem> StoreByItem { get; set; }

	public Ref<Store2>[] Store2 { get; set; }

	public Ref<Teleport> Teleport { get; set; }

	public bool Repairer { get; set; }

	//public Production Production { get; set; }

	public bool Market { get; set; }

	public bool PostOffice { get; set; }

	public bool ExchangeFactionScore { get; set; }

	public short ExchangeFactionScoreMaxFactionLevel { get; set; }

	public sbyte ExchangeFactionScoreMultiplyValue { get; set; }

	public bool JoinLeaveFaction { get; set; }

	public bool TransferFaction { get; set; }

	public bool Depot { get; set; }

	public bool Guild { get; set; }

	public int MaxFactionScore { get; set; }

	public bool EnableCoOwnershipPouch { get; set; }

	public int RewardFactionScore { get; set; }

	public sbyte PeaktimeRewardSunStartHour { get; set; }

	public sbyte PeaktimeRewardSunEndHour { get; set; }

	public sbyte PeaktimeRewardMonStartHour { get; set; }

	public sbyte PeaktimeRewardMonEndHour { get; set; }

	public sbyte PeaktimeRewardTueStartHour { get; set; }

	public sbyte PeaktimeRewardTueEndHour { get; set; }

	public sbyte PeaktimeRewardWedStartHour { get; set; }

	public sbyte PeaktimeRewardWedEndHour { get; set; }

	public sbyte PeaktimeRewardThuStartHour { get; set; }

	public sbyte PeaktimeRewardThuEndHour { get; set; }

	public sbyte PeaktimeRewardFriStartHour { get; set; }

	public sbyte PeaktimeRewardFriEndHour { get; set; }

	public sbyte PeaktimeRewardSatStartHour { get; set; }

	public sbyte PeaktimeRewardSatEndHour { get; set; }

	public Ref<ContributionReward> ContributionReward { get; set; }

	public Ref<ContributionReward> ContributionDayRewardSun { get; set; }

	public Ref<ContributionReward> ContributionDayRewardMon { get; set; }

	public Ref<ContributionReward> ContributionDayRewardTue { get; set; }

	public Ref<ContributionReward> ContributionDayRewardWed { get; set; }

	public Ref<ContributionReward> ContributionDayRewardThu { get; set; }

	public Ref<ContributionReward> ContributionDayRewardFri { get; set; }

	public Ref<ContributionReward> ContributionDayRewardSat { get; set; }

	public sbyte ContributionPeaktimeRewardSunStartHour { get; set; }

	public sbyte ContributionPeaktimeRewardSunEndHour { get; set; }

	public sbyte ContributionPeaktimeRewardMonStartHour { get; set; }

	public sbyte ContributionPeaktimeRewardMonEndHour { get; set; }

	public sbyte ContributionPeaktimeRewardTueStartHour { get; set; }

	public sbyte ContributionPeaktimeRewardTueEndHour { get; set; }

	public sbyte ContributionPeaktimeRewardWedStartHour { get; set; }

	public sbyte ContributionPeaktimeRewardWedEndHour { get; set; }

	public sbyte ContributionPeaktimeRewardThuStartHour { get; set; }

	public sbyte ContributionPeaktimeRewardThuEndHour { get; set; }

	public sbyte ContributionPeaktimeRewardFriStartHour { get; set; }

	public sbyte ContributionPeaktimeRewardFriEndHour { get; set; }

	public sbyte ContributionPeaktimeRewardSatStartHour { get; set; }

	public sbyte ContributionPeaktimeRewardSatEndHour { get; set; }

	public Ref<Reward> PersonalDroppedPouchReward { get; set; }

	public Ref<Reward>[] PersonalDroppedPouchRewardDifficultyType { get; set; }

	public Ref<NpcSealedDungeonReward> RewardSealedDungeon { get; set; }

	public Ref<FieldItemDrop> Fielditemdrop { get; set; }

	public Ref<FieldItemDrop> DeadbodyFielditemdrop { get; set; }

	public Msec DeadbodyPickupDuration { get; set; }

	public bool Burrow { get; set; }

	public Msec ManipulateDuration { get; set; }

	public ObjectPath CasterManipulateAnimname { get; set; }

	public ObjectPath TargetManipulateAnimname { get; set; }

	public ObjectPath CasterManipulateShow { get; set; }

	public ObjectPath TargetManipulateShow { get; set; }

	public Ref<Text> ActionName { get; set; }

	public Ref<Text> ActionDesc { get; set; }

	public Ref<Zone> ManipulateTransitZone { get; set; }

	public ForwardingType[] ForwardingTypes { get; set; }

	public Ref<Quest>[] Quests { get; set; }

	public sbyte[] Missions { get; set; }

	public sbyte[] Cases { get; set; }

	public short[] CaseSubtypes { get; set; }

	public ForwardingType[] LootForwardingTypes { get; set; }

	public Ref<Quest>[] LootQuests { get; set; }

	public sbyte[] LootMissions { get; set; }

	public sbyte[] LootCases { get; set; }

	public Ref<Item>[] LootItem { get; set; }

	public ForwardingType[] FinishBlowForwardingTypes { get; set; }

	public Ref<Quest>[] FinishBlowQuests { get; set; }

	public sbyte[] FinishBlowMissions { get; set; }

	public sbyte[] FinishBlowCases { get; set; }

	public float WalkRatescale { get; set; }

	public float RunRatescale { get; set; }

	public Ref<CreatureAppearance> Appearance { get; set; }

	public ObjectPath Animset { get; set; }

	public ObjectPath FaceAnimset { get; set; }

	public bool Talk { get; set; }

	public sbyte Iconindex { get; set; }

	public float MeshScale { get; set; }

	public sbyte PRadius { get; set; }

	public short NamePosDiff { get; set; }

	public short NamePosStand { get; set; }

	public short NamePosSit { get; set; }

	public short NamePosDead { get; set; }

	public string StartTalkAction { get; set; }

	public string EndTalkAction { get; set; }

	public string SpawnProduction { get; set; }

	public string DeadProduction { get; set; }

	public bool FourLeg { get; set; }

	public ObjectPath DespawnShowdata { get; set; }

	public RaceSeq Race { get; set; }

	public SexSeq Sex { get; set; }

	public JobSeq Job { get; set; }

	public StanceSeq Stance { get; set; }

	public sbyte Level { get; set; }

	public sbyte MasteryLevel { get; set; }

	public Ref<Faction> Faction { get; set; }

	public bool Attackable { get; set; }

	public bool DetectHiding { get; set; }

	//public Ref<NpcMoveAnim> DefaultIdle { get; set; }

	public bool WarfareBoss { get; set; }

	public long MaxHp { get; set; }

	public short MaxSp { get; set; }

	public short ModifyCastSpeedPercent { get; set; }

	public long HpRegen { get; set; }

	public int HpRegenCombat { get; set; }

	public short AttackHitBasePercent { get; set; }

	public short AttackHitValue { get; set; }

	public short AttackPierceBasePercent { get; set; }

	public short AttackParryPiercePercent { get; set; }

	public short AttackPierceValue { get; set; }

	public short AttackConcentrateValue { get; set; }

	public short AttackCriticalBasePercent { get; set; }

	public short AttackCriticalDamagePercent { get; set; }

	public int AttackCriticalValue { get; set; }

	public int AttackCriticalDamageValue { get; set; }

	public short DefendCriticalBasePercent { get; set; }

	public short DefendCriticalDamagePercent { get; set; }

	public int DefendCriticalValue { get; set; }

	public short DefendBouncePercent { get; set; }

	public short DefendDodgeBasePercent { get; set; }

	public short DefendDodgeValue { get; set; }

	public short DefendParryBasePercent { get; set; }

	public short DefendParryValue { get; set; }

	public short DefendParryReducePercent { get; set; }

	public short DefendParryReduceDiff { get; set; }

	public short DefendPerfectParryBasePercent { get; set; }

	public short DefendPerfectParryReducePercent { get; set; }

	public short DefendCounterReducePercent { get; set; }

	public short DefendImmuneBasePercent { get; set; }

	public int AttackPowerCreatureMin { get; set; }

	public int AttackPowerCreatureMax { get; set; }

	public short AttackPowerEquipMin { get; set; }

	public short AttackPowerEquipMax { get; set; }

	public int DefendPowerCreatureValue { get; set; }

	public int DefendPowerEquipValue { get; set; }

	public short DefendResistPowerCreatureValue { get; set; }

	public short DefendResistPowerEquipValue { get; set; }

	public short DefendPhysicalDamageReducePercent { get; set; }

	public short DefendForceDamageReducePercent { get; set; }

	public short AttackDamageModifyPercent { get; set; }

	public int AttackDamageModifyDiff { get; set; }

	public short DefendDamageModifyPercent { get; set; }

	public int DefendDamageModifyDiff { get; set; }

	public short DefendMissBasePercent { get; set; }

	public short AttackStiffDurationBasePercent { get; set; }

	public short AttackStiffDurationValue { get; set; }

	public short DefendStiffDurationBasePercent { get; set; }

	public short DefendStiffDurationValue { get; set; }

	public short CastDurationBasePercent { get; set; }

	public short CastDurationValue { get; set; }

	[Name("job-ability-1")]
	public int JobAbility1 { get; set; }

	[Name("job-ability-2")]
	public int JobAbility2 { get; set; }

	public short HealPowerBasePercent { get; set; }

	public short AoeDefendBasePercent { get; set; }

	public short AoeDefendPowerValue { get; set; }

	public short HateBasePercent { get; set; }

	public short HatePowerCreatureValue { get; set; }

	public int AbnormalAttackPowerValue { get; set; }

	public short AbnormalAttackBasePercent { get; set; }

	public short AbnormalDefendPowerValue { get; set; }

	public short AbnormalDefendBasePercent { get; set; }

	public short AbnormalAttackPowerModify { get; set; }

	public short AbnormalDefendPowerModify { get; set; }

	public short HatePowerModify { get; set; }

	public short HealPowerModify { get; set; }

	public short AoeDefendPowerModify { get; set; }

	public short AttackHitValueModify { get; set; }

	public short AttackCriticalValueModify { get; set; }

	public short DefendCriticalValueModify { get; set; }

	public short DefendDodgeValueModify { get; set; }

	public short DefendParryValueModify { get; set; }

	public short DefendPhysicalValueModify { get; set; }

	public short DefendForceValueModify { get; set; }

	public short AttackStiffDurationValueModify { get; set; }

	public short DefendStiffDurationValueModify { get; set; }

	public short CastDurationValueModify { get; set; }

	public short AttackCriticalDamageModify { get; set; }

	public short DefendCriticalDamageModify { get; set; }

	public short AttackPierceModify { get; set; }

	public short AttackParryPierceModify { get; set; }

	public short DefendParryReduceModify { get; set; }

	public short AttackPerfectParryDamageModify { get; set; }

	public short DefendPerfectParryReduceModify { get; set; }

	public short AttackCounterDamageModify { get; set; }

	public short DefendCounterReduceModify { get; set; }

	//public Grade2 Grade2 { get; set; }

	public string GradeImageset { get; set; }

	public Ref<Text> GradeTooltipText { get; set; }

	//public BossUiType BossUiType { get; set; }

	public sbyte BossGroupId { get; set; }

	//public BossSlot BossSlot { get; set; }

	public bool BossKillHideSlot { get; set; }

	public Ref<GameMessage> BossSpawnMessage { get; set; }

	//public Icon BossIcon { get; set; }

	//public Icon BossAggroIcon { get; set; }

	public string BossAggroIndicator { get; set; }

	public string BossAggroTwinIndicator { get; set; }

	public ObjectPath EndTalkSound { get; set; }

	public Ref<Social> EndTalkSocial { get; set; }

	public ObjectPath BurrowScanedMark { get; set; }

	public ObjectPath BurrowVisualEffect { get; set; }

	public bool UseFootPrint { get; set; }

	public bool UseWaterPrint { get; set; }

	public bool DefaultVisible { get; set; }

	//public MoveType MoveType { get; set; }

	//public SizeType SizeType { get; set; }

	public string Description { get; set; }

	public Ref<Text> Description2 { get; set; }

	public Ref<Text> DyingMessage { get; set; }

	//public RoleType RoleType { get; set; }

	public Ref<Text> DieShout { get; set; }

	//public RaceType RaceType { get; set; }

	public Ref<IndicatorSocial> Indicator { get; set; }

	public bool NeutralFactionNameplateEnemy { get; set; }

	public Ref<NpcResponse>[] BannedResponse { get; set; }

	public Ref<NpcResponse>[] Response { get; set; }

	public Ref<Social>[] StandSocial { get; set; }

	public short[] StandSocialRate { get; set; }
	public bool DieKnockback { get; set; }

	public bool InvokeFxMsg { get; set; }

	public bool DisableNameSpawn { get; set; }

	public bool PlayAdditionalDie { get; set; }

	public Ref<SummonedBeautyShop> SummonedBeautyShop { get; set; }

	//public Ref<Boast> Boast { get; set; }

	public bool AlwaysShowHp { get; set; }

	public bool UseMapUnitGroup { get; set; }

	public Ref<MapUnit>[] MapUnit { get; set; }

	public bool UseImmediateLoad { get; set; }

	//public GhostType GhostType { get; set; }

	public ObjectPath GhostmodeBeyondStartShow { get; set; }

	public ObjectPath GhostmodeBeyondEndShow { get; set; }

	public sbyte SoulNpcSkillLevel { get; set; }

	public short BossChallengeAttractionScore { get; set; }

	public bool IsMysteriousStore { get; set; }

	public Ref<NewbieCare> Newbiecare { get; set; }

	public int AttackAttributeValue { get; set; }

	public short AttackAttributeBasePercent { get; set; }

	public short AttackAttributeModify { get; set; }

	public bool HideNameplate { get; set; }

	public short NameplateHeightModify { get; set; }

	public short AttackAoePierceValue { get; set; }

	public short AttackAbnormalHitBasePercent { get; set; }

	public short AttackAbnormalHitValue { get; set; }

	public short DefendAbnormalDodgeBasePercent { get; set; }

	public short DefendAbnormalDodgeValue { get; set; }

	public short SupportPowerBasePercent { get; set; }

	public short SupportPowerValue { get; set; }

	public int HealPowerValue { get; set; }

	public short HypermovePowerValue { get; set; }

	public short RAttackAoePierceModify { get; set; }

	public short RAttackAbnormalHitModify { get; set; }

	public short RDefendAbnormalDodgeModify { get; set; }

	public short RSupportPowerModify { get; set; }

	public bool JobChangeEnterZone { get; set; }

	public bool JobChange { get; set; }

	//public RaceType2 RaceType2 { get; set; }

	//public AttributeType AttributeType { get; set; }

	public int FatigabilityConsumeAmount { get; set; }

	public short ReleaseContentsGroup { get; set; }
	#endregion

	#region Properties
	public string Map
	{
		get
		{
			var alias = this.ToString();

			var MapUnit = Provider.GetTable<MapUnit>().Where(x => x.ToString() != null && x.ToString().Contains(alias, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
			return MapUnit is null ? null : Provider.GetTable<MapInfo>().FirstOrDefault(x => x.Id == MapUnit.Mapid)?.Name;
		}
	}
	#endregion

	#region Methods
	protected override void LoadHiddenField()
	{
		string alias = this.Attributes.Get<string>("alias");
		var comparer = StringComparison.OrdinalIgnoreCase;

		if (this.Attributes["brain"] is null)
		{
			string BrainInfo;
			if (this.Attributes["boss-npc"] != null) BrainInfo = "Boss";
			else if (alias.StartsWith("CH_", comparer) || alias.StartsWith("CE_", comparer)) BrainInfo = "Citizen";
			else if (alias.StartsWith("MH_", comparer) || alias.StartsWith("ME_", comparer)) BrainInfo = "Monster";
			else return;

			this.Attributes["brain"] = BrainInfo;
			this.Attributes["brain-parameters"] = alias + "_bp";
		}

		if (this.Attributes["formal-radius"] is null)
		{
			if (this.Attributes["radius"] is not null)
				this.Attributes["formal-radius"] = this.Attributes["radius"];
		}
	}

	//public short AbnormalAttackPowerModify { get; set; }
	//public short AbnormalDefendPowerModify { get; set; }
	//public short HatePowerModify { get; set; }
	//public short HealPowerModify { get; set; }
	//public short AoeDefendPowerModify { get; set; }
	//public short AttackHitValueModify { get; set; }
	//public short AttackCriticalValueModify { get; set; }
	//public short DefendCriticalValueModify { get; set; }
	//public short DefendDodgeValueModify { get; set; }
	//public short DefendParryValueModify { get; set; }
	//public short DefendPhysicalValueModify { get; set; }
	//public short DefendForceValueModify { get; set; }
	//public short AttackStiffDurationValueModify { get; set; }
	//public short DefendStiffDurationValueModify { get; set; }
	//public short CastDurationValueModify { get; set; }
	//public short AttackCriticalDamageModify { get; set; }
	//public short DefendCriticalDamageModify { get; set; }
	//public short AttackPierceModify { get; set; }
	//public short AttackParryPierceModify { get; set; }
	//public short DefendParryReduceModify { get; set; }
	//public short AttackPerfectParryDamageModify { get; set; }
	//public short DefendPerfectParryReduceModify { get; set; }
	//public short AttackCounterDamageModify { get; set; }
	//public short DefendCounterReduceModify { get; set; }

	public Creature AbilityTest()
	{
		var creature = new Creature()
		{
			Name = Name2.GetText(),
			Level = Level,
			MasteryLevel = MasteryLevel,

			MaxHp = MaxHp,
			MaxSp = MaxSp,
			//Speed = Speed,
			ModifyCastSpeedPercent = ModifyCastSpeedPercent,
			HpRegen = HpRegen,
			HpRegenCombat = HpRegenCombat,
			AttackHitBasePercent = AttackHitBasePercent,
			AttackHitValue = AttackHitValue,
			AttackPierceBasePercent = AttackPierceBasePercent,   //AttackPierceModify
			AttackParryPiercePercent = AttackParryPiercePercent, //AttackParryPierceModify
			AttackPierceValue = AttackPierceValue,
			AttackCriticalBasePercent = AttackCriticalBasePercent,
			AttackCriticalDamagePercent = AttackCriticalDamagePercent,
			AttackCriticalValue = AttackCriticalValue,
			AttackCriticalDamageValue = AttackCriticalDamageValue,
			DefendCriticalBasePercent = DefendCriticalBasePercent,
			DefendCriticalDamagePercent = DefendCriticalDamagePercent,
			DefendCriticalValue = DefendCriticalValue,
			DefendCriticalDamageValue = DefendCriticalDamageModify,
			DefendBouncePercent = DefendBouncePercent,
			DefendDodgeBasePercent = DefendDodgeBasePercent,
			DefendDodgeValue = DefendDodgeValue, //DefendDodgeValueModify?
			DefendParryBasePercent = DefendParryBasePercent,
			DefendParryValue = DefendParryValue, //DefendParryValueModify?
			DefendParryReducePercent = DefendParryReducePercent,
			DefendParryReduceDiff = DefendParryReduceDiff,
			DefendPerfectParryBasePercent = DefendPerfectParryBasePercent,
			DefendImmuneBasePercent = DefendImmuneBasePercent,
			AttackPowerCreatureMin = AttackPowerCreatureMin,
			AttackPowerCreatureMax = AttackPowerCreatureMax,
			AttackPowerEquipMin = AttackPowerEquipMin,
			AttackPowerEquipMax = AttackPowerEquipMax,
			DefendPowerCreatureValue = DefendPowerCreatureValue,
			DefendPowerEquipValue = DefendPowerEquipValue,
			DefendResistPowerCreatureValue = DefendResistPowerCreatureValue,
			DefendResistPowerEquipValue = DefendResistPowerEquipValue,
			DefendPhysicalDamageReducePercent = DefendPhysicalDamageReducePercent,
			DefendForceDamageReducePercent = DefendForceDamageReducePercent,
			AttackDamageModifyPercent = AttackDamageModifyPercent,
			AttackDamageModifyDiff = AttackDamageModifyDiff,
			DefendDamageModifyPercent = DefendDamageModifyPercent,
			DefendDamageModifyDiff = DefendDamageModifyDiff,
			DefendMissBasePercent = DefendMissBasePercent,
			AttackStiffDurationBasePercent = AttackStiffDurationBasePercent,
			AttackStiffDurationValue = (short)(AttackStiffDurationValue + AttackStiffDurationValueModify),
			DefendStiffDurationBasePercent = DefendStiffDurationBasePercent,
			DefendStiffDurationValue = (short)(DefendStiffDurationValue + DefendStiffDurationValueModify),
			CastDurationBasePercent = CastDurationBasePercent,
			CastDurationValue = (short)(CastDurationValue + CastDurationValueModify),
			AttackConcentrateValue = AttackConcentrateValue,
			DefendPerfectParryReducePercent = DefendPerfectParryReducePercent,
			DefendCounterReducePercent = DefendCounterReducePercent,
			JobAbility1 = JobAbility1,
			JobAbility2 = JobAbility2,
			HealPowerBasePercent = HealPowerBasePercent,
			HealPowerValue = HealPowerValue + HealPowerModify,
			//HealPowerDiff = HealPowerDiff,
			AoeDefendBasePercent = AoeDefendBasePercent,
			AoeDefendPowerValue = AoeDefendPowerValue,
			AbnormalAttackBasePercent = AbnormalAttackBasePercent,
			AbnormalAttackPowerValue = AbnormalAttackPowerValue,
			AbnormalDefendBasePercent = AbnormalDefendBasePercent,
			AbnormalDefendPowerValue = (short)(AbnormalDefendPowerValue + AbnormalDefendPowerModify),
			HateBasePercent = HateBasePercent,
			HatePowerCreatureValue = (short)(HatePowerCreatureValue + HatePowerModify),
			AttackAttributeValue = AttackAttributeValue + AttackAttributeModify,
			AttackAttributeBasePercent = AttackAttributeBasePercent,
		};


		Console.WriteLine(BossNpc.Instance?.BerserkSequenceInvokeTime);

		var level = this.Level;
		var masterylevel = this.MasteryLevel;

		Console.WriteLine(this.Name2.GetText() + $" ({level})");

		creature.TestMethod();

		_ = this.AttackPerfectParryDamageModify;
		_ = this.AttackAoePierceValue;

		_ = this.AttackConcentrateValue;
		_ = this.AttackCounterDamageModify;

		Console.WriteLine($"parry reduce {this.DefendParryReduceModify + this.DefendParryReduceDiff} → " +
			$"{AbilityFunction.DefendParryReducePercent.GetPercent(this.DefendParryReduceModify + this.DefendParryReduceDiff, level, this.DefendParryReducePercent):P3}");

		Console.WriteLine($"perfect parry reduce {this.DefendPerfectParryReduceModify} → " +
			$"{AbilityFunction.DefendPerfectParryReducePercent.GetPercent(this.DefendPerfectParryReduceModify, level, this.DefendPerfectParryReducePercent):P3}");

		Console.WriteLine($"defend-counter {this.DefendCounterReduceModify} → " +
			$"{AbilityFunction.DefendCounterReducePercent.GetPercent(this.DefendCounterReduceModify, level, this.DefendCounterReducePercent):P3}");

		return creature;
	}
	#endregion
}