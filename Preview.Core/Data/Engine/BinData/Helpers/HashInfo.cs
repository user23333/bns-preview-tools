namespace Xylia.Preview.Data.Engine.BinData.Helpers;
public class HashInfo(string path, ulong hash)
{
    public string Path => path;

    public ulong Hash => hash;
}