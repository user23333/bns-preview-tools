using System.Text;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Engine.BinData.Models;
public class DatafileHeader
{
	#region Fields
	public string Magic { get; set; }    
	public byte DatafileVersion { get; set; }
	public BnsVersion ClientVersion { get; set; }
	public long TotalTableSize { get; set; }
	public long ReadTableCount { get; private set; }
	public long AliasMapSize { get; set; }
	public long AliasCount { get; set; }
	public long MaxBufferSize { get; set; }
	public Time64 CreatedAt { get; set; }
	public byte[] Reserved { get; set; }
	#endregion

	#region Methods
	internal void ReadHeaderFrom(DataArchive reader)
	{
		Magic = Encoding.ASCII.GetString(reader.ReadBytes(8));
		DatafileVersion = reader.Read<byte>();
		ClientVersion = reader.Read<BnsVersion>();
		TotalTableSize = reader.ReadLongInt();
		ReadTableCount = reader.ReadLongInt();
		AliasMapSize = reader.ReadLongInt();
		AliasCount = reader.ReadLongInt();
		MaxBufferSize = reader.ReadLongInt();
		CreatedAt = reader.Read<Time64>();
		Reserved = reader.ReadBytes(54);
	}

	internal Action<long> WriteHeaderTo(DataArchiveWriter writer, long tableCount, long aliasCount, bool is64Bit)
	{
		writer.Write(Encoding.ASCII.GetBytes(Magic));
		writer.Write(DatafileVersion);
		writer.Write(ClientVersion);

		Action<long> overwriteSize;

        writer.WriteLongInt(TotalTableSize);
        writer.WriteLongInt(tableCount);
        var offset = writer.Position;
        overwriteSize = x =>
        {
            var oldPosition = writer.Position;
            writer.Seek(offset, SeekOrigin.Begin);
            writer.WriteLongInt(x);
            writer.Seek(oldPosition, SeekOrigin.Begin);
        };
		writer.WriteLongInt(0);
        writer.WriteLongInt(aliasCount);
        writer.WriteLongInt(MaxBufferSize);
        writer.Write(CreatedAt);
		writer.Write(Reserved);

		return overwriteSize;
	}
	#endregion
}