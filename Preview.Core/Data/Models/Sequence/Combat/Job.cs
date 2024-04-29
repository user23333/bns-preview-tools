using System.ComponentModel;
using Xylia.Preview.Common.Attributes;

namespace Xylia.Preview.Data.Models.Sequence;
public enum JobSeq
{
	[Description("")]
	JobNone,

	[Description("blade-master")]
	[Text("Name.job.BladeMaster")]
	검사,

	[Description("kung-fu-fighter")]
	[Text("Name.job.KungFuFighter")]
	권사,

	[Description("force-master")]
	[Text("Name.job.ForceMaster")]
	기공사,

	[Description("shooter")]
	[Text("Name.job.Shooter")]
	격사,

	[Description("destroyer")]
	[Text("Name.job.Destroyer")]
	역사,

	[Description("summoner")]
	[Text("Name.job.Summoner")]
	소환사,

	[Description("assassin")]
	[Text("Name.job.Assassin")]
	암살자,

	[Description("sword-master")]
	[Text("Name.job.SwordMaster")]
	귀검사,

	[Description("warlock")]
	[Text("Name.job.Warlock")]
	주술사,

	[Description("soul-fighter")]
	[Text("Name.job.SoulFighter")]
	기권사,

	[Description("warroir")]
	[Text("Name.job.Warrior")]
	투사,

	[Description("archer")]
	[Text("Name.job.Archer")]
	궁사,

	[Description("spear-master")]
	창술사,

	[Description("thunderer")]
	[Text("Name.job.Thunderer")]
	뇌전술사,

	[Description("dual-blader")]
	[Text("Name.job.Dual-Blader")]
	쌍검사,

	[Description("bard")]
	[Text("Name.job.Bard")]
	악사,

	[Description("pc-max")]
	PcMax,

	[Description("소환수-루키")]
	소환수루키,

	[Description("소환수-striker")]
	소환수striker,

	[Description("소환수-defender")]
	소환수defender,

	[Description("소환수-controller")]
	소환수controller,

	COUNT
}