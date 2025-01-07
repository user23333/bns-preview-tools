using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.BNS.Assets.Exports;
public class UFaceFXAnim : USerializeObject
{
	[UPROPERTY] public object AnimData;
	[UPROPERTY] public object Id;
	[UPROPERTY] public FSoftObjectPath SoundCue;
}