namespace Xylia.Preview.Common.Extension;
public static class MathExtensions
{
	public static int GetLength(this int value) => (int)Math.Floor(Math.Log(value, 10)) + 1;

	public static int GetPercentLength(this int value) => Math.Max(0, value.GetLength() - 2);
}