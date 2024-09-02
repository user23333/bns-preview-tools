using System.ComponentModel;
using System.Globalization;
using CUE4Parse.UE4;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.Math;

namespace CUE4Parse.BNS.Assets.Exports;
[StructFallback]
[TypeConverter(typeof(TintColorConverter))]
public struct TintColor : IUStruct
{
	public FLinearColor SpecifiedColor { get; set; }

	public override readonly string ToString() => SpecifiedColor.ToString();
}

internal class TintColorConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string)) return true;

		return base.CanConvertFrom(context, sourceType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string s)
		{
			return new TintColor() { SpecifiedColor = ToLinearColor(ToFColor(s), true) };
		}

		return base.ConvertFrom(context, culture, value);
	}


	public static FColor ToFColor(string s)
	{
		var argb = uint.Parse(s, NumberStyles.HexNumber);

		// Shift counts and bit masks for A, R, G, B components in ARGB mode
		var a = (byte)(argb >> 24);
		var r = (byte)(argb >> 16);
		var g = (byte)(argb >> 8);
		var b = (byte)(argb >> 0);

		return new FColor(r, g, b, a);
	}

	public static FLinearColor ToLinearColor(FColor c, bool sRGB)
	{
		var floatR = c.R / 255.999f;
		var floatG = c.G / 255.999f;
		var floatB = c.B / 255.999f;
		var floatA = c.A / 255.999f;

		if (sRGB)
		{
			floatR = floatR <= 0.04045F ? floatR / 12.92F : MathF.Pow((floatR + 0.055F) / 1.055F, 2.4F);
			floatG = floatG <= 0.04045F ? floatG / 12.92F : MathF.Pow((floatG + 0.055F) / 1.055F, 2.4F);
			floatB = floatB <= 0.04045F ? floatB / 12.92F : MathF.Pow((floatB + 0.055F) / 1.055F, 2.4F);
		}

		return new FLinearColor(floatR, floatG, floatB, floatA);
	}
}