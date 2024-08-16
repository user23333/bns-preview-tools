using System.Text;
using Xylia.Preview.Common.Attributes;

namespace Xylia.Preview.Data.Engine.BinData.Serialization;
/// <summary>
/// Respents a set of features to support on write table
/// </summary>
public sealed class TableWriterSettings
{
    public ReleaseSide ReleaseSide { get; set; } = ReleaseSide.Client;

    public Encoding Encoding { get; set; }
}