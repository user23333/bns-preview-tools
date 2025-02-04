using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.Definitions;

namespace Xylia.Preview.Tests.DatTests.Tools.Utils;
public static class CreateEnum
{
    public static string Instance(string text)
    {
        var xml = new XmlDocument();
        xml.LoadXml($"<?xml version=\"1.0\"?>\n<table>{text}</table>");

		foreach (XmlElement attribute in xml.SelectNodes("table/attribute"))
		{
            var type = Enum.Parse<AttributeType>("T" + attribute.GetAttribute("type"));
			var sequence = new SequenceDefinitionLoader().Load(attribute, type);

            return Instance(sequence);
        }

        return null;
    }

    public static string Instance(SequenceDefinition sequence)
    {
        if (sequence is null) return null;

        var builder = new StringBuilder();
        builder.AppendLine($"public enum {sequence.Name?.TitleCase()}Seq");
        builder.AppendLine("{");

        foreach (var s in sequence)
        {
            if (new Regex(@"-\d+$").Match(s).Success) builder.AppendLine($"\t[Name(\"{s}\")]");

            builder.AppendLine($"\t{s.TitleCase()},");
        }

        builder.AppendLine($"\tCOUNT");
        builder.AppendLine("}");

        return builder.ToString();
    }
}