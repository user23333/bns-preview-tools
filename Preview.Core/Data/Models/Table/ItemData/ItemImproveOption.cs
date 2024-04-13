using System.Text;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models.Creature;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class ItemImproveOption : ModelElement
{
	#region Attributes
	public MainAbility Ability { get; set; }

	public int AbilityValue { get; set; }

	public Ref<Effect> Effect { get; set; }

	public Ref<Text> EffectDescription { get; set; }

	public Ref<SkillModifyInfoGroup>[] SkillModifyInfoGroup { get; set; }

	public Ref<Text> Additional { get; set; }

	public string DrawOptionIcon { get; set; }
	#endregion

	#region Methods
	public override string ToString()
	{
		var builder = new StringBuilder();

		if (Ability != MainAbility.None) builder.Append("UI.Tooltip.ItemImprove.Ability.Enable".GetText([Ability.GetName(AbilityValue)]));
		if (EffectDescription.HasValue) builder.Append(EffectDescription.GetText());
		if (SkillModifyInfoGroup.Any(x => x.HasValue)) builder.Append(
			string.Format("<font name=\"00008130.UI.Label_Green03_12\">{0}</font>",
			string.Join("<br/>", SkillModifyInfoGroup.Skip(5).SelectNotNull(record => record.Instance))));

		builder.Append(Additional.GetText());
		return builder.ToString();
	}
	#endregion
}