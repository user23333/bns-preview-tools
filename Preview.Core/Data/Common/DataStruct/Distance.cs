namespace Xylia.Preview.Data.Common.DataStruct;
public readonly struct Distance(short value)
{
	#region Properties
	private readonly short Source = value;
	public readonly short Value => (short)(Source / 4);
	#endregion

	#region Operator
	public static implicit operator Distance(short value) => new(value);
	public static implicit operator short(Distance value) => value.Source;

	public static bool operator ==(Distance a, Distance b) => a.Source == b.Source;
	public static bool operator !=(Distance a, Distance b) => !(a == b);

	public override bool Equals(object obj) => obj is Distance other && this == other;

	public override int GetHashCode() => Value.GetHashCode();

	public override string ToString() => Value.ToString();
	#endregion
}