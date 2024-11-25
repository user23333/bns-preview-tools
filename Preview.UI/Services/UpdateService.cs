using System.IO;
using System.Text;
using System.Windows;
using AutoUpdaterDotNET;
using CUE4Parse.UE4.Pak;
using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.Win32;
using Newtonsoft.Json;
using Xylia.Preview.UI.ViewModels;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Xylia.Preview.UI.Services;
internal class UpdateService : IService
{
	const string APP_NAME = "bns-preview-tools";

	#region IService
	static UpdateService()
	{
		#region Registry
		using RegistryKey hkcu = Registry.CurrentUser;
		using RegistryKey softWare = hkcu.CreateSubKey($@"Software\Xylia\{APP_NAME}", true);

		// check version
		var version = VersionHelper.InternalVersion;
		ShowLog = softWare.GetValue("Version")?.ToString() != version.ToString();

		softWare.SetValue("ExecutablePath", Environment.ProcessPath!, RegistryValueKind.String);
		softWare.SetValue("Version", version, RegistryValueKind.String);
		#endregion

		AutoUpdater.RemindLaterTimeSpan = 0;
		AutoUpdater.ReportErrors = true;
		AutoUpdater.DownloadPath = Path.Combine(Environment.CurrentDirectory, "update");
		AutoUpdater.ParseUpdateInfoEvent += ParseUpdateInfoEvent;
		AutoUpdater.CheckForUpdateEvent += CheckForUpdateEvent;
	}

	bool IService.Register()
	{
		Register();
		return true;
	}

	public static void Register(int waitTime = 0)
	{
		Thread.Sleep(waitTime);
		AutoUpdater.Start($"http://tools.bnszs.com/api/update?app={APP_NAME}&version={VersionHelper.InternalVersion}&mode={UserSettings.Default.UpdateMode}");
	}

	private static void ParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
	{
		var updateInfo = JsonConvert.DeserializeObject<UpdateInfoArgs>(args.RemoteData);
		if (updateInfo != null && updateInfo.CurrentVersion is null) throw new Exception(updateInfo.Message);

		args.UpdateInfo = updateInfo;
	}

	private static void CheckForUpdateEvent(UpdateInfoEventArgs args2)
	{
		if (args2 is not UpdateInfoArgs args)
		{
			if (MessageBox.Show(StringHelper.Get("Application_VersionTip3", args2.Error.Message),
				StringHelper.Get("Message_Tip"), MessageBoxButton.OKCancel, MessageBoxImage.Error) == MessageBoxResult.OK)
			{
				// wait current worker finished. 
				Task.Run(() => Register(1000));
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
		public string? Message { get; set; }
		public int NoticeID { get; set; }
		public string? Notice { get; set; }
		public string? Signature { get; set; }
	}

	internal enum UpdateMode
	{
		Stabble,
		Beta,
	}

	internal static bool ShowLog { get; private set; } = false;
	#endregion
}