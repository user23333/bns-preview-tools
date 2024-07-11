using System.Xml;
using Xylia.Preview.Data.Engine.BinData.Models;

namespace Xylia.Preview.Data.Engine.Definitions;
public class TableDefinition : TableHeader
{
	#region Fields
	/// <summary>
	/// element definitions
	/// </summary>
	public List<ElementDefinition> Els { get; internal set; } = [];

	public int MaxId { get; set; }

	public bool AutoKey { get; set; }

	public long Module { get; set; }
	#endregion

	#region Properties
	/// <summary>
	/// table search patterns 
	/// </summary>
	public string Pattern { get; set; }

	/// <summary>
	/// root element
	/// </summary>
	public ElementDefinition ElRoot => Els.FirstOrDefault();

	/// <summary>
	/// main element
	/// </summary>
	public ElementDefinition ElRecord { get; internal set; }

	/// <summary>
	/// Has record element
	/// </summary>
	public bool IsEmpty => ElRecord is null;

	/// <summary>
	/// Is default definition 
	/// </summary>
	internal bool IsDefault { get; private set; }
	#endregion

	#region Methods
	public override string ToString() => this.Name + $" type:{Type}  ver:{MajorVersion}.{MinorVersion} maxid:{MaxId} module:{Module}";

	public byte[] WriteXml()
	{
		using var ms = new MemoryStream();
		using var writer = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true, IndentChars = "\t" });

		writer.WriteStartDocument();
		writer.WriteStartElement("table");
		writer.WriteAttributeString("name", Name);
		writer.WriteAttributeString("version", MajorVersion + "." + MinorVersion);
		writer.WriteAttributeString("module", Module.ToString());
		if (MaxId != 0) writer.WriteAttributeString("maxid", MaxId.ToString());

		foreach (var el in Els)
		{
			writer.WriteStartElement("el");
			writer.WriteAttributeString("name", el.Name);
			writer.WriteAttributeString("child", string.Join(",", el.Children.Select(x => x.Name)));

			el.Attributes.ForEach(attribute => attribute.WriteXml(writer));

			foreach (var sub in el.Subtables)
			{
				writer.WriteStartElement("sub");
				writer.WriteAttributeString("name", sub.Name);
				sub.Attributes.ForEach(attribute => attribute.WriteXml(writer));

				writer.WriteEndElement();
			}


			writer.WriteEndElement();
		}

		writer.WriteEndElement();
		writer.WriteEndDocument();
		writer.Flush();
		return ms.ToArray();
	}

	/// <summary>
	/// create default <see cref="TableDefinition"/> if not found
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static TableDefinition CreateDefault(ushort type)
	{
		var definition = new TableDefinition()
		{
			IsDefault = true,
			Type = type,
			Name = type.ToString(),
			Pattern = type + "Data.xml"
		};

		var elRoot = new ElementDefinition() { Name = "table" };
		var elRecord = new ElementDefinition { Name = "record", Size = 8 };

		elRoot.Children.Add(elRecord);

		definition.Els.Add(elRoot);
		definition.Els.Add(definition.ElRecord = elRecord);

		return definition;
	}
	#endregion
}