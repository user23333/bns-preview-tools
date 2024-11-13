using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class Race2AttributesInfo : ModelElement
{
	#region Attributes
	public RaceType2 MainTypeRace2 { get; set; }

	public enum RaceType2
	{
		None,
		[Text("race1_name")] Race1,
		[Text("race2_name")] Race2,
		[Text("race3_name")] Race3,
		[Text("race4_name")] Race4,
		[Text("race5_name")] Race5,
		[Text("race6_name")] Race6,
		[Text("race7_name")] Race7,
		COUNT
	}

	public AttributeType MainTypeAttributes { get; set; }

	public enum AttributeType
	{
		None,
		[Text("attribute1_name")] Attribute1,
		[Text("attribute2_name")] Attribute2,
		[Text("attribute3_name")] Attribute3,
		[Text("attribute4_name")] Attribute4,
		[Text("attribute5_name")] Attribute5,
		[Text("attribute6_name")] Attribute6,
		[Text("attribute7_name")] Attribute7,
		[Text("attribute8_name")] Attribute8,
		[Text("attribute9_name")] Attribute9,
		[Text("attribute10_name")] Attribute10,
		[Text("attribute11_name")] Attribute11,
		[Text("attribute12_name")] Attribute12,
		COUNT
	}

	public Ref<Text> MainTypeName { get; set; }

	public string MainTypeIcon { get; set; }

	public MainAbilitySeq AttackType { get; set; }

	public Ref<Text> AttackTypeName { get; set; }

	public string AttackTypeIcon { get; set; }

	public MainAbilitySeq DefendType { get; set; }

	public Ref<Text> DefendTypeName { get; set; }

	public string DefendTypeIcon { get; set; }
	#endregion
}