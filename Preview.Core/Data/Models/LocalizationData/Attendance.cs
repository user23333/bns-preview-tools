﻿using Xylia.Preview.Data.Common.Attribute;

namespace Xylia.Preview.Data.Models;
[Side(ReleaseSide.Client)]
public sealed class Attendance : Record
{
	public string Alias;
}