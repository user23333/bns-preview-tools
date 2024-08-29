using System.Diagnostics.CodeAnalysis;

namespace Xylia.Preview.Data.Engine.BinData.Helpers;
/// <summary>
/// Defines methods to support the comparison of camel hump naming convention
/// </summary>
internal sealed class TableNameComparer : IEqualityComparer<string>
{
	public static TableNameComparer Instance => new();

	public bool Equals(string x, string y)
	{
		return string.Equals(
			x.Replace("-", null),
			y.Replace("-", null),
			StringComparison.OrdinalIgnoreCase);
	}

	public int GetHashCode([DisallowNull] string obj)
	{
		return string.GetHashCode(obj.Replace("-", null), StringComparison.OrdinalIgnoreCase);
	}
}