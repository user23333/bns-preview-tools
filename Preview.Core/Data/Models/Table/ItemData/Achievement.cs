using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class Achievement : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public short Id { get; set; }

	public short Step { get; set; }

	public JobSeq Job { get; set; }

	public bool Deprecated { get; set; }

	public bool CompleteFromClient { get; set; }

	public Ref<AchievementRegister>[] RegisterRef { get; set; }

	public RegisterTypeSeq[] RegisterType { get; set; }

	public enum RegisterTypeSeq
	{
		None,
		Above,
		Below,
		BitsOn,
		COUNT
	}

	public int[] RegisterValue { get; set; }

	public bool ProgressShow { get; set; }

	public Ref<Item>[] StepCompleteRewardItem { get; set; }

	public short[] StepCompleteRewardItemCount { get; set; }

	public long StepCompleteRewardGameCash { get; set; }

	public StepCompleteRewardGameCashTypeSeq StepCompleteRewardGameCashType { get; set; }

	public enum StepCompleteRewardGameCashTypeSeq
	{
		GameCash,
		Blue,
		Red,
		COUNT
	}

	public short StepCompleteRewardSkillBuildUpPoint { get; set; }

	public StepCompleteRewardTypeSeq StepCompleteRewardType { get; set; }

	public enum StepCompleteRewardTypeSeq
	{
		Invalid,
		Item,
		GameCash,
		SkillBuildUpPoint,
		COUNT
	}

	public Ref<Item>[] StepCompleteRewardFinalItem { get; set; }

	public short[] StepCompleteRewardFinalItemCount { get; set; }

	public short CurrentStepScore { get; set; }

	public AbilitySeq Ability { get; set; }

	public enum AbilitySeq
	{
		None,
		AttackPowerCreatureMinMax,
		AttackHitValue,
		AttackPierceValue,
		AttackDamageModifyDiff,
		MaxHp,
		DefendPowerCreatureValue,
		DefendDodgeValue,
		DefendParryValue,
		DefendDamageModifyDiff,
		COUNT
	}

	public short AbilityValue { get; set; }

	public Ref<Effect> CompletedEffect { get; set; }

	//public CompletedEffectCategorySeq CompletedEffectCategory { get; set; }

	public short CompletedEffectOrder { get; set; }

	public string TitleFontset { get; set; }

	public string TitleBackgroundImage { get; set; }

	public float TitleBackgroundImagePosX { get; set; }

	public float TitleBackgroundImagePosY { get; set; }

	[Name("category-1")]
	public Category1Seq Category1 { get; set; }

	public enum Category1Seq
	{
		None,
		Growth,
		Item,
		Combat,
		Economy,
		Community,
		COUNT
	}

	[Name("category-2")]
	public Category2Seq Category2 { get; set; }

	public enum Category2Seq
	{
		None,
		LevelUp,
		Quest,
		AcquireSkill,
		Consumable,
		Collect,
		Growth,
		Decompose,
		Repair,
		EquipGem,
		AttachGem,
		Dungeon,
		Faction,
		Etc,
		Auction,
		Production,
		SocialAction,
		Picture,
		Event,
		COUNT
	}

	[Name("map-group-1")]
	public Ref<MapGroup1> MapGroup1 { get; set; }

	public Icon Icon { get; set; }

	public Ref<Text> Name2 { get; set; }

	public Ref<Text> Description2 { get; set; }

	public Ref<Text> TitleName { get; set; }

	public Ref<Text> TitleImageText { get; set; }

	public Ref<Text> TitleThumbnailIconText { get; set; }

	public short SortNo { get; set; }

	public Ref<GameMessage> CompletedGameMessage { get; set; }

	public Ref<TalkSocial> TalkSocial { get; set; }

	public Icon TitleChatUiIcon { get; set; }

	public ObjectPath TitleThumbnailFrameFx { get; set; }

	public sbyte TitleGrade { get; set; }

	public sbyte TitleInfieldUiBorderEffect { get; set; }
	#endregion
}