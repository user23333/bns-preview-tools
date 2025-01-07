using System.Windows;
using System.Windows.Media;
using Xylia.Preview.UI.Controls.Primitives;
using Xylia.Preview.UI.Extensions;

namespace Xylia.Preview.UI.Controls;
public class BnsCustomProgressBarWidget : BnsCustomBaseWidget
{
	#region Public Properties
	private static readonly Type Owner = typeof(BnsCustomProgressBarWidget);

	public static readonly DependencyProperty MaxProgressValueProperty = Owner.Register(nameof(MaxProgressValue), 100);
	public int MaxProgressValue
	{
		get => (int)this.GetValue(MaxProgressValueProperty);
		set => this.SetValue(MaxProgressValueProperty, value);
	}

	public static readonly DependencyProperty InitProgressValueProperty = Owner.Register(nameof(InitProgressValue), 0, callback: OnInitProgressValueChanged);
	public int InitProgressValue
	{
		get => (int)this.GetValue(InitProgressValueProperty);
		set => this.SetValue(InitProgressValueProperty, value);
	}

	public static readonly DependencyProperty ProgressValueProperty = Owner.Register(nameof(ProgressValue), -1, callback: OnProgressValueChanged);
	public int ProgressValue
	{
		get => (int)this.GetValue(ProgressValueProperty);
		set => this.SetValue(ProgressValueProperty, value);
	}
	#endregion

	#region Methods
	private static void OnInitProgressValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var widget = (BnsCustomProgressBarWidget)d;
		if (widget.ProgressValue == -1)
			widget.ProgressValue = (int)e.NewValue;
	}

	private static void OnProgressValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var widget = (BnsCustomProgressBarWidget)d;
		var value = (int)e.NewValue;
		widget.OnProgressValueChanged(value);
	}

	protected void OnProgressValueChanged(int value)
	{
		if (Value != null)
		{
			Value.String.LabelText = $"{value} / {MaxProgressValue}";
		}
	}
	#endregion

	#region Override Methods
	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);

		Value = this.GetChild<BnsCustomLabelWidget>("Value")!;
	}

	protected override void OnRender(DrawingContext dc)
	{
		base.OnRender(dc);

		// TODO: support ProgressImageProperty
		var percent = ProgressValue / MaxProgressValue;
		dc.DrawRectangle(new SolidColorBrush(Colors.SkyBlue), null,
			new Rect(0, 0, RenderSize.Width * percent, RenderSize.Height));
	}
	#endregion

	#region Fields
	private BnsCustomLabelWidget? Value;
	#endregion
}