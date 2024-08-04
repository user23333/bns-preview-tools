using System.Diagnostics;
using System.Xml;
using Xylia.Preview.Data.Common.Exceptions;

namespace Xylia.Preview.Data.Engine.Definitions;
public class SequenceDefinitionLoader
{
	private readonly Dictionary<string, List<SequenceDefinition>> _duplicateSequences = [];

	#region Methods
	internal static SequenceDefinitionLoader LoadFrom(params string[] XmlContents)
	{
		var loader = new SequenceDefinitionLoader();

		foreach (var content in XmlContents)
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(content);

			foreach (XmlElement record in xmlDoc.SelectNodes("table/sequence"))
			{
				string name = record.Attributes["name"]?.Value?.Trim();
				if (loader._duplicateSequences.ContainsKey(name))
					throw BnsDataException.InvalidSequence($"has existed", name);

				var seq = loader.Load(record, name);
				if (seq != null) loader._duplicateSequences[name] = [seq];
			}
		}

		return loader;
	}

	internal SequenceDefinition Load(XmlElement element, string name)
	{
		name = element.GetAttribute("seq").Trim() ?? name;
		SequenceDefinition sequence = new(name);

		var nodes = element.ChildNodes.OfType<XmlElement>();
		if (nodes.Any())
		{
			foreach (var node in nodes)
			{
				sequence.Add(node.GetAttribute("name").Trim());
			}
		}
		else
		{
			if (string.IsNullOrWhiteSpace(name)) return null;

			if (!_duplicateSequences.TryGetValue(name, out var TSeq))
			{
				Trace.WriteLine($"seq `{name}` not defined");
				return null;
			}

			sequence = TSeq.First().Clone();
			sequence.Name = name;
		}

		return sequence;
	}


	public List<SequenceDefinition> LoadFor(IEnumerable<TableDefinition> tableDefs, bool mergeDuplicated)
	{
		var allSequenceDefinitions = new List<SequenceDefinition>();

		foreach (var tableDef in tableDefs)
		{
			LoadForTable(tableDef.ElRecord, allSequenceDefinitions, mergeDuplicated);

			foreach (var subtableDef in tableDef.ElRecord.Subtables)
			{
				LoadForTable(subtableDef, allSequenceDefinitions, mergeDuplicated);
			}
		}

		return allSequenceDefinitions;
	}

	private void LoadForTable(IElementDefinition tableDef, List<SequenceDefinition> allSequenceDefinitions, bool mergeDuplicated)
	{
		List<SequenceDefinition> sequenceDefList = null;

		foreach (var attrDef in tableDef.Attributes)
		{
			if (attrDef.Sequence.Count == 0)
				continue;

			var first = attrDef.Sequence[0];

			if (mergeDuplicated)
			{
				if (!_duplicateSequences.TryGetValue(first, out sequenceDefList))
				{
					sequenceDefList = [];
					_duplicateSequences[first] = sequenceDefList;
				}

				foreach (var sequenceDef in sequenceDefList)
				{
					//if (sequenceDef.Size != attrDef.Size)
					//	continue;

					if (!IsSequenceEqual(sequenceDef, attrDef.Sequence, StringComparer.OrdinalIgnoreCase))
						continue;

					if (attrDef.Sequence.Count > sequenceDef.Count)
					{
						for (var i = sequenceDef.Count; i < attrDef.Sequence.Count; i++)
						{
							sequenceDef.Add(attrDef.Sequence[i]);
						}
					}

					// Assign sequence definition
					attrDef.Sequence = sequenceDef;

					goto FOUND_SEQUENCE;
				}
			}

			// Create new one if we didn't find existing one
			var newSequenceDef = new SequenceDefinition(attrDef.Name); // Use attribute name as sequence name
			newSequenceDef.AddRange(attrDef.Sequence);

			if (mergeDuplicated)
				sequenceDefList.Add(newSequenceDef);
			allSequenceDefinitions.Add(newSequenceDef);

			// Assign sequence definition
			attrDef.Sequence = newSequenceDef;

			FOUND_SEQUENCE:;
		}
	}

	private static bool IsSequenceEqual(IEnumerable<string> a, IEnumerable<string> b, StringComparer comparer)
	{
		using var enumeratorA = a.GetEnumerator();
		using var enumeratorB = b.GetEnumerator();

		while (enumeratorA.MoveNext() & enumeratorB.MoveNext())
		{
			if (comparer.Equals(enumeratorA.Current, enumeratorB.Current))
				continue;

			return false;
		}

		return true;
	}
	#endregion
}