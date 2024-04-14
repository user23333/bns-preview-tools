using System.Collections.Concurrent;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.VirtualFileSystem;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.UI.Helpers;
internal static partial class Commands
{
	public static bool QueryAsset(string path, string? ext, bool useRegistry)
	{
		bool status = false;
		Console.WriteLine($"starting...");

		// convert
		path = FileCache.Provider.FixPath(path, useRegistry) ?? path;
		var filter = path.Split('.')[0];

		// filter
		var props = new ConcurrentDictionary<string, FPropertyTag>();
		foreach (var _gamefile in FileCache.Provider.Files)
		{
			var vfs = ((VfsEntry)_gamefile.Value).Vfs;
			var package = _gamefile.Value.Path;
			if (package.Contains(".uasset") && package.Contains(filter, StringComparison.OrdinalIgnoreCase))
			{
				if (ext is not null)
				{
					var objs = FileCache.Provider.LoadPackage(_gamefile.Key).GetExports().Where(o => o.ExportType == ext);
					if (!objs.Any()) continue;

					if (true) objs.SelectMany(o => o.Properties).ForEach(prop => props.TryAdd(prop.Name.Text, prop));
				}

				status = true;
				Console.WriteLine(string.Concat(vfs.Name, "\t", package));
			}
		}

		foreach (var _property in props.OrderBy(o => o.Key))
			Console.WriteLine(_property.Value.Name + " " + _property.Value.Tag.ToString());

		return status;
	}
}