using System.Diagnostics;
using System.Xml;
using Xylia.Preview.Common.Exceptions;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Engine.Definitions;
[DebuggerDisplay("{Name}  type:{Type} ver:{MajorVersion}.{MinorVersion} module:{Module}")]
public class TableDefinition : TableHeader
{
	#region Fields
	public int MaxId { get; set; }
	public bool AutoKey { get; set; }
	public long Module { get; set; }

	/// <summary>
	/// element definitions
	/// </summary>
	public List<ElementDefinition> Elements { get; internal set; } = [];

	public ElementDefinition DocumentElement => Elements[0];

	/// <summary>
	/// table search patterns 
	/// </summary>
	internal string Pattern { get; set; }

	/// <summary>
	/// Is default definition 
	/// </summary>
	internal bool IsDefault { get; private set; }
	#endregion

	#region Methods
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
			Pattern = type + "Data.xml",
		};

		var element0 = new ElementDefinition() { Name = "table" };
		var element1 = new ElementDefinition { Name = "record", Size = 8 };
		element0.Children.Add(element1);

		definition.Elements.Add(element0);
		definition.Elements.Add(element1);
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
			throw BnsDataException.InvalidDefinition("field `type` or `name` is required!");

		var autokey = tableNode.GetAttribute<bool>("autokey");
		var maxid = tableNode.GetAttribute<int>("maxid");
		var version = BnsVersion.Parse(tableNode.GetAttribute("version"));
		var module = tableNode.GetAttribute<long>("module");
		var pattern = tableNode.Attributes["pattern"]?.Value ?? $"{name.TitleCase()}Data*.xml";
		#endregion

		#region elements
		List<ElementDefinition> elements = [];
		foreach (var el in tableNode.SelectNodes("./el").OfType<XmlElement>())
		{
			elements.Add(new ElementDefinition { Name = el.GetAttribute("name") });
		}

		foreach (var el in elements)
		{
			var source = (XmlElement)tableNode.SelectSingleNode($"./el[@name='{el.Name}']");

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

			// Add auto-id key
			if (!el.Attributes.Any(attribute => attribute.IsKey))
			{
				var autoIdAttr = new AttributeDefinition
				{
					Name = AttributeCollection.s_autoid,
					Type = AttributeType.TInt64,
					Offset = 8,
					Repeat = 1,
					IsKey = true,
					IsHidden = true,
					Min = 0,
					Max = long.MaxValue,
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
					Sequence = [.. subs.Select(x => x.GetAttribute("name"))],
					ReferedTableName = name,
					ReferedEl = 1,
					ReferedElement = el.Name,
					IsHidden = true,
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

				subtable.Name = sub.GetAttribute("name");
				subtable.SubclassType = subIndex++;
				subtable.Children = el.Children;
				subtable.Attributes.AddRange(el.Attributes);
				subtable.ExpandedAttributes.AddRange(el.ExpandedAttributes);
	
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
			var children = source.GetAttribute("child").Split(',').Select(o => o.Trim());
			if (children != null)
			{
				foreach (var child in children)
				{
					ElementDefinition child_el = ushort.TryParse(child, out var index) ?
						elements.ElementAtOrDefault(index) :
						elements.FirstOrDefault(el => el.Name == child);

					if (child_el != null) el.Children.Add(child_el);
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
			MajorVersion = version.Major,
			MinorVersion = version.Minor,
			Elements = elements,
		};
	}
	#endregion
}