using CUE4Parse.Compression;
using CUE4Parse.FileProvider.Objects;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Pak;
public static class PakFileExtension
{
	public static void WriteToDir(this MyPakFileReader pak, string folder, string name)
	{
		var path = Path.Combine(folder, name);
		Directory.CreateDirectory(folder);

		var ms = new MemoryStream();
		pak.Write(new BinaryWriter(ms));
		File.WriteAllBytes(path , ms.ToArray());
	}

	public static void AddFolder(this MyPakFileReader pak, DirectoryInfo folder, string mountpoint = null)
	{
		mountpoint ??= folder.FullName;

		foreach (var file in folder.GetFiles())
		{
			var VfsPath = file.FullName
				.Replace(mountpoint, null)
				.Replace('\\', '/')
				.SubstringAfter("/");

			if (file.Extension == ".ubulk" || file.Extension == ".uexp") pak.Add(file.FullName, VfsPath, CompressionMethod.Oodle);
			else pak.Add(file.FullName, VfsPath);
		}

		foreach (var sub in folder.GetDirectories())
		{
			AddFolder(pak, sub, mountpoint);
		}
	}

	public static List<MyPakFileReader> Split(this MyPakFileReader pak, int capacity = 5000)
	{
		List<MyPakFileReader> paks = new();

		int takeCount = 0;
		while (takeCount < pak.FileCount)
		{
			var sub = new MyPakFileReader(pak.MountPoint);
			var files = (Dictionary<string, GameFile>)sub.Files;
			foreach (var gameFile in pak.Files.Skip(takeCount).Take(takeCount += capacity))
				files.Add(gameFile.Key, gameFile.Value);

			paks.Add(sub);
		}

		return paks;
	}
}						    