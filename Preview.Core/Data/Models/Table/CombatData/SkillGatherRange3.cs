using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public class SkillGatherRange3 : ModelElement
{
	#region Attributes
	//public CastDirTarget CastDirCaster { get; set; }

	//public CastDirTarget CastDirTarget { get; set; }

	public Distance RangeCastMin { get; set; }

	public Distance RangeCastMax { get; set; }

	public Distance RangeCastHeight { get; set; }

	public Distance RangeCastDepth { get; set; }

	public Distance RangeCastLaserWidth { get; set; }

	public Distance[] GatherRadiusMin { get; set; }

	public Distance[] GatherRadiusMax { get; set; }

	public Distance[] GatherLaserWidthMin { get; set; }

	public Distance[] GatherLaserWidthMax { get; set; }

	public Distance[] GatherLaserFrontDistanceMin { get; set; }

	public Distance[] GatherLaserFrontDistanceMax { get; set; }

	public Distance[] GatherLaserBackDistanceMin { get; set; }

	public Distance[] GatherLaserBackDistanceMax { get; set; }

	public Distance ShiftingLaserWidthMin { get; set; }

	public Distance ShiftingLaserWidthMax { get; set; }

	public Distance ShiftingLaserDistanceMin { get; set; }

	public Distance ShiftingLaserDistanceMax { get; set; }

	public Distance ShiftingLaserHeight { get; set; }

	public Distance ShiftingLaserDepth { get; set; }

	public Distance ShiftingLaserOffset { get; set; }

	public Distance[] TargetZ { get; set; }

	//public SummonBasePosType SummonBasePosType { get; set; }

	public Distance SummonBasePosRelativeDistance { get; set; }

	//public SummonBaseDirType SummonBaseDirType { get; set; }

	public short SummonBaseDirRelativeAngle { get; set; }
	#endregion
}