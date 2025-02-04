using CUE4Parse.Compression;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Pak;
public static class PakFileExtension
{
	public static void Save(this MyPakFileReader pak, string path, string sigpath = null)
	{
		var ms = new MemoryStream();
		pak.Write(new BinaryWriter(ms));

		File.WriteAllBytes(path + ".pak", ms.ToArray());
		if (sigpath != null) File.Copy(sigpath, path + ".sig", true);
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
}