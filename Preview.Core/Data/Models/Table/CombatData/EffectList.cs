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
		for (int i = 0; i < EffectTotalCount; i++)
		{
			var s = Effect[i].Value?.Attributes["description-item-random-option"].GetText();
			var w = (double)EffectWeight[i] / EffectTotalWeight;

			yield return new Tuple<string, double>(s, w);
		}

		yield break;
	}
	#endregion
}