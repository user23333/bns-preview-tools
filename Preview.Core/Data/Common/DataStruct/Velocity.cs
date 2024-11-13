namespace Xylia.Preview.Data.Common.DataStruct;
public readonly struct Velocity(ushort value)
{
	#region Properties
	private readonly ushort Source = value;
	public readonly ushort Value => (ushort)(Source / 4);
	#endregion

	#region Operator
	public static implicit operator Velocity(ushort value) => new(value);
	public static implicit operator ushort(Velocity value) => value.Source;

	public static bool operator ==(Velocity a, Velocity b) => a.Source == b.Source;
	public static bool operator !=(Velocity a, Velocity b) => !(a == b);

	public override bool Equals(object obj) => obj is Velocity other && this == other;

	public override int GetHashCode() => Value.GetHashCode();

	public override string ToString() => Value.ToString();
	#endregion
}