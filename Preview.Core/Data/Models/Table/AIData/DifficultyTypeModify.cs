using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Models;
public sealed class DifficultyTypeModify : ModelElement 
{
	#region Attributes
	public string Alias { get; set; }

	public Msec[] BerserkSequenceInvokeTime { get; set; }
	#endregion
}