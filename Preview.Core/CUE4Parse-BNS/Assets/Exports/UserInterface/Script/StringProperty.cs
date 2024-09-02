using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.i18N;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;
namespace CUE4Parse.BNS.Assets.Exports;

[StructFallback]
[TypeConverter(typeof(StringPropertyConverter))]
public class StringProperty : IUStruct, INotifyPropertyChanged
{
	private FPackageIndex _fontset;

	[TypeConverter(typeof(FPackageIndexTypeConverter))]
	public FPackageIndex fontset
	{
		get => _fontset;
		set => SetProperty(ref _fontset, value);
	}

	private FText _labelText;

	public FText LabelText
	{
		get => _labelText;
		set => SetProperty(ref _labelText, value);
	}

	public float SpaceBetweenLines { get; set; }
	public HAlignment HorizontalAlignment { get; set; }
	public VAlignment VerticalAlignment { get; set; }
	public FVector2D ClippingBound { get; set; }
	public ClipMode ClipMode { get; set; }
	public int MaxCharacters { get; set; }

	public FVector2D Padding { get; set; }
	public string ClippingBoundFace_Horizontal { get; set; }  //WidgetFaceFace_Left
	public string ClippingBoundFace_Vertical { get; set; }    //WidgetFaceFace_Top
	public bool bJustification { get; set; }
	public string JustificationType { get; set; }             //BNSCustomJustification_Type_Normal
	public bool bWordWrap { get; set; }
	public bool bIgnoreDPIScale { get; set; }
	public float Opacity { get; set; }
	public string TextDirection { get; set; }             //BNS_UIORIENT_Horizontal
	public float TextScale { get; set; }
	public float AnimScale { get; set; }
	public float LastRenderWidth { get; set; }
	public float LastRenderHeight { get; set; }


	#region Constructors
	public StringProperty()
	{

	}

	public StringProperty(string text, 
		HAlignment horizontalAlignment = HAlignment.HAlign_Center, 
		VAlignment verticalAlignment = VAlignment.VAlign_Center)
	{
		LabelText = text;
		HorizontalAlignment = horizontalAlignment;
		VerticalAlignment = verticalAlignment;
	}
	#endregion

	#region Methods
	public event PropertyChangedEventHandler PropertyChanged;

	internal bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(storage, value))
			return false;

		storage = value;
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		return true;
	}

	public StringProperty Clone()
	{
		return (StringProperty)this.MemberwiseClone();
	}
	#endregion
}

internal class StringPropertyConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => true;

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		return new StringProperty()
		{
			LabelText = new FText(value?.ToString()),
		};
	}
}