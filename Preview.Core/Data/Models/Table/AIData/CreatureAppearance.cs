using System.Diagnostics;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class CreatureAppearance : ModelElement
{
	#region Attributes
	public string Alias { get; set; }

	public RaceSeq Race { get; set; }

	public SexSeq Sex { get; set; }

	public ObjectPath FaceAnimSetName { get; set; }

	public ObjectPath AnimTreeName { get; set; }

	public ObjectPath FaceMeshName { get; set; }

	public ObjectPath BodyMeshName { get; set; }

	public ObjectPath VoiceSetName { get; set; }

	public ObjectPath NpcDialogueSet { get; set; }

	public ObjectPath AttachEffect { get; set; }

	public ObjectPath BodyMaterialName { get; set; }

	public ObjectPath FaceMaterialName { get; set; }

	public ObjectPath Npcattach1 { get; set; }

	public ObjectPath Npcattach2 { get; set; }

	public ObjectPath NpcattachMaterial1 { get; set; }

	public ObjectPath NpcattachMaterial2 { get; set; }

	public bool EnablePhysbrst { get; set; }

	public bool PcCustomize { get; set; }

	public ObjectPath UniqueSoundcue { get; set; }

	public float UniqueSoundculldist { get; set; }

	public float UniqueSoundfadetime { get; set; }

	public float UniqueDelaystoptime { get; set; }

	public float SoundAttenuationScale { get; set; }

	public float SoundVolumeScale { get; set; }

	public sbyte[] Param8 { get; set; }

	public float DecalRadius { get; set; }
	#endregion
}

public struct Param8
{
	#region Constructor
	const int SIZE = 96;
	public sbyte[] Data;

	public Param8(sbyte[] data) => this.Data = data;
	public Param8(string data) => this.Data = data.ToBytes().Select(b => (sbyte)b).ToArray();
	#endregion

	#region Operator
	public static bool operator ==(Param8 a, Param8 b)
	{
		if (SIZE != a.Data.Length || SIZE != b.Data.Length)
			throw new InvalidDataException();

		var flag = true;
		for (int i = 0; i < SIZE; i++)
		{
			if (a.Data[i] != b.Data[i])
			{
				flag = false;
				Trace.WriteLine($"param-{i + 1} ({a.Data[i]} <> {b.Data[i]})");
			}
		}

		return flag;
	}
	public static bool operator !=(Param8 a, Param8 b) => !(a == b);

	public override bool Equals(object other) => other is Param8 param8 && this == param8;
	public override int GetHashCode() => Data.GetHashCode();

	public static implicit operator Param8(sbyte[] data) => new(data);

	public static implicit operator Param8(string data) => new(data);
	#endregion

	#region Methods
	// 0 = HairStyle
	// 1 = 
	// 3 = WhiteEyesColor
	// 4 = PupilColor
	// 5 = 
	// 6 = HairColor
	// 7 = SkinColor
	// 8 = LipsColor
	// 9 = EyeLineColor
	// 10 = EyeShadoeColor
	// 11 = BlusherColor
	// 12 = Image
	// 13 = FaceWrinkles
	// 14 = 
	// 15 = 
	// 16 = 
	// 17 = MakeUpStyle
	// 18 = PupilFigure
	// 20 = PupilSize
	// 21 = EyesWrinklesDepth
	// 22 = ForsheadWrinklesDepth
	// 23 = MouthWrinklesDepth
	// 24 = CheekWrinklesDepth
	// 26 = LipsDepth
	// 27 = EyeLineDepth
	// 28 = EyeShadowDepth
	// 29 = BlusherDepth
	// 30 = LipsShine
	// 31 = SkinFader
	// 32 = FaceFader
	// 33 = EyebrowHeight
	// 34 = EyebrowAngle
	// 35 = ForsheadHeight
	// 36 = EyePosition
	// 37 = EyeWidth
	// 38 = EyeSize
	// 39 = EyeShape
	// 40 = EyeTail
	// 41 = EyeGap
	// 42 = EyeHeight
	// 43 = NosePosition
	// 44 = NoseSize
	// 45 = TipNoseShape
	// 46 = NoseHeight
	// 47 = MouthPosition
	// 48 = MouthWidth
	// 49 = UpperLip
	// 50 = UnderLip
	// 51 = MouthShape
	// 52 = MouthTail
	// 53 = MouthHeight
	// 54 = ChinLength
	// 55 = EndChinShape
	// 56 = ChinHeight
	// 57 = CheekboneShape
	// 58 = Cheek
	// 59 = PupilPosition
	// 60 = JawHeight
	// 65 = PelvisWidth
	// 66 = PelvisThickness
	// 67 = WaistThickness
	// 68 = WaistLength
	// 69 = ThighsWidth
	// 70 = CalfWidth
	// 71 = ThighsLength
	// 72 = CalfLength
	// 73 = UpperBodySize
	// 74 = NeckThickness
	// 75 = NeckLength
	// 76 = ShoulderUpDown
	// 77 = ShoulderRightLeft
	// 78 = ShouderSize
	// 79 = ArmThickness
	// 80 = ArmLength
	// 81 = HandsSize
	// 82 = HandsLength
	// 83 = FeetLength
	// 84 = Bulk
	// 85 = Height
	// 86 = HeadSize
	// 90 = HeadWidth
	// 91 = 
	// 92 = 
	// 93 = 
	// 94 = 

	#endregion
}