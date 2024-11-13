using System.Collections;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class AbilityList : ModelElement, IEnumerable
{
	#region Attributes
	public int Id { get; set; }	
	
	public sbyte ImproveLevel { get; set; }

	public string Alias { get; set; }

	public MainAbilitySeq[] Ability { get; set; }

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
		for (int i = 0; i < AbilityTotalCount; i++)
		{
			var s = string.Format("{0} {1}-{2}", Ability[i].GetText(), AbilityValueMin[i], AbilityValueMax[i]);
			var w = (double)AbilityWeight[i] / AbilityTotalWeight;

			yield return new Tuple<string, double>(s, w);
		}

		yield break;
	}
	#endregion
}