﻿using System.Runtime.CompilerServices;
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
	public static byte[] ToBytes(this string Hex)
	{
		Hex = Hex.UnCompress();
		if (string.IsNullOrWhiteSpace(Hex))
			return [];

		var inputByteArray = new byte[Hex.Length / 2];
		for (var x = 0; x < inputByteArray.Length; x++)
			inputByteArray[x] = (byte)Convert.ToInt32(Hex.Substring(x * 2, 2), 16);

		return inputByteArray;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string UnCompress(this string Cipher)
	{
		if (string.IsNullOrWhiteSpace(Cipher))
			return Cipher;

		StringBuilder builder = new();
		for (int i = 0; i < Cipher.Length; i++)
		{
			char s = Cipher[i];

			if (i + 1 != Cipher.Length && s == '[')
			{
				StringBuilder InsiderBuilder = new();

				int NextId = i + 1;
				char CurChar = Cipher[NextId];

				while (CurChar != ']')
				{
					InsiderBuilder.Append(CurChar);

					int NewId = ++NextId;

					if (Cipher.Length < NewId + 1) throw new InvalidDataException("missing suffix-label");
					CurChar = Cipher[NewId];
				}

				for (int f = 0; f < int.Parse(InsiderBuilder.ToString()); f++) builder.Append('0');
				InsiderBuilder.Clear();
				i = NextId;
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