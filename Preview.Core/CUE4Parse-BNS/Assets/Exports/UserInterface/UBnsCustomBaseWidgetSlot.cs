using System.ComponentModel;
using System.Globalization;
using CUE4Parse.UE4;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.BNS.Assets.Exports;
public class UBnsCustomBaseWidgetSlot : USerializeObject
{
	[UPROPERTY] public bool bAutoSize;
	[UPROPERTY] public FLayoutData LayoutData;
	[UPROPERTY] public bool InbIsReadyOnly;
	[UPROPERTY] public FPackageIndex Parent;
	[UPROPERTY] public FPackageIndex Content;
}

[StructFallback]
public struct FLayoutData : IUStruct
{
	[UPROPERTY]
	public Anchor Anchors;
	[UPROPERTY]
	public Offset Offsets;
	[UPROPERTY]
	public FVector2D Alignments;

	#region Struct
	[StructFallback]
	[TypeConverter(typeof(AnchorsConverter))]
	public struct Anchor(float left, float top, float right, float bottom) : IUStruct
	{
		public FVector2D Minimum = new(left, top);
		public FVector2D Maximum = new(right, bottom);

		public override readonly string ToString() => $"{Minimum.X} {Minimum.Y} {Maximum.X} {Maximum.Y}";

		public readonly static Anchor Full = new(0, 0, 1, 1);

		internal class AnchorsConverter : TypeConverter
		{
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				if (sourceType == typeof(string)) return true;

				return base.CanConvertFrom(context, sourceType);
			}

			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				switch (value)
				{
					case string s when s.Equals("TopLeft", StringComparison.OrdinalIgnoreCase): return new Anchor();
					case string s when s.Equals("TopRight", StringComparison.OrdinalIgnoreCase): return new Anchor(1, 0, 1, 0);
					case string s when s.Equals("BottomLeft", StringComparison.OrdinalIgnoreCase): return new Anchor(0, 1, 0, 1);
					case string s when s.Equals("BottomRight", StringComparison.OrdinalIgnoreCase): return new Anchor(1, 1, 1, 1);
					case string s when s.Equals("Full", StringComparison.OrdinalIgnoreCase): return Full;
					case string s:
					{
						var anchor = new Anchor();

						var array = s.Split(' ', ',');
						if (array.Length == 4)
						{
							anchor.Minimum = new FVector2D(Convert.ToSingle(array[0]), Convert.ToSingle(array[1]));
							anchor.Maximum = new FVector2D(Convert.ToSingle(array[2]), Convert.ToSingle(array[3]));
						}

						return anchor;
					}

					default: return base.ConvertFrom(context, culture, value);
				}
			}
		}
	}

	[StructFallback]
	[TypeConverter(typeof(OffsetsConverter))]
	public struct Offset(float left, float top, float right, float bottom) : IUStruct
	{
		public float Left = left;
		public float Top = top;
		public float Right = right;
		public float Bottom = bottom;

		public override readonly string ToString() => $"{Left} {Top} {Right} {Bottom}";

		internal class OffsetsConverter : TypeConverter
		{
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				if (sourceType == typeof(string)) return true;

				return base.CanConvertFrom(context, sourceType);
			}

			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				switch (value)
				{
					case string s:
					{
						var anchor = new Offset();

						var array = s.Split(' ', ',');
						if (array.Length >= 2)
						{
							anchor.Left = Convert.ToSingle(array[0]);
							anchor.Top = Convert.ToSingle(array[1]);
						}

						if (array.Length == 4)
						{
							anchor.Right = Convert.ToSingle(array[2]);
							anchor.Bottom = Convert.ToSingle(array[3]);
						}

						return anchor;
					}

					default: return base.ConvertFrom(context, culture, value);
				}
			}
		}
	}
	#endregion
}