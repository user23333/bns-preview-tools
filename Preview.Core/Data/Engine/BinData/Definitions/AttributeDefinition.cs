using System.Diagnostics;
using System.Xml;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Common.Exceptions;

namespace Xylia.Preview.Data.Engine.Definitions;
[DebuggerDisplay("{Name} ({Type}) repeat:{Repeat}")]
public class AttributeDefinition
{
	#region Metadata
	public string Name { get; set; }
	public AttributeType Type { get; set; }
	public ushort Repeat { get; set; }
	public ushort ReferedTable { get; set; }
	public ushort ReferedEl { get; set; }
	public ushort Offset { get; set; }
	public ushort Size { get; set; }
	public bool IsDeprecated { get; set; }
	public bool IsKey { get; set; }
	public bool IsRequired { get; set; }
	public bool IsHidden { get; set; }
	public SequenceDefinition Sequence { get; set; }

	public string DefaultValue { get; set; }
	public long Max { get; set; }
	public long Min { get; set; }
	public float FMax { get; set; }
	public float FMin { get; set; }

	public ReleaseSide Side { get; set; } = ReleaseSide.Client | ReleaseSide.Server;
	#endregion

	#region Expand
	public string ReferedTableName { get; set; }
	public string ReferedElement { get; set; }
	public bool CanInput { get; set; } = true;

	internal List<AttributeDefinition> Expands { get; private set; } = [];
	#endregion	  

	#region Methods
	public void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("attribute");
		writer.WriteAttributeString("name", Name);
		writer.WriteAttributeString("type", Type.ToString()[1..]);

		if (IsKey) writer.WriteAttributeString("key", IsKey.ToString());
		if (Repeat > 1) writer.WriteAttributeString("repeat", Repeat.ToString());
		if (IsRequired) writer.WriteAttributeString("required", IsRequired.ToString());
		if (DefaultValue != null) writer.WriteAttributeString("default", DefaultValue);
		if (Min != 0) writer.WriteAttributeString("min", Min.ToString());
		if (Max != 0) writer.WriteAttributeString("max", Max.ToString());
		if (FMin != 0) writer.WriteAttributeString("fmin", FMin.ToString());
		if (FMax != 0) writer.WriteAttributeString("fmax", FMax.ToString());
		if (ReferedTable != 0) writer.WriteAttributeString("ref", ReferedTableName ?? ReferedTable.ToString());
		if (ReferedEl != 0) writer.WriteAttributeString("refel", ReferedEl.ToString());
		if (IsDeprecated) writer.WriteAttributeString("deprecated", IsDeprecated.ToString());
		if (IsHidden) writer.WriteAttributeString("hidden", IsHidden.ToString());

		Sequence?.ForEach(s =>
		{
			writer.WriteStartElement("case");
			writer.WriteAttributeString("name", s);
			writer.WriteEndElement();
		});

