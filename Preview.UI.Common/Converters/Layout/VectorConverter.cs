using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.Core.Math;

namespace Xylia.Preview.UI.Converters;
public class Vector2DConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
	{
		if (sourceType == typeof(string)) return true;

		return base.CanConvertFrom(context, sourceType);
	}

	public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
	{
		if (value is string s)
		{
			var array = s.Split(' ', ',');
			if (array.Length >= 2) return new FVector2D(
				Convert.ToSingle(array[0]),
				Convert.ToSingle(array[1]));

			return FVector2D.ZeroVector;
		}

		return base.ConvertFrom(context, culture, value);
	}
}

public static class VectorEx
{
    public static FVector2D Parse(this Size value)
	{
		return new FVector2D(
			(float)value.Width, 
			(float)value.Height);
	}

	public static HorizontalAlignment Parse(this HAlignment value)
	{
		return value switch
		{
			HAlignment.HAlign_Left => HorizontalAlignment.Left,
			HAlignment.HAlign_Center => HorizontalAlignment.Center,
			HAlignment.HAlign_Right => HorizontalAlignment.Right,
			_ => throw new NotImplementedException(),
		};
	}
}