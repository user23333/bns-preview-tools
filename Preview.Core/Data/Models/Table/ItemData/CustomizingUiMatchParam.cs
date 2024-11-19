using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class CustomizingUiMatchParam : ModelElement
{
	#region Attributes
	public short UiIndex { get; set; }

	public RaceSeq Race { get; set; }

	public SexSeq Sex { get; set; }

	public Ref<Text> SubName { get; set; }

	public sbyte ParamIndex { get; set; }
	#endregion
}