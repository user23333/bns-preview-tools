﻿using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.Definitions;

namespace Xylia.Preview.Tests.DatTests.Tools.Utils;
public static class CreateClass
{
	public static string Instance(string text)
	{
		var xml = new XmlDocument();
		xml.LoadXml(text);

		return Instance(xml);
	}

	public static string Instance(XmlDocument xml)
	{
		var table = TableDefinition.LoadFrom(new SequenceDefinitionLoader(), xml.DocumentElement);
		var builder = new StringBuilder();

		foreach (var element in table.DocumentElement.Children)
		{
			foreach (var attribute in element.Attributes)
			{
				if (attribute.Name is "auto-id" or "type") continue;
				InstanceAttribute(attribute, builder);
			}

			foreach (var sub in element.Subtables)
			{
				builder.AppendLine($"public sealed class {sub.Name.TitleCase()} : {table.Name.TitleCase()}");
				builder.AppendLine("{");

				foreach (var attribute in sub.Attributes.Except(element.Attributes))
				{
					InstanceAttribute(attribute, builder, true);
				}

				builder.Remove(builder.Length - 1, 1);
				builder.AppendLine("}\n");
			}
		}

		return builder.ToString();
	}


	private static void InstanceAttribute(AttributeDefinition attribute, StringBuilder builder, bool SubClass = false)
	{
		var prefix = new string('\t', SubClass ? 1 : 0);

		#region type
		string type = attribute.Type switch
		{
			AttributeType.TInt8 => "sbyte",
			AttributeType.TInt16 => "short",
			AttributeType.TInt32 => "int",
			AttributeType.TInt64 => "long",
			AttributeType.TFloat32 => "float",
			AttributeType.TBool => "bool",
			AttributeType.TString or AttributeType.TNative => "string",
			AttributeType.TSeq or AttributeType.TSeq16 or AttributeType.TProp_seq or AttributeType.TProp_field => attribute.Sequence.Name.TitleCase() + "Seq",
			AttributeType.TRef => $"Ref<{attribute.ReferedTableName?.TitleCase()}>",
			AttributeType.TTRef => $"Ref<ModelElement>",
			AttributeType.TSub => $"Sub<{attribute.ReferedTableName?.TitleCase()}>",
			AttributeType.TIcon => "Icon",
			AttributeType.TXUnknown1 => nameof(TimeUniversal),
			AttributeType.TXUnknown2 => nameof(ObjectPath),
			_ => attribute.Type.ToString()[1..]
		};

		// array
		if (attribute.Repeat > 1) type = $"{type}[]";
		#endregion

		#region meta
		List<string> sys_attr = [];
		if (new Regex(@"-\d").Match(attribute.Name).Success)
			sys_attr.Add($"Name(\"{attribute.Name}\")");

		if (sys_attr.Count != 0) builder.AppendLine($"{prefix}[{string.Join(", ", sys_attr)}]");
		builder.AppendLine($"{prefix}public {type} {attribute.Name.TitleCase()} {{ get; set; }}\n");
		#endregion

		if (attribute.Sequence != null)
		{
			builder.AppendLine(CreateEnum.Instance(attribute.Sequence));
		}
	}
}