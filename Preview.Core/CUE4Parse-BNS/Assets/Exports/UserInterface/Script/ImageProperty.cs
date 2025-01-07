using System.ComponentModel;
using CUE4Parse.BNS.Conversion;
using CUE4Parse.UE4;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse_Conversion.Sounds;
using CUE4Parse_Conversion.Textures;
using SkiaSharp;

namespace CUE4Parse.BNS.Assets.Exports;
[StructFallback]
public record ImageProperty : IUStruct
{
	#region Properties
	public FStructFallback ResultBrush { get; set; }
	[TypeConverter(typeof(FPackageIndexTypeConverter))] public FPackageIndex BaseImageTexture { get; set; }
	[TypeConverter(typeof(FPackageIndexTypeConverter))] public FPackageIndex ImageSet { get; set; }
	public bool EnableImageSet { get; set; }
	public bool EnableBrushOnly { get; set; }
	public FStructFallback ImageBrush { get; set; }
	public FVector2D ImageUV { get; set; }
	public FVector2D ImageUVSize { get; set; }
	public bool EnableDrawImage { get; set; } = true;
	public bool EnableResourceSize { get; set; }
	public bool EnableFullImage { get; set; }
	public bool EnableFittedImage { get; set; }
	public bool EnableFittedImage_MinifyOnly { get; set; }
	public bool EnableClipping { get; set; }
	public bool EnableSkinColor { get; set; }
	public bool EnableSkinAlpha { get; set; }
	public bool EnableAdditiveBlendMode { get; set; }
	public bool EnableResourceGray { get; set; }
	public bool EnableDrawColor { get; set; }
	public float GrayWeightValue { get; set; }
	public float ImageScale { get; set; } = 1.0f;
	public EHorizontalAlignment HorizontalAlignment { get; set; }
	public EVerticalAlignment VerticalAlignment { get; set; }
	public EBNSSperateImageType SperateImageType { get; set; }
	public FVector2D[] CoordinatesArray { get; set; }
	public string SperateType { get; set; }
	public bool EnableMultiImage { get; set; }
	public FStructFallback MultiImage { get; set; }


	public TintColor TintColor { get; set; }
	public FVector2D StaticPadding { get; set; }
	public FVector2D Offset { get; set; }
	public float Opacity { get; set; } = 1;
	#endregion

	#region Methods		
	public static implicit operator SKBitmap(ImageProperty property) => property.Image;

	public SKBitmap Image
	{
		get
		{
			if (cached != null) return cached;

			#region Raw
			SKBitmap bitmap = null;

			if (!EnableDrawImage) bitmap = null;
			else if (EnableImageSet)
			{
				var obj = ImageSet.LoadEx();
				if (obj is UImageSet imageSet) bitmap = imageSet.GetImage();
			}
			else if (BaseImageTexture != null)
			{
				var obj = BaseImageTexture.LoadEx();
				if (obj is UTexture texture) bitmap = texture.Decode()?.Clone(ImageUV, ImageUVSize);
			}

			if (bitmap is null) return null;
			#endregion

			#region Draw
			var tint = TintColor.SpecifiedColor.ToSKColor();

			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					var p = bitmap.GetPixel(i, j);

					// TODO: not working perfectly
					if (EnableDrawColor && tint != default && p.Alpha > 64) p = tint;

					// set alpha channel
					p = p.WithAlpha((byte)(p.Alpha * Opacity));

					bitmap.SetPixel(i, j, p);
				}
			}
			#endregion

			return cached = bitmap;
		}
	}

	public string Tag
	{
		get
		{
			var c = TintColor.SpecifiedColor.ToFColor(true);

			return $"<image path='{BaseImageTexture.GetPathName()}' u='{ImageUV.X}' v='{ImageUV.Y}' ul='{ImageUVSize.X}' vl='{ImageUVSize.Y}' red='{c.R}' green='{c.G}' blue='{c.B}' enablescale='true' scalerate='{ImageScale}'/>";
		}

	}

	public FVector2D Measure(FVector2D RenderSize, out SKBitmap image)
	{
		image = Image;
		if (image is null) return FVector2D.ZeroVector;

		// scale
		if (ImageScale <= 0) ImageScale = 1.0F;
		var w = (EnableResourceSize ? image.Width : RenderSize.X) - StaticPadding.X * 2;
		var h = (EnableResourceSize ? image.Height : RenderSize.Y) - StaticPadding.Y * 2;

		return new FVector2D(ImageScale * w, ImageScale * h);
	}
	#endregion

	#region Fields
	private SKBitmap cached;
	#endregion
}