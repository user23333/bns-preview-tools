using System.Collections.ObjectModel;
using System.Formats.Tar;
using System.Reflection;
using CUE4Parse.Compression;
using Ionic.Zlib;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.BinData.Definitions;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Properties;

namespace Xylia.Preview.Data.Engine.Definitions;
public abstract class DatafileDefinition : Collection<TableDefinition>
{
	#region Properties
	public string Key { get; set; }

	/// <summary>
	/// for parse type
	/// </summary>
	public FileInfo Header { get; set; }
	#endregion

	#region Methods
	private Dictionary<string, TableDefinition> _definitionsByName;
	private Dictionary<ushort, TableDefinition> _definitionsByType;

	public TableDefinition this[ushort index]
	{
		get
		{
			if (_definitionsByType is null) CreateMap();

			return _definitionsByType.GetValueOrDefault(index, null);
		}
	}

	public TableDefinition this[string index]
	{
		get
		{
			if (_definitionsByName is null) CreateMap();

			return _definitionsByName.GetValueOrDefault(index, null);
		}
	}

	protected new void Add(TableDefinition item)
	{
		if (item is null) return;

		// HACK: is not binary table, it affects GetParser
		if (item.Name is "filter-set" or "party-battle-field-zone-time-effect") return;

		base.Add(item);
	}

	internal void CreateMap()
	{
		// this.DistinctBy(def => def.Name, new TableNameComparer());

		_definitionsByType = this.ToDistinctDictionary(x => x.Type, null);
		_definitionsByName = this.ToDistinctDictionary(x => x.Name, new TableNameComparer());
	}

	internal ITypeParser GetParser(Datafile provider)
	{
		var defs = this.Where(x => x.Module != (long)TableModule.Server && x.Module != (long)TableModule.Engine);
		if (defs.Count() == provider.Tables.Max(x => x.Type)) return new DatafileDirect(defs);
		else if (Header != null && Header.Exists) return new DatafileDirect(Header);
		else return new DatafileDetect(provider, defs);
	}
	#endregion
}


internal class DefaultDatafileDefinition : DatafileDefinition
{
	public DefaultDatafileDefinition()
	{
		if (Settings.Default.UseUserDefinition)
		{
			var directory = new DirectoryInfo(Path.Combine(Settings.Default.OutputFolder, "definition"));
			if (!directory.Exists) throw new DirectoryNotFoundException("Missing definition folder!");

			Header = directory.GetFiles("head").FirstOrDefault();

			var loader = new SequenceDefinitionLoader();
			foreach (var file in directory.GetFiles("*.xml"))
			{
				this.Add(TableDefinition.LoadFrom(loader, File.OpenRead(file.FullName)));
			}
		}
		else
		{
			var assembly = Assembly.GetExecutingAssembly();
			var sequence = new SequenceDefinitionLoader();
			assembly.GetManifestResourceNames()
				.Where(name => name.StartsWith("Xylia.Preview.Data.Definition.Sequence"))
				.ForEach(name => sequence.LoadFrom(assembly.GetManifestResourceStream(name)));

			assembly.GetManifestResourceNames()
				.Where(name => name.StartsWith("Xylia.Preview.Data.Definition.") && !name.Contains(".Sequence"))
				.Select(name => TableDefinition.LoadFrom(sequence, assembly.GetManifestResourceStream(name)))
				.ForEach(this.Add);
		}
	}
}

public class CompressDatafileDefinition : DatafileDefinition
{
	public CompressDatafileDefinition(Stream source, CompressionMethod mode)
	{
		var loader = new SequenceDefinitionLoader();

		switch (mode)
		{
			case CompressionMethod.Gzip:
			{
				using var stream = new GZipStream(source, CompressionMode.Decompress);
				using var reader = new TarReader(stream);
				var entries = new List<TarEntry>();
				while (true)
				{
					var entry = reader.GetNextEntry(true);
					if (entry is null)
					{
						new StreamReader(stream).ReadToEnd();
						break;
					}

					entries.Add(entry);
				}

				// to load sequence first
				foreach (var entry in entries
					.OrderBy(x => !x.Name.Contains("/Sequence/"))
					.ThenBy(x => x.Name))
				{
					if (entry.EntryType == TarEntryType.RegularFile && entry.Name.EndsWith(".xml"))
					{
						if (entry.Name.Contains("/Sequence/")) loader.LoadFrom(entry.DataStream);
						else this.Add(TableDefinition.LoadFrom(loader, entry.DataStream));
					}
				}
			}
			break;

			default: throw new NotSupportedException();
		}
	}

	internal static CompressDatafileDefinition Load()
	{
		var key = Settings.Default.DefitionKey;
		if (key is null) return null;

		var path = Path.Combine(Settings.Default.OutputFolder, ".download", key);
		return new CompressDatafileDefinition(File.OpenRead(path), CompressionMethod.Gzip) { Key = key };
	}
}