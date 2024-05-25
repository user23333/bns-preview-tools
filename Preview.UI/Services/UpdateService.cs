using System.IO;
using System.Text;
using System.Windows;
using AutoUpdaterDotNET;
using CUE4Parse.UE4.Pak;
using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.Win32;
using Newtonsoft.Json;
using Serilog;
using Xylia.Preview.UI.ViewModels;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Xylia.Preview.UI.Services;
internal class UpdateService : IService
{
	const string APP_NAME = "bns-preview-tools";
	const int TIMEOUT = 10000;
	internal static bool ShowLog { get; private set; } = false;

	#region IService
	static Timer? CheckThread;

	static UpdateService()
	{
		#region Registry
		using RegistryKey hkcu = Registry.CurrentUser;
		using RegistryKey softWare = hkcu.CreateSubKey($@"Software\Xylia\{APP_NAME}", true);

		// check version
		var version = VersionHelper.InternalVersion;
		ShowLog = softWare.GetValue("Version")?.ToString() != version.ToString();

		softWare.SetValue("ExecutablePath", AppDomain.CurrentDomain.BaseDirectory, RegistryValueKind.String);
		softWare.SetValue("Version", version, RegistryValueKind.String);
		#endregion

#if DEVELOP
		return;
#elif DEBUG
		Growl.Info(StringHelper.Get("Application_VersionTip1"));
#endif
		AutoUpdater.RemindLaterTimeSpan = 0;
		AutoUpdater.ReportErrors = true;
		AutoUpdater.DownloadPath = Path.Combine(Environment.CurrentDirectory, "update");
		AutoUpdater.ParseUpdateInfoEvent += ParseUpdateInfoEvent;
		AutoUpdater.CheckForUpdateEvent += CheckForUpdateEvent;
	}

	public bool Register()
	{
		AutoUpdater.Start($"https://tools.bnszs.com/api/update?app={APP_NAME}&version={VersionHelper.InternalVersion}&mode={UserSettings.Default.UpdateMode}");
		CheckThread = new Timer(f => CheckForUpdateEvent(UpdateInfoArgs.Timeout), null, TIMEOUT, Timeout.Infinite);
		return true;
	}

	private static void ParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
	{
		CheckThread?.Dispose();
		args.UpdateInfo = JsonConvert.DeserializeObject<UpdateInfoArgs>(args.RemoteData);
	}

	private static void CheckForUpdateEvent(UpdateInfoEventArgs args2)
	{
		if (args2 is not UpdateInfoArgs args || args.CurrentVersion is null)
		{
			Log.Error(args2.Error.Message);

			// retry or exit
			if (MessageBox.Show(StringHelper.Get("Application_VersionTip3"), StringHelper.Get("Message_Tip"), MessageBoxButton.OKCancel, MessageBoxImage.Error) == MessageBoxResult.OK)
			{
				new UpdateService().Register();
				return;
			}

			Environment.Exit(500);
		}
		else
		{
			IPlatformFilePak.Signature = Encoding.UTF8.GetBytes(args.Signature);

			if (args.NoticeID < 0 || UserSettings.Default.NoticeId < args.NoticeID)
			{
				UserSettings.Default.NoticeId = args.NoticeID;
				Growl.Info(new GrowlInfo() { Message = args.Notice, WaitTime = 15 });
			}

			var currentVersion = new Version(args.CurrentVersion);
			if (currentVersion <= args.InstalledVersion) return;

			Growl.Ask(StringHelper.Get("Application_VersionTip2",
				StringHelper.Get("ProductName"),
				StringHelper.Get("Settings_UpdateMode_" + UserSettings.Default.UpdateMode.ToString()),
				args.CurrentVersion,
				args.InstalledVersion), isConfirmed =>
				{
					if (isConfirmed && AutoUpdater.DownloadUpdate(args))
						Application.Current.Shutdown();

					return true;
				});
		}
	}
	#endregion

	#region Helpers
	class UpdateInfoArgs : UpdateInfoEventArgs
	{
		public int NoticeID { get; set; }
		public string? Notice { get; set; }
		public string? Signature { get; set; }

		public static UpdateInfoArgs Timeout => new()
		{
			Error = new TimeoutException(),
		};
	}

	internal enum UpdateMode
	{
		Stabble,
		Beta,
	}
	#endregion
}