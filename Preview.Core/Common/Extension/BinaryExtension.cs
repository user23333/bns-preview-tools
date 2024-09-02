using System.Runtime.CompilerServices;
using System.Text;

namespace Xylia.Preview.Common.Extension;
public static class BinaryExtension
{
	#region Common
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Read<T>(this BinaryReader reader)
	{
		var size = Unsafe.SizeOf<T>();
		var buffer = reader.ReadBytes(size);
		return Unsafe.ReadUnaligned<T>(ref buffer[0]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Write<T>(this BinaryWriter writer, T value) where T : struct
	{
		var size = Unsafe.SizeOf<T>();
		var data = new byte[size];

		unsafe
		{
			fixed (byte* p = &Unsafe.As<T, byte>(ref value))
			{
				using UnmanagedMemoryStream ms = new UnmanagedMemoryStream(p, size);
				ms.Read(data, 0, data.Length);
			}
		}

		writer.Write(data);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task SaveAsync(this Stream stream, string path)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(path)!);

		await using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		await stream.CopyToAsync(fs);
		await fs.FlushAsync();

		// return position
		stream.Seek(0, SeekOrigin.Begin);
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GetReadableSize(double size)
	{
		if (size == 0) return "0 B";

		string[] sizes = ["B", "KB", "MB", "GB", "TB"];
		var order = 0;
		while (size >= 1024 && order < sizes.Length - 1)
		{
			order++;
			size /= 1024;
		}

		return $"{size:# ###.##} {sizes[order]}".TrimStart();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ToHex(this byte[] value, bool original = true)
	{
		if (value.Length == 0)
		{
			return string.Empty;
		}

		StringBuilder sb = new();
		for (int i = 0; i < value.Length; i++)
		{
			var b = value[i];

			#region compress 
			if (!original && b == 0)
			{
				int j = i;
				for (; j < value.Length; j++)
					if (value[j] != 0) break;

				if (j - i > 1)
				{
					sb.Append($"[{j - i}]");

					i = j - 1;
					continue;
				}
			}
			#endregion

			sb.AppendFormat("{0:X2}", b);
		}

		return sb.ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static byte[] ToBytes(this string data)
	{
		data = data.UnCompress();
		if (string.IsNullOrWhiteSpace(data))
			return [];

		var inputByteArray = new byte[data.Length / 2];
		for (var x = 0; x < inputByteArray.Length; x++)
			inputByteArray[x] = (byte)Convert.ToInt32(data.Substring(x * 2, 2), 16);

		return inputByteArray;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string UnCompress(this string data)
	{
		if (string.IsNullOrWhiteSpace(data))
			return data;

		StringBuilder builder = new();
		for (int i = 0; i < data.Length; i++)
		{
			char s = data[i];

			if (s == '[')
			{
				StringBuilder num = new();

				for (i++; i <= data.Length; i++)
				{
					if (i == data.Length) throw new InvalidDataException("missing suffix-label");

					var c2 = data[i];
					if (c2 == ']') break;

					num.Append(c2);
				}

				builder.Append('0', int.Parse(num.ToString()));
			}
			else if (s == ']') throw new InvalidDataException("invalid suffix-label");
			else builder.Append(s);
		}

		return builder.ToString().Replace(" ", null);
	}
	#endregion

	#region Search
	public static IEnumerable<long> IndexesOf(this byte[] source, int start, byte[] pattern)
	{
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(pattern);

		long valueLength = source.LongLength;
		long patternLength = pattern.LongLength;

		if ((valueLength == 0) || (patternLength == 0) || (patternLength > valueLength))
		{
			yield break;
		}

		var badCharacters = new long[256];

		for (var i = 0; i < 256; i++)
		{
			badCharacters[i] = patternLength;
		}

		var lastPatternByte = patternLength - 1;

		for (long i = 0; i < lastPatternByte; i++)
		{
			badCharacters[pattern[i]] = lastPatternByte - i;
		}

		long index = start;

		while (index <= valueLength - patternLength)
		{
			for (var i = lastPatternByte; source[index + i] == pattern[i]; i--)
			{
				if (i == 0)
				{
					yield return index;
					break;
				}
			}

			index += badCharacters[source[index + lastPatternByte]];
		}
	}
	#endregion
}