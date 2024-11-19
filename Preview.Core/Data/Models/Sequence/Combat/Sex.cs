using Xylia.Preview.Common.Attributes;

namespace Xylia.Preview.Data.Models.Sequence;
public enum SexSeq
{
	[Text("sex-none")]
	SexNone,

	[Text("Name.sex.male")]
	남,

	[Text("Name.sex.female")]
	여,

	[Text("Name.sex.neuter")]
	중,

	COUNT
}