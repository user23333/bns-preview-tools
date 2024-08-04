namespace Xylia.Preview.Data.Engine.ZoneData;
public class TerrainCell
{
	#region Fields
	public CellType Type;
	public int AreaIdx;
	public int Param2;
	#endregion

	#region Methods
	internal void Read(DataArchive reader)
    {
        Type = (CellType)reader.Read<int>();
        AreaIdx = reader.Read<int>();
        Param2 = reader.Read<int>();
    }

	internal void Write(DataArchiveWriter writer)
    {
        writer.Write((int)Type);
        writer.Write(AreaIdx);
        writer.Write(Param2);
    }
	#endregion

	public enum CellType
	{
		None,
		Unk1, //1 单元格
		Unk2, //2
		Unk3, //3 删除后缺失入场点，导致无法进入地图
		Unk4,
	}
}

public class HeightParam
{
	public short Min;

	public short Max;


	public HeightParam(short Min, short Max)
	{
		this.Min = Min;
		this.Max = Max;
	}
}