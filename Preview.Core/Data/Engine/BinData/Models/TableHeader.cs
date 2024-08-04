using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.Data.Engine.BinData.Models;
public abstract class TableHeader
{
	#region Fields
	/// <summary>
	/// name of table
	/// </summary>
	public string Name { get; set; }

	public byte ElementCount { get; set; }

	/// <summary>
	/// Identifier of table
	/// </summary>
	/// <remarks>generated automatically according to the sorting of the table name</remarks>
	public ushort Type { get; set; }

	/// <summary>
	/// major version of table
	/// </summary>
	public ushort MajorVersion { get; set; }

	/// <summary>
	/// minor version of table
	/// </summary>
	public ushort MinorVersion { get; set; }

	internal int Size { get; set; }

	internal bool IsCompressed { get; set; }
	#endregion

	#region Methods
	internal MessageManager Message = [];

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
	/// compare config version with game real version
	/// </summary>
	internal void CheckVersion((ushort, ushort) version)
	{
		// set version for xml table
		if (this.MajorVersion == 0 && this.MinorVersion == 0)
		{
			MajorVersion = version.Item1;
			MinorVersion = version.Item2;
		}
		// check definition matches the data
		else if (!MatchVersion(version.Item1 , version.Item2))
		{
			Message.Warning($"check table `{this.Name}` version: {version.Item1}.{version.Item2} <> {this.MajorVersion}.{this.MinorVersion}");
		}
	}

	/// <summary>
	/// compare config version with game real version
	/// </summary>
	/// <returns></returns>
	internal bool MatchVersion(ushort major, ushort minor) => this.MajorVersion == major && this.MinorVersion == minor;

	/// <summary>
	/// parse text version
	/// </summary>
	/// <returns></returns>
	public static (ushort, ushort) ParseVersion(string value)
	{
		var version = value.Split('.');
		var major = (ushort)version.ElementAtOrDefault(0).ToInt16();
		var minor = (ushort)version.ElementAtOrDefault(1).ToInt16();

		return (major, minor);
	}
	#endregion
}