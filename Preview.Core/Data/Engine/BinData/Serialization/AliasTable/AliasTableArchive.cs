using System.Text;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Models;
using static Xylia.Preview.Data.Engine.BinData.Serialization.AliasTableBuilder;

namespace Xylia.Preview.Data.Engine.BinData.Serialization;
internal class AliasTableArchive : AliasTable
{
	#region Constructors
	public AliasTableArchive(DataArchive reader)
	{
		// lazy load
		var position = reader.Position;

		RootEntry.Begin = reader.Read<uint>();
		RootEntry.End = reader.Read<uint>();

		Entries = new AliasTableArchiveEntry[reader.Read<int>()];
		if (reader.Is64Bit) reader.Seek(Entries.Length * 16, SeekOrigin.Current);
		else reader.Seek(Entries.Length * 12, SeekOrigin.Current);

		var stringTableSize = reader.Read<int>();
		reader.Seek(stringTableSize, SeekOrigin.Current);

		Source = reader.OffsetedSource(position, reader.Position - position);
	}

	public AliasTableArchive()
	{

	}
	#endregion

	#region Properties
	public DataArchive Source { get; set; }

	public AliasTableArchiveEntry[] Entries { get; internal set; }

	public AliasTableArchiveEntry RootEntry { get; } = new AliasTableArchiveEntry();


	private Dictionary<string, Ref> _table;
	internal override Dictionary<string, Ref> Table
	{
		get
		{
			if (_table is null)
			{
				_table = [];

				//load data
				Read();
				Expand(RootEntry, string.Empty);
			}

			return _table;
		}
	}
	
	internal override int Count => Entries.Length;
	#endregion

	#region Methods
	public void Read()
	{
		var reader = this.Source;
		RootEntry.Begin = reader.Read<uint>();
		RootEntry.End = reader.Read<uint>();

		Entries = new AliasTableArchiveEntry[reader.Read<int>()];

		for (var i = 0; i < Entries.Length; i++)
		{
			Entries[i] = new AliasTableArchiveEntry(reader);
		}

		var stringTableSize = reader.Read<int>(); // Total size of string table
		var stringTable = reader.ReadBytes(stringTableSize);

		var memoryReader = new BinaryReader(new MemoryStream(stringTable), Encoding.ASCII);

		Span<byte> buffer = stackalloc byte[256];

		foreach (var entry in Entries)
		{
			memoryReader.BaseStream.Seek(entry.StringOffset, SeekOrigin.Begin);
			entry.String = KoreanStringComparer.ReadAliasString(memoryReader, buffer);
		}
	}

	private void Expand(AliasTableArchiveEntry entry, string path)
	{
		path += entry.String;

		if (entry.IsLeaf)
		{
			for (uint i = entry.Begin >> 1; i <= entry.End; i++)
				Expand(Entries[(int)i], path);
		}
		else
		{
			Add(entry.ToRef(), path);
		}
	}
	#endregion
}