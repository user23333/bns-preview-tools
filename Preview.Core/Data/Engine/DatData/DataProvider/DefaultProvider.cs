using System.ComponentModel;
using System.Diagnostics;
using CUE4Parse.Utils;
using Serilog;
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
	internal ITypeParser Parser { get; set; }
	#endregion

	#region Methods		
	public virtual string Name { get; protected set; }

	public Locale Locale { get; protected set; }

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
		ReadFrom(XmlData.SearchFiles(PATH.Datafile(Is64Bit)).FirstOrDefault()?.Data, Is64Bit);
		ReadFrom(LocalData?.SearchFiles(PATH.Localfile(Is64Bit)).FirstOrDefault()?.Data, Is64Bit);
		#endregion

		Parser = definitions.GetParser(this);
		Parser.Parse(definitions);
	}

	public virtual void WriteData(string folder, PublishSettings settings)
	{
		#region Rebuild alias map  
		if (settings.RebuildAliasMap)
		{
			// If not complete definition, read the raw AliasMap
			var units = AliasTableUnit.Split(Tables.Any(x => x.Definition.IsDefault) ? AliasTable : null);

			AliasTable = new AliasTable();

			Log.Information("Rebuilding alias map");
			var haveAlias = new HashSet<string>();

			// get alias
			foreach (var table in this.Tables)
			{
				var aliasAttrDef = table.Definition.ElRecord["alias"];
				if (aliasAttrDef == null || table.Archive != null) continue;

				var tableDefName = table.Name.ToLowerInvariant();
				haveAlias.Add(tableDefName);

				foreach (var record in table.Records)
				{
					var alias = record.Attributes.Get<string>("alias");
					if (alias == null) continue;

					AliasTable.Add(record.PrimaryKey, AliasTable.MakeKey(tableDefName, alias));
				}
			}

			// If not complete definition, read the raw AliasMap
			if (units != null)
			{
				foreach (var table in units)
				{
					if (haveAlias.Contains(table.Name)) continue;

					foreach (var record in table.Records)
						AliasTable.Add(record.Key, AliasTable.MakeKey(table.Name, record.Value));
				}
			}

			var temp = new AliasTableBuilder(AliasTable).EndRebuilding();
			AliasTable = temp;
			AliasCount = temp.Entries.Count;
		}
		#endregion


		// Due to incomplete definition, local may missing table
		// UserCommand remove at UE4
		var raw = this.Tables.Where(x => !x.IsBinary);
		var local = this.Tables.Where(x => x.Name == "petition-faq-list" || x.Name == "survey" || x.Name == "text" || (false && x.Name == "user-command"));
		var xml = this.Tables.Except(local).Except(raw);

		// write mode
		if (settings.Mode == Mode.Datafile)
		{
			File.WriteAllBytes(Path.Combine(folder, PATH.Datafile(Is64Bit)), WriteTo([.. xml], settings.Is64bit));
			File.WriteAllBytes(Path.Combine(folder, PATH.Localfile(Is64Bit)), WriteTo([.. local], settings.Is64bit));
		}
		else if (settings.Mode == Mode.Package)
		{
			XmlData.Add(PATH.Datafile(Is64Bit), WriteTo([.. xml], settings.Is64bit));
			XmlData.Write(settings.Is64bit, CompressionLevel.Normal);

			LocalData.Add(PATH.Localfile(Is64Bit), WriteTo([.. local], settings.Is64bit));
		}
		else if (settings.Mode == Mode.PackageThird)
		{
			var replaces = new Dictionary<string, byte[]>
			{
				{ PATH.Datafile(Is64Bit), WriteTo([.. xml], settings.Is64bit) }
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


	#region Constructors
	public DefaultProvider()
	{

	}

	public DefaultProvider(BNSDat xml, BNSDat local, BNSDat config = null)
	{
		this.XmlData = xml;
		this.LocalData = local;
		this.ConfigData = config;
	}

	public static DefaultProvider Load(string GameFolder, IDatSelect selector = default, ResultMode mode = ResultMode.SelectDat)
	{
		if (string.IsNullOrWhiteSpace(GameFolder) || !Directory.Exists(GameFolder))
			throw new WarningException("You must set game folder.");

		// get all
		var datas = new DataCollection(GameFolder);
		var xmls = datas.GetFiles(DatType.Xml, mode);
		var locals = datas.GetFiles(DatType.Local, mode);
		var configs = datas.GetFiles(DatType.Config, mode);

		Debug.Assert(selector != null || xmls.Count <= 1, "please set a dat selector.");

		// get target
		DefaultProvider provider;
		if (xmls.Count == 0) throw new WarningException("invalid game data, maybe specified incorrect directory.");
		else if (selector is null || (xmls.Count == 1 && locals.Count <= 1)) provider = new DefaultProvider(xmls.FirstOrDefault(), locals.FirstOrDefault());
		else provider = selector.Show(xmls, locals);

		// return information
		provider.Name = GameFolder.SubstringAfterLast('\\');
		provider.Is64Bit = provider.XmlData.Bit64;
		provider.Locale = Locale.Current = new Locale(GameFolder);
		provider.ConfigData = configs.FirstOrDefault();

		return provider;
	}
	#endregion
}

public interface IDatSelect
{
	DefaultProvider Show(IEnumerable<FileInfo> Xml, IEnumerable<FileInfo> Local);
}