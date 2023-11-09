﻿using Xylia.Preview.Data.Common.Attribute;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Models;

namespace Xylia.Preview.Data.Models;
public class SkillGatherRange3 : Record
{
	#region Fields	
	public string Alias;


	public Distance RangeCastMin;
	public Distance RangeCastMax;

	public Distance RangeCastDepth;
	public Distance RangeCastHeight;

	[Repeat(5)]
	public Distance[] GatherRadiusMax;

	[Repeat(5)]
	public Distance[] GatherRadiusMin;
	#endregion


	#region Test
	public Distance RadiusMax => this.GatherRadiusMax[0] * 2;

	public Distance CastMax => this.RangeCastMax * 2;
	#endregion
}