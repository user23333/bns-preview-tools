using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.FileProvider;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Common.DataStruct;
[StructLayout(LayoutKind.Sequential)]
[DebuggerDisplay("Ref: {IconTextureRef}, Index.: {IconTextureIndex}")]
public readonly struct IconRef(Ref @ref, short index = 1)
{
	public readonly Ref IconTextureRef = @ref;
	public readonly int IconTextureIndex = index;

	#region Methods
	public static bool operator ==(IconRef a, IconRef b)
	{
		return
			a.IconTextureRef == b.IconTextureRef &&
			a.IconTextureIndex == b.IconTextureIndex;
	}

	public static bool operator !=(IconRef a, IconRef b)
	{
		return !(a == b);
	}

	public readonly bool Equals(IconRef other)
	{
		return IconTextureRef == other.IconTextureRef &&
			IconTextureIndex == other.IconTextureIndex;
	}

	public override readonly bool Equals(object obj)
	{
		return obj is IconRef other && Equals(other);
	}

	public override readonly int GetHashCode()
	{
		return HashCode.Combine(IconTextureRef, IconTextureIndex);
	}

	public Icon GetIcon(IDataProvider provider)
	{
		if (this == default) return null;

		var table = provider.Tables["icontexture"];
		return new Icon(table?[this], (short)IconTextureIndex);
	}
	#endregion
}

[TypeConverter(typeof(IconConvert))]
public class Icon(Record record, short index)
{
	public static Icon Parse(string s, IDataProvider provider)
	{
		if (!string.IsNullOrWhiteSpace(s) && s.Contains(','))
		{
			var split = s.Split(',', 2);
			var alias = split[0];
			if (!short.TryParse(split[^1], out var index))
				throw new Exception("get icon index failed: " + s);

			return new Icon(provider.Tables["icontexture"]?[alias], index);
		}

		throw new NotSupportedException();
	}

	public IconRef GetRef() => new(record?.PrimaryKey ?? default, index);

	public ImageProperty GetImage(IFileProvider pak = null)
	{
		return record.To<IconTexture>()?.GetImage(index, pak);
	}

	public override string ToString() => $"{record},{index}";

	public class IconConvert : TypeConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(ImageProperty) ||
				destinationType == typeof(IconRef)) return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is Icon icon)
			{
				if (destinationType == typeof(ImageProperty)) return icon.GetImage();
				if (destinationType == typeof(IconRef)) return icon.GetRef();
			}

			return null;
		}
	}
}