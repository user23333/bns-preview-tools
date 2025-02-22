﻿using System.Text;
using CUE4Parse.Compression;

namespace Xylia.Preview.Data.Engine.DatData;
public class FileTableEntry
{
	#region Constructor
	internal FileTableEntry(BNSDat owner, DataArchive archive)
	{
		Owner = owner;

		var FilePathLength = (int)archive.ReadLongInt();
		FilePath = Encoding.Unicode.GetString(archive.ReadBytes(FilePathLength * 2));

		Unknown_001 = archive.Read<byte>();
		IsCompressed = archive.Read<bool>();
		IsEncrypted = archive.Read<bool>();
		Unknown_002 = archive.Read<byte>();
		FileDataSizeUnpacked = archive.ReadLongInt();
		FileDataSizeSheared = archive.ReadLongInt();
		FileDataSizeStored = archive.ReadLongInt();
		FileDataOffset = archive.ReadLongInt();
		Padding = archive.ReadBytes(60);
	}

	internal FileTableEntry(BNSDat owner, string path, byte[] data)
	{
		Owner = owner;

		FilePath = path;
		IsCompressed = owner.IsCompressed != CompressionMethod.None;
		IsEncrypted = true;
		Unknown_001 = 2;
		Unknown_002 = 0;
		Padding = new byte[60];
		Data = data;
	}
	#endregion

	#region Fields
	public string FilePath;
	public byte Unknown_001;
	public byte Unknown_002;
	public bool IsCompressed;
	public bool IsEncrypted;

	public long FileDataOffset;        // (relative) offset
	public long FileDataSizeSheared;   // without padding for AES
	public long FileDataSizeStored;
	public long FileDataSizeUnpacked;
	public byte[] Padding;
	internal DataArchive DataArchive; 	

	private byte[] _data;
	#endregion

	#region Properties
	private BNSDat Owner { get; init; }

	private bool IsBinaryXml => Owner.Params.BinaryXmlVersion != BinaryXmlVersion.None && (FilePath.EndsWith(".xml") || FilePath.EndsWith(".x16"));

	public byte[] Data
	{
		set
		{
			_data = value;
			DataArchive = null;
		}
		get
		{
			if (DataArchive != null)
			{
				var buffer = BNSDat.Unpack(DataArchive.CreateStream().ToArray(), FileDataSizeStored, FileDataSizeSheared, FileDataSizeUnpacked, IsEncrypted, Owner.IsCompressed, Owner.Params);

				if (!IsBinaryXml) return buffer;
				else
				{
					var Xml = new BXML_CONTENT(Owner.Params.XOR_KEY);
					Xml.Read(buffer);

					return Xml.ConvertToString();
				}
			}

			return _data;
		}
	}
	#endregion

	#region Methods
	internal void WriteHeader(DataArchiveWriter writer, bool Is64bit, int level, ref long FileDataOffset)
	{
		if (DataArchive is null)
		{
			var data = _data;

			if (IsBinaryXml)
			{
				var bns_xml = new BXML_CONTENT(Owner.Params.XOR_KEY);
				bns_xml.ConvertFrom(data, Owner.Params.BinaryXmlVersion);
				data = bns_xml.Write();
			}

			FileDataSizeUnpacked = data.Length;
			DataArchive = new DataArchive(BNSDat.Pack(data, FileDataSizeUnpacked, out FileDataSizeSheared, out FileDataSizeStored, IsEncrypted, IsCompressed, level, Owner.Params), Is64bit);
		}

		byte[] _filePath = Encoding.Unicode.GetBytes(FilePath);
		if (Is64bit) writer.Write((long)FilePath.Length);
		else writer.Write(FilePath.Length);
		writer.Write(_filePath);

		writer.Write(Unknown_001);
		writer.Write(IsCompressed);
		writer.Write(IsEncrypted);
		writer.Write(Unknown_002);
		writer.WriteLongInt(FileDataSizeUnpacked);
		writer.WriteLongInt(FileDataSizeSheared);
		writer.WriteLongInt(FileDataSizeStored);
		writer.WriteLongInt(FileDataOffset);
		writer.Write(Padding);

		FileDataOffset += FileDataSizeStored;
	}
	#endregion
}