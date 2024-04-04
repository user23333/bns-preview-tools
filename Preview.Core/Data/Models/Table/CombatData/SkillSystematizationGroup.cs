namespace Xylia.Preview.Data.Models;
public class SkillSystematizationGroup : ModelElement
{
	#region Attributes
	public string Name { get; set; }

	public Ref<Text> Name2 { get; set; }

	public Ref<Text> Description { get; set; }

	public sbyte SortNo { get; set; }

	public bool UseBookmark { get; set; }

	public Ref<Text> BookmarkDescription { get; set; }

	public string[] CategoryIconText { get; set; }

	public string[] TrainIconText { get; set; }

	public Ref<IconTexture> IconTexture { get; set; }

	public short IconIndex { get; set; }

	public Ref<Text> TooltipDescription { get; set; }
	#endregion
}