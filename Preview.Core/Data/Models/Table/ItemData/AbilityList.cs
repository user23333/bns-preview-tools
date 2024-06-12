using System.Collections;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class AbilityList : ModelElement, IEnumerable
{
	#region Attributes
	public MainAbility[] Ability { get; set; }

	public short[] AbilityWeight { get; set; }

	public int AbilityTotalWeight { get; set; }

	public sbyte AbilityTotalCount { get; set; }

	public short[] AbilityValueMin { get; set; }

	public short[] AbilityValueMax { get; set; }

	public Ref<RandomDistribution>[] AbilityValueDistribution { get; set; }

	public bool DrawEnable { get; set; }
	#endregion

	#region Methods
	public IEnumerator GetEnumerator()
	{
		var result = new List<Tuple<string, string>>();

		for (int i = 0; i < AbilityTotalCount; i++)
		{
			result.Add(new(
				string.Format("{0} {1}-{2}", Ability[i].GetText(), AbilityValueMin[i], AbilityValueMax[i]),
				((double)AbilityWeight[i] / AbilityTotalWeight).ToString("P2")));
		}

		return result.GetEnumerator();
	}
	#endregion
}