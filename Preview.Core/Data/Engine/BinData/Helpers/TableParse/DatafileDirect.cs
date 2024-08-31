﻿using IniParser;
using Xylia.Preview.Data.Engine.Definitions;

namespace Xylia.Preview.Data.Engine.BinData.Helpers;
/// <summary>
/// parse from known define
/// </summary>
public sealed class DatafileDirect : ITypeParser
{
	#region Helper
	readonly Dictionary<string, ushort> by_name = new(new TableNameComparer());

	public bool TryGetName(ushort key, out string name) => throw new NotSupportedException();
	public bool TryGetKey(string name, out ushort key) => by_name.TryGetValue(name, out key);
	#endregion

	#region Constructors
	public DatafileDirect(FileInfo path)
	{
		var data = new FileIniDataParser().ReadFile(path.FullName);

		foreach (var table in data["table"])
		{
			var type = ushort.Parse(table.KeyName);
			by_name[table.Value] = type;
		}
	}

	public DatafileDirect(IEnumerable<TableDefinition> definitions)
	{
		ushort type = 1;

		foreach (var definition in definitions)
		{
			by_name[definition.Name] = type++;
		}
	}
	#endregion
}