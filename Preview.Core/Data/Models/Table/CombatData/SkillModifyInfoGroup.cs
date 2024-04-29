using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class SkillModifyInfoGroup : ModelElement, IHaveDesc
{
	#region Attributes
	public JobStyleSeq JobStyle { get; set; }

	public Ref<SkillModifyInfo>[] SkillModifyInfo { get; set; }
	#endregion

	#region Methods
	public string Description => string.Format("<font name=\"00008130.UI.Label_Green03_12\">{0}</font>", 
		string.Join("<br/>", SkillModifyInfo.SelectNotNull(o => o.Instance?.Describe)));
	#endregion
}