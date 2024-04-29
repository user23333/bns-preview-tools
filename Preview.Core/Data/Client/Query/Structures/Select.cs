namespace Xylia.Preview.Data.Client;

/// <summary>
/// Represent a Select expression
/// </summary>
internal class Select(BsonExpression expression, bool all)
{
	public BsonExpression Expression { get; } = expression;

	public bool All { get; } = all;
}
