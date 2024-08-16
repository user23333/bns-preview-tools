using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public abstract class Skill3 : ModelElement, IHaveName
{
	#region Attributes
	public short[] RevisedEventProbabilityInExec { get; set; }

	public Ref<SkillSystematization>[] Systematization { get; set; }
	public short DamageRatePvp { get; set; }
	public short DamageRateStandardStats { get; set; }


	public Ref<Text> Name2 { get; set; }

	public KeyCommandSeq ShortCutKey { get; set; }
	public KeyCommandSeq ShortCutKeyClassic { get; set; }
	public KeyCommandSeq ShortCutKeySimpleContext { get; set; }

	[Name("main-tooltip-1")] public Ref<SkillTooltip>[] MainTooltip1 { get; set; }
	[Name("main-tooltip-2")] public Ref<SkillTooltip>[] MainTooltip2 { get; set; }
	public Ref<SkillTooltip>[] SubTooltip { get; set; }
	public Ref<SkillTooltip>[] StanceTooltip { get; set; }
	public Ref<SkillTooltip>[] ConditionTooltip { get; set; }

	public string IconTexture { get; set; }
	public short IconIndex { get; set; }


	public sealed class ActiveSkill : Skill3
	{
		public FlowTypeSeq FlowType { get; set; }
		public enum FlowTypeSeq
		{
			KeepMainslot,
			LeaveCaster,
			TransferSimslot,
			DirectlySimslot,
		}

		public Ref<SkillDashAttribute3> DashAttribute { get; set; }

		public Ref<Filter> TargetFilter { get; set; }
		public Ref<SkillGatherRange3> GatherRange { get; set; }


		public Ref<SkillCastCondition3> CastCondition { get; set; }
		public Msec CastDuration { get; set; }

		public sbyte GlobalRecycleGroup { get; set; }
		public Msec GlobalRecycleGroupDuration { get; set; }
		public RecycleGroup RecycleGroup { get; set; }
		public sbyte RecycleGroupId { get; set; }
		public Msec RecycleGroupDuration { get; set; }
		public RecycleGroup BoundRecycleGroup { get; set; }
		public sbyte BoundRecycleGroupId { get; set; }

		public short ConsumeHpValue { get; set; }
		public ConsumeType ConsumeHpType { get; set; }
		public short[] ConsumeSpValue { get; set; }
		public ConsumeType[] ConsumeSpType { get; set; }
		public short ConsumeSummonedHpValue { get; set; }
		public ConsumeType ConsumeSummonedHpType { get; set; }


		public sbyte FlowRepeat { get; set; }
		public sbyte ExpandedFlowRepeatCount { get; set; }
		public sbyte ExpandedFlowRepeatStartFlowStep { get; set; }
	}

	public sealed class PassiveSkill : Skill3
	{

	}

	public sealed class Action : Skill3
	{

	}
	#endregion


	#region Methods
	public string Name => Name2.GetText();

	public ImageProperty Icon => FileCache.Data.Provider.GetTable<IconTexture>()[IconTexture]?.GetImage(IconIndex);

	public KeyCommand CurrentShortCutKey => KeyCommand.Cast(this.ShortCutKey);

	public string CastingRange
	{
		get
		{
			if (this is ActiveSkill activeSkill)
			{
				// NOTE: the method has been modified
				var CastCondition = activeSkill.GatherRange.Instance;
				if (CastCondition is null) return null;

				var RangeMin = CastCondition.RangeCastMin;
				var RangeMax = CastCondition.RangeCastMax;

				return (RangeMin == RangeMax && RangeMin == 0 ? "Name.Skill.CastingRange.Default" :
					((RangeMin == RangeMax || RangeMin == 0) ? "Name.Skill.CastingRange" :
					"Name.Skill.CastingRange.MinMax")).GetText()
				.Replace("2:skill.cast-condition.range-min", "3:integer")
				.Replace("2:skill.cast-condition.range-max", "2:integer")
				.Replace([this, RangeMax, RangeMin]);
			}

			return null;
		}
	}
	#endregion
}