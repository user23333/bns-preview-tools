using System.IO;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.UI.Helpers;
internal class TestProvider
{
	public static void Set(string basePath)
	{
		var dir = new DirectoryInfo(basePath).GetDirectories()[^1];
		Set(dir);
	}

	public static void Set(DirectoryInfo directory)
	{
		FileCache.Data = new BnsDatabase(new FolderProvider(directory.FullName));
	}
}