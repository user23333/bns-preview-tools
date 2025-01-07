using System.Diagnostics.CodeAnalysis;

namespace Xylia.Preview.Data.Engine.BinData.Helpers;
/// <summary>
/// Defines methods to support the comparison of camel hump naming convention
/// </summary>
public sealed class TableNameComparer : IComparer<string>, IEqualityComparer<string>
{
	public static TableNameComparer Instance => new();

	public int Compare(string x, string y)
	{
		return string.Compare(
			x.Replace("-", null) + "data",
			y.Replace("-", null) + "data",
			StringComparison.OrdinalIgnoreCase);
	}

	public bool Equals(string x, string y) => Compare(x, y) == 0;

	public int GetHashCode([DisallowNull] string obj)
	{
		return string.GetHashCode(obj.Replace("-", null), StringComparison.OrdinalIgnoreCase);
	}
}