using CUE4Parse.UE4.Assets.Exports;
using Xylia.Preview.Common;

namespace Xylia.Preview.Data.Common.DataStruct;
public readonly struct ObjectPath(string path)
{
	public readonly string Path = path;

	#region Methods
	public bool IsValid => !string.IsNullOrWhiteSpace(Path);

	public override string ToString() => Path;

	public UObject LoadObject() => Globals.GameProvider.LoadObject(Path);

	public T LoadObject<T>() where T : UObject => Globals.GameProvider.LoadObject<T>(Path);
	#endregion
}