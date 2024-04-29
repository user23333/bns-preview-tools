using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Common.Abstractions;
/// <summary>
/// Specify the conversion operation to <see cref="Integer"/>
/// </summary>
public interface IInteger : IConvertible
{
	object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(this, conversionType, provider);
	
	bool IConvertible.ToBoolean(IFormatProvider provider) => throw new NotSupportedException();

	byte IConvertible.ToByte(IFormatProvider provider) => throw new NotSupportedException();

	char IConvertible.ToChar(IFormatProvider provider) => throw new NotSupportedException();

	DateTime IConvertible.ToDateTime(IFormatProvider provider) => throw new NotSupportedException();

	decimal IConvertible.ToDecimal(IFormatProvider provider) => throw new NotSupportedException();

	short IConvertible.ToInt16(IFormatProvider provider) => throw new NotSupportedException();

	int IConvertible.ToInt32(IFormatProvider provider) => throw new NotSupportedException();

	long IConvertible.ToInt64(IFormatProvider provider) => throw new NotSupportedException();

	sbyte IConvertible.ToSByte(IFormatProvider provider) => throw new NotSupportedException();

	float IConvertible.ToSingle(IFormatProvider provider) => throw new NotSupportedException();

	string IConvertible.ToString(IFormatProvider provider) => this.ToString();

	ushort IConvertible.ToUInt16(IFormatProvider provider) => throw new NotSupportedException();

	uint IConvertible.ToUInt32(IFormatProvider provider) => throw new NotSupportedException();

	ulong IConvertible.ToUInt64(IFormatProvider provider) => throw new NotSupportedException();
}