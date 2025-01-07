using Xylia.Preview.Common.Extension;

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
		if (string.IsNullOrEmpty(s)) return default;

		var strs = s.Split('.', ',');
		return new BnsVersion(
			strs.ElementAtOrDefault(0).To<ushort>(),
			strs.ElementAtOrDefault(1).To<ushort>(),
			strs.ElementAtOrDefault(2).To<ushort>(),
			strs.ElementAtOrDefault(3).To<ushort>());
	}
	#endregion
}