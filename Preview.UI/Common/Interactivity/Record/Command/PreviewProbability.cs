using System.Windows;
using CUE4Parse.Utils;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;

namespace Xylia.Preview.UI.Common.Interactivity;
internal class PreviewProbability : RecordCommand
{
	#region Methods
	protected override List<string> Type =>
	[
		"item",
		"npc",
		"zoneenv2",
	];

	protected override bool CanExecute(Record record)
	{
		switch (record.OwnerName)
		{
			case "item":
			{
				var pages = ItemTooltipPanel.DecomposePage.LoadFrom(record.To<Item>().DecomposeInfo);
				var GlyphReward = record.Attributes["glyph-reward"];

				return pages.Count > 0 || GlyphReward != null;
			}

			case "zoneenv2":
			{
				return record.Attributes["reward"] is not null;
			}

			default: return true;
		}
	}

	protected override void Execute(Record record)
	{
		var dispatcher = Application.Current.Dispatcher;

		switch (record.OwnerName)
		{
			case "item":
			{
				var pages = ItemTooltipPanel.DecomposePage.LoadFrom(record.To<Item>().DecomposeInfo);
				if (pages.Count > 0)
				{
					var reward = pages[0].DecomposeReward;
					dispatcher.Invoke(() => new RewardTooltipPanel() { DataContext = reward }.Show());
				}

				var GlyphReward = record.Attributes.Get<GlyphReward>("glyph-reward");
				if (GlyphReward != null) dispatcher.Invoke(() => new RewardTooltipPanel() { DataContext = GlyphReward }.Show());

				break;
			}

			case "npc":
			{
				var PersonalDroppedPouchReward = record.Attributes.Get<Reward>("personal-dropped-pouch-reward");
				var PersonalDroppedPouchRewardDifficultyType1 = record.Attributes.Get<Reward>("personal-dropped-pouch-reward-difficulty-type-1");
				var PersonalDroppedPouchRewardDifficultyType2 = record.Attributes.Get<Reward>("personal-dropped-pouch-reward-difficulty-type-2");
				var PersonalDroppedPouchRewardDifficultyType3 = record.Attributes.Get<Reward>("personal-dropped-pouch-reward-difficulty-type-3");

				// client is missing fields
				var RewardTable = record.Owner.Owner.GetTable<Reward>("reward");
				var RewardDefault = (record.Attributes.Get<Reward>("reward-default") ?? RewardTable[FixAlias(PersonalDroppedPouchReward)] ?? RewardTable[record.ToString()]);
				var RewardDifficultyType1 = record.Attributes.Get<Reward>("reward-difficulty-type-1") ?? RewardTable[FixAlias(PersonalDroppedPouchRewardDifficultyType1)];
				var RewardDifficultyType2 = record.Attributes.Get<Reward>("reward-difficulty-type-2") ?? RewardTable[FixAlias(PersonalDroppedPouchRewardDifficultyType2)];
				var RewardDifficultyType3 = record.Attributes.Get<Reward>("reward-difficulty-type-3") ?? RewardTable[FixAlias(PersonalDroppedPouchRewardDifficultyType3)];

				// display
				var rewards = new List<NameObject<object>>()
				{
					new(RewardDefault, StringHelper.Get("UI.RandomBox.Probability.CommonDroppedPouch")) { Flag = true },
					new(PersonalDroppedPouchReward, StringHelper.Get("UI.RandomBox.Probability.PersonalDroppedPouch")),
					new(RewardDifficultyType1, StringHelper.Get("UI.RandomBox.Probability.CommonDroppedPouch.Difficulty1")),
					new(PersonalDroppedPouchRewardDifficultyType1, StringHelper.Get("UI.RandomBox.Probability.PersonalDroppedPouch.Difficulty1")),
					new(RewardDifficultyType2, StringHelper.Get("UI.RandomBox.Probability.CommonDroppedPouch.Difficulty2")),
					new(PersonalDroppedPouchRewardDifficultyType2, StringHelper.Get("UI.RandomBox.Probability.PersonalDroppedPouch.Difficulty2")),
					new(RewardDifficultyType3, StringHelper.Get("UI.RandomBox.Probability.CommonDroppedPouch.Difficulty3")),
					new(PersonalDroppedPouchRewardDifficultyType3, StringHelper.Get("UI.RandomBox.Probability.PersonalDroppedPouch.Difficulty3")),
				};
				dispatcher.Invoke(() => new ItemGrowth2TooltipPanel { DataContext = rewards }.Show());
				break;
			}

			case "zoneenv2":
			{
				var Reward = record.Attributes.Get<Record>("reward");

				var rewards = new List<NameObject<object>>()
				{
					new(Reward?.To<Reward>(), "UI.RandomBox.Probability.PersonalDroppedPouch".GetText()) { Flag = true },
				};
				dispatcher.Invoke(() => new ItemGrowth2TooltipPanel { DataContext = rewards }.Show());
				break;
			}

			default: throw new NotSupportedException();
		}
	}

	private static string? FixAlias(Reward record)
	{
		return record?.Attributes.Get<string>("alias")?.SubstringBeforeLast("_personal", StringComparison.OrdinalIgnoreCase);
	}
	#endregion
}