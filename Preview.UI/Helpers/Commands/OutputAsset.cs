using System.Collections.Concurrent;
using System.IO;
using CUE4Parse.BNS;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.BNS.Conversion;
using CUE4Parse.UE4.Assets.Exports.Sound;
using CUE4Parse_Conversion.Sounds;
using CUE4Parse_Conversion.Textures;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Helpers;
internal static partial class Commands
{
	public static void OutputSoundWave()
	{
		using var provider = new GameFileProvider(UserSettings.Default.GameFolder, true);
		var assets = provider.AssetRegistryModule.GetAssets(x => x.AssetClass.Text == "SoundWave").ToArray();
		Console.WriteLine($"total: {assets.Length}");

		#region Progress
		int current = 0;
		int cursor = Console.CursorTop;

		var timer = new System.Timers.Timer(1000);
		timer.Elapsed += (_, _) =>
		{
			Console.SetCursorPosition(0, cursor);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, cursor);
			Console.Write($"output {(double)current / assets.Length:P0}");
		};
		timer.Start();
		#endregion

		Parallel.ForEach(assets, asset =>
		{
			try
			{
				current++;

				var Object = provider.LoadObject<USoundWave>(asset.ObjectPath);
				if (Object != null)
				{
					Object.Decode(true, out var audioFormat, out var data);
					File.WriteAllBytes(Exporter.FixPath(UserSettings.Default.OutputFolderResource, Object.GetPathName()) + "." + audioFormat, data);
				}
			}
			catch
			{

			}
		});
	}

	public static void OutputFontSet()
	{
		using var provider = new GameFileProvider(UserSettings.Default.GameFolder, true);
		var assets = provider.AssetRegistryModule.GetAssets(x => x.AssetClass.Text == "FontSet").ToArray();
		Console.WriteLine($"total: {assets.Length}");

		#region Progress
		int current = 0;
		int cursor = Console.CursorTop;

		var timer = new System.Timers.Timer(1000);
		timer.Elapsed += (_, _) =>
		{
			Console.SetCursorPosition(0, cursor);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, cursor);
			Console.Write($"output {(double)current / assets.Length:P0}");
		};
		timer.Start();
		#endregion

		var data = new ConcurrentDictionary<string, string>();
		Parallel.ForEach(assets, asset =>
		{
			try
			{
				current++;

				var FontSet = provider.LoadObject<UFontSet>(asset.ObjectPath);
				if (FontSet != null)
				{
					var FontColor = FontSet.FontColors?.Load<UFontColor>();
					if (FontColor != null)
					{
						var color = FontColor.FontColor.Hex;
						var text = asset.ObjectPath2 ?? asset.ObjectPath;

						data.TryAdd(text, $"<p style=\"color:#{color};\">{text}</p>");
					}
				}
			}
			catch
			{

			}
		});

		data.TryAdd("@header", "<body style=\"background-color: gray;\">");
		File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\font.html", data.OrderBy(x => x.Key).Select(x => x.Value));
	}
}