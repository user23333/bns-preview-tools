using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using CUE4Parse.BNS.Assets.Exports;
using SkiaSharp.Views.WPF;
using Xylia.Preview.Common;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Converters;
using Xylia.Preview.UI.Documents;
using Xylia.Preview.UI.Documents.Primitives;
using Xylia.Preview.UI.Extensions;

namespace Xylia.Preview.UI.Controls.Primitives;
public abstract class BnsCustomBaseWidget : UserWidget
{
	#region Constructors
	internal BnsCustomBaseWidget()
	{
		Children = new WidgetCollection(this);

		// Create TextContainer associated with it
		_container = new TextContainer(this);
		_container.ChangedHandler += (s, e) => OnContainerChanged(e);

		SetCurrentValue(ExpansionComponentListProperty, new ExpansionCollection());
		SetCurrentValue(StringProperty, new StringProperty());
		SetCurrentValue(TimersProperty, new Dictionary<int, Time64>());
	}
	#endregion

	#region DependencyProperty 
	private static readonly Type Owner = typeof(BnsCustomBaseWidget);
	public static readonly DependencyProperty BaseImagePropertyProperty = Owner.Register<ImageProperty>(nameof(BaseImageProperty));
	public static readonly DependencyProperty StringProperty = Owner.Register<StringProperty>(nameof(String), null, callback: OnStringChanged);
	public static readonly DependencyProperty MetaDataProperty = Owner.Register(nameof(MetaData), string.Empty, callback: OnMetaChanged);
	public static readonly DependencyProperty ExpansionComponentListProperty = Owner.Register<ExpansionCollection>(nameof(ExpansionComponentList));
	public static readonly DependencyProperty TimersProperty = Owner.Register<IDictionary<int, Time64>>("Timers", null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits);

	public static readonly DependencyProperty AutoResizeHorizontalProperty = Owner.Register<bool>(nameof(AutoResizeHorizontal), default, FrameworkPropertyMetadataOptions.AffectsParentArrange);
	public static readonly DependencyProperty AutoResizeVerticalProperty = Owner.Register<bool>(nameof(AutoResizeVertical), default, FrameworkPropertyMetadataOptions.AffectsParentArrange);
	public static readonly DependencyProperty HorizontalResizeLinkProperty = Owner.Register<BnsCustomResizeLink>(nameof(HorizontalResizeLink), default, FrameworkPropertyMetadataOptions.AffectsParentArrange);
	public static readonly DependencyProperty VerticalResizeLinkProperty = Owner.Register<BnsCustomResizeLink>(nameof(VerticalResizeLink), default, FrameworkPropertyMetadataOptions.AffectsParentArrange);


	public ImageProperty? BaseImageProperty
	{
		get { return (ImageProperty)GetValue(BaseImagePropertyProperty); }
		set { SetValue(BaseImagePropertyProperty, value); }
	}

	public StringProperty String
	{
		get { return (StringProperty)GetValue(StringProperty); }
		set { SetValue(StringProperty, value); }
	}

	public string MetaData
	{
		get { return (string)GetValue(MetaDataProperty); }
		set { SetValue(MetaDataProperty, value); }
	}

	public ExpansionCollection ExpansionComponentList
	{
		get { return (ExpansionCollection)GetValue(ExpansionComponentListProperty); }
		set { SetValue(ExpansionComponentListProperty, value); }
	}

	internal IDictionary<int, Time64> Timers
	{
		get { return (IDictionary<int, Time64>)GetValue(TimersProperty); }
		set { SetValue(TimersProperty, value); }
	}


	public BnsCustomResizeLink HorizontalResizeLink
	{
		get { return (BnsCustomResizeLink)GetValue(HorizontalResizeLinkProperty); }
		set { SetValue(HorizontalResizeLinkProperty, value); }
	}

