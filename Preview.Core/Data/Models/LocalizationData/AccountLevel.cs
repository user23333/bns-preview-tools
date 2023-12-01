﻿using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed unsafe class AccountLevel : Record
{
	public Ref<Text> Name;

	public long Exp;
}