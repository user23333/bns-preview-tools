using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class Glyph : ModelElement
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public Ref<Text> Name { get; set; }

	public GlyphTypeSeq GlyphType { get; set; }

	public enum GlyphTypeSeq
	{
		None,
		Normal,
		Special,
		Material,
		COUNT
	}

	public ColorSeq Color { get; set; }

	public enum ColorSeq
	{
		None,
		Red,
		Yellow,
		Blue,
		COUNT
	}

	public sbyte Grade { get; set; }

	public Icon Icon { get; set; }

	public bool IsRepresentative { get; set; }

	public Ref<ConditionEvent> ConditionEvent { get; set; }

	public ConditionEventTypeSeq ConditionEventType { get; set; }

	public enum ConditionEventTypeSeq
	{
		Attack,
		Defense,
		Utility,
		Special,
		None,
		COUNT
	}

	public Ref<ConditionEvent> ConditionEventMin { get; set; }

	public Ref<ConditionEvent> ConditionEventMax { get; set; }

	public Ref<Text> FlavorText { get; set; }

	public sbyte RewardTier { get; set; }

	public AttachAbility[] Ability { get; set; }

	public int[] AbilityValue { get; set; }

	public short AbilityId { get; set; }

	public Ref<Filter> DungeonCondition { get; set; }

	public short GroupId { get; set; }
	#endregion

	#region Methods
	public string GlyphName => $"<font name='00008130.Program.Fontset_ItemGrade_{Grade}'>{Name.GetText()}</font>";

	public string GlyphDescription => LinqExtensions.Join(BR.Tag, LinqExtensions.Tuple(Ability, AbilityValue).Select(x => x.Item1.GetText(x.Item2)).ToArray());

	public string Description => GlyphType == GlyphTypeSeq.Material ? "UI.GlyphToolTip.Desc.Material".GetText() : ConditionEvent.Instance?.GetTooltipText1();
	#endregion
}