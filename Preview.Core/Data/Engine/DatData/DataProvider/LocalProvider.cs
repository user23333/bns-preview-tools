using System.Xml;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Engine.BinData.Serialization;
using Xylia.Preview.Data.Engine.Definitions;

namespace Xylia.Preview.Data.Engine.DatData;
public class LocalProvider(string source) : DefaultProvider
{
	#region Properties
	/// <summary>
	/// Check source is dat file
	/// </summary>
	public bool CanSave { get; protected set; }

	/// <summary>
	/// Determeting whether overwrite when already have backup
	/// </summary>
	public bool HaveBackup { get; set; }

	public Table TextTable => this.Tables["text"];
	#endregion

	#region Override Methods
	public override string Name => Path.GetFileName(source);

	public override Stream[] GetFiles(string pattern) => [File.OpenRead(pattern)];

	public override void LoadData(DatafileDefinition definitions)
	{
		this.Tables = [];
		this.CanSave = this.HaveBackup = false;
		if (string.IsNullOrWhiteSpace(source)) return;

		var ext = Path.GetExtension(source);
		switch (ext)
		{
			case ".xml" or ".x16":
			{
				var definition = definitions["text"];
				definition.Pattern = source;

				Tables.Add(new Table() { Owner = this, Name = "text", Definition = definition });
				break;
			}

			case ".dat":
			{
				this.CanSave = true;

				LocalData = new FileInfo(source);
				Is64Bit = LocalData.Bit64;
				ReadFrom(LocalData.SearchFiles(PATH.Localfile(Is64Bit)).FirstOrDefault()?.Data, Is64Bit);

				// detect text table type
				Parser = definitions.GetParser(this);
				Parser.Parse(definitions);
				break;
			}
		}
	}
	#endregion

	#region Public Methods
	/// <summary>
	/// Replace existed text.
	/// </summary>
	/// <param name="table">text table</param>
	/// <param name="files">.x16 file path</param>
	public static void ReplaceText(Table table, params FileInfo[] files)
	{
		ArgumentNullException.ThrowIfNull(table);

		foreach (var file in files)
		{
			XmlDocument xml = new() { PreserveWhitespace = true };
			xml.Load(file.FullName);

			foreach (XmlElement element in xml.DocumentElement!.SelectNodes($"./" + table.Definition.ElRecord.Name)!)
			{
				var alias = element.Attributes["alias"]?.Value;
				var text = element.InnerXml;

				var record = table[alias];
				if (record != null) record.Attributes["text"] = text;
			}
		}
	}

	/// <summary>
	/// [Test] Replace text to alias.
	/// </summary>
	/// <param name="table"></param>
	public static void ReplaceText(Table table)
	{
		foreach (var record in table)
		{
			var alias = record.Attributes.Get<string>("alias");
			if (alias != null) record.Attributes["text"] = alias;
		}
	}


	/// <summary>
	/// Save as dat
	/// </summary>
	/// <remarks>
	/// <see langword="Source"/> must be a dat file
	/// </remarks>
	/// <param name="text"></param>
	public void Save(byte[] data)
	{
		var table = TextTable;
		ArgumentNullException.ThrowIfNull(table);

		using var stream = new MemoryStream(data);
		table.LoadXml(stream).ForEach(a => a.Invoke());

		WriteData(source, new RebuildSettings() { Is64bit = Is64Bit, Mode = Mode.Package });
	}

	public override void WriteData(string folder, RebuildSettings settings)
	{
		var replaces = new Dictionary<string, byte[]>
		{
			{ PATH.Localfile(Is64Bit), WriteTo([.. Tables], settings.Is64bit) }
		};

		var param = new PackageParam(folder, settings.Is64bit);
		ThirdSupport.Pack(param, replaces, !HaveBackup);

		HaveBackup = true;
	}
	#endregion
}