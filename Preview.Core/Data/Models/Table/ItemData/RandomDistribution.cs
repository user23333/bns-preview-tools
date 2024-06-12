namespace Xylia.Preview.Data.Models;
public sealed class RandomDistribution : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public short[] Weight { get; set; }
	#endregion
}