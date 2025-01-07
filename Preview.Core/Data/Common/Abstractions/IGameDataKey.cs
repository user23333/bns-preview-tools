using System.Runtime.InteropServices;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Common.Abstractions;
public interface IGameDataKey
{
	internal unsafe Ref Key()
	{
		int size = Marshal.SizeOf(this);
		IntPtr ptr = Marshal.AllocHGlobal(size);
		Marshal.StructureToPtr(this, ptr, false);
		return *(Ref*)ptr;
	}
}

internal interface IMultiKeyRefGenerator
{
	long Create(string[] keyTextList);
}