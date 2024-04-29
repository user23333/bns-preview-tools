using System.Runtime.InteropServices;
using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Common.DataStruct;

[StructLayout(LayoutKind.Sequential)]
public readonly struct Distance : IInteger
{
	private readonly short source;
	private Distance(int value) => this.source = (short)value;

	public readonly short Value => (short)(source / 4);


	#region Operator
	public static implicit operator Distance(short value) => new(value * 4);
	public static implicit operator short(Distance value) => value.source;

	public static Distance operator *(Distance distance, int value) => new(distance.source * value);

	public static Distance operator /(Distance distance, int value) => new(distance.source / value);

	public static Distance operator +(Distance distance1, Distance distance2) => new(distance1.source + distance2.source);

	public static bool operator ==(Distance a, Distance b) => a.source == b.source;

	public static bool operator !=(Distance a, Distance b) => !(a == b);

	public override bool Equals(object obj) => obj is Distance other && this == other;

	public override int GetHashCode() => source.GetHashCode();

	public override string ToString() => Value.ToString();
	#endregion

	#region IConvertible
	TypeCode IConvertible.GetTypeCode() => TypeCode.Int16;

	short IConvertible.ToInt16(IFormatProvider provider) => Value;

	double IConvertible.ToDouble(IFormatProvider provider) => source * 0.01 * 2;
	#endregion
}