using System.Collections;
using System.Xml;
using Serilog;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Engine.Definitions;

namespace Xylia.Preview.Data.Models;
/// <summary>
/// attributes of data record 
/// </summary>
public class AttributeCollection : IEnumerable<AttributeValue>
{
	#region Constructors
	internal AttributeCollection(Record record)
	{
		this.record = record;
	}

	internal AttributeCollection(Record record, XmlElement element, ElementDefinition definition, int index = -1) : this(record)
	{
		#region attribute
		foreach (XmlAttribute item in element.Attributes)
		{
			var name = item.Name;
			attributes[name] = item.Value;
		}

		// Native
		if (!string.IsNullOrEmpty(element.InnerXml))
		{
			var attr = definition.Attributes.FirstOrDefault(a => a.Type == AttributeType.TNative);
			if (attr != null) attributes[attr.Name] = element.InnerXml;
		}

		attributes[s_autoid] = index;
		#endregion

		#region children
		var provider = record.Owner.Owner;
		foreach (var child in definition.Children)
		{
			var table = new Table() { Owner = provider, Definition = new TableDefinition() { ElRecord = child, Name = child.Name } };
			table.LoadElement(element, null);

			record.Children[child.Name] = [.. table.Records];
		}
		#endregion
	}
	#endregion

	#region Fields
	internal const string s_autoid = "auto-id";
	internal const string s_type = "type";

	/// <summary>
	/// Owner element
	/// </summary>
	protected readonly Record record;

	/// <summary>
	/// for xml element
	/// </summary>
	protected readonly Dictionary<string, object> attributes = [];
	#endregion


	#region Public Methods
	public object this[string name] { get => Get(name, out _); set => Set(name, value); }

	public object this[AttributeDefinition key] { get => Get(key.Name, out _); set => Set(key, value); }

	public void CheckAttribute(params string[] attrNames)
	{
		foreach (string text in attrNames)
		{
			if (this[text] == null)
			{
				throw new Exception(string.Format("{0} Attribute is Required Field", text));
			}
		}
	}

	internal void BuildData(bool isKey = false)
	{
		// convert to binary
		void SetData(AttributeDefinition attribute) => record.Attributes.Set(attribute, record.Attributes.Get(attribute));

		// implement IGameDataKeyParser
		if (isKey)
		{
			record.Definition.ExpandedAttributes.Where(attr => attr.IsKey).ForEach(SetData);
		}
		else
		{
			// create data
			record.Definition.ExpandedAttributes.ForEach(SetData);
			attributes.Clear();
		}
	}


	// Getters
	public bool TryGetValue(string name, out KeyValuePair<AttributeDefinition, object> pair)
	{
		var value = Get(name, out var definition);
		pair = new(definition, value);

		return definition != null || value != null;
	}

	public object Get(string name, out AttributeDefinition attribute)
	{
		// get expand attribute
		attribute = record.Definition[name];

		if (attribute is null)
		{
			// get base attribute
			var attributeBase = record.Definition.GetAttribute(name);
			if (attributeBase != null)
			{
				var value = Array.CreateInstance(typeof(object), attributeBase.Repeat);
				for (int i = 0; i < value.Length; i++) value.SetValue(Get(attributeBase.Expands[i]), i);

				return value;
			}
			// create if xelement 
			else if (attributes.ContainsKey(name))
			{
				attribute = new AttributeDefinition() { Name = name, Type = AttributeType.TString };
			}
		}

		return Get(attribute);
	}

	public object Get(AttributeDefinition attribute)
	{
		if (attribute is null) return null;

		// from source
		if (attributes.Count != 0)
		{
			var value = attributes.GetValueOrDefault(attribute.Name, attribute.DefaultValue);
			if (value is string s) value = AttributeConverter.ConvertBack(s, attribute, record.Owner.Owner);

			return value;
		}

		// from binary 
		return AttributeConverter.ConvertTo(record, attribute, record.Owner.Owner);
	}

	public T Get<T>(string name)
	{
		return Get(name, out _).To<T>();
	}


	// Setters
	public void Set(string name, object value)
	{
		var attribute = record?.Definition[name];
		if (attribute is null) return;

		if (value is string s)
			value = AttributeConverter.ConvertBack(s, attribute, record.Owner.Owner);

		Set(attribute, value);
		return;
	}

	public void Set(AttributeDefinition attribute, object value)
	{
		// skip virtual attribute
		if (attribute.Name == s_type) return;

		switch (attribute.Type)
		{
			case AttributeType.TSeq:
			case AttributeType.TProp_seq:
			{
				var seqIndex = (sbyte)attribute.Sequence.IndexOf((string)value);
				if (seqIndex == -1)
				{
					Log.Error($"Invalid sequence, name: '{attribute.Name}' value: '{value}'");
					seqIndex = 0;
				}

				value = seqIndex;
				break;
			}

			case AttributeType.TSeq16:
			case AttributeType.TProp_field:
			{
				var seqIndex = (short)attribute.Sequence.IndexOf((string)value);
				if (seqIndex == -1)
				{
					Log.Error($"Invalid sequence, name: '{attribute.Name}' value: '{value}'");
					seqIndex = 0;
				}

				value = seqIndex;
				break;
			}

			case AttributeType.TRef:
			{
				var record = value as Record;
				value = record?.PrimaryKey ?? default;
				break;
			}
			case AttributeType.TTRef:
			{
				var record = value as Record;
				value = new TRef(record);
				break;
			}
			case AttributeType.TIcon:
			{
				var icon = value as Icon;
				value = icon?.GetRef() ?? default;
				break;
			}

			case AttributeType.TString:
			{
				value = record.StringLookup.AppendString((string)value, out _);
				break;
			}
			case AttributeType.TNative:
			{
				var offset = record.StringLookup.AppendString((string)value, out var size);
				value = new Native(size, offset);
				break;
			}
			case AttributeType.TXUnknown2:
			{
				value = record.StringLookup.AppendString(((ObjectPath)value).Path, out _);
				break;
			}
		}

		// set data
		record.Data.Set(attribute.Offset, value);
	}
	#endregion

	#region IEnumerable
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	public IEnumerator<AttributeValue> GetEnumerator()
	{
		if (attributes.Count != 0)
		{
			foreach (var attribute in attributes)
			{
				var value = attribute.Value;
				var definition = record?.Definition?[attribute.Key];

				// convert type
				if (value is string s && definition != null)
				{
					value = AttributeConverter.ConvertBack(s, definition, record.Owner.Owner);
				}

				// virtual definition, ensure the name can be getted
				definition ??= new AttributeDefinition() { Name = attribute.Key, Type = AttributeType.TString };
				yield return new AttributeValue(definition, value);
			}
		}
		else
		{
			foreach (var definition in record.Definition.ExpandedAttributesSorted)
			{
				var value = AttributeConverter.ConvertTo(record, definition, record.Owner.Owner);
				yield return new AttributeValue(definition, value);
			}
		}
	}

	public override string ToString() => this.Aggregate($"<{record.Name} ", (sum, now) => sum + $"{now.Name}=\"{now}\" ", result => result + "/>");
	#endregion
}