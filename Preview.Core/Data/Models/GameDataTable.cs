using System.Collections;
using System.Diagnostics;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Models;

namespace Xylia.Preview.Data.Models;
public class GameDataTable<T> : IEnumerable<T>, IEnumerable, IDisposable where T : ModelElement
{
	#region Constructors
	internal GameDataTable(Table source)
	{
		Helper = ModelElement.TypeHelper.Get(typeof(T));
		Source = source;

		Trace.WriteLine($"[{DateTime.Now}] load table `{source.Name}` successful ({source.Records.Count})");
	}
	#endregion

	#region Properties
	private readonly ModelElement.TypeHelper Helper;
	private List<T> elements;

	public Table Source { get; }

	public List<T> Elements => elements ??= Source.Records.Select(LoadElement).ToList();
	#endregion

	#region Methods
	public T this[Ref Ref]
	{
		get => LoadElement(Source[Ref]);
	}

	public T this[string alias]
	{
		get => LoadElement(Source[alias]);
	}

	protected T LoadElement(Record record)
	{
		if (record is null) return null;

		var type = record.SubclassType == -1 ? null : record.Definition.Name;
		var element = ModelElement.As(record, Helper.CreateInstance<T>(type));
		element.LoadHiddenField();

		return element;
	}
	#endregion

	#region IDisposable
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public IEnumerator<T> GetEnumerator()
	{
		foreach (var element in this.Elements)
			yield return element;

		yield break;
	}

	public void Dispose()
	{
		Source.Dispose();
		Elements?.Clear();

		GC.SuppressFinalize(this);
	}
	#endregion
}