namespace Xylia.Preview.Data.Common.DataStruct;
public readonly struct BnsVersion(ushort major, ushort minor, ushort build, ushort revision) : IComparable<BnsVersion>
{
	#region Fields
	public ushort Major { get; } = major;
	public ushort Minor { get; } = minor;
	public ushort Build { get; } = build;
	public ushort Revision { get; } = revision;
	#endregion

	#region Methods
	public readonly override string ToString() => $"{Major}.{Minor}.{Build}.{Revision}";

	public readonly int CompareTo(BnsVersion other)
	{
		if (this.Major != other.Major) return this.Major - other.Major;
		if (this.Minor != other.Minor) return this.Minor - other.Minor;
		if (this.Build != other.Build) return this.Build - other.Build;
		if (this.Revision != other.Revision) return this.Revision - other.Revision;

		return 0;
	}

	public static BnsVersion Parse(string s)
	{
		var strs = s.Split('.', 4);

		return new BnsVersion(
			ushort.Parse(strs[0]),
			ushort.Parse(strs[1]),
			ushort.Parse(strs[2]),
			ushort.Parse(strs[3]));
	}

	public static bool TryParse(string s, out BnsVersion result)
	{
		try
		{
			result = Parse(s);
			return true;
		}
		catch
		{
			result = default;
			return false;
		}
	}
	#endregion
}