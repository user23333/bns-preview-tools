//GeoZone::InitializeByRecordMap zone(1290) boundary (sector) [-16,153] - [28,197] but terrain(1290) is too small [-96,101] - [2,195]
//[geo-terran], FindCell failed; invalid z, zlow
//[geo-cube], GeoCube initialize failed, nearby

using System.Diagnostics;
using Xylia.Preview.Data.Common.DataStruct;
using static Xylia.Preview.Data.Engine.ZoneData.TerrainCell;

namespace Xylia.Preview.Data.Engine.ZoneData;
public class Terrain
{
	#region Fields
	public short Version = 23;
	public long FileSize;
	public int TerrainID;

	public Vector16 Vector1;
	public Vector16 Vector2;
	public short Xmin;
	public short Xmax;
	public short Ymin;
	public short Ymax;
	public short XRange;
	public short YRange;

	public TerrainCell[] AreaList;


	public long Height1_Offset = 0;
	public List<short> Heights1 = [];

	public long Height2_Count = 0;
	public long Height2_Offset = 0;
	public List<HeightParam> Heights2 = [];

	public long Unk4 = 0;    //删除后可以正常运行

	public long GroupOffset = 0;
	public long GroupCount = 0;
	public short[] GroupMeta;

	public long UnkOffset = 0;
	public List<short> UnkData;
	#endregion

	#region Methods
	internal void Read(DataArchive reader)
	{
		#region Header
		Version = reader.Read<short>();
		FileSize = reader.Read<long>();
		TerrainID = reader.Read<int>();

		//  Pos
		Vector1 = reader.Read<Vector16>();
		Vector2 = reader.Read<Vector16>();

		//  Cell
		//读取边界，这里的数据可能是区块
		//比例关系  1区块 = 64坐标系单位  (1 cell = 64 pos)
		Xmin = reader.Read<short>();   //terrain-start-x
		Xmax = reader.Read<short>();
		Ymin = reader.Read<short>();   //terrain-start-y
		Ymax = reader.Read<short>();
		XRange = reader.Read<short>();  //count-x
		YRange = reader.Read<short>();  //count-y

		var Color1 = reader.Read<int>();
		var Color2 = reader.Read<int>();
		var Color3 = reader.Read<int>();

		Console.WriteLine($"#cblue# 区域范围 {Vector1} ~ {Vector2}");
		Console.WriteLine($"Sector  [{Xmin},{Ymin}] ~ [{Xmax},{Ymax}]  ({XRange},{YRange})");

		long MaxIndex = reader.Read<long>();       //最大区块索引
		Height1_Offset = reader.Read<long>(); //区域位置信息偏移1
		Height2_Count = reader.Read<long>();  //区域位置2 对象数量
		Height2_Offset = reader.Read<long>(); //区域位置2 信息偏移
		Unk4 = reader.Read<long>();           //地形 7430 不为0
		GroupOffset = reader.Read<long>();    //组信息位置偏移
		GroupCount = reader.Read<long>();     //
		UnkOffset = reader.Read<long>();      //未知区域偏移
		#endregion

		#region Cell
		AreaList = new TerrainCell[XRange * YRange];
		for (int i = 0; i < AreaList.Length; i++)
		{
			var area = AreaList[i] = new TerrainCell();
			area.Read(reader);
		}

		//对于类型3，Param2 指类型分区的数量    而其他类型则常为0，未知作用
		Console.WriteLine($"类型1 {AreaList.Where(a => a.Type == CellType.Unk1).Count()}    类型2 {AreaList.Where(a => a.Type == CellType.Unk2).Count()}");
		Console.WriteLine($"类型3 {AreaList.Where(a => a.Type == CellType.Unk3).Count()}    类型4 {AreaList.Where(a => a.Type == CellType.Unk4).Count()}");
		#endregion

		#region 高度区域偏移
		Heights1 = new List<short>();
		Heights2 = new List<HeightParam>();

		reader.Position = Height1_Offset + 2;
		while (reader.Position < Height2_Offset + 2) Heights1.Add(reader.Read<short>());

		while (reader.Position < GroupOffset + 2) Heights2.Add(new HeightParam(reader.Read<short>(), reader.Read<short>()));


		Console.WriteLine($"unk4: {Unk4}  Heights: {Heights1.Count},{Heights2.Count}  GroupOffset:{GroupOffset} UnkOffset: {UnkOffset}");
		#endregion

		#region 未知集合
		reader.Position = GroupOffset + 2;

		GroupMeta = new short[GroupCount];
		for (int i = 0; i < GroupMeta.Length; i++) GroupMeta[i] = reader.Read<short>();
		#endregion

		#region 未知集合2
		if (GroupOffset != UnkOffset)
		{
			Debug.WriteLine($"{GroupOffset} ({GroupCount})   " + UnkOffset);
			Debug.WriteLine(reader.Position + "  " + reader.Length);
		}
		#endregion

		reader.Close();
		reader.Dispose();
	}

	internal void Write(DataArchiveWriter writer)
	{
		#region Initialize
		writer.Write(Version);
		writer.Write(FileSize);
		writer.Write(TerrainID);
		writer.Write(Vector1);
		writer.Write(Vector2);

		writer.Write(Xmin);
		writer.Write(Xmax);
		writer.Write(Ymin);
		writer.Write(Ymax);
		writer.Write(XRange);
		writer.Write(YRange);

		writer.Write(0);
		writer.Write(112);
		writer.Write(0);

		//MaxIndex
		writer.Write((long)(AreaList.Max(a => a.Type == CellType.Unk1 || a.Type == CellType.Unk2 ? a.AreaIdx : 0) + 1));

		//偏移数据到最后重写
		writer.Write(0L);   //Height1_Offset
		writer.Write((long)Heights2.Count);
		writer.Write(0L);   //Height2_Offset
		writer.Write(Unk4);
		writer.Write(0L);
		writer.Write((long)(GroupMeta?.Length ?? 0));
		writer.Write(0L);
		#endregion

		#region 处理单元格区域
		foreach (var CurArea in AreaList)
		{
			CurArea.Write(writer);
		}
		#endregion

		// 高度集合
		var heigthOffset1 = writer.Position;
		Heights1.ForEach(o => writer.Write(o));

		var heigthOffset2 = writer.Position;
		Heights2.ForEach(o =>
		{
			writer.Write(o.Min);
			writer.Write(o.Max);
		});

		// 未知集合
		var groupOffset = writer.Position;
		foreach (var c in GroupMeta) writer.Write(c);

		#region Rewrite
		//重写长度
		writer.Position = 2;
		writer.Write(writer.Length - 2);

		//heigthOffset1
		writer.Position = 0x3A;
		writer.Write(heigthOffset1 - 2);

		//heigthOffset2
		writer.Position = 0x4A;
		writer.Write(heigthOffset2 - 2);

		//groupOffset
		writer.Position = 0x5A;
		writer.Write(groupOffset - 2);

		//groupOffset2
		writer.Position = 0x6A;
		writer.Write(groupOffset - 2);

		writer.Flush();
		writer.Close();
		writer.Dispose();
		#endregion
	}
	#endregion
}