using System.Collections.Concurrent;
using CUE4Parse.BNS;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.VirtualFileSystem;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Helpers;
internal static partial class Commands
{
	public static bool QueryAsset(string path, string? ext)
	{
		Console.WriteLine($"starting...");

		bool status = false;
		using var provider = new GameFileProvider(UserSettings.Default.GameFolder);

		// convert
		path = provider.FixPath(path) ?? path;
		var filter = path.Split('.')[0];

		// filter
		var props = new ConcurrentDictionary<string, FPropertyTag>();
		foreach (var gamefile in provider.Files)
		{
			if (gamefile.Value is VfsEntry vfsEntry)
			{
				var package = gamefile.Value.Path;
				if (package.Contains(".uasset") && package.Contains(filter, StringComparison.OrdinalIgnoreCase))
				{
					if (ext is not null)
					{
						var objs = provider.LoadPackage(gamefile.Key).GetExports().Where(o => o.ExportType.Equals(ext, StringComparison.OrdinalIgnoreCase));
						if (!objs.Any()) continue;

						if (true) objs.SelectMany(o => o.Properties).ForEach(prop => props.TryAdd(prop.Name.Text, prop));
					}

					status = true;
					Console.WriteLine(string.Concat(vfsEntry.Vfs.Name, "\t", package));
				}
			}
		}

		foreach (var _property in props.OrderBy(o => o.Key))
			Console.WriteLine(_property.Value.Name + " " + _property.Value.Tag.ToString());

		return status;
	}
}