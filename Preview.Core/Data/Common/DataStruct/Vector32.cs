using System.Runtime.InteropServices;
using CUE4Parse.UE4.Objects.Core.Math;

namespace Xylia.Preview.Data.Common.DataStruct;

[StructLayout(LayoutKind.Sequential)]
public struct Vector32(int x, int y, int z)
{
	public int X = x;
	public int Y = y;
	public int Z = z;

	public static Vector32 Parse(string input)
	{
		var items = input.Split(',');

		if (items.Length != 3)
			throw new ArgumentException("Invalid Vector32 string input");

		return new Vector32(
			int.Parse(items[0]),
			int.Parse(items[1]),
			int.Parse(items[2])
		);
	}

	public static implicit operator FVector(Vector32 vector) => new(vector.X * 4, vector.Y * 4, vector.Z * 4);

	public static implicit operator Vector32(FVector vector) => new((int)(vector.X / 4), (int)(vector.Y / 4), (int)(vector.Z / 4));

	public static bool operator ==(Vector32 a, Vector32 b)
	{
		return
			a.X == b.X &&
			a.Y == b.Y &&
			a.Z == b.Z;
	}

	public static bool operator !=(Vector32 a, Vector32 b)
	{
		return !(a == b);
	}

	public bool Equals(Vector32 other)
	{
		return X == other.X && Y == other.Y && Z == other.Z;
	}

	public override bool Equals(object obj)
	{
		return obj is Vector32 other && Equals(other);
	}

	public override int GetHashCode() => HashCode.Combine(X, Y, Z);

	public override string ToString() => $"{X},{Y},{Z}";
}