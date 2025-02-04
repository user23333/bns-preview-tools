using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.BNS.Conversion;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse_Conversion.Textures;
using SkiaSharp;
using Xylia.Preview.Common;

namespace Xylia.Preview.Data.Models.Document;
public class Image : HtmlElementNode
{
	#region Fields
	public string Imagesetpath;
	public string Imagesetpath2;
	public string Imagesetpath3;

	public string Path;
	public int U;
	public int V;
	public int UL;
	public int VL;
	public int Width;
	public int Height;

	public string Path2;
	public int U2;
	public int V2;
	public int UL2;
	public int VL2;
	public int Width2;
	public int Height2;

	public byte Red;
	public byte Green;
	public byte Blue;

	/// <summary>
	/// Relative to line height
	/// </summary>
	public bool Enablescale;
	public float Scalerate;
	#endregion

	#region Methods
	private SKColor Color => new(Red, Green, Blue);

	private SKBitmap _bitmap;
	public SKBitmap Bitmap
	{
		set => _bitmap = value;
		get => _bitmap ??= Globals.GameProvider.LoadObject<UImageSet>(Imagesetpath)?.GetImage(Color) ??
			Globals.GameProvider.LoadObject<UTexture2D>(Path)?.Decode()?.Clone(U, V, UL, VL, Color);
	}
	#endregion
}