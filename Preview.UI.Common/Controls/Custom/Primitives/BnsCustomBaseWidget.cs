using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.Core.i18N;
using SkiaSharp.Views.WPF;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Converters;
using Xylia.Preview.UI.Documents;
using Xylia.Preview.UI.Documents.Primitives;
using Xylia.Preview.UI.Extensions;

namespace Xylia.Preview.UI.Controls.Primitives;
public abstract class BnsCustomBaseWidget : UserWidget, IMetaData
{
	#region Constructors
	internal BnsCustomBaseWidget()
	{
		Children = new UIElementCollectionEx(this);

		// Create TextContainer associated with it
		_container = new TextContainer(this);
		_container.ChangedHandler += (s, e) => OnContainerChanged(e);

		SetCurrentValue(StringProperty, new StringProperty());
		SetCurrentValue(ExpansionComponentListProperty, new ExpansionCollection());
	}
	#endregion

	#region DependencyProperty 
	private static readonly Type Owner = typeof(BnsCustomBaseWidget);
	public static readonly DependencyProperty BaseImagePropertyProperty = Owner.Register<ImageProperty>(nameof(BaseImageProperty), null);
	public static readonly DependencyProperty StringProperty = Owner.Register<StringProperty>(nameof(String), null, callback: OnStringChanged);
	public static readonly DependencyProperty MetaDataProperty = Owner.Register(nameof(MetaData), string.Empty, callback: IMetaData.UpdateData);
	public static readonly DependencyProperty ExpansionComponentListProperty = Owner.Register<ExpansionCollection>(nameof(ExpansionComponentList));

	public static readonly DependencyProperty HorizontalResizeLinkProperty = Owner.Register<ResizeLink>(nameof(HorizontalResizeLink), default, FrameworkPropertyMetadataOptions.AffectsParentArrange);
	public static readonly DependencyProperty VerticalResizeLinkProperty = Owner.Register<ResizeLink>(nameof(VerticalResizeLink), default, FrameworkPropertyMetadataOptions.AffectsParentArrange);

	public ImageProperty BaseImageProperty
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

	public ResizeLink HorizontalResizeLink
	{
		get { return (ResizeLink)GetValue(HorizontalResizeLinkProperty); }
		set { SetValue(HorizontalResizeLinkProperty, value); }
	}

	public ResizeLink VerticalResizeLink
	{
		get { return (ResizeLink)GetValue(VerticalResizeLinkProperty); }
		set { SetValue(VerticalResizeLinkProperty, value); }
	}

	public bool AutoResizeHorizontal { get; set; }
	public float MaxAutoResizeHorizontal { get; set; }
	public float MinAutoResizeHorizontal { get; set; }

	public bool AutoResizeVertical { get; set; }
	public float MaxAutoResizeVertical { get; set; }
	public float MinAutoResizeVertical { get; set; }
	#endregion

	#region StringProperty
	internal readonly TextContainer _container;

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

	internal void OnContainerChanged(EventArgs e)
	{
		InvalidateMeasure();
		InvalidateArrange();
		InvalidateVisual();
	}

	//  IMetaData
	public void UpdateString(string? text)
	{
		this.String ??= new StringProperty();
		this.String.LabelText = new FText(text);
		this.OnStringChanged(String);
	}

	void IMetaData.UpdateTooltip(string? text)
	{
		this.ToolTip = text;
	}
	#endregion


	#region Protected Methods
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
		DrawString(dc, String, MetaData);

		#region ExpansionComponent
		if (!ExpansionComponentList.IsEmpty())
		{
			// render
			foreach (var e in ExpansionComponentList)
			{
				if (!e.bShow) continue;

				if (e.ExpansionType == ExpansionComponent.Type_IMAGE)
				{
					DrawImage(dc, e.ImageProperty);
				}
				else if (e.ExpansionType == ExpansionComponent.Type_STRING)
				{
					DrawString(dc, e.StringProperty, e.MetaData);
				}
				else Debug.Assert(string.IsNullOrEmpty(e.ExpansionType.PlainText));
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

	protected Size DrawString(DrawingContext? ctx, StringProperty p, string MetaData)
	{
		if (p is null) return default;

		var document = _container.Document = new Paragraph() { FontSet = p.fontset, HorizontalAlignment = (HorizontalAlignment)p.HorizontalAlignment };
		IMetaData.UpdateData(document, new(StringProperty, p, MetaData));
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

	private void OnResizeLink(ResizeLink resizeLink, Size constraint, ref Rect rc, bool horizontal)
	{
		if (resizeLink is null || !resizeLink.bEnable) return;

		#region Rect 
		Rect rect = new Rect(constraint);
		float offset = resizeLink.Offset1;

		var l = this.GetChild<UserWidget>(resizeLink.LinkWidgetName1, true);
		if (l != null)
		{
			rect = l.GetFinalRect();
			offset = l.Visibility == Visibility.Visible ? resizeLink.Offset1 : 0f;
		}

		var size = horizontal ? rc.Width : rc.Height;
		var left = horizontal ? rect.Left : rect.Top;
		var right = horizontal ? rect.Right : rect.Bottom;
		#endregion

		#region Final
		double final;
		switch (resizeLink.Type)
		{
			case EBnsCustomResizeLinkType.BNS_CUSTOM_BORDER_LINK_LEFT:
				final = left + offset;
				break;

			case EBnsCustomResizeLinkType.BNS_CUSTOM_BORDER_LINK_CENTER:
				final = (right - left - offset - size) / 2;
				break;

			case EBnsCustomResizeLinkType.BNS_CUSTOM_BORDER_LINK_RIGHT:
				final = right - offset - size;
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