namespace Xylia.Preview.Data.Client;

/// <summary>
/// Represent an OrderBy definition
/// </summary>
internal class OrderBy(BsonExpression expression, int order)
{
	public BsonExpression Expression { get; } = expression;

	public int Order { get; set; } = order;
}
