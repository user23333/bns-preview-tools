using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class Job : ModelElement
{
	#region Attributes
	public JobSeq job { get; set; }
	#endregion

	#region Properties
	public KeyCommand CurrentActionKey => KeyCommand.Cast(KeyCommandSeq.Action3);
	#endregion

	#region Methods
	public static Job GetJob(JobSeq seq) => FileCache.Data.Provider.GetTable<Job>()[(byte)seq];

	public static IEnumerable<JobSeq> PcJobs => Enum.GetValues<JobSeq>().Where(x => x > JobSeq.JobNone && x < JobSeq.PcMax);
	#endregion
}