﻿using System.Diagnostics;
using System.Xml;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Exceptions;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;

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
	public long Min { get; set; }
	public long Max { get; set; }
	public float FMin { get; set; }
	public float FMax { get; set; }
	public bool IsDynamic { get; set; }
	#endregion

	#region Expand
	public ReleaseSide Side { get; set; } = ReleaseSide.Client | ReleaseSide.Server;
	public string ReferedTableName { get; set; }
	public string ReferedElement { get; set; }
	internal List<AttributeDefinition> Expands { get; private set; } = [];
	#endregion

	#region Methods
	public string Range => Type switch
	{
		AttributeType.TBool => null,
		AttributeType.TFloat32 => $"{FMin} ~ {FMax}",
		AttributeType.TRef => $"ref ({ReferedTableName})",
		AttributeType.TTRef => $"ref",
		_ => $"{Min} ~ {Max}"
	};

	internal AttributeDefinition Clone()
	{
		var newAttrDef = (AttributeDefinition)MemberwiseClone();
		this.Expands.Add(newAttrDef);

		return newAttrDef;
	}

	internal static AttributeDefinition LoadFrom(XmlElement node, SequenceDefinitionLoader loader)
	{
		try
		{
			var Name = node.GetAttribute<string>("name").Trim();
			var Type = Enum.TryParse("T" + node.GetAttribute("type"), true, out AttributeType t) ? t :
				throw new Exception($"Failed to determine attribute type: {Name}");
			var Required = node.GetAttribute<bool>("required");
			var Hidden = node.GetAttribute<bool>("hidden");
			var DefaultValue = node.GetAttribute<string>("default");
			var MinValue = node.GetAttribute<long>("min");
			var MaxValue = node.GetAttribute<long>("max");

			#region Check
			ArgumentException.ThrowIfNullOrEmpty(Name);

			//side
			var side = ReleaseSide.None;
			if (node.GetAttribute("client", true)) side |= ReleaseSide.Client;
			if (node.GetAttribute("server", true)) side |= ReleaseSide.Server;

			//seq
			var seq = loader.Load(node, Type);

			// fix default value
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
					DefaultValue = DefaultValue.To<float>().ToString("0.00");
					break;

				case AttributeType.TBool:
					DefaultValue = DefaultValue switch
					{
						"true" => "y",
						"false" or null => "n",
						_ => DefaultValue,
					};
					break;

				case AttributeType.TRef:
				case AttributeType.TIcon:
				case AttributeType.TTRef:
					if (DefaultValue == "0") DefaultValue = null;
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
					// Ignore unnecessary attribute output
					if (DefaultValue is null && Hidden) DefaultValue = seq?.FirstOrDefault();
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
				IsDeprecated = node.GetAttribute<bool>("deprecated"),
				IsKey = node.GetAttribute<bool>("key"),
				IsRequired = Required,
				IsHidden = Hidden,
				Type = Type,
				Offset = node.GetAttribute<ushort>("offset"),
				Repeat = node.GetAttribute<ushort>("repeat", 1),
				ReferedTableName = node.GetAttribute<string>("ref"),
				ReferedEl = node.GetAttribute<byte>("refel"),
				Sequence = seq,
				DefaultValue = DefaultValue,
				Max = MaxValue,
				Min = MinValue,
				FMin = node.GetAttribute<float>("fmin", float.MinValue),
				FMax = node.GetAttribute<float>("fmax", float.MaxValue),
				IsDynamic = node.GetAttribute<bool>("dynamic"),
				Side = side,
			};
		}
		catch (Exception ex)
		{
			Debug.WriteLine(node.OuterXml);
			throw BnsDataException.InvalidDefinition($"Load attribute failed: {node.OuterXml}", ex);
		}
	}

	internal static ushort GetSize(AttributeType attributeType, bool is64 = false) => attributeType switch
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