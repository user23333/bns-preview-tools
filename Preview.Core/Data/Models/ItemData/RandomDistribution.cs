﻿using Xylia.Preview.Data.Common.Attribute;
using Xylia.Preview.Data.Engine.BinData.Models;

namespace Xylia.Preview.Data.Models;
[Side(ReleaseSide.Server)]
public sealed class RandomDistribution : Record 
{
	public string Alias;
}