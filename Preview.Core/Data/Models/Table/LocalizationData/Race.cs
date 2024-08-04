using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models.Sequence;
using static Xylia.Preview.Data.Models.Item;

namespace Xylia.Preview.Data.Models;

[Side(ReleaseSide.Client)]
public sealed class Race : ModelElement
{
	#region Attributes
	public RaceSeq race { get; set; }

	public Ref<Text> Name2 { get; set; }

	public ObjectPath LobbyRaceImageset { get; set; }

	public ObjectPath CharacterInfoRaceImageset { get; set; }

	public Ref<Text> Desc { get; set; }

	public sbyte MaleCustomizeZoomcameraAddHeight { get; set; }

	public sbyte FemaleCustomizeZoomcameraAddHeight { get; set; }

	public ObjectPath LobbyRaceBgm { get; set; }
	#endregion

	#region Methods
	public static Race Get(RaceSeq? seq) => FileCache.Data.Provider.GetTable<Race>()?.FirstOrDefault(record => record.race == seq);

	public static Race Get(RaceSeq2 seq) => seq switch
	{
		RaceSeq2.Kun => Get(RaceSeq.건),
		RaceSeq2.Gon => Get(RaceSeq.곤),
		RaceSeq2.Lyn => Get(RaceSeq.린),
		RaceSeq2.Jin => Get(RaceSeq.진),
		RaceSeq2.SummonedCat => Get(RaceSeq.고양이),

		_ => null,
	};
	#endregion
}