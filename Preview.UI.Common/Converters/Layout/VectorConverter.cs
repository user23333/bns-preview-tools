using System.Windows;
using CUE4Parse.UE4.Objects.Core.Math;

namespace Xylia.Preview.UI.Converters;
public static class VectorEx
{
    public static FVector2D Parse(this Size value)
	{
		return new FVector2D(
			(float)value.Width, 
			(float)value.Height);
	}
}