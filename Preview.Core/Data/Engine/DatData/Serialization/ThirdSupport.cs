using System.Data;
using System.Text.RegularExpressions;
using Xylia.Preview.Data.Engine.DatData.Third;

namespace Xylia.Preview.Data.Engine.DatData;
public static class ThirdSupport
{
	/// <summary>
	/// extra package data
	/// </summary>
	/// <param name="param"></param>
	/// <exception cref="ArgumentException"></exception>
	public static void Extract(PackageParam param)
	{
		#region Initialize
		if (string.IsNullOrWhiteSpace(param.FolderPath) && string.IsNullOrWhiteSpace(param.PackagePath))
		{
			throw new ArgumentException("invalid path");
		}
		else if (string.IsNullOrWhiteSpace(param.FolderPath))
		{
			param.FolderPath = Path.GetDirectoryName(param.PackagePath) + @"\Export\" + Path.GetFileNameWithoutExtension(param.PackagePath);
		}
		#endregion

		// delete old files
		if (Directory.Exists(param.FolderPath))
			Directory.Delete(param.FolderPath, true);

		// extract
		Parallel.ForEach(new BNSDat(param).FileTable, file =>
		{
			string path = Path.Combine(param.FolderPath, file.FilePath);
			Directory.CreateDirectory(Path.GetDirectoryName(path));

			File.WriteAllBytes(path, file.Data);
		});

		Console.WriteLine("Extract completed");
	}

	/// <summary>
	/// package data by thild party
	/// </summary>
	/// <param name="param"></param>
	/// <param name="replaces"></param>
	/// <exception cref="ArgumentException"></exception>
	public static void Pack(PackageParam param, IReadOnlyDictionary<string, byte[]> replaces = null, bool useBackup = true)
	{
		#region Initialize
		if (string.IsNullOrWhiteSpace(param.FolderPath) && string.IsNullOrWhiteSpace(param.PackagePath))
		{
			throw new ArgumentException("invalid path");
		}
		else if (string.IsNullOrWhiteSpace(param.PackagePath))
		{
			var dir = new DirectoryInfo(param.FolderPath);
			param.PackagePath = dir.Parent.FullName + "\\" + dir.Name + ".dat";
		}
		else if (string.IsNullOrWhiteSpace(param.FolderPath))
		{
			param.FolderPath = Path.GetDirectoryName(param.PackagePath) + @"\Export\" + Path.GetFileNameWithoutExtension(param.PackagePath);
		}
		#endregion

		#region Replace
		// extract all then replace
		if (replaces != null)
		{
			Extract(param);

			var files = new BNSDat(param).FileTable.Select(file => file.FilePath).ToList();
			foreach (var replace in replaces)
			{
				#region target
				var Target = new List<string>();
				if (files.Contains(replace.Key)) Target.Add(replace.Key);
				else if (replace.Key.Contains('*') || replace.Key.Contains('?'))
				{
					Target.AddRange(files.Where(f => new Regex(replace.Key, RegexOptions.IgnoreCase).Match(f).Success));
				}
				else
				{
					Console.WriteLine("append: " + replace.Key);
					Target.Add(replace.Key);
				}
				#endregion

				Target.ForEach(x => File.WriteAllBytes(param.FolderPath + "\\" + x, replace.Value));
			}
		}

		// create backup
		if (useBackup && File.Exists(param.PackagePath))
			File.Copy(param.PackagePath, param.PackagePath + ".bak", true);
		#endregion

		#region Execute
		var rsa = BnsCompression.GetRSAKeyBlob(param.RSA_KEY);
		double value = BnsCompression.CreateFromDirectory(param.FolderPath, param.PackagePath, param.Bit64, param.CompressionLevel,
			param.AES_KEY, (uint)param.AES_KEY.Length, rsa, (uint)rsa.Length,
			param.BinaryXmlVersion, (string fileName, ulong fileSize) => BnsCompression.DelegateResult.Continue);
		#endregion
	}
}