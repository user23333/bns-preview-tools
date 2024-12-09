using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public class Zone : ModelElement
{
	#region Attributes
	public int Id { get; set; }

	public string Alias { get; set; }

	public enum ZoneType2Seq
	{
		None,
		Persistent,
		Single,
		Instant,
	}

	public ZoneType2Seq ZoneType2 { get; set; }

	public Ref<MapInfo> Map { get; set; }

	public Ref<MapArea> Area { get; set; }
	#endregion
}

public interface IAttraction : IHaveName
{
	string Description { get; }

	Ref<AttractionRewardSummary> RewardSummary { get; }


	#region Methods
	public string GetName() => (this switch
	{
		FieldZone zone		=> "UI.AttractionTooltip.Name.FieldZone." + zone.UiTextGrade,
		ClassicFieldZone zone => "UI.AttractionTooltip.Name.FieldZone." + zone.UiTextGrade,
		JackpotBossZone zone => "UI.AttractionTooltip.Name.JackpotBossZone." + zone.UiTextGrade,
		TendencyField zone  => "UI.AttractionTooltip.Name.TendencyField." + zone.UiTextGrade,
		Cave2 zone			=> "UI.AttractionTooltip.Name.Dungeon." + zone.UiTextGrade,
		Dungeon zone		=> "UI.AttractionTooltip.Name.Dungeon." + zone.UiTextGrade,
		RaidDungeon zone	=> "UI.AttractionTooltip.Name.RaidDungeon." + zone.UiTextGrade,
		Duel				=> "UI.AttractionTooltip.Name.Duel",
		GuildBattleFieldZone or PartyBattleFieldZone => "UI.AttractionTooltip.Name.BattleField",
		PublicRaid			=> "UI.AttractionTooltip.Name.PublicRaid",
		_					=> "UI.AttractionTooltip.Name.Field.1"
	}).GetText([null, Name]);

	public string GetDifficulty()
	{
		switch (this)
		{
			case Dungeon.Normal dungeon:
			{
				string s = "UI.AttractionMap.Easy".GetText();
				if (dungeon.UseDifficultyNormal) s += "UI.AttractionMap.Normal".GetText();
				if (dungeon.UseDifficultyHard) s += "UI.AttractionMap.Hard".GetText();
				return s;
			}

			case Dungeon.Sealed: return "UI.AttractionMap.Ancient".GetText();

			default: return null;
		}
	}

	public string GetPcMax()
	{
		var pcmax = this switch
		{
			Cave2 zone => zone.PcMax,
			Dungeon zone => zone.PcMax,
			PartyBattleFieldZone zone => zone.PcMax,
			_ => 0,
		};

		return pcmax == 0 ? null : """<image enablescale="true" imagesetpath="00015590.Tag_Persons" scalerate="1.2"/>""" + "UI.AttractionMap.PCMax".GetText([pcmax]);
	}
	#endregion
}