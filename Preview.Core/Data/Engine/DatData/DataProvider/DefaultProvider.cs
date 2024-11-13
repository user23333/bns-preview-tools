using System.ComponentModel;
using System.Diagnostics;
using CUE4Parse.Utils;
using Serilog;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Engine.BinData.Serialization;
using Xylia.Preview.Data.Engine.Definitions;

namespace Xylia.Preview.Data.Engine.DatData;
public class DefaultProvider : Datafile, IDataProvider
{
	#region Fields
	public BNSDat XmlData { get; protected set; }
	protected BNSDat LocalData { get; set; }
	protected BNSDat ConfigData { get; set; }

	internal ITypeParser Parser;
	#endregion

	#region Constructors
	protected DefaultProvider()
	{

	}

	public DefaultProvider(BNSDat xml, BNSDat local, BNSDat config = null)
	{
		this.XmlData = xml;
		this.LocalData = local;
		this.ConfigData = config;
	}
	#endregion

	#region IDataProvider	
	public virtual string Name { get; protected set; }

	public Locale Locale { get; protected set; } = new();

	public virtual Stream[] GetFiles(string pattern)
	{
		return (XmlData.SearchFiles(pattern) ?? []).Concat(
			ConfigData?.SearchFiles(pattern) ?? []).Concat(
			LocalData?.SearchFiles(pattern) ?? []).Select(x =>
			new MemoryStream(x.Data)).ToArray();
	}

	public virtual void LoadData(DatafileDefinition definitions)
	{
		#region Tables        
		// xml table
		Tables = [];
		Tables.Add(new() { Name = "contextscript" });
		Tables.Add(new() { Name = "quest" });
		Tables.Add(new() { Name = "questrewardskill3" });
		Tables.Add(new() { Name = "skill-training-sequence" });
		Tables.Add(new() { Name = "summoned-sequence" });
		Tables.Add(new() { Name = "tutorialskillsequence" });
		Tables.Add(new() { Name = "surveyquestions" });

		// binary table
		ReadFrom(XmlData.SearchFiles(DataSearch.Datafile(Is64Bit)).FirstOrDefault()?.Data, Is64Bit);
		ReadFrom(LocalData?.SearchFiles(DataSearch.Localfile(Is64Bit)).FirstOrDefault()?.Data, Is64Bit);
		#endregion

		Parser = definitions.GetParser(this);
		Parser.Parse(definitions);
	}

	public virtual void WriteData(string folder, RebuildSettings settings)
	{
		#region Rebuild alias map  
		if (settings.RebuildAliasMap)
		{
			Log.Information("Rebuilding alias map");
			var aliasTable = new AliasTable();
			var haveAlias = new HashSet<string>();

			// create alias	map
			foreach (var table in Tables)
			{
				var aliasAttrDef = table.Definition.ElRecord["alias"];
				if (aliasAttrDef == null || table.Archive != null) continue;

				var tableDefName = table.Name.ToLowerInvariant();
				haveAlias.Add(tableDefName);

				table.Records.ForEach(aliasTable.Add);
			}

			// If incomplete definition, read the raw map
			if (Tables.Any(x => x.Definition.IsDefault))
			{
				foreach (var table in AliasTableUnit.Split(AliasTable))
				{
					if (haveAlias.Contains(table.Name)) continue;

					table.Records.ForEach(x => aliasTable.Add(x.Key, AliasTable.MakeKey(table.Name, x.Value)));
				}
			}

			// build
			AliasTable = new AliasTableBuilder(aliasTable).EndRebuilding();
			AliasCount = AliasTable.Count;
		}
		#endregion

		// UserCommand move to datafile after UE4
		var raw = Tables.Where(x => !x.IsBinary);
		var local = Tables.Where(x => x.Name == "petition-faq-list" || x.Name == "survey" || x.Name == "text" || (false && x.Name == "user-command"));
		var xml = Tables.Except(local).Except(raw);

		// write mode
		if (settings.Mode == Mode.Datafile)
		{
			File.WriteAllBytes(Path.Combine(folder, DataSearch.Datafile(Is64Bit)), WriteTo([.. xml], settings.Is64bit));
			File.WriteAllBytes(Path.Combine(folder, DataSearch.Localfile(Is64Bit)), WriteTo([.. local], settings.Is64bit));
		}
		else if (settings.Mode == Mode.Package)
		{
			XmlData.Add(DataSearch.Datafile(Is64Bit), WriteTo([.. xml], settings.Is64bit));
			XmlData.Write(settings.Is64bit, CompressionLevel.Normal);

			LocalData.Add(DataSearch.Localfile(Is64Bit), WriteTo([.. local], settings.Is64bit));
		}
		else if (settings.Mode == Mode.PackageThird)
		{
			var replaces = new Dictionary<string, byte[]>
			{
				{ DataSearch.Datafile(Is64Bit), WriteTo([.. xml], settings.Is64bit) }
			};

			ThirdSupport.Pack(XmlData.Params, replaces);
		}
	}

	public virtual void Dispose()
	{
		XmlData?.Dispose();
		LocalData?.Dispose();
		ConfigData?.Dispose();
		XmlData = LocalData = ConfigData = null;

		AliasTable?.Clear();
		AliasTable = null;
		Tables?.Clear();
		Tables = null;

		GC.SuppressFinalize(this);
		GC.Collect();
	}
	#endregion

	#region Methods
	public static DefaultProvider Load(string folder, IDatSelect selector = default, string pattern = "*.dat")
	{
		var searcher = new DataSearch(folder, pattern);
		var xmls = searcher.Get(DataSearch.DatType.Xml);
		var locals = searcher.Get(DataSearch.DatType.Local);
		var configs = searcher.Get(DataSearch.DatType.Config);

		Debug.Assert(selector != null || xmls.Count() <= 1, "Please set a dat selector.");

		// get target
		DefaultProvider provider;
		if (!xmls.Any()) throw new WarningException("invalid game data, maybe specified incorrect directory.");
		else if (selector is null || (xmls.Count() == 1 && locals.Count() <= 1)) provider = new DefaultProvider(xmls.FirstOrDefault(), locals.FirstOrDefault());
		else provider = selector.Show(xmls, locals, searcher.Locale);

		// return information
		provider.Name = folder.SubstringAfterLast('\\');
		provider.Is64Bit = provider.XmlData.Bit64;
		provider.Locale = Locale.Current = searcher.Locale;
		provider.ConfigData = configs.FirstOrDefault();

		return provider;
	}
	#endregion
}

public interface IDatSelect
{
	DefaultProvider Show(IEnumerable<FileInfo> xmls, IEnumerable<FileInfo> locals, Locale locale);
}