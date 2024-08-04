using System.Windows;
using CUE4Parse.Utils;
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
				var pages = ItemTooltipPanel.DecomposePage.LoadFrom(record.As<Item>().DecomposeInfo);
				return pages.Count > 0;
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
		switch (record.OwnerName)
		{
			case "item":
			{
				var pages = ItemTooltipPanel.DecomposePage.LoadFrom(record.As<Item>().DecomposeInfo);
				if (pages.Count > 0)
				{
					var reward = pages[0].DecomposeReward;
					Application.Current.Dispatcher.Invoke(() => new RewardTooltipPanel() { DataContext = reward }.Show());
				}
				break;
			}

			case "npc":
			{
				var PersonalDroppedPouchReward = record.Attributes.Get<Record>("personal-dropped-pouch-reward");
				var PersonalDroppedPouchRewardDifficultyType1 = record.Attributes.Get<Record>("personal-dropped-pouch-reward-difficulty-type-1");
				var PersonalDroppedPouchRewardDifficultyType2 = record.Attributes.Get<Record>("personal-dropped-pouch-reward-difficulty-type-2");
				var PersonalDroppedPouchRewardDifficultyType3 = record.Attributes.Get<Record>("personal-dropped-pouch-reward-difficulty-type-3");

				// client is missing fields
				var RewardTable = record.Owner.Owner.GetTable("reward");
				var RewardDefault = record.Attributes.Get<Record>("reward-default") ?? RewardTable[FixAlias(PersonalDroppedPouchReward)] ?? RewardTable[record.ToString()];
				var RewardDifficultyType1 = record.Attributes.Get<Record>("reward-difficulty-type-1") ?? RewardTable[FixAlias(PersonalDroppedPouchRewardDifficultyType1)];
				var RewardDifficultyType2 = record.Attributes.Get<Record>("reward-difficulty-type-2") ?? RewardTable[FixAlias(PersonalDroppedPouchRewardDifficultyType2)];
				var RewardDifficultyType3 = record.Attributes.Get<Record>("reward-difficulty-type-3") ?? RewardTable[FixAlias(PersonalDroppedPouchRewardDifficultyType3)];

				// display
				var rewards = new List<NameObject<object>>()
				{
					new(RewardDefault?.As<Reward>(), "UI.RandomBox.Probability.CommonDroppedPouch".GetText()) { Flag = true },
					new(PersonalDroppedPouchReward?.As<Reward>(), "UI.RandomBox.Probability.PersonalDroppedPouch".GetText()),
					new(RewardDifficultyType1?.As<Reward>(), "UI.RandomBox.Probability.CommonDroppedPouch.Difficulty1".GetText()),
					new(PersonalDroppedPouchRewardDifficultyType1?.As<Reward>(), "UI.RandomBox.Probability.PersonalDroppedPouch.Difficulty1".GetText()),
					new(RewardDifficultyType2?.As<Reward>(), "UI.RandomBox.Probability.CommonDroppedPouch.Difficulty2".GetText()),
					new(PersonalDroppedPouchRewardDifficultyType2?.As<Reward>(), "UI.RandomBox.Probability.PersonalDroppedPouch.Difficulty2".GetText()),
					new(RewardDifficultyType3?.As<Reward>(), "UI.RandomBox.Probability.CommonDroppedPouch.Difficulty3".GetText()),
					new(PersonalDroppedPouchRewardDifficultyType3?.As<Reward>(), "UI.RandomBox.Probability.PersonalDroppedPouch.Difficulty3".GetText()),
				};
				Application.Current.Dispatcher.Invoke(() => new ItemGrowth2TooltipPanel { DataContext = rewards }.Show());
				break;
			}

			case "zoneenv2":
			{
				var Reward = record.Attributes.Get<Record>("reward");

				var rewards = new List<NameObject<object>>()
				{
					new(Reward?.As<Reward>(), "UI.RandomBox.Probability.PersonalDroppedPouch".GetText()) { Flag = true },
				};
				Application.Current.Dispatcher.Invoke(() => new ItemGrowth2TooltipPanel { DataContext = rewards }.Show());
				break;
			}

			default: throw new NotSupportedException();
		}
	}

	private static string? FixAlias(Record record)
	{
		return record?.ToString().SubstringBeforeLast("_personal", StringComparison.OrdinalIgnoreCase);
	}
	#endregion
}