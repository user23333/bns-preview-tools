using System.Text;
using Xylia.Preview.Common.Attributes;

namespace Xylia.Preview.Data.Engine.BinData.Serialization;
/// <summary>
/// Respents a set of features to support on write table
/// </summary>
public sealed class TableWriterSettings
{
	/// <summary>
	/// Gets or sets the release side.
	/// </summary>
	public ReleaseSide ReleaseSide { get; set; } = ReleaseSide.Client;

	/// <summary>
	/// Gets or sets the type of text encoding to use.
	/// </summary>
	/// <returns>The text encoding to use. The default is <see cref="Encoding.UTF8"/>.</returns>
	public Encoding Encoding { get; set; } = Encoding.UTF8;
}