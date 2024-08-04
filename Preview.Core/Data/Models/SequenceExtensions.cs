using System.ComponentModel;
using System.Reflection;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.Data.Models.Sequence;
public static partial class SequenceExtensions
{
	public static object CastSeq(this string value, string name)
	{
		var type = name.ToEnum<SequenceType>();
		if (type == SequenceType.None) return null;
		else if (type == SequenceType.KeyCap) return KeyCap.Cast(KeyCap.GetKeyCode(value));
		else if (type == SequenceType.KeyCommand) return KeyCommand.Cast(value.ToEnum<KeyCommandSeq>());

		throw new InvalidCastException($"Cast Failed: {name} > {value}");
	}

	public static bool CheckSeq<T>(this T[] seqs, T value) where T : Enum
	{
		T _default = default;

		// check default
		if (value.Equals(_default)) return true;
		return seqs.Any(x => x.Equals(value)) || seqs.All(x => x.Equals(_default));
	}


	public static object LoadSequence(Type type, string val)
	{
		return Enum.Parse(type, val.Replace('-', '_'), true);
	}

	public static object LoadPropSeq(Type type, string val)
	{
		foreach (string text in Enum.GetNames(type))
		{
			MemberInfo[] member = type.GetMember(text);
			if (member != null && member.Length != 0)
			{
				object[] customAttributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
				if (customAttributes != null && customAttributes.Length != 0 && string.Compare(((DescriptionAttribute)customAttributes[0]).Description, val) == 0)
				{
					return Enum.Parse(type, text);
				}
			}
		}
		return null;
	}



	/// <summary>
	/// Set a flag value in the current instance.
	/// </summary>
	/// <param name="seq"></param>
	/// <param name="mask"></param>
	/// <param name="offset"></param>
	/// <param name="status">add or delete</param>
	public static void SetFlag(this Enum seq, ref long mask, int offset, bool status = true)
	{
		// first left shift get flag value, the second left shift is to store in mask
		var flag = (long)1 << (int)(object)seq;

		if (status) mask |= flag << offset;
		else mask &= ~flag << offset;
	}

	/// <summary>
	///  Determines whether one or more bit fields are set in the current instance.
	/// </summary>
	/// <param name="seq"></param>
	/// <param name="mask">An enumeration value</param>
	/// <returns></returns>
	public static bool InFlag(this Enum seq, int mask)
	{
		return (mask & (1 << (int)(object)seq)) != 0;
	}


	/// <summary>
	/// Gets relative description
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string GetText<T>(this T value) where T : Enum
	{
		// get text according attribute
		var text = value.GetAttribute<TextAttribute>()?.Alias.GetText();
		if (text != null) return text;

		// get description
		var description = value.GetAttribute<DescriptionAttribute>()?.Description;
		if (description != null) return description;

		// don't return default
		return value is 0 ? null : value.ToString();
	}
}