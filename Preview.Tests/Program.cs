﻿using Xylia.Preview.Tests.DatTests.Tools;
using Xylia.Preview.Tests.PakTests;

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