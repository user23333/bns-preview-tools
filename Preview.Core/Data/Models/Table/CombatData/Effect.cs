using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class Effect : ModelElement, IHaveName
{
	#region Attributes
	public Ref<Text> Name2 { get; set; }

	public Ref<Text> Name3 { get; set; }

	public short Level { get; set; }

	public bool ShowIcon { get; set; }

	public bool ShowLeftTime { get; set; }

	public bool ShowLinkbar { get; set; }

	public ObjectPath TargetIndicatorIcon { get; set; }

	public bool TargetIndicatorShow { get; set; }

	public bool ShowJobIndicator { get; set; }

	public bool SaveDb { get; set; }

	public sbyte MissProbability { get; set; }

	public Msec PassiveDuration { get; set; }

	public Msec PassiveInterval { get; set; }

	public int ExpirationDurationSec { get; set; }

	public sbyte StackAmount { get; set; }

	public sbyte StackCount { get; set; }

	public bool ReattachEffectAfterChangingStackCount { get; set; }

	public Ref<Effect> TransformEffect { get; set; }

	//public BuffType { get; set; }

	//public BindingSkillStepType { get; set; }

	//public ImmuneBreakerAttribute { get; set; }

	//public EffectAttribute[] Attribute { get; set; }

	public long AttributeValue { get; set; }

	[Name("attribute-value-2")]
	public long AttributeValue2 { get; set; }

	[Name("attribute-value-3")]
	public long AttributeValue3 { get; set; }

	//public ImmuneAttribute[] ImmuneAttribute { get; set; }

	public long ImmuneAttributeValue { get; set; }

	[Name("immune-attribute-value-2")]
	public long ImmuneAttributeValue2 { get; set; }

	[Name("immune-attribute-value-3")]
	public long ImmuneAttributeValue3 { get; set; }

	public EffectFlag[] Flag { get; set; }

	public EffectFlag MaxStackFlag { get; set; }

	//public EffectFunction[] Function { get; set; }

	public bool DropFieldItem { get; set; }

	public bool DropWeaponFieldItem { get; set; }

	public sbyte DetachCount { get; set; }

	//public DetachSlot[] DetachSlot { get; set; }

	public bool ApplyDurationFormula { get; set; }

	public bool ChangeDefaultStanceByDetachTimeout { get; set; }

	public bool TargetCombatMode { get; set; }

	public bool AoeDamage { get; set; }

	public short AttackAttributeCoefficientPercent { get; set; }



	//public [] EventEffectTarget { get; set; }

	//public Ref<Effect>[] EventEffect { get; set; }

	//public [] SecondSlotEventEffectTarget { get; set; }

	//public Ref<Effect>[] SecondSlotEventEffect { get; set; }

	//public [] ThirdSlotEventEffectTarget { get; set; }

	//public Ref<Effect>[] ThirdSlotEventEffect { get; set; }

	//public [] FourthSlotEventEffectTarget { get; set; }

	//public Ref<Effect>[] FourthSlotEventEffect { get; set; }

	public CreatureField[] ModifyAbility { get; set; }

	public long[] ModifyAbilityDiff { get; set; }

	public short[] ModifyAbilityPercent { get; set; }

	//public PassiveMoveanimIdle { get; set; }

	public bool PauseAutoTargeting { get; set; }

	public bool NotTargetable { get; set; }

	public bool IsDotEffect { get; set; }

	public bool NoCriticalHit { get; set; }

	public bool PartyBroadcast { get; set; }

	//public UiSlot { get; set; }

	//public UiCategory { get; set; }

	//public bool UseExtraSkillStackCount { get; set; }

	//public [] CombatJob { get; set; }

	public bool UiDifficult { get; set; }

	public bool IsTransformEffect { get; set; }

	public bool IsReuseStandbyEffect { get; set; }

	//public ItemType { get; set; }

	//public bool IsPowergaugeEffect { get; set; }

	//public bool IgnoreHideBuffGraphEffect { get; set; }

	//public bool IsBattleRoyalFieldPcInfo { get; set; }

	//public BattleRoyalFieldEffectPouchGroup { get; set; }

	//public GroceryEffectType { get; set; }

	public short GroceryEffectLevel { get; set; }

	public bool KnockbackJump { get; set; }

	public sbyte Idleanimpriority { get; set; }

	public ObjectPath NormalComponent { get; set; }

	public ObjectPath CriticalComponent { get; set; }

	public ObjectPath BackNormalComponent { get; set; }

	public ObjectPath BackCriticalComponent { get; set; }

	public ObjectPath BuffContinuanceComponent { get; set; }

	public ObjectPath ImmuneBuffComponent { get; set; }

	public ObjectPath DetachShow { get; set; }

	public ObjectPath DispelShow { get; set; }

	public bool PlayDetachShowHide { get; set; }

	public ObjectPath DieShow { get; set; }

	public ObjectPath AdditionalDieShow { get; set; }

	public ObjectPath ExhaustionShow { get; set; }

	public ObjectPath ImmunedShow { get; set; }

	public ObjectPath CasterShow { get; set; }

	public ObjectPath FireShow { get; set; }

	public bool AnimationFreeze { get; set; }

	public Ref<IconTexture> IconTexture { get; set; }

	public short IconIndex { get; set; }

	public Icon Icon { get; set; }

	public Ref<Text> Description2 { get; set; }

	public Ref<Text> Description3 { get; set; }

	public Ref<Text> Description4 { get; set; }

	public Ref<Text> Description5 { get; set; }

	public Ref<Text> DescriptionConstellation { get; set; }

	//public [] Description2ArgType { get; set; }

	public string[] Description2ArgValue { get; set; }

	public Msec MountAttachDuration { get; set; }

	public Msec MountDetachDuration { get; set; }

	//public Ref<LinkMoveAnim> LinkerAbnormal { get; set; }

	//public Ref<LinkMoveAnim> LinkeeAbnormal { get; set; }

	public Msec LinkAttachDuration { get; set; }

	public Msec LinkDetachDuration { get; set; }

	public bool CatchLegsPhysics { get; set; }

	public bool CatchArmsPhysics { get; set; }

	public bool CatchBodyPhysics { get; set; }

	public bool CameraBlock { get; set; }

	//public BattleMessageType { get; set; }

	public string FlashSoundMode { get; set; }

	public sbyte Skillresultpriority { get; set; }

	public ObjectPath HitShow { get; set; }

	public ObjectPath MissShow { get; set; }

	public ObjectPath DodgeShow { get; set; }

	public ObjectPath ParryShow { get; set; }

	public ObjectPath PerfectParryShow { get; set; }

	public ObjectPath BounceShow { get; set; }

	public ObjectPath ConterShow { get; set; }

	public ObjectPath CriticalHitShow { get; set; }

	public bool Damagesignal { get; set; }

	public bool PlayDespawnShow { get; set; }

	public bool PlayTransit { get; set; }

	public Msec TransitTime { get; set; }

	//public DuelWarp { get; set; }

	public Ref<GameMessage> AttachNotifyMessage { get; set; }

	public Ref<Text> DescriptionItemRandomOption { get; set; }
	#endregion

	#region Methods
	public string Name => Name2.GetText();

	public ImageProperty FrontIcon => IconTexture.Value?.GetImage(IconIndex);

	protected override void LoadHiddenField()
	{
		if (Attributes["power-percent-max"] is not null)
			return;

		var type = this.Attributes.Get<string>("type");
		if (type == "melee-physical-attack" ||
			type == "melee-physical-attack-hate" ||
			type == "melee-physical-attack-drain" ||
			type == "force-attack-hp-drain" ||
			type == "force-attack-sp-drain" ||
			type == "melee-physical-attack-sp-drain" ||
			type == "melee-physical-attack-hp-sp-drain" ||
			type == "force-attack-hp-sp-drain" ||
			type == "range-physical-attack" ||
			type == "force-attack" ||
			type == "force-attack-hate"
			)
		{
			this.Attributes["power-percent-max"] = "100";
			this.Attributes["power-percent-min"] = "100";
		}
	}
	#endregion
}