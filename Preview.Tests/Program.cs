using Xylia.Preview.Tests.DatTests.Tools;

namespace Xylia.Preview.Tests;
internal class Program
{
	[STAThread]
	static void Main()
	{
#if DEVELOP
		new System.Windows.Application().Run(new TestPanel());
		return;
#endif
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new MainForm());
	}
}