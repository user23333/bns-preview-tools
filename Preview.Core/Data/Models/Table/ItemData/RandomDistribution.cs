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
	public Tuple<int, double>[] Do(int min, int max)
	{
		var range = max - min + 1;
		var array = new int[Math.Min(100, range)];
		int arg = range / array.Length, vga = range % array.Length;

		Console.WriteLine("{0} {1} {2}", arg, vga, 0);

		var current = array[0] = min;
		for (int i = 1; i < array.Length; i++)
		{
			current += arg;
			if (i % arg == 0) current += 1;

			array[i] = current;
		}

		return LinqExtensions.Tuple(array, Do(array));
	}

	public double[] Do(Array array)
	{
		var result = new double[array.Length];
		int length = array.Length - 1, pos = 0;
		int arg = 100 / length, vga = 100 % length, group = length / vga;

		//Console.WriteLine("{0} {1} {2}", arg, vga, group);

		for (int x = 0; x < array.Length; x++)
		{
			int w;
			if (x == length)
			{
				w = this.Weight[100];
			}
			else
			{
				var l = arg;
				if (x % group == 0 && pos < 99) l += 1;

				w = this.Weight.Skip(pos).Take(l).Sum(x => x);
				pos += l;
			}

			result[x] = (double)w / TotalWeight;
		}

		return result;
	}

	/// <summary>
	/// use equal distribution 
	/// </summary>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <returns></returns>
	public static Tuple<int, double>[] Equal(int min, int max)
	{
		var array = new Tuple<int, double>[max - min + 1];
		var weight = 1d / array.Length;
		for (int i = 0; i < array.Length; i++) array[i] = new(i + min, weight);

		return array;
	}
	#endregion
}