﻿using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Engine.BinData.Serialization;

namespace Xylia.Preview.Data.Engine.BinData.Models;
public abstract class Datafile
{
	#region Fields
	public byte DatafileVersion { get; set; } = 5;
	public BnsVersion ClientVersion { get; set; }
	public Time64 CreatedAt { get; set; }
	public long AliasCount { get; set; }
	public long AliasMapSize { get; set; }
	internal AliasTable AliasTable { get; set; }
	public TableCollection Tables { get; set; }
	public bool Is64Bit { get; protected set; }
	#endregion

	#region	Serialize
	protected void ReadFrom(byte[] bytes, bool is64bit)
	{
		if (bytes is null) return;

		using var reader = new DataArchive(bytes, is64bit);

		var bin = new DatafileHeader();
		bin.ReadHeaderFrom(reader);

		if (bin.ReadTableCount > 10)
		{
			this.DatafileVersion = bin.DatafileVersion;
			this.ClientVersion = bin.ClientVersion;
			this.CreatedAt = bin.CreatedAt;
			this.AliasCount = bin.AliasCount;
			this.AliasMapSize = bin.AliasMapSize;
			this.AliasTable = new AliasTableArchive(reader);
		}

		for (var tableId = 0; tableId < bin.ReadTableCount; tableId++)
		{
			this.Tables.Add(TableArchive.LazyLoad(reader));
		}
	}

	protected byte[] WriteTo(Table[] tables, bool is64bit)
	{
		using var writer = new DataArchiveWriter(is64bit);

		var datafileHeader = new DatafileHeader
		{
			Magic = "TADBOSLB",
			Reserved = new byte[54],
			CreatedAt = CreatedAt, //DateTime.Now,
			DatafileVersion = DatafileVersion,
			ClientVersion = ClientVersion,
			MaxBufferSize = 0x0,   //MaxBufferSize = AliasMapSize ?
			TotalTableSize = 0x1,  //TotalTableSize = bytes.Length - reader.Position - bin.ReadTableCount * 4
		};

		var overwriteNameTableSize = datafileHeader.WriteHeaderTo(writer, tables.Length, AliasCount, is64bit);

		if (AliasTable == null) 
			overwriteNameTableSize(AliasMapSize);

		if (tables.Length > 10)
		{
			if (AliasTable is not AliasTableArchive alias) 
				throw new NullReferenceException("Missing AliasTable on main datafile.");

			var oldPosition = writer.Position;
			AliasTableWriter.WriteTo(writer, alias);

			var nameTableSize = writer.Position - oldPosition;
			this.AliasMapSize = nameTableSize;
			this.AliasCount = alias.Entries.Length;
			overwriteNameTableSize(nameTableSize);
		}

		var tableWriter = new TableWriter();
        foreach (var table in tables)
		{
			tableWriter.WriteTo(writer, table, is64bit);
		}

		writer.Flush();
		return writer.ToArray();
	}
	#endregion
}