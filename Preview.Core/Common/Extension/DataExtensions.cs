using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Common;
internal static class DataExtensions
{
	// Getters
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static unsafe T Get<T>(this Record record, int offset) where T : unmanaged
	{
		return Get<T>(record.Data, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static unsafe T Get<T>(this byte[] data, int offset) where T : unmanaged
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(data.Length, offset);

		fixed (byte* ptr = data)
		{
			return *(T*)(ptr + offset);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static unsafe T Get<T>(this Span<byte> data, int offset) where T : unmanaged
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(data.Length, offset);

		fixed (byte* ptr = data)
		{
			return *(T*)(ptr + offset);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static unsafe string GetNStringUTF16(this byte[] array, int offset)
	{
		fixed (byte* memory = array)
			return new string((char*)(memory + offset));
	}

	// Setters
	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static unsafe void Set<T>(this byte[] data, int offset, T value) where T : unmanaged
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(data.Length, offset);

		fixed (byte* ptr = data)
		{
			*(T*)(ptr + offset) = value;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public static unsafe void Set(this byte[] data, int offset, object value)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(data.Length, offset);

		if (value is null) return;
		if (value is string) throw new InvalidDataException("String is not expected type.");

		fixed (byte* ptr = data)
		{
			var ptr2 = new nint(ptr + offset);
			Marshal.StructureToPtr(value, ptr2, true);
		}
	}
}