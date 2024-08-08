using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Vanara.PInvoke;
using Xylia.Preview.UI.Controls.Primitives;

namespace Xylia.Preview.UI.Controls;
public class BnsCustomWindowWidget : BnsCustomBaseWidget
{
	#region Constructors
	public BnsCustomWindowWidget()
	{
		//HeightProperty.OverrideMetadata(typeof(BnsCustomWindowWidget), new FrameworkPropertyMetadata(new PropertyChangedCallback(_OnHeightChanged)));
		//MinHeightProperty.OverrideMetadata(typeof(BnsCustomWindowWidget), new FrameworkPropertyMetadata(new PropertyChangedCallback(_OnMinHeightChanged)));
		//MaxHeightProperty.OverrideMetadata(typeof(BnsCustomWindowWidget), new FrameworkPropertyMetadata(new PropertyChangedCallback(_OnMaxHeightChanged)));
		//WidthProperty.OverrideMetadata(typeof(BnsCustomWindowWidget), new FrameworkPropertyMetadata(new PropertyChangedCallback(_OnWidthChanged)));
		//MinWidthProperty.OverrideMetadata(typeof(BnsCustomWindowWidget), new FrameworkPropertyMetadata(new PropertyChangedCallback(_OnMinWidthChanged)));
		//MaxWidthProperty.OverrideMetadata(typeof(BnsCustomWindowWidget), new FrameworkPropertyMetadata(new PropertyChangedCallback(_OnMaxWidthChanged)));

		DataContextChanged += (s, e) => OnDataChanged(e);

		SetValue(TitleProperty, GetType().Name);
	}
	#endregion

	#region Properties
	public event EventHandler? Closed;

