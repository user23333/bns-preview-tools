using System.Collections;
using System.ComponentModel;

namespace Xylia.Preview.Common.Extension;
public static class TypeInfoExtensions
{
	public static T As<T>(this object @this) => (T)As(@this, typeof(T));

	internal static object As(this object @this, Type type)
	{
		if (@this != null)
		{
			Type targetType = type;

			if (@this.GetType() == targetType)
			{
				return @this;
			}

			TypeConverter converter = TypeDescriptor.GetConverter(@this);
			if (converter != null)
			{
				if (converter.CanConvertTo(targetType))
				{
					return converter.ConvertTo(@this, targetType);
				}
			}

			converter = TypeDescriptor.GetConverter(targetType);
			if (converter != null)
			{
				if (converter.CanConvertFrom(@this.GetType()))
				{
					return converter.ConvertFrom(@this);
				}
			}

			if (@this == DBNull.Value)
			{
				return null;
			}
		}

		return @this;
	}

	internal static bool IsEnumerable(this Type type)
	{
		return
			type != typeof(string) &&
			typeof(IEnumerable).IsAssignableFrom(type);
	}

	public static Type GetBaseType(this object value, Type stopper = null)
	{
		ArgumentNullException.ThrowIfNull(value);

		var type = value.GetType();
		if (stopper != null && stopper.IsAssignableFrom(type))
		{
			while (true)
			{
				var baseType = type.BaseType;
				if (baseType is null || baseType == stopper) break;

				type = baseType;
			}
		}

		return type;
	}
}