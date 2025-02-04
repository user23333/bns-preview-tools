using System.IO;
using System.Text.RegularExpressions;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.UI.Helpers;
internal class TestProvider
{
	public static void Set(string basePath, Locale locale)
	{
		var dir = new DirectoryInfo(basePath).GetDirectories()
			.Where(o => Regex.IsMatch(o.Name, "^[0-9]{1,8}$")).Last();
		Set(dir, locale);
	}

	public static void Set(DirectoryInfo directory, Locale locale)
	{
		var provider = new FolderProvider(directory.FullName, locale);
		Globals.GameData = new BnsDatabase(provider, Globals.Definition);
	}
}