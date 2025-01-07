using Xylia.Preview.Common.Exceptions;

namespace Xylia.Preview.Data.Engine.Definitions;
public class SequenceDefinition : List<string>
{
	#region Constructor
	public SequenceDefinition() { }

	public SequenceDefinition(IEnumerable<string> seqs, AttributeType type = AttributeType.TSeq) : base(seqs)
	{
		Check(type);
	}
	#endregion

	#region Properties
	public string Name { get; set; }
	#endregion

	#region Methods
	/// <summary>
	/// Check count of the sequence
	/// </summary>
	/// <param name="type"></param>
	/// <exception cref="BnsDataException"></exception>
	private void Check(AttributeType type)
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