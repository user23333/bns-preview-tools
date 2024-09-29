﻿using System.Collections;
using System.ComponentModel;

namespace Xylia.Preview.Common.Extension;
public static class TypeInfoExtensions
{
	/// <summary>
	///  Converts the given value object to the specified type
	/// </summary>
	/// <typeparam name="T">The type to convert the value parameter to</typeparam>
	/// <param name="value">The object to convert</param>
	/// <returns></returns>
	public static T To<T>(this object value)
	{
		value = To(value, typeof(T));
		return value is null ? default : (T)value;
	}

	internal static object To(this object value, Type type, Func<object> failFunc = null)
	{
		if (value != null)
		{
			if (value.GetType() == type)
			{
				return value;
			}

			// system EnumConverter is incomplete
			if (type.IsEnum)
			{
				value.ToString().TryParseToEnum(type, out value);
				return value;
			}

			var converter = TypeDescriptor.GetConverter(value);
			if (converter != null && converter.CanConvertTo(type))
			{
				return converter.ConvertTo(value, type);
			}

			converter = TypeDescriptor.GetConverter(type);
			if (converter != null && converter.CanConvertFrom(value.GetType()))
			{
				return converter.ConvertFrom(value);
			}

			if (failFunc != null) value = failFunc?.Invoke();
		}

		return value;
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