using System.Text;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class ItemImproveOption : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public sbyte Level { get; set; }

	public MainAbilitySeq Ability { get; set; }

	public int AbilityValue { get; set; }

	public Ref<Effect> Effect { get; set; }

	public Ref<Text> EffectDescription { get; set; }

	public Ref<SkillModifyInfoGroup>[] SkillModifyInfoGroup { get; set; }

	public Ref<Text> AdditionalDescription { get; set; }

	public string DrawOptionIcon { get; set; }
	#endregion

	#region Methods
	public override string ToString()
	{
		var builder = new StringBuilder();

		if (Ability != MainAbilitySeq.None) builder.Append("UI.Tooltip.ItemImprove.Ability.Enable".GetText([Ability.GetText(AbilityValue)]));
		if (EffectDescription.HasValue) builder.Append(EffectDescription.GetText());
		if (SkillModifyInfoGroup.Any(x => x.HasValue)) builder.Append(string.Join("<br/>", 
			SkillModifyInfoGroup.Skip(5).SelectNotNull(record => record.Value?.Description)));

		builder.Append(AdditionalDescription.GetText());
		return builder.ToString();
	}
	#endregion
}