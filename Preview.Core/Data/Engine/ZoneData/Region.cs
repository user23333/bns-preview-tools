using System.Diagnostics;

namespace Xylia.Preview.Data.Engine.ZoneData;
public class Region
{
    #region Fields
    public short Version;
    public long FileSize;   

    public int RegionID;
    public short Xmin;
    public short Xmax;
    public short Ymin;
    public short Ymax;

    public short XRange;
    public short YRange;

    public List<RegionArea> RegionArea;
	#endregion

	#region Methods
	internal void Read(DataArchive reader)
    {
        #region Head
        Version = reader.Read<short>();
        if (Version != 21) Console.WriteLine("地形版本号不为21");

        FileSize = reader.Read<long>();
		RegionID = reader.Read<int>();
		XRange = reader.Read<short>();
		YRange = reader.Read<short>();
		Xmin = reader.Read<short>();
		Xmax = reader.Read<short>();
		Ymin = reader.Read<short>();
		Ymax = reader.Read<short>();

		if (Xmax - Xmin + 1 != XRange) throw new Exception("X 边界异常");
        if (Ymax - Ymin + 1 != YRange) throw new Exception("Y 边界异常");
        Console.WriteLine($"Xmin={Xmin} | Xmax={Xmax} | Ymin={Ymin} | Ymax={Ymax} ({XRange * YRange} 区块)");

        long InfoSize = reader.Read<long>();   //头区域长度 (不包括版本号)
        long AreaOffset = reader.Read<long>(); //区块设置区偏移
        long UnkData = reader.Read<long>();    //未知数据
        if (UnkData != 0) throw new Exception("UnkData 异常");
		#endregion

		#region Area head
		//区块长度  XRange * YRange * 4
		var AreaReader = new BinaryReader(new MemoryStream(reader.ReadBytes((int)(AreaOffset - InfoSize))));
        RegionArea = [];

        if (AreaReader.BaseStream.Length != XRange * YRange * 4)
            throw new Exception("缺失区块");

        for (int Param1 = 0; Param1 < XRange; Param1++)
        {
            for (int Param2 = 0; Param2 < YRange; Param2++)
            {
                var Offset = AreaReader.ReadInt32();
                if (Offset != -1) Trace.WriteLine($"[区块 {Param1} - {Param2}] {Offset}");

                RegionArea.Add(new RegionArea()
                {
                    X = Xmin + Param1 + 1,
                    Y = Ymin + Param2 + 1,

                    Offset = Offset,
                });
            }
        }
        #endregion

        #region Area Body
        //如果所有区块的偏移点都为 -1，则读取结束。否则需要读取偏移区数据
        //偏移区域用于限制区域进入、龙脉和传送门对象的存在。
        if (AreaOffset != FileSize)
        {
            #region Initialize
            var size = (int)(reader.Length - reader.Position);
			Console.WriteLine($"总字节: {reader.Length}   剩余字节: " + size);

            var OffsetArea = reader.ReadBytes(size);
			var OffsetAreaReader = new BinaryReader(new MemoryStream(OffsetArea));
            #endregion

            #region 获取结束偏移
            RegionArea LastArea = null;
            foreach (var a in RegionArea.Where(a => a.Offset != -1))
            {
                if (LastArea != null) LastArea.EndOffset = a.Offset;
                LastArea = a;
            }

            LastArea.EndOffset = OffsetArea.Length;
            #endregion

            foreach (var a in RegionArea.Where(a => a.Offset != -1))
            {
                OffsetAreaReader.BaseStream.Position = a.Offset;
                a.Data = OffsetAreaReader.ReadBytes(a.EndOffset - a.Offset);
            }
        }
		#endregion

		reader.Close();
		reader.Dispose();
    }

	internal void Write(DataArchiveWriter writer)
    {
        XRange = (short)(Xmax - Xmin + 1);
        YRange = (short)(Ymax - Ymin + 1);

        #region Head
        writer.Write(Version);
        writer.Write(0L);   //文件总大小
        writer.Write(RegionID);
        writer.Write(XRange);
        writer.Write(YRange);
        writer.Write(Xmin);
        writer.Write(Xmax);
        writer.Write(Ymin);
        writer.Write(Ymax);

        var InfoOffset = writer.Position;
        writer.Write(writer.Length + 22);  //InfoSize
        writer.Write(0L);     //AreaOffset			     
        writer.Write(0L);
        #endregion

        #region Area Head
        int offset = 0;
        foreach (var a in RegionArea)
        {
            if (a.Data is null || a.Data.Length == 0) writer.Write(-1);
            else
            {
                writer.Write(offset);
				offset += a.Data.Length;
            }
        }
        #endregion

        #region Area Body
        long OffsetArea = writer.Length;
        foreach (var a in RegionArea)
        {
            if (a.Data is not null && a.Data.Length != 0)
                writer.Write(a.Data);
        }
        #endregion

        #region Rewrite
        writer.Position = InfoOffset + 8;
        writer.Write(OffsetArea - 2);

        writer.Position = 2;
        writer.Write(writer.Length - 2);
        #endregion
    }
	#endregion
}

public class RegionArea
{
	public int X;

	public int Y;

	public int Offset;

	/// <summary>
	/// 结束偏移，读取时使用
	/// </summary>
	public int EndOffset;

	public byte[] Data;
}