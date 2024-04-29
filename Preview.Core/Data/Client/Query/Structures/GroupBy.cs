namespace Xylia.Preview.Data.Client;

/// <summary>
/// Represent an GroupBy definition (is based on OrderByDefinition)
/// </summary>
internal class GroupBy(BsonExpression expression, BsonExpression select, BsonExpression having)
{
	public BsonExpression Expression { get; } = expression;

	public BsonExpression Select { get; } = select;

	public BsonExpression Having { get; } = having;
}
