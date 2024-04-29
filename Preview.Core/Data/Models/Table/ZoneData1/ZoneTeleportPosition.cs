using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class ZoneTeleportPosition : ModelElement
{
	#region Attributes
	public int Zone { get; set; }

	public Vector32 Position { get; set; }

	public short Yaw { get; set; }
	#endregion
}