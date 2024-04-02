﻿using Xylia.Preview.Common.Attributes;

namespace Xylia.Preview.Data.Models.Sequence;
public enum RecycleGroup
{
    None,

    [Name("class")]
    Class,

    [Name("item-1")]
    Item1,

    [Name("item-2")]
    Item2,

    [Name("class-2")]
    Class2,

    [Name("db")]
    DB,

    [Name("gadget")]
    Gadget,

	COUNT
}