using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.Tests.DatTests.Tools;
internal class CommonConvert(byte[] value)
{
    public CommonConvert(long value) : this(BitConverter.GetBytes(value)) { }

    public int Length => value.Length;

    public int? Int32 => value.Length >= 4 ? BitConverter.ToInt32(value, 0) : null;

    public long? Long => value.Length >= 8 ? BitConverter.ToInt64(value, 0) : null;

    public short? Short1 => value.Length >= 2 ? BitConverter.ToInt16(value, 0) : null;

    public short? Short2 => value.Length >= 4 ? BitConverter.ToInt16(value, 2) : null;

    public float? Float => value.Length >= 4 ? BitConverter.ToSingle(value, 0) : null;


    public override string ToString() => value.ToHex();

    public object MainValue =>
        Length >= 8 ? Long :
        Length >= 4 ? Int32 :
        Length >= 2 ? Short1 :
        null;
}