namespace Xylia.Preview.Data.Common.DataStruct;
public struct BnsVersion
{
	public ushort Major { get; }
	public ushort Minor { get; }
	public ushort Build { get; }
	public ushort Revision { get; }

	public BnsVersion(string data)
	{
		var strings = data.Split('.');

		this.Major = ushort.Parse(strings[0]);
		this.Minor = ushort.Parse(strings[1]);
		this.Build = ushort.Parse(strings[2]);
		this.Revision = ushort.Parse(strings[3]);
	}

	public BnsVersion(ushort major, ushort minor, ushort build, ushort revision)
	{
		this.Major = major;
		this.Minor = minor;
		this.Build = build;
		this.Revision = revision;
	}


	#region Methods
	public readonly override string ToString() => $"{Major}.{Minor}.{Build}.{Revision}";


	public static implicit operator BnsVersion(string data) => new BnsVersion(data);
	#endregion
}