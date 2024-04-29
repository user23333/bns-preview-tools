using System.Diagnostics;
using CUE4Parse.UE4.Objects.UObject;

namespace Xylia.Preview.Common.Extension;
public static partial class EnumExtension
{
	public static T ToEnum<T>(this object obj, T def = default) where T : Enum
	{
		if (obj is FName n) obj = n.Text;

		if (obj is string s)
		{
			if (s.TryParseToEnum(typeof(T), out var value))
				return (T)value;
		}

		return def;
	}

	public static bool TryParseToEnum(this string str, Type type, out object value)
	{
		value = default;

		if (string.IsNullOrWhiteSpace(str)) return false;
		if (str.Contains('-')) return Enum.TryParse(type, str.Replace("-", null), true, out value);
		if (byte.TryParse(str, out _) && Enum.TryParse(type, "N" + str, true, out value)) return true;

		// convert
		var flag = Enum.TryParse(type, str, true, out value);
		if (!flag) Debug.WriteLine($"cast enum failed: {str} => {type}");

		return flag;
	}
}