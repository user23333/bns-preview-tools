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
	#region Static Methods
	const string APP_NAME = "bns-preview-tools";
	internal static bool ShowLog { get; private set; } = false;

	static UpdateService()
	{
		#region Registry
		using RegistryKey hkcu = Registry.CurrentUser;
		using RegistryKey softWare = hkcu.CreateSubKey($@"Software\Xylia\{APP_NAME}", true);

		ShowLog = softWare.GetValue("Version")?.ToString() != VersionHelper.InternalVersion.ToString();

		softWare.SetValue("ExecutablePath", AppDomain.CurrentDomain.BaseDirectory, RegistryValueKind.String);
		softWare.SetValue("Version", VersionHelper.InternalVersion, RegistryValueKind.String);
		#endregion

#if DEVELOP
		return;
#elif DEBUG
		Growl.Info(StringHelper.Get("Application_VersionTip1"));
#endif
		AutoUpdater.RemindLaterTimeSpan = 0;
		AutoUpdater.ParseUpdateInfoEvent += ParseUpdateInfoEvent;
		AutoUpdater.CheckForUpdateEvent += CheckForUpdateEvent;
	}

	private static void ParseUpdateInfoEvent(ParseUpdateInfoEventArgs args)
	{
		args.UpdateInfo = JsonConvert.DeserializeObject<UpdateInfoArgs>(args.RemoteData);
	}

	private static void CheckForUpdateEvent(UpdateInfoEventArgs args)
	{
		if (args is UpdateInfoArgs args2)
			CheckForUpdateEvent(args2);
	}

	private static void CheckForUpdateEvent(UpdateInfoArgs args)
	{
		if (args.CurrentVersion is null)
		{
			Log.Error(args.Error.Message);
			Growl.Error(StringHelper.Get("Application_VersionTip3"));
			MessageBox.Show(StringHelper.Get("Application_VersionTip3"), icon: MessageBoxImage.Error);

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

	#region Structs
	class UpdateInfoArgs : UpdateInfoEventArgs
	{
		public int NoticeID { get; set; }
		public string? Notice { get; set; }
		public string? Signature { get; set; }
	}

	internal enum UpdateMode
	{
		Stabble,
		Beta,
	}
	#endregion


	#region IService
	public bool Register()
	{
		AutoUpdater.Start($"https://tools.bnszs.com/api/update?app={APP_NAME}&version={VersionHelper.InternalVersion}&mode={UserSettings.Default.UpdateMode}");
		return true;
	}
	#endregion
}