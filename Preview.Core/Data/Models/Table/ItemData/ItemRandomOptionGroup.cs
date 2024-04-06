using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class ItemRandomOptionGroup : ModelElement
{
	#region Attributes
	public int Id { get; set; }
	public JobSeq Job { get; set; }

	public Ref<SkillTrainByItemList>[] SkillTrainByItemList { get; set; }
	public sbyte SkillTrainByItemListTotalCount { get; set; }
	public sbyte SkillTrainByItemListSelectMin { get; set; }
	public sbyte SkillTrainByItemListSelectMax { get; set; }
	public Ref<Text> SkillTrainByItemListTitle { get; set; }
	#endregion


	#region Methods
	public void TestMethod()
	{
		Console.WriteLine($"{SkillTrainByItemListTitle} {SkillTrainByItemListSelectMin}-{SkillTrainByItemListSelectMax}");

		foreach (var SkillTrainByItemList in SkillTrainByItemList.SelectNotNull(x => x.Instance))
		{
			Console.WriteLine($"# {SkillTrainByItemList}");

			SkillTrainByItemList.ChangeSet.ForEach(SkillTrainByItem =>
			{
				Console.WriteLine(SkillTrainByItem);
				Console.WriteLine(SkillTrainByItem.Description2);
				Console.WriteLine();
			});
		}
	}
	#endregion
}