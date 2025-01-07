using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class BossNpc : ModelElement 
{
	#region Attributes
	public string Alias { get; set; }

	public short[] SpPoint { get; set; }

	public ObjectPath[] SpShow { get; set; }

	public ObjectPath ImmuneBreakerDisabaleShow { get; set; }

	public Msec BerserkSequenceInvokeTime { get; set; }

	public Ref<DifficultyTypeModify> DifficultyTypeModify { get; set; }

	public bool UseSecondGauge { get; set; }

	public short DefaultGp { get; set; }

	public short[] GpSection { get; set; }

	public sbyte GpSectionCnt { get; set; }

	public UiStyleSeq UiStyle { get; set; }	   

	public enum UiStyleSeq
	{
		None,
		Fury,
		AbsorbLevel,
		DoubleSided,
		COUNT
	}

	public Ref<Text> UiTooltip { get; set; }

	public ObjectPath UiDoubleSidedLeftImageset { get; set; }

	public ObjectPath UiDoubleSidedRightImageset { get; set; }

	public short UiDoubleSidedLeftColorR { get; set; }

	public short UiDoubleSidedLeftColorG { get; set; }

	public short UiDoubleSidedLeftColorB { get; set; }

	public short UiDoubleSidedLeftColorA { get; set; }

	public short UiDoubleSidedRightColorR { get; set; }

	public short UiDoubleSidedRightColorG { get; set; }

	public short UiDoubleSidedRightColorB { get; set; }

	public short UiDoubleSidedRightColorA { get; set; }

	public short[] UiFuryColorR { get; set; }

	public short[] UiFuryColorG { get; set; }

	public short[] UiFuryColorB { get; set; }

	public short[] UiFuryColorA { get; set; }

	public bool UseBreak { get; set; }

	public short BreakGaugeLimit { get; set; }

	public short BreakGaugeLimitIncreaseValue { get; set; }

	public sbyte MaxBreakCount { get; set; }
	#endregion
}