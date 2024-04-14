using System.Collections;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Common.Extension;
public static class TypeInfoExtensions
{
    internal static bool IsEnumerable(this Type type)
    {
        return
            type != typeof(String) &&
            typeof(IEnumerable).IsAssignableFrom(type);
    }

	public static Type GetBaseType(this object value)
	{
		ArgumentNullException.ThrowIfNull(value);

		var type = value.GetType();
		if (value is ModelElement)
		{
			while (true)
			{
				var baseType = type.BaseType;
				if (baseType is null || baseType == typeof(ModelElement)) break;

				type = baseType;
			}
		}

		return type;
	}
}