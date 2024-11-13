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
	public DatafileDirect(IEnumerable<TableDefinition> definitions)
	{
		ushort type = 0;

		foreach (var definition in definitions.OrderBy(x => x.Name.Replace("-", null) + "data"))
		{
			if (definition.Type == 0) definition.Type = ++type;
			else type = definition.Type;

			by_name[definition.Name] = definition.Type;
		}
	}
	#endregion
}