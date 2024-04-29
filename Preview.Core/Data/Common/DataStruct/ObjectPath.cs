using CUE4Parse.UE4.Assets.Exports;
using Xylia.Preview.Data.Helpers;

namespace Xylia.Preview.Data.Common.DataStruct;
public readonly struct ObjectPath(string path)
{
	#region Properties
	public readonly string Path = path;
	#endregion

	#region Methods
	public override string ToString() => Path;

	public UObject LoadObject() => FileCache.Provider.LoadObject(Path);

	public T LoadObject<T>() where T : UObject => FileCache.Provider.LoadObject<T>(Path);
	#endregion
}