		writer.WriteEndElement();
	}

	internal AttributeDefinition Clone()
	{
		var newAttrDef = (AttributeDefinition)MemberwiseClone();
		this.Expands.Add(newAttrDef);

		return newAttrDef;
	}

	internal static AttributeDefinition LoadFrom(XmlElement node, SequenceDefinitionLoader loader)
	{
		var Name = node.GetAttribute<string>("name").Trim();
		var Type = Enum.TryParse("T" + node.GetAttribute("type"), true, out AttributeType type) ? type : 
			throw BnsDataException.InvalidDefinition($"Failed to determine attribute type: {Name}");
		var Repeat = ushort.TryParse(node.Attributes["repeat"]?.Value, out var tmp) ? tmp : (ushort)1;
		var RefTable = node.GetAttribute<string>("ref");
		var RefEl = node.GetAttribute<byte>("refel");
		var Offset = node.GetAttribute<ushort>("offset");
		var Deprecated = node.GetAttribute<bool>("deprecated");
		var Key = node.GetAttribute<bool>("key");
		var Required = node.GetAttribute<bool>("required");
		var Hidden = node.GetAttribute<bool>("hidden");
		var DefaultValue = node.GetAttribute<string>("default");
		var MinValue = node.GetAttribute<long>("min");
		var MaxValue = node.GetAttribute<long>("max");
		var FMinValue = node.GetAttribute<float>("fmin");
		var FMaxValue = node.GetAttribute<float>("fmax");

		#region Check
		if (Deprecated) return null;
		ArgumentException.ThrowIfNullOrEmpty(Name);

		//side
		var side = ReleaseSide.None;
		if (node.GetAttribute("client", true)) side |= ReleaseSide.Client;
		if (node.GetAttribute("server", true)) side |= ReleaseSide.Server;

		//seq
		var seq = loader.Load(node);
		seq?.Check(Type);

		//default
		if (string.IsNullOrEmpty(DefaultValue)) DefaultValue = null;
		switch (Type)
		{
			case AttributeType.TInt8:
				DefaultValue ??= "0";
				if (MinValue == 0) MinValue = sbyte.MinValue;
				if (MaxValue == 0) MaxValue = sbyte.MaxValue;
				break;

			case AttributeType.TInt16:
			case AttributeType.TDistance:
			case AttributeType.TAngle:
				DefaultValue ??= "0";
				if (MinValue == 0) MinValue = short.MinValue;
				if (MaxValue == 0) MaxValue = short.MaxValue;
				break;

			case AttributeType.TVelocity:
				DefaultValue ??= "0";
				break;

			case AttributeType.TInt32:
			case AttributeType.TMsec:
				DefaultValue ??= "0";
				if (MinValue == 0) MinValue = int.MinValue;
				if (MaxValue == 0) MaxValue = int.MaxValue;
				break;


			case AttributeType.TInt64:
				DefaultValue ??= "0";
				if (MinValue == 0) MinValue = long.MinValue;
				if (MaxValue == 0) MaxValue = long.MaxValue;
				break;

			case AttributeType.TFloat32:
				DefaultValue = DefaultValue.ToFloat32().ToString("0.00");
				if (FMinValue == 0) FMinValue = float.MinValue;
				if (FMaxValue == 0) FMaxValue = float.MaxValue;
				break;

			case AttributeType.TBool:
				DefaultValue ??= "n";
				break;


			case AttributeType.TRef:
			case AttributeType.TIcon:
			case AttributeType.TTRef:
				break;


			case AttributeType.TString:
			case AttributeType.TNative:
			case AttributeType.TXUnknown2:
				DefaultValue ??= "";
				break;

			case AttributeType.TSeq:
			case AttributeType.TSeq16:
			case AttributeType.TProp_seq:
			case AttributeType.TProp_field:
			{
				if (DefaultValue is null && seq != null)
				{
					//DefaultValue = seq.Default;

					// Ignore unnecessary attribute output
					if (Required || Hidden) DefaultValue ??= seq.FirstOrDefault();
				}

				break;
			}

			case AttributeType.TVector16:
				DefaultValue ??= "0,0,0";
				break;

			case AttributeType.TVector32:
				DefaultValue ??= "0,0,0";
				break;

			case AttributeType.TIColor:
				DefaultValue ??= new IColor().ToString();
				break;

			case AttributeType.TScript_obj:
				break;

			case AttributeType.TTime64:
			case AttributeType.TXUnknown1:
				break;
		}
		#endregion

		return new AttributeDefinition
		{
			Name = Name,
			IsDeprecated = Deprecated,
			IsKey = Key,
			IsRequired = Required,
			IsHidden = Hidden,
			Type = Type,
			Offset = Offset,
			Repeat = Repeat,
			ReferedTableName = RefTable,
			ReferedEl = RefEl,
			Sequence = seq,
			DefaultValue = DefaultValue,
			Max = MaxValue,
			Min = MinValue,
			FMax = FMaxValue,
			FMin = FMinValue,
			Side = side,
		};
	}

	public static ushort GetSize(AttributeType attributeType, bool is64 = false) => attributeType switch
	{
		AttributeType.TInt8 or
		AttributeType.TBool or
		AttributeType.TSeq or
		AttributeType.TProp_seq => 1,

		AttributeType.TInt16 or
		AttributeType.TSub or
		AttributeType.TDistance or
		AttributeType.TVelocity or
		AttributeType.TSeq16 or
		AttributeType.TProp_field => 2,

		AttributeType.TIColor => 3,

		AttributeType.TInt64 or
		AttributeType.TTime64 or
		AttributeType.TRef or
		AttributeType.TXUnknown1 or
		AttributeType.TXUnknown2 => 8,

		AttributeType.TTRef or
		AttributeType.TIcon or
		AttributeType.TVector32 or
		AttributeType.TBox => 12,

		AttributeType.TScript_obj => 20,
		AttributeType.TString => is64 ? (ushort)8 : (ushort)4,
		AttributeType.TNative => is64 ? (ushort)12 : (ushort)8,

		_ => 4,
	};
	#endregion
}