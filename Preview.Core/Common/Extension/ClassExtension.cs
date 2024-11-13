using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Xylia.Preview.Common.Extension;
public static partial class ClassExtension
{
	public static PropertyInfo GetProperty<T>(this T instance, string name)
	{
		var flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase;

		name = name?.Replace("-", null);
		return name is null ? null : instance.GetType().GetProperty(name, flags);
	}

	public static bool IsNullOrDefault<T>(this T argument)
	{
		// deal with normal scenarios
		if (argument == null) return true;
		if (object.Equals(argument, default(T))) return true;

		// deal with non-null nullables
		Type methodType = typeof(T);
		if (Nullable.GetUnderlyingType(methodType) != null) return false;

		// deal with boxed value types
		Type argumentType = argument.GetType();
		if (argumentType.IsValueType && argumentType != methodType)
		{
			object obj = Activator.CreateInstance(argument.GetType());
			return obj.Equals(argument);
		}

		return false;
	}


	#region Attribute
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T GetAttribute<T>(this object value) where T : Attribute
	{
		switch (value)
		{
			case null:
				return null;
			case Enum e:
				return value.GetType().GetField(e.ToString()).GetAttribute<T>();
			case MemberInfo m:
				var attributes = Attribute.GetCustomAttributes(m, typeof(T), false);
				if (attributes.Length == 0) return default;

				return (T)attributes[0];

			default:
				var t = value.GetType();
				return t.GetCustomAttribute<T>();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool HasAttribute<T>(this object value) where T : Attribute => value.HasAttribute(out T _);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool HasAttribute<T>(this object value, out T attribute) where T : Attribute => (attribute = value.GetAttribute<T>()) != null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GetDescription(this object value) => value.GetAttribute<DescriptionAttribute>()?.Description;
	#endregion
}