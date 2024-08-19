using System.IO;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Helpers;
internal class TestProvider
{
	public static void Set(DirectoryInfo directory)
	{
		var path = directory.GetDirectories()[^1].FullName;
		Set(path);
	}

	public static void Set(string? path = null)
	{
		path ??= Path.Combine(UserSettings.Default.OutputFolder, "data");
		FileCache.Data = new BnsDatabase(new FolderProvider(path));
	}
}