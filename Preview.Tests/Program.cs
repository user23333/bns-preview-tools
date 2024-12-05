using Xylia.Preview.Tests.DatTests.Tools;

namespace Xylia.Preview.Tests;
internal class Program
{
	[STAThread]
	static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new MainForm());
	}
}