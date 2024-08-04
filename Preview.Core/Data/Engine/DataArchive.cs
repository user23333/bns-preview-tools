using System.Runtime.CompilerServices;
using System.Text;
using CUE4Parse.Utils;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common;

namespace Xylia.Preview.Data.Engine;
internal class DataArchive : Stream
{
	#region Constructor
	private readonly byte[] _data;
	public bool Is64Bit { get; init; }
	public long StartAddress { get; init; }

	public DataArchive(byte[] data, bool is64Bit = false, long offset = 0, long size = -1)
	{
		ArgumentNullException.ThrowIfNull(data);
		_data = data;

		Is64Bit = is64Bit;
		Position = offset;
		Length = size == -1 ? _data.Length : offset + size;
	}
	#endregion

	#region Override Methods
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int Read(byte[] buffer, int offset, int count)
	{
		int n = (int)(Length - Position - StartAddress);
		if (n > count) n = count;
		if (n <= 0)
			return 0;

		if (n <= 8)
		{
			int byteCount = n;
			while (--byteCount >= 0)
				buffer[offset + byteCount] = _data[Position - StartAddress + byteCount];
		}
		else
			Buffer.BlockCopy(_data, (int)Position, buffer, offset, n);
		Position += n;

		return n;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override long Seek(long offset, SeekOrigin origin)
	{
		Position = origin switch
		{
			SeekOrigin.Begin => offset,
			SeekOrigin.Current => Position + offset,
			SeekOrigin.End => Length + offset,
			_ => throw new ArgumentOutOfRangeException()
		};
		return Position;
	}
	public override void Flush() { }
	public override bool CanSeek { get; } = true;
	public override long Length { get; }
	public override long Position { get; set; }
	public override bool CanRead { get; } = true;
	public override bool CanWrite { get; } = false;
	public override void SetLength(long value) { throw new InvalidOperationException(); }
	public override void Write(byte[] buffer, int offset, int count) { throw new InvalidOperationException(); }
	#endregion

	#region Methods
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T Read<T>()
	{
		var position = this.Position;
		var result = Read<T>(ref position);
		this.Position = position;
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual T Read<T>(ref long offset)
	{
		var size = Unsafe.SizeOf<T>();
		var result = Unsafe.ReadUnaligned<T>(ref _data[offset - StartAddress]);
		offset += size;
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual byte[] ReadBytes(int count)
	{
		var result = new byte[count];
		if (count == 0) return result;

		Unsafe.CopyBlockUnaligned(ref result[0], ref _data[Position - StartAddress], (uint)count);
		Position += count;
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long ReadLongInt()
	{
		if (Is64Bit) return Read<long>();
		else return Read<int>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual unsafe void Serialize(byte* ptr, int length)
	{
		Unsafe.CopyBlockUnaligned(ref ptr[0], ref _data[Position - StartAddress], (uint)length);
		Position += length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual T[] ReadArray<T>(int length)
	{
		var size = length * Unsafe.SizeOf<T>();
		var result = new T[length];
		if (length > 0) Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref result[0]), ref _data[Position - StartAddress], (uint)size);
		Position += size;
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void ReadArray<T>(T[] array)
	{
		if (array.Length == 0) return;
		var size = array.Length * Unsafe.SizeOf<T>();
		Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref array[0]), ref _data[Position - StartAddress], (uint)size);
		Position += size;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ReadString()
	{
		var count = Read<int>();
		if (count == 0) return null;
		else if (count > 0) return Encoding.UTF8.GetString(ReadBytes(count));
		else return Encoding.ASCII.GetString(ReadBytes(-count * 2));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string ReadBinaryString(long offset, int maxLength = 500)
	{
		// get position
		if (offset <= 0) return null;

		// get unicode string
		bool flag = false;
		byte[] data = new byte[maxLength];

		for (int i = 0; i < maxLength; i++)
		{
			byte b1 = Read<byte>(ref offset), b2 = Read<byte>(ref offset);
			if (b1 == 0 && b2 == 0) flag = true;
			else if (flag) break;

			data[2 * i] = b1;
			data[2 * i + 1] = b2;
		}

		return data.GetNStringUTF16(0);
	}


	public IEnumerable<long> IndexesOf(byte[] pattern)
	{
		return _data.IndexesOf(0, pattern).Select(x => x + StartAddress);
	}

	public MemoryStream CreateStream()
	{
		return new MemoryStream(_data, (int)(Position - StartAddress), (int)(Length - Position));
	}

	public DataArchive OffsetedSource(long offset, long size)
	{
		if (Position + offset > int.MaxValue)
			throw new OverflowException("Offset doesn't fit inside 32-bit integer");

		if (size > int.MaxValue)
			throw new OverflowException("Size doesn't fit inside 32-bit integer");

		return new DataArchive(_data, Is64Bit, offset, size);
	}
	#endregion
}