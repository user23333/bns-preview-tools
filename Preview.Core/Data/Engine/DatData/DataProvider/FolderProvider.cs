using CUE4Parse.Utils;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Engine.Definitions;

namespace Xylia.Preview.Data.Engine.DatData;
public class FolderProvider(string path, EPublisher publisher = default) : IDataProvider
{
	#region Properties
	public string Name { get; init; } = path.SubstringAfterLast('\\');
	public Locale Locale { get; init; } = new(publisher);
	public Time64 CreatedAt => default;
	public BnsVersion ClientVersion => default;
	public TableCollection Tables { get; private set; }
	#endregion

	#region Methods
	public virtual Stream[] GetFiles(string pattern) => new DirectoryInfo(path).GetFiles(pattern, SearchOption.AllDirectories).Select(x => x.OpenRead()).ToArray();

	public void LoadData(DatafileDefinition definitions)
	{
		this.Tables = [];

		// TODO: ignore server table
		foreach (var definition in definitions.OrderBy(x => x.Name))
		{
			Tables.Add(new() { Owner = this, Name = definition.Name });
		}
	}

	public void Dispose()
	{
		Tables?.Clear();
		Tables = null;

		GC.SuppressFinalize(this);
	}
	#endregion
}