using System.Collections;
using System.ComponentModel;

namespace Xylia.Preview.Common.Extension;
public static class TypeInfoExtensions
{
	public static bool IsEnumerable(this Type type)
	{
		return
			type != typeof(string) &&
			typeof(IEnumerable).IsAssignableFrom(type);
	}

	/// <summary>
	///  Converts the given value object to the specified type
	/// </summary>
	/// <typeparam name="T">The type to convert the value parameter to</typeparam>
	/// <param name="value">The object to convert</param>
	/// <returns></returns>
	public static T To<T>(this object value)
	{
		try { value = To(value, typeof(T)); }
		catch { value = null; }

		return value is null ? default : (T)value;
	}

	internal static object To(this object value, Type type, Func<object, Type, object> failFunc = null)
	{
		if (value != null)
		{
			// rewrite
			if (type == value.GetType())
			{
				return value;
			}
			else if (type.IsEnum)
			{
				// system EnumConverter is incomplete
				value.ToString().TryParseToEnum(type, out value);
				return value;
			}
			else if (type.IsArray && value is Array array)
			{
				type = type.GetElementType();
				var newValue = Array.CreateInstance(type, array.Length);

				int idx = 0;
				foreach (var x in array) newValue.SetValue(x.To(type, failFunc), idx++);
				return newValue;
			}

			// converter
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

			// fail	invoke
			if (failFunc != null) return failFunc?.Invoke(value, type);
			throw new InvalidCastException();
		}

		return value;
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