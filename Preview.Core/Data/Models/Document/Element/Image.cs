using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.BNS.Conversion;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse_Conversion.Textures;
using SkiaSharp;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.Data.Models.Document;
public class Image : HtmlElementNode
{
    #region Fields
    // jpgpath
    public string Imagesetpath { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }
    public string Path { get => GetAttributeValue<string>(); set => SetAttributeValue(value); }

    public int U { get => GetAttributeValue<int>(); set => SetAttributeValue(value); }
    public int V { get => GetAttributeValue<int>(); set => SetAttributeValue(value); }
    public int UL { get => GetAttributeValue<int>(); set => SetAttributeValue(value); }
    public int VL { get => GetAttributeValue<int>(); set => SetAttributeValue(value); }
    public int Width { get => GetAttributeValue<int>(); set => SetAttributeValue(value); }
    public int Height { get => GetAttributeValue<int>(); set => SetAttributeValue(value); }

    public sbyte Red { get => GetAttributeValue<sbyte>(); set => SetAttributeValue(value); }
    public sbyte Green { get => GetAttributeValue<sbyte>(); set => SetAttributeValue(value); }
    public sbyte Blue { get => GetAttributeValue<sbyte>(); set => SetAttributeValue(value); }

    /// <summary>
    /// Relative to line height
    /// </summary>
    public bool Enablescale { get => GetAttributeValue<bool>(); set => SetAttributeValue(value); }

    public float Scalerate { get => GetAttributeValue<float>(); set => SetAttributeValue(value); }
    #endregion

    #region Methods
    private SKBitmap _bitmap;

    public SKBitmap Bitmap
    {
        set => _bitmap = value;
        get => _bitmap ??= FileCache.Provider.LoadObject<UImageSet>(Imagesetpath)?.GetImage() ??
            FileCache.Provider.LoadObject<UTexture2D>(Path)?.Decode()?.Clone(U, V, UL, VL);
    }
    #endregion
}