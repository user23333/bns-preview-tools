using Xylia.Preview.Data.Common.Exceptions;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Client;
/// <summary>
/// Class that execute QueryPlan returing results
/// </summary>
internal class QueryExecutor(BnsDatabase engine, string collection, Query query, IEnumerable<AttributeDocument> source)
{
	public DataReader ExecuteQuery()
	{
		return new DataReader(RunQuery(), null);

		IEnumerable<AttributeDocument> RunQuery()
		{
			var table = engine.Provider.Tables[collection] ?? throw BnsDataException.InvalidTable(collection);

			// execute optimization before run query (will fill missing _query properties instance)
			var optimizer = new QueryOptimization(query, source, Collation.Binary);

			var queryPlan = optimizer.ProcessQuery();

			var pipe = queryPlan.GetPipe();

			// call safepoint just before return each document
			foreach (var doc in pipe.Pipe(table.Records, queryPlan))
			{
				yield return doc;
			}
		}
	}
}