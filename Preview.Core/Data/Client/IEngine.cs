using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Client;
public interface IEngine : IDisposable
{
	string Name { get; }

	string Desc { get; }

	Time64 CreatedAt { get; }

	#region Methods
	void Initialize();

	/// <summary>
	/// Execute SQL commands and return as data reader
	/// </summary>
	IDataReader Execute(string command, AttributeDocument parameters = null);

	/// <summary>
	/// Execute SQL commands and return as data reader
	/// </summary>
	IDataReader Execute(string command, params AttributeValue[] args);

	bool Commit();

	bool Rollback();

	IDataReader Query(string collection, Query query);

	int Insert(string collection, IEnumerable<AttributeDocument> docs);

	int Update(string collection, IEnumerable<AttributeDocument> docs);

	int UpdateMany(string collection, BsonExpression transform, BsonExpression predicate);

	int Upsert(string collection, IEnumerable<AttributeDocument> docs);

	int Delete(string collection, IEnumerable<AttributeValue> ids);

	int DeleteMany(string collection, BsonExpression predicate);
	#endregion
}