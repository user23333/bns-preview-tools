using System.Diagnostics;
using CUE4Parse.UE4;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.i18N;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Data.Common.DataStruct;

namespace CUE4Parse.BNS.Assets.Exports;
[StructFallback]
[DebuggerDisplay("{ExpansionName}")]
public class UBnsCustomExpansionComponent : IUStruct
{
	#region Properties
	public bool bEnableSubState { get; set; }
	public bool bPostExpansitonRender { get; set; }
	public bool bShow { get; set; }
	public bool bVisibleFlag { get; set; }
	public EBNSCustomExpansionComponentType ExpansionType { get; set; }
	public FName ExpansionName { get; set; }
	public string MetaData { get; set; }
	public EBNSCustomWidgetStateType WidgetState { get; set; }
	public EBNSCustomExpansionWidgetSubState WidgetSubState { get; set; }
	public FStructFallback PublisherVisible { get; set; }
	public FStructFallback MetaDataByPublisher { get; set; }
	public ImageProperty ImageProperty { get; set; }
	public StringProperty StringProperty { get; set; }
	#endregion

	#region Methods
	//GetOwnerWidget
	//PlayAlphaAnimation
	//ResetAlphaAnimation

	public UBnsCustomExpansionComponent Clone()
	{
		var component = (UBnsCustomExpansionComponent)this.MemberwiseClone();
		if (ImageProperty != null) component.ImageProperty = ImageProperty with { };
		if (StringProperty != null) component.StringProperty = StringProperty with { };

		return component;
	}

	public void SetEnableGray() { }

	public void SetExpansionShow(bool value)
	{
		this.bShow = value;
	}

	public void SetVisibleFlag(ESlateVisibility flag)
	{

	}

	public void SetValue(object value)
	{
		// set field or properties
		if (ExpansionType == EBNSCustomExpansionComponentType.STRING)
		{
			if (value is StringProperty p) StringProperty = p;
			else StringProperty.LabelText = new FText(value?.ToString());
		}
		else if (ExpansionType == EBNSCustomExpansionComponentType.IMAGE)
		{
			switch (value)
			{
				case Icon icon: ImageProperty = icon.GetImage(); break;
				case ImageProperty property: ImageProperty = property; break;
				case FPackageIndex index:
				{
					if (ImageProperty.EnableImageSet) ImageProperty.ImageSet = index;
					else ImageProperty.BaseImageTexture = index;
					break;
				}

				default: ImageProperty.BaseImageTexture = null; break;
			}
		}
		else throw new NotSupportedException();
	}
	#endregion
}