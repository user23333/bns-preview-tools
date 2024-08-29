using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using CUE4Parse.Compression;
using CUE4Parse.Utils;
using ICSharpCode.SharpZipLib.Tar;
using Ionic.Zlib;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Properties;

namespace Xylia.Preview.Data.Engine.Definitions;
public abstract class DatafileDefinition : Collection<TableDefinition>
{
	/// <summary>
	/// for parse type
	/// </summary>
	public FileInfo Header = null;

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

		base.Add(item);
	}

	internal void CreateMap()
	{
		// this.DistinctBy(def => def.Name, new TableNameComparer());

		_definitionsByType = this.ToDistinctDictionary(x => x.Type, null);
		_definitionsByName = this.ToDistinctDictionary(x => x.Name, new TableNameComparer());
	}

	public ITypeParser GetParser(Datafile provider)
	{
		// Actually, it is directly defined in the game program, but we cannot get it.
		if (Header != null && Header.Exists) return new DatafileDirect(Header);
		else return new DatafileDetect(provider, this);
	}
}


internal class DefaultDatafileDefinition : DatafileDefinition
{
	public DefaultDatafileDefinition()
	{
		if (Settings.Default.UseUserDefinition)
		{
			var UserDefs = new DirectoryInfo(Path.Combine(Settings.Default.OutputFolder, "definition"));
			if (!UserDefs.Exists) throw new DirectoryNotFoundException("Missing definition folder");

			Header = UserDefs.GetFiles("head").FirstOrDefault();

			var loader = new SequenceDefinitionLoader();
			foreach (var file in UserDefs.GetFiles("*.xml"))
			{
				this.Add(TableDefinition.LoadFrom(loader, File.ReadAllText(file.FullName)));
			}
		}
		else
		{
			var assembly = Assembly.GetExecutingAssembly();
			var sequence = new SequenceDefinitionLoader();
			assembly.GetManifestResourceNames()
				.Where(name => name.StartsWith("Xylia.Preview.Data.Definition.Sequence"))
				.Select(name => new StreamReader(assembly.GetManifestResourceStream(name)).ReadToEnd())
				.ForEach(sequence.LoadFrom);

			assembly.GetManifestResourceNames()
				.Where(name => name.StartsWith("Xylia.Preview.Data.Definition.") && !name.Contains(".Sequence"))
				.Select(name => new StreamReader(assembly.GetManifestResourceStream(name)).ReadToEnd())
				.Select(res => TableDefinition.LoadFrom(sequence, res))
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
				using var tar = new TarInputStream(stream, Encoding.UTF8);

				string root = tar.GetNextEntry().Name;  //root folder entry
				while (true)
				{
					var entry = tar.GetNextEntry();
					if (entry is null) break;

					byte[] buffer = new byte[entry.Size];
					tar.Read(buffer, 0, buffer.Length);

					var name = entry.Name.SubstringAfter(root);
					if (name.EndsWith('/')) continue;
					else if (name.StartsWith("Sequence/"))
					{
						loader.LoadFrom(Encoding.UTF8.GetString(buffer));
					}
					else
					{
						this.Add(TableDefinition.LoadFrom(loader, Encoding.UTF8.GetString(buffer)));
					}
				}
			}
			break;

			default: throw new NotSupportedException();
		}
	}
}