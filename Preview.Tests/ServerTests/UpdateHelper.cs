using System.IO;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine;

namespace Xylia.Preview.Tests.ServerTests;
[TestClass]
public class UpdateHelper
{
	[TestMethod]
	[DataRow("BNSR/Binaries/Win64/BNSR.exe", 50, 4)]
	[DataRow("BNSR/Content/local/ZNCS/data/xml64.dat", 49, 4)]
	[DataRow("BNSR/Content/local/ZNCS/Korean/data/local64.dat", 49, 0)]
	[DataRow("BNSR/Content/Paks/Pak_0_N_PA_UI_11-WindowsNoEditor.pak", 49, 7)]
	public void Download(string path, int version = 1, int part = 0)
	{
		var client = new HttpClient();
		client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");

		var name = Path.Combine("C:\\Users\\Xylia\\downloads", path);

		if (part == 0)
		{
			var url = $"http://bnsneo.ncupdate.com/BNSNEO_LIVE/{version}/Patch/{path}.zip.{version}";
		}
		else
		{
			var archive = new DataArchiveWriter();

			for (int i = 1; i <= part; i++)
			{
				var url = $"http://bnsneo.ncupdate.com/BNSNEO_LIVE/{version}/Patch/{path}.z{i:00}.{version}";
				var stream = DownloadFileAsync(url, name + $".z{i:00}").Result;
				stream.Seek(0, SeekOrigin.Begin);
				stream.CopyTo(archive);
			}

			archive.Seek(0, SeekOrigin.Begin);
			archive.SaveAsync(name + ".7z").Wait();
		}
	}

	static async Task<Stream> DownloadFileAsync(string url, string path)
	{
		// TODO: check sum
		if (File.Exists(path)) return File.OpenRead(path);

		using HttpClient client = new HttpClient();
		using HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
		using Stream stream = await response.Content.ReadAsStreamAsync();

		Directory.CreateDirectory(Path.GetDirectoryName(path)!);
		var fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 8192, true);
		byte[] buffer = new byte[8192];
		int bytesRead;
		long totalBytesRead = 0;
		long totalBytes = response.Content.Headers.ContentLength ?? -1;

		while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
		{
			await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
			totalBytesRead += bytesRead;
		}

		return fileStream;
	}
}