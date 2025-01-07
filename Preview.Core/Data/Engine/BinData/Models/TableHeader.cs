using Serilog;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.Data.Engine.BinData.Models;
public abstract class TableHeader
{
	#region Fields
	/// <summary>
	/// name of table
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// element count of table
	/// </summary>
	public byte ElementCount { get; set; }

	/// <summary>
	/// Identifier of table
	/// </summary>
	/// <remarks>Generated automatically according to the sorting of the table name.</remarks>
	public ushort Type { get; set; }

	/// <summary>
	/// major version of table
	/// </summary>
	internal ushort MajorVersion { get; set; }

	/// <summary>
	/// minor version of table
	/// </summary>
	internal ushort MinorVersion { get; set; }

	internal int Size { get; set; }

	internal bool IsCompressed { get; set; }
	#endregion

	#region Methods
	internal void ReadHeaderFrom(DataArchive reader)
	{
		ElementCount = reader.Read<byte>();
		Type = reader.Read<ushort>();
		MajorVersion = reader.Read<ushort>();
		MinorVersion = reader.Read<ushort>();
		Size = reader.Read<int>();
		IsCompressed = reader.Read<bool>();
	}

	internal void WriteHeaderTo(DataArchiveWriter writer)
	{
		writer.Write(ElementCount);
		writer.Write(Type);
		writer.Write(MajorVersion);
		writer.Write(MinorVersion);
	}


	/// <summary>
	/// Compare config version with data version
	/// </summary>
	internal void CheckVersion(string version)
	{
		var strs = version.Split('.' , 2);

		CheckVersion(
			strs.ElementAtOrDefault(0).To<ushort>(),
			strs.ElementAtOrDefault(1).To<ushort>());
	}

	/// <summary>
	/// Compare config version with data version
	/// </summary>
	internal void CheckVersion(ushort major, ushort minor)
	{
		// set version for xml table
		if (MajorVersion == 0 && MinorVersion == 0)
		{
			MajorVersion = major;
			MinorVersion = minor;
		}
		// check definition matches the data
		else if (MajorVersion != major || MinorVersion != minor)
		{
			Log.Warning($"check table `{this.Name}` version: {this.Version} <> {major}.{minor}", "Warning");
		}
	}

	public string Version => string.Format("{0}.{1}", MajorVersion, MinorVersion);
	#endregion
}