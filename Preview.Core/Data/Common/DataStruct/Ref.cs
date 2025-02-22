﻿using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Common.DataStruct;
[StructLayout(LayoutKind.Sequential)]
[TypeConverter(typeof(RefConverter))]
public struct Ref(int id, int variant = 0) : IComparable<Ref>
{
	public readonly int Id = id;
	public readonly int Variant = variant;

	#region Operator
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator long(Ref r) => Unsafe.As<Ref, long>(ref r);
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static implicit operator Ref(long key) => Unsafe.As<long, Ref>(ref key);

	public static implicit operator Ref(TRef tref) => tref.Ref;
	public static implicit operator Ref(IconRef iconRef) => iconRef.IconTextureRef;

	public static bool operator ==(Ref a, Ref b) => Unsafe.As<Ref, long>(ref a) == Unsafe.As<Ref, long>(ref b);
	public static bool operator !=(Ref a, Ref b) => !(a == b);

	public static Ref Parse(string input) => TryParse(input, out var key) ? key : throw new ArgumentException("Invalid Ref string input");

	public static bool TryParse(string input, out Ref key)
	{
		key = default;

		var split = input?.Split('.', 2) ?? [];
		if (split.Length >= 1 && int.TryParse(split[0], out var id))
		{
			var variant = split.Length == 2 ? int.Parse(split[1]) : 0;
			key = new Ref(id, variant);
			return true;
		}

		return false;
	}
	#endregion

	#region Methods
	public override readonly string ToString() => Variant == 0 ? $"{Id}" : $"{Id}.{Variant}";

	public bool Equals(Ref other)
	{
		return Unsafe.As<Ref, long>(ref this) == Unsafe.As<Ref, long>(ref other);
	}

	public override bool Equals(object obj)
	{
		return obj is Ref other && Equals(other);
	}

	public override readonly int GetHashCode() => HashCode.Combine(Id, Variant);

	public readonly int CompareTo(Ref other)
	{
		return this.Id == other.Id ?
			this.Variant - other.Variant :
			this.Id - other.Id;
	}

	public readonly Record GetRecord(IDataProvider provider, ushort type, ushort element)
	{
		if (this == default) return null;

		return provider.Tables[type]?[this];
	}
	#endregion
}

internal class RefConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type type)
	{
		if (type == typeof(string)) return true;  // Double is not used at game

		return base.CanConvertFrom(context, type);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) => value switch
	{
		string s => Ref.Parse(s),
		_ => throw new NotSupportedException()
	};
}