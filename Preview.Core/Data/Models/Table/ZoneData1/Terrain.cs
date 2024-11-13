namespace Xylia.Preview.Data.Models;
public sealed class Terrain : ModelElement
{
	#region Attributes
	public short Id { get; set; }

	public string Alias { get; set; }

	public string UmapName { get; set; }

	public short TerrainStartX { get; set; }

	public short TerrainStartY { get; set; }

	public short CountX { get; set; }

	public short CountY { get; set; }

	public int TerrainOriginX { get; set; }

	public int TerrainOriginY { get; set; }

	public string Description { get; set; }

	public bool SkipCooking { get; set; }

	public short ReleaseContentsGroup { get; set; }
	#endregion
}