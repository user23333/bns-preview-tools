using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class ConditionEvent : ModelElement
{
	#region Attributes
	public short Id { get; set; }

	public short Score { get; set; }

	public string Alias { get; set; }

	public Ref<Text> Name { get; set; }

	public Msec InstantEventRecycleTime { get; set; }

	[Name("tooltip-text-1")]
	public Ref<Text> TooltipText1 { get; set; }

	[Name("tooltip-arg-type-1")]
	public TooltipArgTypeSeq[] TooltipArgType1 { get; set; }

	[Name("tooltip-arg-1")]
	public int[] TooltipArg1 { get; set; }

	[Name("tooltip-text-2")]
	public Ref<Text> TooltipText2 { get; set; }

	[Name("tooltip-arg-type-2")]
	public TooltipArgTypeSeq[] TooltipArgType2 { get; set; }

	[Name("tooltip-arg-2")]
	public int[] TooltipArg2 { get; set; }

	[Name("tooltip-text-3")]
	public Ref<Text> TooltipText3 { get; set; }

	[Name("tooltip-arg-type-3")]
	public TooltipArgTypeSeq[] TooltipArgType3 { get; set; }

	[Name("tooltip-arg-3")]
	public int[] TooltipArg3 { get; set; }

	[Name("tooltip-text-4")]
	public Ref<Text> TooltipText4 { get; set; }

	[Name("tooltip-arg-type-4")]
	public TooltipArgTypeSeq[] TooltipArgType4 { get; set; }

	[Name("tooltip-arg-4")]
	public int[] TooltipArg4 { get; set; }

	public enum TooltipArgTypeSeq
	{
		None,
		Time,
		StackCount,
		Percent,
		Counter,
		Distance,
		Number,
		COUNT
	}
	#endregion

	#region Methods
	public string GetTooltipText1()
	{
		return TooltipText1.GetText([null, .. LinqExtensions.Tuple(TooltipArgType1, TooltipArg1).Select(GetTooltipArg)]);
	}

	private string GetTooltipArg(Tuple<TooltipArgTypeSeq, int> tuple)
	{
		var type = tuple.Item1;
		var arg = tuple.Item2;
		return type switch
		{
			TooltipArgTypeSeq.Time => "UI.Tooltip.Sequence.time".GetText([new Msec(arg).TotalSeconds]),
			TooltipArgTypeSeq.StackCount => "UI.Tooltip.Sequence.stack-count".GetText([arg]),
			TooltipArgTypeSeq.Percent => "UI.Tooltip.Sequence.percent".GetText([arg * 0.1]),
			TooltipArgTypeSeq.Counter => "UI.Tooltip.Sequence.counter".GetText([arg]),
			TooltipArgTypeSeq.Distance => "UI.Tooltip.Sequence.distance".GetText([arg * 0.01]),
			TooltipArgTypeSeq.Number => "UI.Tooltip.Sequence.number".GetText([arg]),
			_ => null,
		};
	}
	#endregion
}