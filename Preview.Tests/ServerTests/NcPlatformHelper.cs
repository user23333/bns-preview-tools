using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xylia.Preview.Tests.ServerTests;
[TestClass]
public class NcPlatformHelper
{
	const string AppName = "NcPlatform";
	// /AppIndex /Email /SpawnPorts /HideWindow /RunService /InstallService /UserName /Password

	[TestMethod]
	[DataRow(@"D:\Build\BladeAndSoul\Server\StServers\Bin\SpawnSrv.exe")]
	public void Install(string path)
    {
		Execute(new ProcessStartInfo
		{
			FileName = "sc",
			Arguments = $"create {AppName} binPath= \"{path} /AppIndex 1 /RunService\"",
			UseShellExecute = false,
			RedirectStandardOutput = true,
		});
	}

	[TestMethod]
	public void UnInstall()
	{
		Execute(new ProcessStartInfo
		{
			FileName = "sc",
			Arguments = $"delete {AppName}",
			UseShellExecute = false,
			RedirectStandardOutput = true,
		});
	}

	[TestMethod]
	public void Start()
	{
		Execute(new ProcessStartInfo
		{
			FileName = "sc",
			Arguments = $"start {AppName}",
			UseShellExecute = false,
			RedirectStandardOutput = true,
		});
	}

	[TestMethod]
	public void Stop()
	{
		Execute(new ProcessStartInfo
		{
			FileName = "sc",
			Arguments = $"stop {AppName}",
			UseShellExecute = false,
			RedirectStandardOutput = true,
		});
	}


	private static void Execute(ProcessStartInfo startinfo)
	{
		var proc = Process.Start(startinfo);

		while (!proc.StandardOutput.EndOfStream)
		{
			Debug.WriteLine(proc.StandardOutput.ReadLine());
		}
	}
}