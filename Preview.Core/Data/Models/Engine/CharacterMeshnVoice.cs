using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models.Engine;
public class CharacterMeshnVoice : ModelElement
{
	#region Attributes
	public short Id { get; set; }

	public TypeMeshSeq TypeMesh { get; set; }

	public enum TypeMeshSeq
	{
		HairMesh,
		TailMesh,
		BodyMesh,
		AccessoryMesh,
		VoiceSet,
		COUNT
	}

	public RaceSeq Race { get; set; }

	public SexSeq Sex { get; set; }

	public string Alias { get; set; }

	public short DataVersion { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public ObjectPath ResourceName { get; set; }

	public ObjectPath[] SubMaterialName { get; set; }
	#endregion
}