using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Data.Common.DataStruct;
public readonly struct Sub
{
	public readonly short Subclass;

	#region Methods
	public readonly string GetName(IDataProvider provider, ushort type, ushort element)
	{
		return provider.Tables[type]?.Definition.Elements[element].SubtableByType(Subclass).Name;
	}
	#endregion
}