	/// <summary>
	///     The DependencyProperty for TitleProperty.
	/// </summary>
	public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
		typeof(string), typeof(BnsCustomWindowWidget), new FrameworkPropertyMetadata(string.Empty));

	/// <summary>
	///     The data that will be displayed as the title of the window.
	///     Hosts are free to display the title in any manner that they
	///     want.  For example, the browser may display the title set via
	///     the Title property somewhere besides the caption bar
	/// </summary>
	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}
	#endregion


	#region Override Methods
	protected override void OnInitialized(EventArgs e)
	{
		AutoResizeVertical = true;

		base.OnInitialized(e);

		// Maybe miss name if this is root element
		if (string.IsNullOrEmpty(Name)) Name = this.GetType().Name;

		// Ensure background don't conver else
		var Background = this.GetChild<BnsCustomImageWidget>("Background");
		if (Background != null) SetZOrder(Background, -1);

		var CloseButton = this.GetChild<BnsCustomLabelButtonWidget>("Close");
		if (CloseButton != null) CloseButton.Click += (_, _) => OnCloseClick();
	}

	protected override void OnRender(DrawingContext dc)
	{
		// HACK: There is an issue with the background image of this widget
		// don't render it at now

		var background = Background;
		if (background != null) dc.DrawRectangle(background, null, new Rect(RenderSize));
	}

	protected virtual void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{

	}

	protected virtual void OnClosing(CancelEventArgs e)
	{
		Host = null;
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		base.OnKeyUp(e);
		if (e.Key == Key.Escape) OnCloseClick();
	}

	protected virtual IntPtr WndProc(HWND hwnd, uint msg, IntPtr wParam, IntPtr lParam)
	{
		switch ((User32.WindowMessage)msg)
		{
			case User32.WindowMessage.WM_DESTROY:
				User32.PostQuitMessage(0);
				break;
			case User32.WindowMessage.WM_CREATE:
			{
				//创建控件或者子窗口

				HWND hBtn = User32.CreateWindow("button", "测试", User32.WindowStyles.WS_CHILD | User32.WindowStyles.WS_VISIBLE | (int)User32.ButtonStyle.BS_PUSHBUTTON, 30, 30, 80, 30, hwnd,
					(HMENU)1000,//控件ID
					lParam, IntPtr.Zero);

				User32.CreateWindow(GetType().Name, "123", 0, 0, 0, 80, 30, hwnd, (HMENU)1001, lParam, IntPtr.Zero);
				break;
			}
			default: return User32.DefWindowProc(hwnd, msg, wParam, lParam);

		}

		return IntPtr.Zero;
	}
	#endregion

	#region Public Methods
	/// <summary>
	/// Opens a <see langword="PresentationFramework."/><see cref="Window"/> to display the widget
	/// </summary>
	public void Show(bool ShowBorder = true)
	{
#if DEBUG && false
		ShowHelper(ShowWindowCommand.SW_NORMAL);
#else
		Host = new HostWindow
		{
			Content = this,
			ResizeMode = ResizeMode.NoResize,
			SizeToContent = SizeToContent.WidthAndHeight,
			Title = this.Name,
			WindowStyle = ShowBorder ? WindowStyle.SingleBorderWindow : WindowStyle.None,
		};
		Host.Closing += (s, e) => OnClosing(e);
		Host.Show();
#endif
	}
	#endregion


	#region Private Helpers
	private Window? Host;
	public readonly static Color BackgroundColor = Color.FromArgb(255, 30, 79, 122);

	private class HostWindow : Window
	{
		protected override Size MeasureOverride(Size availableSize)
		{      
			//boarder size
			User32.GetWindowRect(new WindowInteropHelper(this).Handle, out var windowRect);
			User32.GetClientRect(new WindowInteropHelper(this).Handle, out var clientRect);

			if (Content is UIElement child)
			{
				child.Measure(availableSize);
				return new Size(
					child.DesiredSize.Width + windowRect.Width - clientRect.Width, 	  
					child.DesiredSize.Height + windowRect.Height - clientRect.Height);
			}

			return Size.Empty;
		}
	}

	private void OnCloseClick()
	{
		Host?.Close();
		Closed?.Invoke(this, new EventArgs());
	}


	public bool ShowHelper(ShowWindowCommand showWindowCommand)
	{
		Vanara.PInvoke.MSG msg;

		if (!InitApplication())
			return false;

		if (!InitInstance(showWindowCommand))
			return false;

		while (User32.GetMessage(out msg, IntPtr.Zero, 0, 0) != 0)
		{
			User32.TranslateMessage(msg);
			User32.DispatchMessage(msg);
		}

		return msg.wParam == IntPtr.Zero;
	}

	private bool InitApplication()
	{
		var wcx = new User32.WNDCLASSEX();
		wcx.cbSize = (uint)Marshal.SizeOf(wcx);
		wcx.style = User32.WindowClassStyles.CS_VREDRAW | User32.WindowClassStyles.CS_HREDRAW;
		wcx.lpfnWndProc = WndProc;
		wcx.cbClsExtra = 0;
		wcx.cbWndExtra = 0;
		wcx.hInstance = IntPtr.Zero;
		wcx.hIcon = User32.LoadIcon(IntPtr.Zero, new IntPtr((int)2));
		wcx.hCursor = User32.LoadCursor(IntPtr.Zero, User32.IDC_ARROW);
		wcx.hbrBackground = Gdi32.GetStockObject(Gdi32.StockObjectType.WHITE_BRUSH);
		wcx.lpszMenuName = "MainMenu";
		wcx.lpszClassName = GetType().Name;

		var ret = User32.RegisterClassEx(wcx);
		if (ret.IsInvalid)
		{
			string message = new Win32Exception(Marshal.GetLastWin32Error()).Message;
			Console.WriteLine("调用 RegisterClasEx 失败, error = {0}", message);
			return false;
		}

		return true;
	}

	private bool InitInstance(ShowWindowCommand showWindowCommand)
	{
		var hwnd = User32.CreateWindowEx(
			 User32.WindowStylesEx.WS_EX_APPWINDOW,
			 this.GetType().Name,
			 this.Title,
			 User32.WindowStyles.WS_OVERLAPPEDWINDOW,   // top-level window
			 User32.CW_USEDEFAULT,    // 0 horizontal position 
			 User32.CW_USEDEFAULT,    // 0 vertical position 
			 400,                                // 400 宽 
			 300,                                // 300 高 
			 IntPtr.Zero,                        // no owner window 
			 IntPtr.Zero,                        // use class menu 
			 IntPtr.Zero,                        // handle to application instance 
			 IntPtr.Zero);                       // no window-creation data 

		if (hwnd == IntPtr.Zero)
		{
			string error = new Win32Exception(Marshal.GetLastWin32Error()).Message;
			Console.WriteLine("初始化实例 失败, error = {0}", error);
			return false;
		}
		User32.ShowWindow(hwnd, showWindowCommand);
		User32.UpdateWindow(hwnd);
		return true;
	}
	#endregion
}