using System.Text;

namespace Xylia.Preview.Data.Engine.BinData.Serialization;
internal class AliasTableWriter
{
	private static readonly Encoding KoreanEncoding
		= CodePagesEncodingProvider.Instance.GetEncoding(949);

	public static void WriteTo(DataArchiveWriter writer, AliasTableArchive table)
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

        foreach (var entry in table.Entries)
        {
            entry.StringOffset = (int)stringTableMemory.Position;
            stringTableWriter.Write(KoreanEncoding.GetBytes(entry.String));
            stringTableWriter.Write((byte)0);

            writer.WriteLongInt(entry.StringOffset);
            writer.Write(entry.Begin);
            writer.Write(entry.End);
        }

        stringTableWriter.Flush();
		writer.Write((uint)stringTableMemory.Length);
		writer.Flush();

		var buffer = stringTableMemory.GetBuffer();
		writer.Write(buffer, 0, (int)stringTableMemory.Length);
	}
}