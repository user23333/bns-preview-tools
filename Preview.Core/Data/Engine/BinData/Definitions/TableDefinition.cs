using System.Diagnostics;
using System.Text;
using System.Xml;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Exceptions;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Engine.Definitions;
[DebuggerDisplay("{Name}  type:{Type} ver:{MajorVersion}.{MinorVersion} module:{Module}")]
public class TableDefinition : TableHeader
{
	#region Fields
	public int MaxId;
	public bool AutoKey;
	public long Module;

	/// <summary>
	/// table search patterns 
	/// </summary>
	public string Pattern;

	/// <summary>
	/// element definitions
	/// </summary>
	public List<ElementDefinition> Els { get; internal set; } = [];
	#endregion

	#region Properties
	/// <summary>
	/// root element
	/// </summary>
	internal ElementDefinition ElRoot => Els.FirstOrDefault();

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
	internal void SetChild(string el, params string[] child)
	{
		ElementDefinition Find(string name) => Els.Find(x => x.Name == name);

		Find(el).Children = [.. child.Select(Find)];
	}


	public byte[] WriteXml()
	{
		using var ms = new MemoryStream();
		using var writer = XmlWriter.Create(ms, new XmlWriterSettings() { Indent = true, IndentChars = "\t" , Encoding = new UTF8Encoding(false) });

		writer.WriteStartDocument();
		writer.WriteStartElement("table");
		writer.WriteAttributeString("name", Name);
		writer.WriteAttributeString("version", MajorVersion + "." + MinorVersion);
		writer.WriteAttributeString("module", Module.ToString());
		if (MaxId != 0) writer.WriteAttributeString("maxid", MaxId.ToString());
		if (Pattern != null) writer.WriteAttributeString("pattern", Pattern);

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
	internal static TableDefinition CreateDefault(ushort type)
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

	public static TableDefinition LoadFrom(SequenceDefinitionLoader loader, Stream stream)
	{
		var doc = new XmlDocument();
		doc.Load(stream);

		return LoadFrom(loader, doc.DocumentElement);
	}

	public static TableDefinition LoadFrom(SequenceDefinitionLoader loader, XmlElement tableNode)
	{
		#region table 
		var type = tableNode.GetAttribute<ushort>("type");
		var name = tableNode.GetAttribute<string>("name");
		if (type == 0 && string.IsNullOrWhiteSpace(name))
			throw BnsDataException.InvalidDefinition("`type` or `name` field is required in table!");

		var autokey = tableNode.GetAttribute<bool>("autokey");
		var maxid = tableNode.GetAttribute<int>("maxid");
		var version = ParseVersion(tableNode.GetAttribute("version"));
		var module = tableNode.GetAttribute<long>("module");
		var pattern = tableNode.Attributes["pattern"]?.Value ?? $"{name.TitleCase()}Data*.xml";
		#endregion

		#region els
		List<ElementDefinition> els = [];
		foreach (var source in tableNode.SelectNodes("./el").OfType<XmlElement>())
		{
			var el = new ElementDefinition { Name = source.GetAttribute("name") };
			els.Add(el);

			// HACK: is record element
			if (els.Count == 2)
			{
				// el.AutoKey = autokey;
				el.MaxId = maxid;
			}
		}

		foreach (var el in els)
		{
			var source = (XmlElement)tableNode.SelectSingleNode($"./el[@name='{el.Name}']");
			var Inherit = source.GetAttribute<bool>("inherit");
			if (Inherit)
			{
				// TODO
				continue;
			}


			#region body
			foreach (var attrDef in source.ChildNodes.OfType<XmlElement>()
				.Where(e => e.Name == "attribute")
				.Select(e => AttributeDefinition.LoadFrom(e, loader)))
			{
				if (attrDef.IsDeprecated) continue;

				el.Attributes.Add(attrDef);

				// Expand repeated attributes if needed
				if (attrDef.Repeat == 1)
				{
					el.ExpandedAttributes.Add(attrDef);
					continue;
				}

				for (var i = 1; i <= attrDef.Repeat; i++)
				{
					var newAttrDef = attrDef.Clone();
					newAttrDef.Name += $"-{i}";
					newAttrDef.Repeat = 1;
					el.ExpandedAttributes.Add(newAttrDef);
				}
			}

			// Add auto key id
			if (!el.Attributes.Any(attribute => attribute.IsKey))
			{
				var autoIdAttr = new AttributeDefinition
				{
					Name = AttributeCollection.s_autoid,
					Type = AttributeType.TInt64,
					IsKey = true,
					IsHidden = true,
					Offset = 8,
					Repeat = 1,
					CanInput = false,
				};

				el.AutoKey = true;
				el.Attributes.Insert(0, autoIdAttr);
				el.ExpandedAttributes.Insert(0, autoIdAttr);
			}

			// Add type key
			var subs = source.ChildNodes.OfType<XmlElement>().Where(e => e.Name == "sub");
			if (subs.Any())
			{
				var typeAttr = new AttributeDefinition
				{
					Name = AttributeCollection.s_type,
					Type = AttributeType.TSub,
					Offset = 2,
					Repeat = 1,
					ReferedTableName = name,
					ReferedElement = el.Name,
				};

				el.Attributes.Insert(0, typeAttr);
				el.ExpandedAttributes.Insert(0, typeAttr);
			}

			el.RefreshSize(el.ExpandedAttributes, true);
			el.CreateAttributeMap();
			#endregion

			#region sub
			short subIndex = 0;
			foreach (var sub in subs)
			{
				var subtable = new ElementSubDefinition();
				el.Subtables.Add(subtable);

				subtable.Name = sub.Attributes["name"].Value;
				subtable.SubclassType = subIndex++;

				// Add parent attributes
				subtable.Attributes.AddRange(el.Attributes);
				subtable.ExpandedAttributes.AddRange(el.ExpandedAttributes);
				subtable.Children.AddRange(el.Children);

				foreach (var attrDef in sub.ChildNodes.OfType<XmlElement>()
					.Select(e => AttributeDefinition.LoadFrom(e, loader)))
				{
					if (attrDef.IsDeprecated) continue;

					// HACK: Handle case when there's name conflict in subtable
					if (el.Attributes.Any(x => x.Name == attrDef.Name))
					{
						attrDef.Name += "-rep";
					}

					subtable.Attributes.Add(attrDef);

					// Expand repeated attributes if needed
					if (attrDef.Repeat == 1)
					{
						subtable.ExpandedAttributes.Add(attrDef);
						subtable.ExpandedAttributesSubOnly.Add(attrDef);
						continue;
					}

					for (var i = 1; i <= attrDef.Repeat; i++)
					{
						var newAttrDef = attrDef.Clone();
						newAttrDef.Name += $"-{i}";
						newAttrDef.Repeat = 1;
						subtable.ExpandedAttributes.Add(newAttrDef);
						subtable.ExpandedAttributesSubOnly.Add(newAttrDef);
					}
				}

				subtable.RefreshSize(subtable.ExpandedAttributesSubOnly, true, el.Size);
				subtable.CreateAttributeMap();
			}

			el.CreateSubtableMap();
			#endregion

			#region children
			var children = source.Attributes["child"]?.Value.Split(',').Select(o => o.Trim());
			if (children != null)
			{
				foreach (var child in children)
				{
					ElementDefinition child_el = ushort.TryParse(child, out var index) ?
						els.ElementAtOrDefault(index) :
						els.FirstOrDefault(el => el.Name == child);

					if (child_el != null)
						el.Children.Add(child_el);
				}
			}
			#endregion
		}
		#endregion

		return new TableDefinition
		{
			Name = name,
			Type = type,
			Pattern = pattern,
			Module = module,
			MajorVersion = version.Item1,
			MinorVersion = version.Item2,

			Els = els,
			ElRecord = els.FirstOrDefault().Children.FirstOrDefault(),
		};
	}
	#endregion
}