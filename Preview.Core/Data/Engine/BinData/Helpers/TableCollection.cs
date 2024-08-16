using System.Collections.ObjectModel;
using System.Diagnostics;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Engine.BinData.Helpers;
public class TableCollection : Collection<Table>
{
	#region Fields
	Dictionary<ushort, Table> _tableByType;
	Dictionary<string, Table> _tableByName;

	readonly Dictionary<Table, object> _tables = [];
	#endregion

	#region Methods
	public Table this[ushort index]
	{
		get
		{
			lock (this)
			{
				return (_tableByType ??= this.Where(x => x.Type > 0).ToDistinctDictionary(o => o.Type))
					 .GetValueOrDefault(index);
			}
		}
	}

	public Table this[string index]
	{
		get
		{
			lock (this)
			{
				if (string.IsNullOrWhiteSpace(index)) return default;
				if (ushort.TryParse(index, out var type)) return this[type];
				if (index.Equals("skill", StringComparison.OrdinalIgnoreCase)) index = "skill3";

				// Create hashmap
				var table = (_tableByName ??= this.ToDistinctDictionary(x => x.Name, TableNameComparer.Instance)).GetValueOrDefault(index);
				if (table is null) Debug.WriteLine($"Invalid typed reference, refered table doesn't exist: '{index}'");

				return table;
			}
		}
	}


	public Record GetRecord(string table, string alias) => this[table]?[alias];

	public Record GetRecord(string value)
	{
		if (value is null) return null;

		var array = value.Split(':', 2);
		if (array.Length < 2)
		{
			Serilog.Log.Warning($"TRef get failed, value: {value}");
			return null;
		}

		return GetRecord(array[0], array[1]);
	}

	public Icon GetIcon(string value)
	{
		if (value is null) return null;

		var colon = value.LastIndexOf(',');
		if (colon == -1) return null;

		var array = new[] { value[..colon], value[(colon + 1)..] };
		return new Icon(
			GetRecord("icontexture", array[0]), 
			short.Parse(array[1]));
	}
	#endregion

	#region GameDataTable
	public GameDataTable<T> Get<T>(string name = null, bool reload = false) where T : ModelElement =>
		Get<T>(this[name ?? typeof(T).Name], reload);

	public GameDataTable<T> Get<T>(Table table, bool reload) where T : ModelElement
	{
		if (table is null) return null;

		lock (_tables)
		{
			if (reload || !_tables.TryGetValue(table, out var Models))
				_tables[table] = Models = new GameDataTable<T>(table);

			return Models as GameDataTable<T>;
		}
	}

	protected override void ClearItems()
	{
		_tables.Clear();
		_tableByType?.Clear();
		_tableByName?.Clear();
		this.ForEach(x => x.Dispose());

		base.ClearItems();
	}
	#endregion
}