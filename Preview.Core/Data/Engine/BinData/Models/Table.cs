using System.Collections;
using System.Collections.Concurrent;
using System.Data;
using System.Text;
using System.Xml;
using K4os.Hash.xxHash;
using Newtonsoft.Json;
using Serilog;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Engine.BinData.Serialization;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Engine.BinData.Models;
/// <summary>
/// bns data table
/// </summary>
[JsonConverter(typeof(TableConverter))]
public class Table : TableHeader, IDisposable, IEnumerable<Record>
{
	#region Data
	/// <summary>
	/// table owner
	/// </summary>
	public IDataProvider Owner { get; set; }

	private TableDefinition definition;

	/// <summary>
	/// data struct definition
	/// </summary>
	public TableDefinition Definition
	{
		get => definition;
		set
		{
			// create default def if null
			value ??= TableDefinition.CreateDefault(this.Type);

			definition = value;
			Name = value.Name;

			this.CheckVersion((definition.MajorVersion, definition.MinorVersion));
		}
	}


	public bool IsBinary { get; internal set; }

	internal TableArchive Archive { get; set; }

	internal StringLookup GlobalString { get; set; }

	/// <summary>
	/// TODO: Hack because the table seems to offset it randomly?
	/// </summary>
	internal int RecordCountOffset { get; set; }

	/// <summary>
	/// TODO: Hack because idk where this padding is coming from
	/// </summary>
	internal byte[] Padding { get; set; }

	protected List<Record> _records;

	/// <summary>
	/// element collection
	/// </summary>
	public List<Record> Records
	{
		internal set => _records = value;
		get
		{
			lock (this) { if (_records == null) LoadAsync().Wait(); }
			return _records;
		}
	}


	private readonly Dictionary<Ref, Record> ByRef = [];

	internal AliasTable AliasTable;
	#endregion


	#region Load Methods
	public Task LoadAsync() => Task.Run(() =>
	{
		if (_records != null) return;

		if (IsBinary) LoadData();
		else LoadXml(Owner.GetFiles(Definition.Pattern));
	});

	private void LoadData()
	{
		Archive.ReadFrom(this);
		Archive = null;

		foreach (var record in _records)
		{
			ByRef[record.PrimaryKey] = record;
		}
	}

	/// <summary>
	/// load data from xml
	/// </summary>
	/// <returns>data build actions</returns>
	public List<Action> LoadXml(params Stream[] streams)
	{
		this.Clear();
		_records = [];

		var actions = new List<Action>();
		foreach (var stream in streams)
		{
			XmlDocument xml = new() { PreserveWhitespace = true };
			xml.Load(stream);
			stream.Close();

			var documentElement = xml.DocumentElement;
			string type = documentElement.Attributes["type"]?.Value;
			string version = documentElement.Attributes["version"]?.Value;

			CheckVersion(ParseVersion(version));

			// ignore step data
			// TutorialSkillSequenceLoader ?
			if (type != null && string.Compare(type, Name, true) != 0)
			{
				Log.Error($"[game-data-loader], load error. invalid type, fileName:{Name}, type:{type}");
			}

			LoadElement(documentElement, actions);
		}

		return actions;
	}

	/// <summary>
	/// load xml element
	/// </summary>
	/// <param name="parent"></param>
	/// <param name="actions">data build action collection</param>
	internal void LoadElement(XmlElement parent, ICollection<Action> actions)
	{
		_records ??= [];

		var length = _records.Count;
		var elements = parent.SelectNodes($"./" + Definition.ElRecord.Name).OfType<XmlElement>().ToArray();

		// load data
		ConcurrentBag<Tuple<int, Record>> records = [];
		Parallel.For(0, elements.Length, index =>
		{
			var element = elements[index];

			// get definition
			var definition = Definition.ElRecord.SubtableByName(element.GetAttribute(AttributeCollection.s_type), Message);
			var record = new Record
			{
				Owner = this,
				Data = new byte[definition.Size],
				DataSize = definition.Size,
				ElementType = ElementType.Element,
				SubclassType = definition.SubclassType,
				StringLookup = IsCompressed ? new StringLookup() : GlobalString,
			};

			// create attributes and primary key
			record.Attributes = new(record, element, Definition.ElRecord, length + index + 1);
			record.Attributes.BuildData(definition, true);

			records.Add(new Tuple<int, Record>(index, record));

			//Log.Warning($"[game-data-loader], load {Name} error, msg:{0}, fileName:{1}, nodeName:{element.Name}, record:{element.OuterXml}");
		});

		// insert element
		foreach (var record in records.OrderBy(x => x.Item1).Select(x => x.Item2))
		{
			_records.Add(ByRef[record.PrimaryKey] = record);

			// The ref is not determined at this time
			actions?.Add(new Action(() => record.Attributes.BuildData(record.Definition)));
		}
	}

