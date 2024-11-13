using Xylia.Preview.Common.Exceptions;

namespace Xylia.Preview.Data.Engine.Definitions;
public class SequenceDefinition(string name) : List<string>
{
	public string Name { get; set; } = name;

	#region Methods
	public SequenceDefinition Clone() => MemberwiseClone() as SequenceDefinition;

	/// <summary>
	/// Check count of the sequence
	/// </summary>
	/// <param name="type"></param>
	/// <exception cref="BnsDataException"></exception>
	public void Check(AttributeType type)
	{
		switch (type)
		{
			case AttributeType.TSeq:
			case AttributeType.TProp_seq:
				if (Count > sbyte.MaxValue) throw BnsDataException.InvalidSequence($"seq exceeding maximum size, use `Seq16` instead.", Name);
				break;

			case AttributeType.TSeq16:
			case AttributeType.TProp_field:
				if (Count > short.MaxValue) throw BnsDataException.InvalidSequence($"seq exceeding maximum size, use `Seq32` instead.", Name);
				break;

			default: throw BnsDataException.InvalidSequence($"invalid attribute type, use `Seq` instead.", Name);
		}
	}
	#endregion
}