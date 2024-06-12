using System.Collections;

namespace Xylia.Preview.Data.Models;
public class EffectList : ModelElement , IEnumerable
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<Effect>[] Effect { get; set; }

	public short[] EffectWeight { get; set; }

	public int EffectTotalWeight { get; set; }

	public sbyte EffectTotalCount { get; set; }
	#endregion

	#region Methods
	public IEnumerator GetEnumerator()
	{
		var result = new List<Tuple<string, string>>();

		for (int i = 0; i < EffectTotalCount; i++)
		{
			result.Add(new(Effect[i].Instance?.Attributes["description-item-random-option"].GetText(),
				((double)EffectWeight[i] / EffectTotalWeight).ToString("P2")));
		}

		return result.GetEnumerator();
	}
	#endregion
}