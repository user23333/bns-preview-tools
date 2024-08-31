using System.Collections;
using System.Diagnostics;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Models;

namespace Xylia.Preview.Data.Models;
public class GameDataTable<T> : IEnumerable<T>, IEnumerable, IDisposable where T : ModelElement
{
	#region Constructors
	internal GameDataTable(Table source)
	{
		Source = source;
		ElementTypeConverter.Get(typeof(T));

		Trace.WriteLine($"{DateTime.Now} load table `{source.Name}` successful ({source.Records.Count})");
	}
	#endregion

	#region Properties
	private List<T> elements;

	public Table Source { get; }

	public List<T> Elements
	{
		get
		{
			if (elements == null)
			{
				elements = new List<T>(new T[Source.Records.Count]);
				Parallel.For(0, elements.Count, i => elements[i] = LoadElement(Source.Records[i]));
			}

			return elements;
		}
	}
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

	protected virtual T LoadElement(Record record) => record.As<T>();
	#endregion


	#region Interface
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public IEnumerator<T> GetEnumerator() => Elements.GetEnumerator();

	public void Dispose()
	{
		Source.Dispose();
		elements?.Clear();

		GC.SuppressFinalize(this);
	}
	#endregion
}