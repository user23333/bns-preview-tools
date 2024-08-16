using System.Text;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class SetItem : ModelElement, IHaveName, IHaveDesc
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
				var Effect1 = Attributes.Get<Record>($"count-{id}-effect-1")?.As<Effect>();
				var Effect2 = Attributes.Get<Record>($"count-{id}-effect-2")?.As<Effect>();
				var SkillModifyInfoGroup = LinqExtensions.For(10, x => Attributes.Get<Record>($"count-{id}-skill-modify-info-group-{x}")?.As<SkillModifyInfoGroup>());
				var Tooltip1 = Attributes.Get<BnsBoolean>($"count-{id}-tooltip-1");
				var Tooltip2 = Attributes.Get<BnsBoolean>($"count-{id}-tooltip-2");
				var Talksocial = LinqExtensions.For(8, x => Attributes.Get<Record>($"count-{id}-talksocial-{x}")?.As<TalkSocial>());
				var SkillSkin = Attributes.Get<Record>($"count-{id}-skill-skin")?.As<SkillSkin>();

				if (Tooltip1)
				{
					builder.Append($"UI.ItemTooltip.SetItemIndex.{id}.Enable".GetText());
					builder.Append($"UI.ItemTooltip.SetItemEffect.Effect".GetText([null, Effect1, string.Empty]));
					builder.Append(SkillModifyInfoGroup[0]?.Description);
					builder.Append(BR.Tag);
				}
			}

			return builder.ToString();
		}
	}
	#endregion
}