namespace Xylia.Preview.Data.Models;
public sealed class ZoneEnv2Spawn : ModelElement
{
    #region Attributes
    public string Alias { get; set; }

    public int Zone { get; set; }

    public short Id { get; set; }

    public Ref<ZoneEnv2> Env2 { get; set; }

    public Ref<ZoneEnv2Place> Env2place { get; set; }

    public Ref<ZoneEnv2SpawnRandomGroup> RandomGroup { get; set; }

    public Ref<ZoneEnv2Spawn> RequiredEnv { get; set; }

    public MapUnit.MapDepthSeq MapunitMapDepth { get; set; }

    public MapUnit.MapDepthSeq MapunitArenaDungeonMapDepth { get; set; }
    #endregion
}