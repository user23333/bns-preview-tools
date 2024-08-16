using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Objects.Core.Math;
using SkiaSharp;

namespace CUE4Parse.UE4.Objects.Core.Serialization;
public static class FLinearColorEx
{
	public static SKColor ToSKColor(this FLinearColor color) => color.ToFColor(true).ToSKColor();

	public static SKColor ToSKColor(this FColor color) => Unsafe.As<FColor, SKColor>(ref color);
}