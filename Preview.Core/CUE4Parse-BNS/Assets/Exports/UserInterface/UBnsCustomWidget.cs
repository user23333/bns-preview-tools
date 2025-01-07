using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.BNS.Assets.Exports;
public class UBnsCustomCandidateWidget : UBnsCustomBaseWidget
{

}

public class UBnsCustomCaptionWidget : UBnsCustomBaseWidget
{

}

public class UBnsCustomColumnListWidget : UBnsCustomBaseWidget
{
	[UPROPERTY] public bool AttachScrollBar;
	[UPROPERTY] public bool bEnableDragScrollFromChild;
	[UPROPERTY] public bool bEnableDragScroll;
	[UPROPERTY] public bool bAutoRename;
	[UPROPERTY] public float MinAutoHeight;
	[UPROPERTY] public float DragScrollOffsetLimit;
	[UPROPERTY] public int AutoHeightColumnIndex;
	[UPROPERTY] public float PixelScrollValue;
	[UPROPERTY] public float ColumnGap;
	[UPROPERTY] public float RowGap;
	//SetImageProperties
	//SetImagePropertiesEx
	[UPROPERTY] public int ColumnCount;
	//SetImageSet
	//SetMultiImage
	//SetOpacity
	[UPROPERTY] public int RowCount;
}

public class UBnsCustomEditBoxWidget : UBnsCustomBaseWidget
{
	//OnEditboxClearInputFocus
	//OnEditboxInputFocus
	//OnEditboxCaretDown
	//OnEditboxCaretUp
	//OnEditboxCaret
	//OnEditboxControl
	//OnEditboxLanguage
	//OnEditboxSelect
	//OnEditboxComposited
	//OnEditboxCompositioning
	//OnEditboxChange
	//OnEditboxEnter


	////[UPROPERTY] public bool bEnableGlobalState;
	////[UPROPERTY] public FPackageIndex KeyInputEvent;
	////[UPROPERTY] public ImageProperty BackgroundImageProperty;
	////[UPROPERTY] public ImageProperty FocusedBackgroundImageProperty;
}

public class UBnsCustomImageWidget : UBnsCustomBaseWidget
{
	public bool EnableResourceSize;
}

public class UBnsCustomLabelWidget : UBnsCustomBaseWidget
{

}

public class UBnsCustomLabelButtonWidget : UBnsCustomBaseWidget
{
	//OnRepeat
	//ActiveOnlyFontSet
	//DisabledFontSet
	//PressedFontSet
	//MouseOverFontSet
	//BnsCustomMultiLIneEditBoxStringStyle

	[UPROPERTY] public ImageProperty NormalImageProperty;
	[UPROPERTY] public ImageProperty ActivatedImageProperty;
	[UPROPERTY] public ImageProperty PressedImageProperty;
	[UPROPERTY] public ImageProperty DisableImageProperty;
	[UPROPERTY] public ImageProperty ActiveOnlyImageProperty;
	[UPROPERTY] public ImageProperty CheckedNormalImageProperty;
	[UPROPERTY] public ImageProperty CheckedActivatedImageProperty;
	[UPROPERTY] public ImageProperty CheckedPressedImageProperty;
	[UPROPERTY] public ImageProperty CheckedDisableImageProperty;
	[UPROPERTY] public ImageProperty CheckedActiveOnlyImageProperty;

	[UPROPERTY] public FPackageIndex PresedEvent;  // SOUND
}


public class UBnsCustomGraphMapWidget : UBnsCustomColumnListWidget
{
	[UPROPERTY] public FPackageIndex EdgeActiveEvent;
	[UPROPERTY] public FPackageIndex EdgePresedEvent;
	[UPROPERTY] public FPackageIndex EdgeClickedEvent;
	[UPROPERTY] public FPackageIndex NodeActiveEvent;
	[UPROPERTY] public FPackageIndex NodePresedEvent;
	[UPROPERTY] public FPackageIndex NodeClickedEvent;

	[UPROPERTY] public float MaxZoomRatio;
	[UPROPERTY] public float MinZoomRatio;
	[UPROPERTY] public bool bZoomByMouseLocation;
	[UPROPERTY] public FStructFallback[] VerticalRulerInfoArray;
	[UPROPERTY] public FPackageIndex VerticalRuler_Background_ImageSet;
	[UPROPERTY] public FStructFallback[] HorizontalRulerInfoArray;
	[UPROPERTY] public FPackageIndex HorizontalRuler_Background_ImageSet;
	[UPROPERTY] public float ZoomRatio;
	[UPROPERTY] public FStructFallback[] NodeArray;
	[UPROPERTY] public FStructFallback[] EdgeArray;
	[UPROPERTY] public FVector2D CenterPos;
	[UPROPERTY] public float MinNodeNumVertical;
	[UPROPERTY] public float MinNodeNumHorizontal;
	[UPROPERTY] public float MinNodeSizeVertical;
	[UPROPERTY] public float MinNodeSizeHorizontal;
	[UPROPERTY] public FVector2D RatioToAdjustCenterPosBound;
	[UPROPERTY] public FVector2D Background_Padding;
	[UPROPERTY] public EUIAlignment CenterPos_AlignmentVertical;
	[UPROPERTY] public EUIAlignment CenterPos_AlignmentHorizontal;
	[UPROPERTY] public FVector2D Background2_StaticPadding;
	//Background2_Image
	//Background2_Coordinates	
	//Background1_ImageSet
	[UPROPERTY] public int MaxLineNumVertical;
	[UPROPERTY] public int MaxLineNumHorizontal;
	[UPROPERTY] public float LineSize;
	[UPROPERTY] public FPackageIndex[] GraphEdgeImageArray;
	[UPROPERTY] public float ArrowWidth;
	[UPROPERTY] public float ArrowLength;
}

