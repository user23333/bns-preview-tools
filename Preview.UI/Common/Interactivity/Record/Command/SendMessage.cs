using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Vanara.PInvoke;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.ViewModels;
using static Xylia.Preview.Data.Models.ChatChannelOption;

namespace Xylia.Preview.UI.Common.Interactivity;
internal class SendMessage : RecordCommand
{
	protected override List<string> Type => ["item"];

	protected override bool CanExecute(Record record)
	{
		// check plugin status
		var root = new DirectoryInfo(UserSettings.Default.GameFolder);
		var plugin = root?.GetFiles("libiconv2017_cl64.dll", SearchOption.AllDirectories).FirstOrDefault();
		if (plugin is null || FileVersionInfo.GetVersionInfo(plugin.FullName).InternalName != "bnszs") return false;

		// check admin level
		var identity = WindowsIdentity.GetCurrent();
		var principal = new WindowsPrincipal(identity);
		return principal.IsInRole(WindowsBuiltInRole.Administrator);
	}

	protected override void Execute(Record record)
	{
		switch (record.To<ModelElement>())
		{
			case Item item: COPYDATASTRUCTSTR.Send(item.ItemName, CategorySeq.ChatGodsayNormal); break;
			default: throw new NotSupportedException();
		}
	}
}

public struct COPYDATASTRUCTSTR
{
	public long dwData;
	public int cbData;
	[MarshalAs(UnmanagedType.LPWStr)]
	public string lpData;

	public static void Send(nint hwnd, string text, CategorySeq category)
	{
		Debug.WriteLine($"send: {text}");

		var cds = new COPYDATASTRUCTSTR()
		{
			cbData = text.Length * 2 + 1,
			lpData = text,
			dwData = (int)category
		};
		User32.SendMessage(hwnd, User32.WindowMessage.WM_COPYDATA, 0x100, ref cds);
	}

	public static void Send(string text, CategorySeq category = CategorySeq.Default)
	{
		var hwnd = (nint)User32.FindWindow("UnrealWindow", null);
		if (hwnd == IntPtr.Zero) return;

		Send(hwnd, text, category);
	}
}