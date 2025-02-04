using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class Stance : ModelElement
{
	#region Attributes
	public StanceSeq StanceType { get; set; }

	public Ref<Text> Name2 { get; set; }

	public string SecondgaugeName => Attributes.Get<SecondgaugeNameSeq>("secondgauge-name").GetText();
	public enum SecondgaugeNameSeq
	{
		[Text("Name.stance.secondgauge-name.none")] None,
		[Text("Name.stance.secondgauge-name.blade")] Blade,
		[Text("Name.stance.secondgauge-name.soul")] Soul,
		[Text("Name.stance.secondgauge-name.fighting-spirit")] FightingSpirit,
		[Text("Name.stance.secondgauge-name.force-energy")] ForceEnergy,
		[Text("Name.stance.secondgauge-name.fury")] Fury,
		[Text("Name.stance.secondgauge-name.chakra")] Chakra,
		COUNT
	}

	public string SecondgaugeUseType => Attributes.Get<SecondgaugeUseTypeSeq>("secondgauge-use-type").GetText();
	public enum SecondgaugeUseTypeSeq
	{
		None,
		[Text("Name.stance.secondgauge-use-type.use-sp")] UseSp,
		[Text("Name.stance.secondgauge-use-type.use-fp")] UseFp,
		COUNT
	}
	#endregion
}