using System.ComponentModel;
using System.Globalization;
using Xylia.Preview.Common;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models.Sequence;
using static Xylia.Preview.Data.Models.Item;

namespace Xylia.Preview.Data.Models;
[Side(ReleaseSide.Client)]
[TypeConverter(typeof(RaceConverter))]
public sealed class Race : ModelElement, IHaveName
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
	public string Name => Name2.GetText();

	private static Race Get(RaceSeq? seq) => Globals.GameData.Provider.GetTable<Race>()?.FirstOrDefault(record => record.race == seq);

	public class RaceConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(RaceSeq) || sourceType == typeof(RaceSeq2)) return true;

			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return value switch
			{
				RaceSeq seq => Get(seq),
				RaceSeq2.RaceNone => Get(RaceSeq.RaceNone),
				RaceSeq2.Kun => Get(RaceSeq.건),
				RaceSeq2.Gon => Get(RaceSeq.곤),
				RaceSeq2.Lyn => Get(RaceSeq.린),
				RaceSeq2.Jin => Get(RaceSeq.진),
				RaceSeq2.SummonedCat => Get(RaceSeq.고양이),
				_ => null,
			};
		}
	}
	#endregion
}