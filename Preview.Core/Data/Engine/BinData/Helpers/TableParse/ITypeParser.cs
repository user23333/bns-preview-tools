﻿using Xylia.Preview.Data.Engine.Definitions;

namespace Xylia.Preview.Data.Engine.BinData.Helpers;
public interface ITypeParser
{
    /// <summary>
    /// Get table name from type
    /// </summary>
    /// <param name="key"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    bool TryGetName(ushort key, out string name);

    /// <summary>
    /// Get table type from name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    bool TryGetKey(string name, out ushort key);


    /// <summary>
    /// parse table type from name
    /// </summary>
    /// <param name="definitions"></param>
    public void Parse(IEnumerable<TableDefinition> definitions)
    {
        foreach (var def in definitions)
        {
            if (def.Type == 0 && TryGetKey(def.Name, out var _type))
                def.Type = _type;

            foreach (var element in def.Elements)
            {
				foreach (var attribute in element.ExpandedAttributes)
				{
					var TypeName = attribute.ReferedTableName;
					if (TypeName != null && TryGetKey(TypeName, out var type))
						attribute.ReferedTable = type;
				}

				foreach (var subtable in element.Subtables)
				{
					foreach (var attribute in subtable.ExpandedAttributes)
					{
						var TypeName = attribute.ReferedTableName;
						if (TypeName != null && TryGetKey(TypeName, out var type))
							attribute.ReferedTable = type;
					}
				}
			}
        }
    }
}