using System.Runtime.InteropServices;

namespace Xylia.Preview.Data.Common.DataStruct;
[StructLayout(LayoutKind.Sequential)]
public struct FColor(float r, float g, float b)
{
	public float R = r;
	public float G = g;
	public float B = b;

	#region Methods
	public static FColor Parse(string input)
	{
		var items = input.Split(',');

		if (items.Length != 3)
			throw new ArgumentException("Invalid FColor string input");

		return new FColor(
			float.Parse(items[0]),
			float.Parse(items[1]),
			float.Parse(items[2])
		);
	}

	public readonly override int GetHashCode() => HashCode.Combine(R, G, B);

	public readonly override string ToString() => $"{R:F4},{G:F4},{B:F4}";
	#endregion
}