public class UBnsCustomMinimapWidget : UBnsCustomBaseWidget
{

}

public class UBnsCustomProgressBarWidget : UBnsCustomBaseWidget
{
	//DecrementHighLightProgressImageStyle
	//IncrementHighLightProgressImageStyle
	//ProgressColorSectionDataArray
	//ProgressMarkImageDataArray
	//ProgressSectionDataArray
	//SeparatorImageSet
	//BaseSectionPassiveColor
	//BaseSectionActiveColor
	//BorderPadding
	//BarFillType
	//OnMarkImageDeActivated
	//OnMarkImageActivated
	//ProgressOrientation
	//ChildValueMarkerImage
	[UPROPERTY] public bool bTextPercentageValue;
	[UPROPERTY] public int MaxProgressValue;
	[UPROPERTY] public int InitProgressValue;


	// progressimageproperty
	// backgroundimageproperty
	// XY incrementscale
	// XY incrementlifetimescale
	// XY decrementscale
	// canassignwidgetid
	// scalehorizontalalignment
	// scaleverticalalignment
}

public class UBnsCustomScrollBarWidget : UBnsCustomBaseWidget
{
	[UPROPERTY] public bool bUsedScrollIndex;
	[UPROPERTY] public bool ScrollHide;
}

public class UBnsCustomSizerWidget : UBnsCustomBaseWidget
{
	//VerticalFace
	//HorizontalFace
	[UPROPERTY] public float MaxVerticalRatio;
	[UPROPERTY] public float MinVerticalRatio;
	[UPROPERTY] public float MaxHorizontalRatio;
	[UPROPERTY] public float MinHorizontalRatio;
	[UPROPERTY] public bool EnableVerticalFace;
	[UPROPERTY] public bool EnableHorizontalFace;
}

public class UBnsCustomSliderBarWidget : UBnsCustomBaseWidget
{
	[UPROPERTY] public bool bIsDrag;
	[UPROPERTY] public bool bPressedLeftButton;

	////[UPROPERTY] public ImageProperty BackgroundImageProperty;
	////[UPROPERTY] public ImageProperty HighlightImageProperty;
	////[UPROPERTY] public Orientation SliderOrientation;
	////[UPROPERTY] public bool SliderSnap;
	////[UPROPERTY] public float SliderStepValue;
	////[UPROPERTY] public bool bReverseDirection;
}

public class UBnsCustomToggleButtonWidget : UBnsCustomLabelButtonWidget
{
	[UPROPERTY] public bool bIsChecked;
	[UPROPERTY] public EBNSCustomToggleType ToggleType;
}

public class UBnsCustomUIMeshWidget : UBnsCustomBaseWidget
{
	//AspectRatio
	//MeshScale
	//CameraPosition
	//RoomType
	//AutoCameraType
	//CaptureRatio
	[UPROPERTY] public bool bEnableThumbnail;
	[UPROPERTY] public bool bEnableOwnerClipping;
}

public class UBnsCustomUISceneWidget : UBnsCustomBaseWidget
{

}

public class UBnsCustomWindowWidget : UBnsCustomBaseWidget
{
	//OnWindowKillFocus
	//OnWindowFocus
	//OnFunctionKeyPressed
	//DualRenderBrush
	[UPROPERTY] public bool bDualRenderInversePos;
	[UPROPERTY] public float fDualRenderGap;
	//eDualRenderFace
	[UPROPERTY] public bool bEnableDualRender;
	[UPROPERTY] public float fVisibleBlendHideDelayTime;
	[UPROPERTY] public float fVisibleBlendShowDelayTime;
	[UPROPERTY] public bool bEnableTabKey;
	//AspectRatioUpperBoundBeforeAdjust
	//AspectRatioLowerBoundBeforeAdjust
	//EanableFittedHorizontalAspectRatio
	[UPROPERTY] public bool bIgnoreClearInputFocus;
	//AdjustAspectRatio
	[UPROPERTY] public bool bIgnoreCustomize;
	[UPROPERTY] public bool bEnableCustomize;
	[UPROPERTY] public bool bEnableTopOrder;
	[UPROPERTY] public ImageProperty CustomizeImageProperty;
}

public class UBnsWebBrowser : UBnsCustomBaseWidget
{
	[UPROPERTY] public bool bEnableStartShowScrollbar;
	[UPROPERTY] public bool bEnableWindowClipboard;
	[UPROPERTY] public bool bEnableWebResize;
	[UPROPERTY] public bool bUseLocalFont;
	[UPROPERTY] public bool bUseExternalLink;
}



public class UBnsUISceneGroupUserWidget : USerializeObject
{
	[UPROPERTY] public bool bPauseGameWhileActive;
	[UPROPERTY] public bool bRenderParentScenes;
	[UPROPERTY] public bool bDisplayCursor;
	[UPROPERTY] public string SceneInputMode;
	[UPROPERTY] public int SceneGroupId;
}