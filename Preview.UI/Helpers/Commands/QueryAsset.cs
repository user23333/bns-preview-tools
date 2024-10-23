using CUE4Parse.BNS;
using CUE4Parse.UE4.VirtualFileSystem;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Helpers;
internal static partial class Commands
{
	public static bool QueryAsset(string path, string? ext)
	{
		Console.WriteLine($"starting...");

		using var provider = new GameFileProvider(UserSettings.Default.GameFolder);
		var comparer = StringComparison.OrdinalIgnoreCase;
		bool status = false;

		// convert
		path = provider.FixPath(path) ?? path;
		var filter = path.Split('.')[0];

		// filter
		foreach (var gamefile in provider.Files)
		{
			if (gamefile.Value is VfsEntry vfsEntry)
			{
				var package = gamefile.Value.Path;
				if (filter != null && !package.Contains(filter, comparer)) continue;

				// extension & tag
				if (package.EndsWith(".uasset"))
				{
					if (ext != null && !provider.LoadPackage(vfsEntry).GetExports().Any(o => o.ExportType.Equals(ext, comparer))) continue;
				}

				status = true;
				Console.WriteLine(string.Concat(vfsEntry.Vfs.Name, "\t", vfsEntry.Path));
			}
		}

		return status;
	}
}