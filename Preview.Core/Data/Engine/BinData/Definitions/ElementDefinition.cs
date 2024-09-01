﻿using System.Diagnostics;
using System.Text.RegularExpressions;
using CUE4Parse.Utils;
using Serilog;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.Data.Engine.Definitions;
public abstract class IElementDefinition
{
	#region Properies
	public string Name { get; set; }
	public ushort Size { get; set; }
	public bool AutoKey { get; internal set; }
	public long MaxId { get; set; }
	public virtual short SubclassType { get; set; }

	public List<AttributeDefinition> Attributes { get; } = [];
	public List<AttributeDefinition> ExpandedAttributes { get; private set; } = [];
	public List<ElementDefinition> Children { get; } = [];
	#endregion

	#region Helper
	public override string ToString() => this.Name;

	private Dictionary<string, AttributeDefinition> _attributesDictionary = [];
	private Dictionary<string, AttributeDefinition> _expandedAttributesDictionary = [];

	public AttributeDefinition this[string name] => _expandedAttributesDictionary.GetValueOrDefault(name, null);
	public AttributeDefinition GetAttribute(string name) => _attributesDictionary.GetValueOrDefault(name, null);

	internal void CreateAttributeMap()
	{
		_attributesDictionary = Attributes.ToDictionary(x => x.Name);
		_expandedAttributesDictionary = ExpandedAttributes.ToDictionary(x => x.Name);

		//sort
		ExpandedAttributes = [.. ExpandedAttributes.OrderBy(o => !o.IsKey)
			.ThenBy(o => o.Type == AttributeType.TNative)
			.ThenBy(o => Regex.Replace(o.Name, @"\d+", match => match.Value.PadLeft(4, '0')))];
	}

	internal void RefreshSize(IEnumerable<AttributeDefinition> attributes, bool is64, int Offset = 16)
	{
		int Offset_Key = 8;
		foreach (var attribute in attributes.OrderBy(x => !x.IsKey))
		{
			// skip
			if (attribute.IsDeprecated || !attribute.Side.HasFlag(ReleaseSide.Client))
				continue;

			#region set offset
			int offset = 0;
			if (attribute.Offset != 0) offset = attribute.Offset;
			else if (attribute.IsKey) offset = Offset_Key;
			else offset = Offset;

			// auto align
			var size = AttributeDefinition.GetSize(attribute.Type, is64);
			if (size == 2) offset = offset.Align(2);
			else if (size != 1) offset = offset.Align(4);

			attribute.Offset = (ushort)offset;
			attribute.Size = size;

			// create new alias
			if (attribute.Name.Equals("unk-")) attribute.Name = "unk" + attribute.Offset;
			#endregion

			#region next offset
			offset += attribute.Size;

			if (attribute.IsKey)
			{
				Offset_Key = offset;
				Offset = Math.Max(Offset, offset);
			}
			else Offset = Math.Max(Offset, offset);
			#endregion
		}

		this.Size = (ushort)Offset.Align(4);
	}
	#endregion
}


public class ElementDefinition : IElementDefinition
{
	#region Properies
	// always -1 on base table definition
	public override short SubclassType { get => -1; set => throw new NotSupportedException(); }

	public List<ElementSubDefinition> Subtables { get; } = [];
	#endregion

	#region Helper
	private Dictionary<string, ElementSubDefinition> _subtablesDictionary = [];

	internal void CreateSubtableMap() => _subtablesDictionary = Subtables.ToDictionary(x => x.Name);

	internal IElementDefinition SubtableByName(string name)
	{
		// There are some special of Step
		// HACK: we will directly return to the main table now
		if (Name == "step") return this;

		bool IsEmpty = string.IsNullOrEmpty(name);
		if (Subtables.Count == 0)
		{
			Debug.Assert(IsEmpty);
			return this;
		}
		else if (!IsEmpty && _subtablesDictionary.TryGetValue(name, out var definition)) return definition;
		else
		{
			Log.Warning($"Invalid attribute, table:{this.Name}, name:type, value:{name}");
			// throw new ArgumentOutOfRangeException(nameof(name));

			return Subtables.First();
		}
	}

	internal IElementDefinition SubtableByType(short type, Record record = null)
	{
		lock (this)
		{
			IElementDefinition definition = null;

			if (type == -1)
			{
				definition = this;
			}
			else
			{
				// check type is in subtable, append missing sub definition
				for (short subIndex = (short)Subtables.Count; subIndex < type + 1; subIndex++)
				{
					var subtable = new ElementSubDefinition();
					Subtables.Add(subtable);

					subtable.Name = subIndex.ToString();
					subtable.SubclassType = subIndex;

					// Add parent expanded attributes
					subtable.ExpandedAttributes.AddRange(this.ExpandedAttributes);
					subtable.Size = this.Size;
					subtable.CreateAttributeMap();
				}

				definition = Subtables[type];
			}

			// check data size
			if (record != null) CheckSize(definition, record);

			return definition;
		}
	}


	private static void CheckSize(IElementDefinition definition, Record record)
	{
		if (record.DataSize != definition.Size)
		{
			var block = (record.DataSize - definition.Size) / 4;
			Debug.WriteLine($"check field size, " +
				$"table: {record.Owner.Name} " +
				$"type: {(record.SubclassType == -1 ? "null" : definition.Name)} " +
				$"size: {definition.Size} <> {record.DataSize} block: {block}", "Warning");

			if (block > 0)
			{
				// create unknown attribute
				for (int i = 0; i < block; i++)
				{
					var offset = (ushort)(definition.Size + i * 4);
					definition.ExpandedAttributes.Add(new AttributeDefinition()
					{
						Name = "unk" + offset,
						Size = 4,
						Offset = offset,
						Type = AttributeType.TInt32,
						DefaultValue = "0",
						Min = int.MinValue,
						Max = int.MaxValue,
						Repeat = 1,
					});
				}

				definition.Size = record.DataSize;
				definition.CreateAttributeMap();
			}
		}
	}
	#endregion
}

public class ElementSubDefinition : IElementDefinition
{
	public List<AttributeDefinition> ExpandedAttributesSubOnly { get; } = [];
}