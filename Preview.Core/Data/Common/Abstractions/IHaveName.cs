namespace Xylia.Preview.Data.Common.Abstractions;
public interface IHaveName
{
	string Name { get; }
}

public interface IAttraction : IHaveName
{
	string Description { get; }
}