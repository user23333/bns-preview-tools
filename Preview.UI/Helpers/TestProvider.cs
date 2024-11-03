using System.IO;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.UI.Helpers;
internal class TestProvider
{
	public static void Set(string basePath, EPublisher publisher)
	{
		var dir = new DirectoryInfo(basePath).GetDirectories()[^1];
		Set(dir, publisher);
	}

	public static void Set(DirectoryInfo directory, EPublisher publisher)
	{
		var provider = new FolderProvider(directory.FullName, publisher);
		FileCache.Data = new BnsDatabase(provider, FileCache.Definition);
	}
}