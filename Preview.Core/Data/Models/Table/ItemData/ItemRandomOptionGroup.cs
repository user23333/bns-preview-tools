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

				for (int x = 0; x < 6; x++)
				{
					var OriginSkill = SkillTrainByItem.OriginSkill[x].Instance;
					if (OriginSkill != null) Console.WriteLine(OriginSkill?.Name2 + "   " + SkillTrainByItem.ChangeSkill[x].Instance?.Name2);
				}

				Console.WriteLine();
			});
		}
	}
	#endregion
}