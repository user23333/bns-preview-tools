using Xylia.Preview.Data.Engine.DatData;

namespace Xylia.Preview.Data.Common.DataStruct;
/// <summary>
///  Represents an instant in time
/// </summary>
public struct Time32(int Ticks)	: ITime
{
	#region Properties
	public int Ticks = Ticks;

    public readonly int Year => throw new NotImplementedException();

    public readonly int Month => throw new NotImplementedException();

    public readonly int Day => throw new NotImplementedException();

    public readonly sbyte Hour => throw new NotImplementedException();

    public readonly int Minute => throw new NotImplementedException();

    public readonly int Second => throw new NotImplementedException();
	#endregion

	#region Override Methods
	readonly ulong ITime.Ticks => (ulong)Ticks;
	#endregion

	#region Static Methods
    public static Time32 Parse(string input, EPublisher publisher)
	{
		throw new NotImplementedException();
	}
	#endregion
}