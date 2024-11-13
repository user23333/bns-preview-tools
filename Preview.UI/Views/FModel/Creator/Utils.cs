using CUE4Parse.UE4.Assets.Exports;
using Xylia.Preview.Common;

namespace FModel.Creator;
public static class Utils
{
	public static bool TryLoadObject<T>(string fullPath, out T export) where T : UObject
	{
		return Globals.GameProvider.TryLoadObject(fullPath, out export);
	}
}