using System.Diagnostics;
using System.IO;
using System.Text;
using Serilog;
using Serilog.Events;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Services;
internal class LogService : TextWriter, IService
{
	#region IService
	public bool Register()
	{
		var folder = UserSettings.Default.OutputFolder;

		if (!Directory.Exists(folder)) return false;
		var logs = Path.Combine(folder, "Logs");

		// clear logs
		var days = UserSettings.Default.KeepLogTime;
		if (days > 0)
		{
			var today = DateTime.Now;

			foreach(var file in Directory.GetFiles(logs, "*.log"))
			{
				var name = Path.GetFileNameWithoutExtension(file);
				if (DateTime.TryParse(name, out var time) && (today - time).Days > days) File.Delete(file);
			}
		}

		// If output directory exists, register the service
		string template = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message:lj}{NewLine}{Exception}";
		Log.Logger = new LoggerConfiguration()
			.WriteTo.Debug(LogEventLevel.Warning, outputTemplate: template)
			.WriteTo.File(Path.Combine(logs, $"{DateTime.Now:yyyy-MM-dd}.log"), outputTemplate: template)
			.CreateLogger();

		return true;
	}
	#endregion

	#region Redirect
	public override Encoding Encoding => Encoding.UTF8;

	public override void WriteLine(string? value)
	{
		// base.WriteLine(value);
		Debug.WriteLine(value);
	}
	#endregion
}