	public void CheckSize()
	{
		Records.GroupBy(o => o.SubclassType).OrderBy(o => o.Key).ForEach(type =>
		{
			var def = Definition.ElRecord.SubtableByType(type.Key, type.First());
		});
	}
	#endregion

	#region Get Methods
	public Record this[Ref Ref]
	{
		get
		{
			if (Ref == default) return null;
			lock (this) { if (_records == null) LoadAsync().Wait(); }

			if (ByRef.TryGetValue(Ref, out var item)) return item;

#if DEVELOP
			System.Diagnostics.Debug.WriteLine($"[{Name}] get failed, id: {Ref.Id} variation: {Ref.Variant}");
#endif
			return null;
		}
	}

	public Record this[string alias]
	{
		get
		{
			if (string.IsNullOrEmpty(alias)) return null;
			if (Ref.TryPrase(alias, out var key)) return this[key];

			lock (this)
			{
				if (AliasTable is null)
				{
					AliasTable = new();

					var def = Definition.ElRecord["alias"];
					if (def != null) Records?.ForEach(x => AliasTable.Add(x));
				}
			}

			return this[AliasTable.Find(AliasTable.MakeKey(Name, alias))];
		}
	}
	#endregion

	#region Serialize Methods
	public List<HashInfo> WriteXml(string folder, TableWriterSettings settings = null)
	{
		var hash = new List<HashInfo>();

		var name = Definition.Pattern.Replace("*", null);
		var path = Path.Combine(folder, name);
		Directory.CreateDirectory(Path.GetDirectoryName(path));

		var data = WriteXml(settings ?? new()
		{
			ReleaseSide = ReleaseSide.Client,
			Encoding = path.EndsWith(".x16", StringComparison.OrdinalIgnoreCase) ? Encoding.Unicode : Encoding.UTF8,
		});
		File.WriteAllBytes(path, data);

		hash.Add(new HashInfo(name, XXH64.DigestOf(data)));
		return hash;
	}

	public byte[] WriteXml(TableWriterSettings settings, params Record[] records)
	{
		ArgumentNullException.ThrowIfNull(settings);

		using var ms = new MemoryStream();
		using var writer = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true, IndentChars = "\t", Encoding = settings.Encoding });

		// document
		writer.WriteStartDocument();
		writer.WriteStartElement(Definition.ElRoot.Name);
		writer.WriteAttributeString("release-module", "LocalizationData");
		writer.WriteAttributeString("release-side", settings.ReleaseSide.ToString().ToLower());
		writer.WriteAttributeString("type", Definition.Name);
		writer.WriteAttributeString("version", MajorVersion + "." + MinorVersion);
		// write version when mismatch
		if (!MatchVersion(Definition.MajorVersion, Definition.MinorVersion))
			writer.WriteAttributeString("def-version", Definition.MajorVersion + "." + Definition.MinorVersion);

		// write file path
		writer.WriteComment($" {Name}.xml ");

		// records
		if (records.Length == 0) records = [.. Records];
		records.ForEach(record => record.WriteXml(writer, Definition.ElRecord));

		// finish
		writer.WriteEndElement();
		writer.WriteEndDocument();
		writer.Flush();
		return ms.ToArray();
	}
	#endregion


	#region Interface
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public IEnumerator<Record> GetEnumerator()
	{
		foreach (var record in this.Records)
			yield return record;

		yield break;
	}

	public virtual void Clear()
	{
		// prevent reload
		Archive = null;
		GlobalString = new StringLookup();

		_records?.Clear();
		_records = null;

		ByRef.Clear();
		AliasTable?.Table.Clear();
	}

	public void Dispose()
	{
		this.Clear();
		definition = null;

		GC.SuppressFinalize(this);
	}
	#endregion
}