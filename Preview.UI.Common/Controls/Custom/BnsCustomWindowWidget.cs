using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Xylia.Preview.UI.Controls.Primitives;

namespace Xylia.Preview.UI.Controls;
public class BnsCustomWindowWidget : BnsCustomBaseWidget
{
	#region Constructors
	public BnsCustomWindowWidget()
	{
		this.Loaded += (s, e) => OnLoaded(e);
	}
	#endregion

	#region Properties
	public event EventHandler? Closed;

	/// <summary>
	/// Implement automatic layout in vertical tree 
	/// </summary>
	protected List<UIElement> VerticalTree { get; set; } = [];
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

	protected override Size MeasureOverride(Size constraint)
	{
		var size = base.MeasureOverride(constraint);

		if (VerticalTree.Count > 0)
		{
			size.Height = VerticalTree.Sum(child => child.DesiredSize.Height);
		}

		return size;
	}

	protected override Size ArrangeOverride(Size constraint)
	{
		Vector offset;

		foreach (UIElement child in VerticalTree)
		{
			var rect = ArrangeChild(child, constraint);
			rect.Y = offset.Y;
			offset.Y += child.DesiredSize.Height;
			child.Arrange(rect);
		}

		foreach (UIElement child in Children)
		{
			if (child == null || VerticalTree.Contains(child)) continue;

			child.Arrange(ArrangeChild(child, constraint));
		}

		return constraint;
	}


	protected virtual void OnLoaded(RoutedEventArgs e)
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
	public void Show()
	{
		Host = new Window
		{
			Content = this,
			Background = new SolidColorBrush(BackgroundColor),
			Foreground = new SolidColorBrush(Colors.White),
			ResizeMode = ResizeMode.NoResize,
			SizeToContent = SizeToContent.WidthAndHeight,
			Title = this.Name,
			WindowStyle = WindowStyle.None,
		};
#if DEBUG
		Host.WindowStyle = WindowStyle.SingleBorderWindow;
#endif
		Host.Closing += (s, e) => OnClosing(e);
		Host.Show();
	}
	#endregion


	#region Private Helpers
	private void OnCloseClick()
	{
		Host!.Close();
		Closed?.Invoke(this, new EventArgs());
	}
	#endregion

	#region Fields
	public readonly static Color BackgroundColor = Color.FromArgb(255, 30, 79, 122);

	private Window? Host;
	#endregion
}