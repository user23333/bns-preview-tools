﻿using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.BNS.Assets.Exports;
public sealed class UShowFaceFxUE4Key : ShowKeyBase
{
	[UPROPERTY] public FSoftObjectPath FaceFXActorObj;
	[UPROPERTY] public FSoftObjectPath FaceFXAnimObj;
	[UPROPERTY] public FPackageIndex FaceFXAnimBlueprintClass;
}