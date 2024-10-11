using System.Windows;
using System.Windows.Media;
using CUE4Parse.BNS.Assets.Exports;
using SkiaSharp.Views.WPF;

namespace Xylia.Preview.UI.Documents;
public class Image : BaseElement<Data.Models.Document.Image>
{
	#region Constructors
	public Image()
	{

	}

	internal Image(ImageProperty property)
	{
		Element = new() 
		{ 
			Bitmap = property.Image,
			Enablescale = true , 
			Scalerate = property.ImageScale
		};
	}
	#endregion

	#region UIElement 
	protected override Size MeasureCore(Size availableSize)
	{
		ArgumentNullException.ThrowIfNull(Element);

		var bitmap = Element.Bitmap;
		if (bitmap is null) return new Size();

		//scale
		var size = new Size(bitmap.Width , bitmap.Height);

		if (Element.Enablescale)
		{
			size.Height = FontSize * Element.Scalerate;
			size.Width *= size.Height / bitmap.Height;
		}

		return size;
	}

	protected internal override void OnRender(DrawingContext ctx)
	{
		var bitmap = Element.Bitmap?.ToWriteableBitmap();
		if (bitmap != null) ctx.DrawImage(bitmap, FinalRect);
	}
	#endregion
}