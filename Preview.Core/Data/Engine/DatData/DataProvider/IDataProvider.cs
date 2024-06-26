using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Engine.BinData.Serialization;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Engine.DatData;
/// <summary>
/// bns data package provider
/// </summary>
public interface IDataProvider : IDisposable , ITextProvider
{
	#region Fields
	/// <summary>
	/// DataSource Name
	/// </summary>
	string Name { get; }

    /// <summary>
    /// DataSource localized information
    /// </summary>
    Locale Locale => default;

	/// <summary>
	/// DataSource Timestamp
	/// </summary>
	Time64 CreatedAt { get; }

	/// <summary>
	/// DataSource Version
	/// </summary>
	BnsVersion ClientVersion { get; }

    /// <summary>
    /// bns data table
    /// </summary>
    TableCollection Tables { get; }

	//AliasTable AliasTable { get; }
	#endregion

	#region Properties
	/// <summary>
	/// Indicates whether the provider is a special server
	/// </summary>
	bool IsNeo => Locale.Publisher is EPublisher.ZNcs or EPublisher.ZTx;

	string ITextProvider.this[string alias] => GetTable("text")[alias]?.Attributes.Get<string>("text");
	#endregion

	#region Methods
	/// <summary>
	/// Get raw file stream
	/// </summary>
	/// <remarks>The result will not be null</remarks>
	/// <returns></returns>
	Stream[] GetFiles(string pattern);

	/// <summary>
	/// Get table
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public Table GetTable(string name) => Tables[name];

	/// <summary>
	/// Get model table
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="name"></param>
	/// <param name="reload"></param>
	/// <returns></returns>
	public GameDataTable<T> GetTable<T>(string name = null, bool reload = false) where T : ModelElement => Tables.Get<T>(name , reload);


	/// <summary>
	/// Load package
	/// </summary>
	/// <remarks>In some case, require automatic parse <see langword="definitions"/></remarks>
	/// <param name="definitions"></param>
	public void LoadData(DatafileDefinition definitions);

	/// <summary>
	/// Write package
	/// </summary>
	/// <param name="folder"></param>
	/// <param name="settings"></param>
	public void WriteData(string folder, PublishSettings settings) => throw new NotImplementedException();
	#endregion
}