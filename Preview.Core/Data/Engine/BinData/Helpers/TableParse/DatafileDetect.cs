using System.Collections.ObjectModel;
using System.Diagnostics;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Definitions;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Engine.BinData.Helpers;
/// <summary>
/// parse by auto detect  
/// </summary>
/// <remarks>This will to load NameTable and LazyTable, cause large memory usage.</remarks>
public sealed class DatafileDetect : ITableParseType
{
	#region Helpers
	readonly Dictionary<int, string> by_id = [];
	readonly Dictionary<string, ushort> by_name = new(new TableNameComparer());

	public bool TryGetName(ushort key, out string name) => by_id.TryGetValue(key, out name);

	public bool TryGetKey(string name, out ushort key) => by_name.TryGetValue(name, out key);

	private void Add(string name, int type)
	{
		by_id[type] = name;

		// fix detect fail
		if (name == "unlocated-store") Add("unlocated-store-ui", type + 1);
	}

	private void CreateNameMap(TableDefinition[] definitions)
	{
		// create map
		by_name.Clear();
		by_id.ForEach(o => by_name[o.Value] = (ushort)o.Key);

		// insert missing
		int LastId = 1;
		string LastName = null;
		foreach (var o in by_id)
		{
			if (string.IsNullOrEmpty(o.Value) || o.Key != by_name[o.Value]) continue;

			for (int i = 0; i < definitions.Length; i++)
			{
				// find current definition
				if (o.Key - LastId <= 1) break;
				if (definitions[i].Name.Equals(o.Value, StringComparison.OrdinalIgnoreCase))
				{
					// find last definition
					int j;
					for (j = i; j > 0; j--)
					{
						if (definitions[j].Name.Equals(LastName, StringComparison.OrdinalIgnoreCase)) break;
					}

					// insert missing tables
					if (i - j == o.Key - LastId)
					{
						for (int x = j; x < i; x++)
						{
							var d = definitions[x];
							by_name[d.Name] = (ushort)(LastId + x - j);
						}
					}
					break;
				}
			}

			LastId = o.Key;
			LastName = o.Value;
		}
	}
	#endregion


	#region Load Methods
	public DatafileDetect(Datafile data, Collection<TableDefinition> definitions)
	{
		Read(data.Tables, AliasTableUnit.Split(data.AliasTable));
		CreateNameMap(definitions.Where(x => x.Module != TableModule.Server).ToArray());
	}

	/// <summary>
	/// create map by detect data
	/// </summary>
	/// <param name="tables"></param>
	/// <param name="AliasTable"></param>
	private void Read(IEnumerable<Table> tables, List<AliasTableUnit> AliasTable)
	{
#if DEVELOP
		AliasTable.ForEach(t => Debug.WriteLine(t.Name));
#endif
		tables.ForEach(table => by_id[table.Type] = "");
		Parallel.ForEach(tables, table =>
		{
			// skip check xml table
			if (table.SearchPattern != null || table.Records.Count == 0) return;

			var record1 = table.Records[0];
			var record2 = table.Records[^1];
			var str1 = GetLookup(record1);
			var str2 = table.IsCompressed ? GetLookup(record2) : str1;

			#region common
			// local provider not has 
			if (AliasTable != null)
			{
				var lsts = new List<string>();
				foreach (var lst in AliasTable)
				{
					// compare alias
					if (!str1.Contains(lst[record1.PrimaryKey])) continue;
					if (!str2.Contains(lst[record2.PrimaryKey])) continue;

					// do not directly return
					// due to exist issue when tables with identical aliases 
					lsts.Add(lst.Name);
				}

				lsts.ForEach(x => Add(x, table.Type));
				return;
			}
			#endregion

			#region else
			if (table.IsCompressed)
			{
				if (table.Size > 5000000)
				{
					var FieldSize = record1.DataSize;
					if (FieldSize == 28 || FieldSize == 36)
					{
						Add("text", table.Type);
						return;
					}
				}
			}
			#endregion
		});

		GC.Collect();
	}

	private static HashSet<string> GetLookup(Record record) => record.StringLookup.Strings.ToHashSet(StringComparer.OrdinalIgnoreCase);
	#endregion
}

internal class AliasTableUnit(string name)
{
	public string Name = name;
	public override string ToString() => Name;

	internal Dictionary<Ref, string> Records { get; } = [];

	public string this[Ref Ref] => Records.GetValueOrDefault(Ref);


	internal static List<AliasTableUnit> Split(AliasTable aliasTable)
	{
		if (aliasTable is null) return null;

		var tables = new Dictionary<string, AliasTableUnit>(StringComparer.OrdinalIgnoreCase);

		foreach (var table in aliasTable.Table)
		{
			var ls = table.Key.Split(':', 2);
			if (ls.Length < 2) continue;

			var name = ls[0];
			var alias = ls[1];

			if (!tables.TryGetValue(name, out var collection))
				tables[name] = collection = new(name);

			// maybe dumplicate key
			collection.Records.TryAdd(table.Value, alias);
		}

		return [.. tables.Values];
	}
}