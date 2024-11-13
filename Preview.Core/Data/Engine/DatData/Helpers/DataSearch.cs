using Xylia.Preview.Common;

namespace Xylia.Preview.Data.Engine.DatData;
internal class DataSearch
{
	#region Methods
	private readonly List<FileInfo> files = [];
	public Locale Locale;

	public DataSearch(string path, string pattern = "*.dat")
	{
		var directory = new DirectoryInfo(path);
		if (!directory.Exists) throw new DirectoryNotFoundException(Globals.TextProvider.Get("Exception_InvalidPath"));

		Locale = new Locale(directory);
		files.AddRange(directory.GetFiles(pattern, SearchOption.AllDirectories));
	}

	public IEnumerable<FileInfo> Get(DatType type)
	{
		List<string> names = type switch
		{
			DatType.Config => [CONFIG64, CONFIG],
			DatType.Xml => [XML64, XML, DATAFILE64, DATAFILE],
			DatType.Local => [LOCAL64, LOCAL, LOCALFILE64, LOCALFILE],
			_ => throw new NotSupportedException()
		};

		return files.Where(f => names.Contains(f.Name, StringComparer.OrdinalIgnoreCase));
	}

	public enum DatType
	{
		Local,
		Xml,
		Config,
	}
	#endregion

	#region Constants
	public const string XML = "xml.dat", XML64 = "xml64.dat";
	public const string CONFIG = "config.dat", CONFIG64 = "config64.dat";
	public const string LOCAL = "local.dat", LOCAL64 = "local64.dat";
	public const string DATAFILE = "datafile.bin", DATAFILE64 = "datafile64.bin";
	public const string LOCALFILE = "localfile.bin", LOCALFILE64 = "localfile64.bin";

	public static string Datafile(bool Is64Bit) => Is64Bit ? DATAFILE64 : DATAFILE;
	public static string Localfile(bool Is64Bit) => Is64Bit ? LOCALFILE64 : LOCALFILE;
	#endregion
}