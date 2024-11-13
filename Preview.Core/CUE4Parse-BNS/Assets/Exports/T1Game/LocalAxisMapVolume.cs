using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;

namespace CUE4Parse.BNS.Assets.Exports;
public class LocalAxisMapVolume : USerializeObject
{
	#region Properties
	[UPROPERTY] public int DiscoverId;
	[UPROPERTY] public string DiscoverVolumeName;
	[UPROPERTY] public string AxisDescription;
	[UPROPERTY] public int Mapid;
	#endregion
}