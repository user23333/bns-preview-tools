using Xylia.Preview.Data.Engine.BinData.Models;

namespace Xylia.Preview.Data.Engine.BinData.Serialization;
internal class TableWriter
{
	public static int GlobalCompressionBlockSize = ushort.MaxValue;
	private readonly RecordCompressedWriter _recordCompressedWriter;
	private readonly RecordUncompressedWriter _recordUncompressedWriter;

	public TableWriter(int compressionBlockSize = -1)
	{
		if (compressionBlockSize == -1)
			compressionBlockSize = GlobalCompressionBlockSize;

		_recordCompressedWriter = new RecordCompressedWriter(compressionBlockSize);
		_recordUncompressedWriter = new RecordUncompressedWriter();
	}


	public void WriteTo(DataArchiveWriter writer, Table table, bool is64Bit)
	{
		// LazyTable
		if (table.Archive != null)
		{
			table.WriteHeaderTo(writer);
			writer.Write(table.Size);
			writer.Write(table.IsCompressed);

			using var stream = table.Archive.LazyStream();
			stream.Seek(12, SeekOrigin.Begin);
			stream.CopyTo(writer);
		}
		else
		{
			table.WriteHeaderTo(writer);

			if (table.IsCompressed) WriteCompressed(writer, table);
			else WriteLoose(writer, table, is64Bit);
		}
	}

	private void WriteCompressed(DataArchiveWriter writer, Table table)
	{
		var recordCompressedWriter = _recordCompressedWriter;
		recordCompressedWriter.BeginWrite(writer);

		foreach (var record in table.Records.OrderBy(x => x.PrimaryKey.Variant).ThenBy(x => x.PrimaryKey.Id))
		{
			recordCompressedWriter.WriteRecord(writer, record.Data, record.StringLookup.Data);
		}

		recordCompressedWriter.EndWrite(writer);
	}

	private void WriteLoose(DataArchiveWriter writer, Table table, bool is64Bit)
	{
		// HACK: read config or find reason
		if (table.RecordCountOffset == 0 &&
			table.Name != null && CountOffsetTable.Contains(table.Name))
			table.RecordCountOffset = 1;

		_recordUncompressedWriter.SetRecordCountOffset(table.RecordCountOffset);
		_recordUncompressedWriter.BeginWrite(writer, is64Bit && !table.IsCompressed && table.ElementCount == 1);

		foreach (var record in table.Records)
		{
			_recordUncompressedWriter.WriteRecord(writer, record.Data);
		}

		_recordUncompressedWriter.EndWrite(writer, table.Padding,
			table.Records.Count == 0
				? new MemoryStream()
				: new MemoryStream(table.Records.First().StringLookup.Data));
	}


	private static string[] CountOffsetTable { get; set; } = [
		"account-post-charge",
		"alarm-message-time-table",
		"appearance-item",
		"arenamatchingrule",
		"arenaportal",
		"attachment",
		"attraction-group",
		"attractionrewardsummary",
		"auto-combat-customized-skill-cast-condition",
		"auto-combat-skill-cast-condition",
		"badge-appearance",
		"battleroyalfieldeffectpouchmesh",
		"benefit-ability",
		"board-gacha",
		"board-gacha-reward",
		"boast",
		"bossnpc",
		"campfire",
		"cave2",
		"challenge-party",
		"challengelistreward",
		"cinematic",
		"contents-guide",
		"contentsjournal",
		"contentsjournal2noti",
		"contentsjournalrecommenditem",
		"contributionreward",
		"craft-group-recipe",
		"craft-recipe-step",
		"difficulty-type-modify",
		"district",
		"duel",
		"duel-bot",
		"duel-bot-challenge-strategic-tool",
		"duel-bot-training-room-version",
		"duel-npc-challenge-group",
		"duel-observer-skill-slot",
		"effect-group",
		"effect-list",
		"emoticon",
		"envresponse",
		"event-contents",
		"feedback",
		"feedback-boss-npc",
		"feedback-rank",
		"feedback-skill-score",
		"field-item-move-anim",
		"fielditemdrop",
		"formula-constant",
		"game-message",
		"guild-battle-field-zone",
		"guild-discount",
		"guildcustomizepreset",
		"hyper-racing-game-reward",
		"indicator-idle",
		"item-buy-price",
		"item-combat",
		"item-event",
		"item-graph",
		"item-graph-seed-group",
		"item-random-ability-section",
		"item-transform-retry-cost",
		"item-usable-group",
		"itemexchange",
		"itemskill",
		"itemtransformrecipe",
		"itemtransformupgradeitem",
		"itemusablerelation",
		"job-style-specialization",
		"jumpingcharacter2",
		"linkmoveanim",
		"loadingimage",
		"map-group-2",
		"market-register-amount-tax-rate",
		"market-sale-income-tax-rate",
		"market-targeted-sale-income-tax",
		"partymatch",
		"passive-effect-move-anim",
		"pcskill3",
		"pet",
		"pet-food-recovery",
		"random-distribution",
		"randombox-preview",
		"relic-enhance-cost",
		"relic-set-item",
		"relic-system",
		"set-item",
		"skill-by-equipment",
		"skill-combo-2",
		"skill-modify-info",
		"skill-modify-info-group",
		"skill-train-by-item-list",
		"skilldashattribute3",
		"skillgatherrange3",
		"skillmodifylimit",
		"skillresultcontroll3",
		"skillsystematizationfiltergroup",
		"skillsystematizationgroup",
		"skilltargetfilter3",
		"skilltooltipattribute",
		"slatescroll",
		"slatescrollstone",
		"soul-npc-skill",
		"standidle",
		"summonedbeautyshop",
		"summonedstandidle",
		"talksocial",
		"teen-body-material",
		"treasure-board-page",
		"user-command",
		"vehicle-appearance",
		"virtual-item",
		"weeklytimetable",
		"zoneenv2place",
		"zoneteleportposition",
		"zonetriggereventcond",
	];
}