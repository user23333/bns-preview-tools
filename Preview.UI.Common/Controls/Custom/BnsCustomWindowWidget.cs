using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Vanara.PInvoke;
using Xylia.Preview.UI.Controls.Primitives;
using Xylia.Preview.UI.Extensions;

namespace Xylia.Preview.UI.Controls;
public class BnsCustomWindowWidget : BnsCustomBaseWidget
{
	#region Constructors
	static BnsCustomWindowWidget()
	{
		DefaultStyleKeyProperty.OverrideMetadata(Owner, new FrameworkPropertyMetadata(Owner));
		DataContextProperty.OverrideMetadata(Owner, new FrameworkPropertyMetadata(OnDataChanged));
	}

	public BnsCustomWindowWidget()
	{
		AutoResizeVertical = true;
		SetValue(TitleProperty, GetType().Name);
	}
	#endregion

	#region Events
	public event EventHandler? Closed;
	#endregion

	#region Properties
	private static readonly Type Owner = typeof(BnsCustomWindowWidget);

	/// <summary>
	///     The DependencyProperty for TitleProperty.
	/// </summary>
	public static readonly DependencyProperty TitleProperty = Owner.Register(nameof(Title), string.Empty);

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

	protected bool WindowDisplayAffinity { get; set; }
	#endregion


	#region Override Methods
	protected override void OnInitialized(EventArgs e)
	{
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

	protected virtual void OnClosing(CancelEventArgs e)
	{
		Host = null;
	}

	protected override void OnKeyUp(KeyEventArgs e)
	{
		base.OnKeyUp(e);
		if (e.Key == Key.Escape) OnCloseClick();
	}

	private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var widget = (BnsCustomWindowWidget)d;
		widget.OnDataChanged(e);
	}

	protected virtual void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{

	}
	#endregion

	#region Public Methods
	/// <summary>
	/// Opens a <see langword="PresentationFramework."/><see cref="Window"/> to display the widget.
	/// </summary>
	public void Show(Window? owner = null)
	{
		Host ??= new HostWindow(this);
		Host.SetOwner(owner);
		Host.Show();
	}
	#endregion


	#region Private Helpers
	private HostWindow? Host;
	public static implicit operator Window(BnsCustomWindowWidget w) => w.Host ??= new HostWindow(w);

	private class HostWindow : Window
	{			 
		private readonly BnsCustomWindowWidget _content;

		public HostWindow(BnsCustomWindowWidget content, bool capture = true)
		{
			Content = _content = content;
			Title = content.Name;
			ResizeMode = ResizeMode.NoResize;
			SizeToContent = SizeToContent.WidthAndHeight;
			WindowStyle = WindowStyle.SingleBorderWindow;

			// event
			Closing += (s, e) => content.OnClosing(e);
			content.SizeChanged += (s, e) => this.InvalidateMeasure();
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			if (Content is not UIElement child) return Size.Empty;

			// boarder size
			User32.GetWindowRect(new WindowInteropHelper(this).Handle, out var windowRect);
			User32.GetClientRect(new WindowInteropHelper(this).Handle, out var clientRect);

			// content size
			child.Measure(availableSize);
			return new Size(
				child.DesiredSize.Width + windowRect.Width - clientRect.Width,
				child.DesiredSize.Height + windowRect.Height - clientRect.Height);
		}

		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			User32.SetWindowDisplayAffinity(new WindowInteropHelper(this).Handle,
				_content.WindowDisplayAffinity ? User32.WindowDisplayAffinity.WDA_MONITOR : User32.WindowDisplayAffinity.WDA_NONE);
		}

		public void SetOwner(Window? owner)
		{
			if (owner is null) return;

			Owner = owner;
			WindowStartupLocation = WindowStartupLocation.CenterOwner;
		}
	}

	private void OnCloseClick()
	{
		Host?.Close();
		Closed?.Invoke(this, new EventArgs());
	}
	#endregion
}