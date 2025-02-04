using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class WorldAccountCardCollection : ModelElement
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public Ref<Text> CollectionName { get; set; }

	public short CollectionSeason { get; set; }

	public sbyte CollectionGrade { get; set; }

	public Ref<WorldAccountCard>[] CollectionCard { get; set; }

	public short[] CollectionCardCount { get; set; }

	public short[] CollectionCardPoint { get; set; }

	public short CollectionActivatePoint { get; set; }

	public short[] AbilityActivatePointRange { get; set; }

	public AttachAbilitySeq[] Ability { get; set; }

	public int[] AbilityBaseValue { get; set; }

	public short[] EffectActivatePointRange { get; set; }

	public Ref<Effect>[] Effect { get; set; }

	public string[] EffectDesc { get; set; }

	public Ref<Item> CompletionRewardItem { get; set; }

	public short CompletionRewardItemCount { get; set; }

	public TimeUniversal StartTime { get; set; }

	public TimeUniversal EndTime { get; set; }

	public bool CanNotUsed { get; set; }

	public string MainImage { get; set; }

	public CollectionTypeSeq CollectionType { get; set; }

	public enum CollectionTypeSeq
	{
		None,
		Event,
		COUNT
	}
	#endregion
}