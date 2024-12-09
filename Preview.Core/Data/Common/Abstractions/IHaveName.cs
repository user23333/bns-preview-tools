using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Common.Abstractions;
public interface IHaveName
{
	string Name { get; }
}

public static class Extension
{
	public static string GetName(this ModelElement obj) => obj is IHaveName instance ? instance.Name : obj?.ToString();
}