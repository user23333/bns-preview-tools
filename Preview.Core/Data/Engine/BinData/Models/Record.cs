using System.Xml;
using Newtonsoft.Json;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Engine.BinData.Models;
using Xylia.Preview.Data.Engine.Definitions;

namespace Xylia.Preview.Data.Models;
[JsonConverter(typeof(RecordConverter))]
public sealed unsafe class Record : IElement, IDisposable
{
	#region Constructors
	internal Record()
	{
		Attributes = new(this);
	}
	#endregion

	#region Fields
	public ElementType ElementType
	{
		get
		{
			fixed (byte* ptr = Data) return (ElementType)ptr[0];
		}
		set
		{
			fixed (byte* ptr = Data) ptr[0] = (byte)value;
		}
	}

	public short SubclassType
	{
		get
		{
			fixed (byte* ptr = Data) return ((short*)(ptr + 2))[0];
		}
		set
		{
			fixed (byte* ptr = Data) ((short*)(ptr + 2))[0] = value;
		}
	}

	public ushort DataSize
	{
		get
		{
			fixed (byte* ptr = Data) return ((ushort*)(ptr + 4))[0];
		}
		set
		{
			fixed (byte* ptr = Data) ((ushort*)(ptr + 4))[0] = value;
		}
	}

	public Ref PrimaryKey
	{
		get
		{
			fixed (byte* ptr = Data) return ((Ref*)(ptr + 8))[0];
		}
		set
		{
			fixed (byte* ptr = Data) ((Ref*)(ptr + 8))[0] = value;
		}
	}

	public byte[] Data { get; internal set; }

	public StringLookup StringLookup { get; internal set; }

	public Table Owner { get; internal set; }

	public AttributeCollection Attributes { get; internal set; }

	internal Dictionary<string, Record[]> Children { get; set; } = [];
	#endregion

	#region Properties
	internal IElementDefinition Definition => Owner.Definition.ElRecord.SubtableByType(SubclassType, this);

	public string OwnerName => Owner.Name.ToLower();

	public string Name => Definition.Name;

	public bool HasChildren => Children.Count > 0;

	internal ModelElement Model { get; set; }
	#endregion


	#region Serialize
	public void WriteXml(XmlWriter writer, ElementDefinition el)
	{
		writer.WriteStartElement(el.Name);

		// attribute
		foreach (var attribute in Attributes)
		{
			if (attribute.Name == AttributeCollection.s_autoid) continue;

			// set value, it seem that WriteRaw must be last  
			var value = AttributeConverter.ToString(attribute.Definition, attribute.RawValue);
			if (value is null) continue;

			if (attribute.Type == AttributeType.TNative) writer.WriteRaw(value);
			else writer.WriteAttributeString(attribute.Name, value);
		}

		// children
		foreach (var el_child in el.Children)
		{
			if (this.Children.TryGetValue(el_child.Name, out var childs))
				childs.ForEach(child => child.WriteXml(writer, el_child));
		}

		writer.WriteEndElement();
	}
	#endregion

	#region Interface
	public override string ToString() => Attributes.Get<string>("alias") ?? PrimaryKey.ToString();

	public void Dispose()
	{
		Data = null;
		StringLookup = null;
		Attributes = null;

		GC.SuppressFinalize(this);
	}
	#endregion
}