﻿using System.Text;

namespace Xylia.Preview.Data.Engine.BinData.Serialization;
internal class AliasTableWriter
{
	private static readonly Encoding KoreanEncoding
		= CodePagesEncodingProvider.Instance.GetEncoding(949);

	public static void WriteTo(DataArchiveWriter writer, AliasTableArchive table, bool is64Bit)
	{
		if (table.Source != null)
		{
			using var stream = table.Source.CreateStream();
			stream.CopyTo(writer);
			return;
		}

		writer.Write(table.RootEntry.Begin);
		writer.Write(table.RootEntry.End);
		writer.Write(table.Entries.Count);

		var stringTableMemory = new MemoryStream();
		var stringTableWriter = new BinaryWriter(stringTableMemory, Encoding.Default, true);

		if (is64Bit)
		{
			foreach (var entry in table.Entries)
			{
				var offset = stringTableMemory.Position;

				entry.StringOffset = (int)offset;

				stringTableWriter.Write(KoreanEncoding.GetBytes(entry.String));
				stringTableWriter.Write((byte)0);

				WriteEntry64(writer, entry);
			}
		}
		else
		{
			foreach (var entry in table.Entries)
			{
				var offset = stringTableWriter.BaseStream.Position;

				entry.StringOffset = (int)offset;

				stringTableWriter.Write(KoreanEncoding.GetBytes(entry.String));
				stringTableWriter.Write((byte)0);

				WriteEntry(writer, entry);
			}
		}

		stringTableWriter.Flush();
		writer.Write((uint)stringTableMemory.Length);
		writer.Flush();

		var buffer = stringTableMemory.GetBuffer();
		writer.Write(buffer, 0, (int)stringTableMemory.Length);
	}

	private static void WriteEntry(DataArchiveWriter writer, AliasTableArchiveEntry entry)
	{
		writer.Write((int)entry.StringOffset);
		writer.Write(entry.Begin);
		writer.Write(entry.End);
	}

	private static void WriteEntry64(DataArchiveWriter writer, AliasTableArchiveEntry entry)
	{
		writer.Write(entry.StringOffset);
		writer.Write(entry.Begin);
		writer.Write(entry.End);
	}
}