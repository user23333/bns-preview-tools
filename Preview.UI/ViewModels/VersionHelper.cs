using System.Reflection;

namespace Xylia.Preview.UI.ViewModels;
internal static class VersionHelper
{
	public static Version InternalVersion => Assembly.GetEntryAssembly()!.GetName().Version!;

	public static string Version => InternalVersion.ToString(3);

	public static DateTime Time => new DateTime(2000, 1, 1).AddDays(InternalVersion.Revision);
}