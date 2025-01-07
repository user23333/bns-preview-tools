using System.Text;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class SetItem : ModelElement, IHaveName
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<Text> Name2 { get; set; }

	public Ref<Text>[] SlotName { get; set; }

	public string[] SlotTagIcon { get; set; }

	public EquipType[] SlotEquipType { get; set; }
	#endregion

	#region Methods
	public string Name => Name2.GetText();

	public string Description
	{
		get
		{
			var builder = new StringBuilder();

			for (int id = 1; id <= 10; id++)
			{
				var Effect = Attributes.Get<Effect[]>($"count-{id}-effect");
				var SkillModifyInfoGroup = Attributes.Get<SkillModifyInfoGroup[]>($"count-{id}-skill-modify-info-group");
				var Tooltip = Attributes.Get<bool[]>($"count-{id}-tooltip");
				var TalkSocial = Attributes.Get<TalkSocial[]>($"count-{id}-talksocial");
				var SkillSkin = Attributes.Get<SkillSkin>($"count-{id}-skill-skin");

				if (Tooltip[0])
				{
					builder.Append($"UI.ItemTooltip.SetItemIndex.{id}.Enable".GetText());
					builder.Append($"UI.ItemTooltip.SetItemEffect.Effect".GetText([null, Effect[0], string.Empty]));
					builder.Append(SkillModifyInfoGroup[0]?.Description);
					builder.Append(BR.Tag);
				}
			}

			return builder.ToString();
		}
	}
	#endregion
}