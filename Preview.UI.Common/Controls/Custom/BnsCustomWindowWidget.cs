using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
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
	#endregion

	#region Public Methods
	/// <summary>
	/// Opens a <see langword="PresentationFramework."/><see cref="Window"/> to display the widget
	/// </summary>
	public void Show(bool ShowBorder = true)
	{
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
	#endregion
}