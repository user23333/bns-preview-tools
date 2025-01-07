using CUE4Parse.Compression;

namespace Xylia.Preview.Data.Engine.DatData;
public class CompressProvider : FolderProvider
{
	private readonly BNSDat Package;

	public CompressProvider(string path, Locale locale = default) : base(path, locale)
	{
		if (!File.Exists(path)) throw new FileNotFoundException();

		Package = new BNSDat(new PackageParam(path)
		{
			BinaryXmlVersion = BinaryXmlVersion.None,
			CompressionMethod = CompressionMethod.Oodle,
		});
	}

	public override Stream[] GetFiles(string pattern) => Package!.SearchFiles(pattern).Select(x => new MemoryStream(x.Data)).ToArray();
}