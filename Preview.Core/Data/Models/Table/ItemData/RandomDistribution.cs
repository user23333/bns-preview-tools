using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.Data.Models;
public sealed class RandomDistribution : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public short[] Weight { get; set; }

	public int TotalWeight { get; set; }
	#endregion

	#region Methods
	public double[] Do(Array array)
	{
		var result = new double[array.Length];
		int length = array.Length - 1;
		int arg = 100 / length, vga = 100 % length;
		int pos = 0;

		for (int x = 0; x < array.Length; x++)
		{
			int w;
			if (x == array.Length - 1)
			{
				w = this.Weight[100];
			}
			else if (x == 0)
			{
				w = this.Weight.Skip(pos).Take(arg + vga).Sum(x => x);
				pos += arg + vga;
			}
			else
			{
				w = this.Weight.Skip(pos).Take(arg).Sum(x => x);
				pos += arg;
			}

			result[x] = (double)w / TotalWeight;
		}

		return result;
	}

	public Tuple<int, double>[] Do(int min, int max)
	{
		var array = new int[max - min + 1];
		for (int i = 0; i < array.Length; i++) array[i] = i + min;

		return LinqExtensions.Tuple(array , Do(array));
	}
	#endregion
}