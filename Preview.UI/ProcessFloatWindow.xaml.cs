using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using Vanara.PInvoke;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.UI;
public partial class ProcessFloatWindow
{
	#region Constructor
	private ProcessFloatWindow()
	{
		InitializeComponent();

		var interval = 1000;
		var prevTime = Process.GetCurrentProcess().TotalProcessorTime;
		timer = new DispatcherTimer(new TimeSpan(TimeSpan.TicksPerMillisecond * interval), DispatcherPriority.Normal, (_, _) =>
		{
			var process = Process.GetCurrentProcess();
			var size = process.PrivateMemorySize64;
			var value = (process.TotalProcessorTime - prevTime).TotalMilliseconds / interval / Environment.ProcessorCount;
			prevTime = process.TotalProcessorTime;

			UsedCPU.Text = value.ToString("P0");
			UsedMemory.Text = BinaryExtension.GetReadableSize(size);
		}, Dispatcher);
	}
	#endregion

	#region Properies
	static ProcessFloatWindow? _instance;
	public static ProcessFloatWindow Instance
	{
		get
		{
			if (_instance is null ||
				(PresentationSource.FromVisual(_instance)?.IsDisposed ?? true))
				_instance = new();

			return _instance;
		}
	}

	readonly DispatcherTimer timer;
	#endregion


	#region Methods
	private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		this.DragMove();
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		// hide in task manager
		User32.SetWindowLong(new WindowInteropHelper(this).Handle, User32.WindowLongFlags.GWL_EXSTYLE, (int)User32.WindowStylesEx.WS_EX_TOOLWINDOW);
		timer.Start();
	}

	private void Window_Closed(object sender, EventArgs e)
	{
		timer?.Stop();
	}
	#endregion

	#region Helpers
	public static void ClearMemory()
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();

		if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			Kernel32.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, SizeT.MinValue, SizeT.MinValue);
	}
	#endregion
}