	public BnsCustomResizeLink VerticalResizeLink
	{
		get { return (BnsCustomResizeLink)GetValue(VerticalResizeLinkProperty); }
		set { SetValue(VerticalResizeLinkProperty, value); }
	}

	public bool AutoResizeHorizontal
	{
		get => (bool)GetValue(AutoResizeHorizontalProperty);
		set => SetValue(AutoResizeHorizontalProperty, value);
	}
	public float MinAutoResizeHorizontal { get; set; } = 0;
	public float MaxAutoResizeHorizontal { get; set; } = float.PositiveInfinity;

	public bool AutoResizeVertical
	{
		get => (bool)GetValue(AutoResizeVerticalProperty);
		set => SetValue(AutoResizeVerticalProperty, value);
	}
	public float MinAutoResizeVertical { get; set; } = 0;
	public float MaxAutoResizeVertical { get; set; } = float.PositiveInfinity;
	#endregion

	#region StringProperty
	internal readonly TextContainer _container;

	internal void OnContainerChanged(EventArgs e)
	{
		InvalidateMeasure();
		InvalidateArrange();
		InvalidateVisual();
	}

	public static async void OnMetaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var widget = (BnsCustomBaseWidget)d;
		if (e.NewValue is not string s || string.IsNullOrEmpty(s)) return;

		// return if in design
		if (DesignerProperties.GetIsInDesignMode(d)) return;

		// HACK: wait to load text data 
		var provider = Globals.GameData.Provider;
		await Task.Run(() => provider.GetTable<Text>());

