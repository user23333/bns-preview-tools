using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;
using static Xylia.Preview.Data.Models.Item;

namespace Xylia.Preview.Data.Models;
public sealed class ItemBrandTooltip : ModelElement, IHaveName
{
	#region Attributes
	public int BrandId { get; set; }

	public ItemConditionType ItemConditionType { get; set; }

	public Ref<Text> Name2 { get; set; }

	[Name("game-category-3")]
	public GameCategory3Seq GameCategory3 { get; set; }

	public sbyte ItemGrade { get; set; }

	public sbyte EquipLevel { get; set; }

	public sbyte EquipMasteryLevel { get; set; }

	public JobSeq[] EquipJobCheck { get; set; }

	public SexSeq2 EquipSex { get; set; }

	public RaceSeq2 EquipRace { get; set; }

	public sbyte EquipSoloDuelGrade { get; set; }

	public sbyte EquipTeamDuelGrade { get; set; }

	public Icon Icon { get; set; }

	public Icon TagIcon { get; set; }

	public Icon TagIconGrade { get; set; }

	public Ref<Text> MainInfo { get; set; }

	public Ref<Text> SubInfo { get; set; }

	public Ref<Text> Description2 { get; set; }

	public Ref<Text> Description4Title { get; set; }

	public Ref<Text> Description5Title { get; set; }

	public Ref<Text> Description6Title { get; set; }

	public Ref<Text> Description4 { get; set; }

	public Ref<Text> Description5 { get; set; }

	public Ref<Text> Description6 { get; set; }

	public Ref<Text> StoreDescription { get; set; }

	public Ref<Item> TitleItem { get; set; }
	#endregion

	#region Properties
	public string BrandName => $"<link id='item-brand:{ToString()}'>{BrandNameOnly}</link>";
	public string BrandNameOnly => $"<font name='00008130.Program.Fontset_ItemGrade_{ItemGrade}'>{Name2.GetText() ?? ToString()}</font>";	

	public string Name => Name2.GetText() ?? ToString();
	#endregion
}