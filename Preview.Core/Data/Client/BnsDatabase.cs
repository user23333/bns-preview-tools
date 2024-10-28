using CUE4Parse.UE4.Pak;
using Xylia.Preview.Data.Common.Exceptions;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Client;
public class BnsDatabase : IEngine, IDisposable
{
	#region Constructors
	public BnsDatabase(IDataProvider provider, DatafileDefinition definition = null)
	{
		_provider = provider;
		_definition = definition ?? new DefaultDatafileDefinition();

		ArgumentNullException.ThrowIfNull(provider);
		BnsDataException.ThrowIfMismatch(provider.Locale.Publisher, _definition.Publisher);
	}
	#endregion

	#region Database

	public void Initialize()
	{
		lock (this)
		{
			if (IsInitialized) return;

			IPlatformFilePak.DoSignatureCheck();

			_provider.LoadData(_definition);
			_definition.CreateMap();

			// Bind definitions to tables
			foreach (var table in _provider.Tables)
			{
				table.Owner = _provider;

				// represents from xml
				if (table.Type == 0)
				{
					ArgumentException.ThrowIfNullOrEmpty(table.Name);
					table.Definition = _definition[table.Name];
					table.Type = table.Definition.Type;
				}
				else
				{
					table.Definition = _definition[table.Type];
				}
			}

			IsInitialized = true;
		}
	}

	public IDataReader Execute(string command, AttributeDocument parameters = null)
	{
		ArgumentNullException.ThrowIfNull(command);

		var tokenizer = new Tokenizer(command);
		var sql = new SqlParser(this, tokenizer, parameters);
		var reader = sql.Execute();

		return reader;
	}

	public IDataReader Execute(string command, params AttributeValue[] args)
	{
		var p = new AttributeDocument();
		var index = 0;

		foreach (var arg in args)
		{
			p[index.ToString()] = arg;
			index++;
		}

		return this.Execute(command, p);
	}

	public bool Commit()
	{
		throw new NotImplementedException();
	}

	public bool Rollback()
	{
		throw new NotImplementedException();
	}

	public IDataReader Query(string collection, Query query)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(collection);
		ArgumentNullException.ThrowIfNull(query);

		IEnumerable<AttributeDocument> source = null;

		// test if is an system collection
		//if (collection.StartsWith("$"))
		//{
		//	SqlParser.ParseCollection(new Tokenizer(collection), out var name, out var options);

		//	// get registered system collection to get data source
		//	var sys = this.GetSystemCollection(name);

		//	source = sys.Input(options);
		//	collection = sys.Name;
		//}

		var exec = new QueryExecutor(this, collection, query, source);

		return exec.ExecuteQuery();
	}

	public int Insert(string collection, IEnumerable<AttributeDocument> docs)
	{
		throw new NotImplementedException();
	}

	public int Update(string collection, IEnumerable<AttributeDocument> docs)
	{
		throw new NotImplementedException();
	}

	public int UpdateMany(string collection, BsonExpression transform, BsonExpression predicate)
	{
		throw new NotImplementedException();
	}

	public int Upsert(string collection, IEnumerable<AttributeDocument> docs)
	{
		throw new NotImplementedException();
	}

	public int Delete(string collection, IEnumerable<AttributeValue> ids)
	{
		throw new NotImplementedException();
	}

	public int DeleteMany(string collection, BsonExpression predicate)
	{
		throw new NotImplementedException();
	}

	#endregion

	#region Fields
	public bool IsInitialized { get; private set; }

	private IDataProvider _provider;
	private DatafileDefinition _definition;

	public IDataProvider Provider
	{
		protected set => _provider = value;
		get
		{
			Initialize();
			return _provider;
		}
	}
	#endregion

	#region Interface
	string IEngine.Name => Provider.Name;
	string IEngine.Desc => Path.Combine(
		Provider.Locale.Publisher.ToString(),
		Provider.CreatedAt.ToString("yyMMdd", null));

	public void Dispose()
	{
		_provider?.Dispose();
		_provider = null;
		_definition = null;

		GC.SuppressFinalize(this);
		GC.Collect();
	}
	#endregion
}