		foreach (var meta in s.Split(';'))
		{
			var ls = meta.Split('=', 2);
			if (ls.Length < 2) continue;

			switch (ls[0])
			{
				case "textref": widget.UpdateString(ls[1].GetText()); return;
				case "tooltip": widget.UpdateTooltip(ls[1].GetText()); return;
				case "config":
				case "width":
				case "height": return;

				default: Debug.Print("meta is not supported!"); break;
			}
		}
	}

	private static void OnStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var widget = (BnsCustomBaseWidget)d;
		var value = (StringProperty)e.NewValue;

		widget.OnStringChanged(value);
		if (value != null) value.PropertyChanged += (_, _) => widget.OnStringChanged(value);
	}

	protected void OnStringChanged(StringProperty p)
	{
		// TODO: raise event
		OnContainerChanged(EventArgs.Empty);
	}

	protected void UpdateString(string text)
	{
		this.String.LabelText = text;
		this.OnStringChanged(String);
	}

	protected void UpdateTooltip(string text)
	{
		this.ToolTip = text;
	}
	#endregion

	#region Timer
	public void SetTimer(byte index, Time64 time)
	{
		// HACK: redraw
		if (Timers.Count == 0)
		{
			var timer = new DispatcherTimer();
			timer.Tick += ((s, e) => this?.InvalidateVisual());
			timer.Interval = new TimeSpan(0, 0, 0, 1);
			timer.IsEnabled = true;
			timer.Start();
		}

		Timers[index] = time;
	}
	#endregion

	#region Protected Methods
	protected override Size MeasureOverride(Size constraint)
	{
		if (AutoResizeHorizontal) constraint.Width = double.PositiveInfinity;
		if (AutoResizeVertical) constraint.Height = double.PositiveInfinity;

		var size = base.MeasureOverride(constraint);
		return new Size(
			Math.Min(MaxAutoResizeHorizontal, Math.Max(MinAutoResizeHorizontal, size.Width)),
			Math.Min(MaxAutoResizeVertical, Math.Max(MinAutoResizeVertical, size.Height)));
	}

	protected override Rect ArrangeChild(UIElement child, Size constraint)
	{
		var rcChild = base.ArrangeChild(child, constraint);

		if (child is BnsCustomBaseWidget widget)
		{
			OnResizeLink(widget.HorizontalResizeLink, constraint, ref rcChild, true);
			OnResizeLink(widget.VerticalResizeLink, constraint, ref rcChild, false);
		}

		// support for scroll
		if (child is not BnsCustomScrollBarWidget)
		{
			rcChild.X -= ScrollOffset.X;
			rcChild.Y -= ScrollOffset.Y;
		}

		return rcChild;
	}

	protected override void OnRender(DrawingContext dc)
	{
		// Using the Background brush, draw a rectangle that fills the render bounds of the widget.
		var background = Background;
		if (background != null) dc.DrawRectangle(background, null, new Rect(RenderSize));

		// Draw BaseImage & String 
		DrawImage(dc, BaseImageProperty);
		DrawString(dc, String);

		#region ExpansionComponent
		if (!ExpansionComponentList.IsEmpty())
		{
			// render
			foreach (var e in ExpansionComponentList)
			{
				if (!e.bShow) continue;

				switch (e.ExpansionType)
				{
					case EBNSCustomExpansionComponentType.IMAGE:
						DrawImage(dc, e.ImageProperty);
						break;

					case EBNSCustomExpansionComponentType.STRING:
						DrawString(dc, e.StringProperty /*, e.MetaData*/);
						break;
				}
			}
		}
		#endregion
	}


	protected void DrawImage(DrawingContext ctx, ImageProperty p)
	{
		if (p is null) return;

		// layout
		var size = p.Measure(RenderSize.Parse(), out var source);
		if (source != null)
		{
			var pos = LayoutData.ComputeOffset(RenderSize, size, p.HorizontalAlignment, p.VerticalAlignment, p.StaticPadding, p.Offset);
			ctx?.DrawImage(source.ToWriteableBitmap(), new Rect(pos, new Size(size.X, size.Y)));
		}
	}

	protected Size DrawString(DrawingContext? ctx, StringProperty p, TextContainer? container = null)
	{
		if (p is null) return default;

		// data
		var document = container != null ? _container.Document : new P();
		document.UpdateString(p);
		BaseElement.InheritDependency(this, document);
			  
		// layout
		var size = document.Measure(RenderSize);
		var pos = LayoutData.ComputeOffset(RenderSize, size.Parse(), p.HorizontalAlignment, p.VerticalAlignment, p.Padding, p.ClippingBound);

		if (ctx != null)
		{
			document.Arrange(new Rect(pos, size));
			document.OnRender(ctx);
		}

		return size;
	}

	private void OnResizeLink(BnsCustomResizeLink link, Size constraint, ref Rect rc, bool horizontal)
	{
		if (link is null || !link.bEnable) return;

		#region Rect 
		Rect rect = new(constraint);
		float offset = link.Offset1;

		var l = this.GetChild<UserWidget>(link.LinkWidgetName1, true);
		if (l != null)
		{
			rect = l.GetFinalRect();
			offset = l.Visibility == Visibility.Visible ? link.Offset1 : 0f;
		}

		var size = horizontal ? rc.Width : rc.Height;
		var left = horizontal ? rect.Left : rect.Top;
		var right = horizontal ? rect.Right : rect.Bottom;
		#endregion

		#region Final
		double final;
		switch (link.Type)
		{
			case EBnsCustomResizeLinkType.BNS_CUSTOM_BORDER_LINK_LEFT:
				final = left + offset;
				break;

			case EBnsCustomResizeLinkType.BNS_CUSTOM_BORDER_LINK_RIGHT:
				final = right - offset - size;
				break;

			case EBnsCustomResizeLinkType.BNS_CUSTOM_BORDER_LINK_CENTER:
				final = (right - left - offset - size) / 2;
				break;

			case EBnsCustomResizeLinkType.BNS_CUSTOM_WIDGET_LINK_LEFT:
				final = left - offset - size;
				break;

			case EBnsCustomResizeLinkType.BNS_CUSTOM_WIDGET_LINK_RIGHT:
				final = right + offset;
				break;

			default: return;
		}

		if (horizontal) rc.X = final;
		else rc.Y = final;
		#endregion
	}
	#endregion
}