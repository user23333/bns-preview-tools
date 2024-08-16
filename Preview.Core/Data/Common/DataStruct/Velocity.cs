using System.Runtime.InteropServices;
using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Common.DataStruct;
[StructLayout(LayoutKind.Sequential)]
public readonly struct Velocity : IInteger
{
	private readonly ushort source;
	private Velocity(int value) => this.source = (ushort)value;

	public readonly ushort Value => (ushort)(source / 4);


	#region Operator
	public static implicit operator Velocity(ushort value) => new(value * 4);
	public static implicit operator ushort(Velocity value) => value.source;
	public static Velocity operator *(Velocity Velocity, int value) => new(Velocity.source * value);

	public static Velocity operator /(Velocity Velocity, int value) => new(Velocity.source / value);

	public static Velocity operator +(Velocity Velocity1, Velocity Velocity2) => new(Velocity1.source + Velocity2.source);

	public static bool operator ==(Velocity a, Velocity b) => a.source == b.source;

	public static bool operator !=(Velocity a, Velocity b) => !(a == b);

	public override bool Equals(object obj) => obj is Velocity other && this == other;

	public override int GetHashCode() => source.GetHashCode();

	public override string ToString() => Value.ToString();
	#endregion

	#region IConvertible
	TypeCode IConvertible.GetTypeCode() => TypeCode.UInt16;

	ushort IConvertible.ToUInt16(IFormatProvider provider) => Value;

	double IConvertible.ToDouble(IFormatProvider provider) => source * 0.01 * 2;
	#endregion
}