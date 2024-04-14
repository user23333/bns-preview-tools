using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class SkillModifyInfoGroup : ModelElement, IAttraction
{
	#region Attributes
	public JobStyleSeq JobStyle { get; set; }

	public Ref<SkillModifyInfo>[] SkillModifyInfo { get; set; }
	#endregion

	#region Methods
	public string Name => base.ToString();

	public string Description => string.Join("<br/>", SkillModifyInfo.SelectNotNull(o => o.Instance?.Describe));
	#endregion
}