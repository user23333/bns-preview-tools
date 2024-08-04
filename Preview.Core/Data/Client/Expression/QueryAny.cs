using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Client;
public class QueryAny
{
    /// <summary>
    /// Returns all documents for which at least one value in arrayFields is equal to value
    /// </summary>
    public BsonExpression EQ(string arrayField, AttributeValue value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arrayField);

        return BsonExpression.Create($"{arrayField} ANY = {value}");
    }

    /// <summary>
    /// Returns all documents for which at least one value in arrayFields are less tha to value (&lt;)
    /// </summary>
    public BsonExpression LT(string arrayField, AttributeValue value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arrayField);

        return BsonExpression.Create($"{arrayField} ANY < {value}");
    }

    /// <summary>
    /// Returns all documents for which at least one value in arrayFields are less than or equals value (&lt;=)
    /// </summary>
    public BsonExpression LTE(string arrayField, AttributeValue value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arrayField);

        return BsonExpression.Create($"{arrayField} ANY <= {value}");
    }

    /// <summary>
    /// Returns all documents for which at least one value in arrayFields are greater than value (&gt;)
    /// </summary>
    public BsonExpression GT(string arrayField, AttributeValue value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arrayField);

        return BsonExpression.Create($"{arrayField} ANY > {value}");

    }

    /// <summary>
    /// Returns all documents for which at least one value in arrayFields are greater than or equals value (&gt;=)
    /// </summary>
    public BsonExpression GTE(string arrayField, AttributeValue value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arrayField);

        return BsonExpression.Create($"{arrayField} ANY >= {value}");
    }

    /// <summary>
    /// Returns all documents for which at least one value in arrayFields are between "start" and "end" values (BETWEEN)
    /// </summary>
    public BsonExpression Between(string arrayField, AttributeValue start, AttributeValue end)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arrayField);

        return BsonExpression.Create($"{arrayField} ANY BETWEEN {start} AND {end}");
    }

    /// <summary>
    /// Returns all documents for which at least one value in arrayFields starts with value (LIKE)
    /// </summary>
    public BsonExpression StartsWith(string arrayField, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arrayField);
		ArgumentException.ThrowIfNullOrWhiteSpace(value);

		return BsonExpression.Create($"{arrayField} ANY LIKE {AttributeValue.Create(value + "%")}");
    }

    /// <summary>
    /// Returns all documents for which at least one value in arrayFields are not equals to value (not equals)
    /// </summary>
    public BsonExpression Not(string arrayField, AttributeValue value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(arrayField);

        return BsonExpression.Create($"{arrayField} ANY != {value}");
    }
}