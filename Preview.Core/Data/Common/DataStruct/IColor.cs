using System.Runtime.InteropServices;

namespace Xylia.Preview.Data.Common.DataStruct;
[StructLayout(LayoutKind.Sequential)]
public struct IColor(byte r, byte g, byte b)
{
	public byte R = r;
	public byte G = g;
	public byte B = b;

	#region Methods
	public static IColor Parse(string input)
	{
		var items = input.Split(',');

		if (items.Length != 3)
			throw new ArgumentException("Invalid Color string input");

		return new IColor(
			byte.Parse(items[0]),
			byte.Parse(items[1]),
			byte.Parse(items[2])
		);
	}

	public static bool operator ==(IColor a, IColor b)
	{
		return
			a.R == b.R &&
			a.G == b.G &&
			a.B == b.B;
	}

	public static bool operator !=(IColor a, IColor b)
	{
		return !(a == b);
	}

	public readonly bool Equals(IColor other)
	{
		return R == other.R && G == other.G && B == other.B;
	}

	public readonly override bool Equals(object obj)
	{
		return obj is IColor other && Equals(other);
	}

	public readonly override int GetHashCode()
	{
		return HashCode.Combine(R, G, B);
	}

	public readonly override string ToString() => $"{R},{G},{B}";
	#endregion
}