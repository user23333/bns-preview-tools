using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class JobStyle : ModelElement
{
	#region Attributes
	public string IntroduceJobStyleName => this.Attributes["introduce-job-style-name"].GetText();
	#endregion

	#region Methods
	public static JobStyle GetJobStyle(JobSeq Job, JobStyleSeq JobStyle)
	{
		if (Job == JobSeq.JobNone) return null;

		return FileCache.Data.Provider.GetTable<JobStyle>()[(long)Job + ((long)JobStyle << 8)];
	}
	#endregion
}