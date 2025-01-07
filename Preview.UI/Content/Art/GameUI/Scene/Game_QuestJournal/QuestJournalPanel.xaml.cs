using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Extensions;
using Xylia.Preview.UI.GameUI.Scene.Game_NpcTalk;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
using Xylia.Preview.UI.Helpers.Output.Tables;
using static Xylia.Preview.Data.Models.Quest;

namespace Xylia.Preview.UI.GameUI.Scene.Game_QuestJournal;
public partial class QuestJournalPanel
{
	#region Constructors
	readonly QuestJournalPanelViewModel _viewModel;

	public QuestJournalPanel()
	{
		InitializeComponent();

		QuestJournal_Tab_RadioButton_1.Header = "UI.QuestJournal.ProgressTab".GetText();
		QuestJournal_Tab_RadioButton_2.Header = "UI.QuestJournal.CompletedTab".GetText();
		QuestJournal_Tab_RadioButton_3.Header = "UI.QuestJournal.CompletedDailyTab".GetText();
		QuestJournal_Tab_RadioButton_4.Header = "UI.QuestJournal.LetterQuestTab".GetText();

		// Filter
		Filter_Category.ItemsSource = Enum.GetValues<CategorySeq>().Where(x => x < CategorySeq.COUNT);
		Filter_ContentType.ItemsSource = Enum.GetValues<ContentTypeSeq>().Where(x => x > ContentTypeSeq.None && x < ContentTypeSeq.COUNT);
		Filter_ResetType.ItemsSource = Enum.GetValues<ResetTypeSeq>().Where(x => x > ResetTypeSeq.None && x < ResetTypeSeq.COUNT);

		// Progress
		DataContext = _viewModel = new QuestJournalPanelViewModel();
		_viewModel.Source = CollectionViewSource.GetDefaultView(Globals.GameData.Provider.GetTable<Quest>().OrderBy(x => x.PrimaryKey));
		QuestJournal_ProgressQuestList.ItemsSource = _viewModel.Source;

		// Completed
		List<Quest> CompletedQuest = [];
		QuestEpic.GetEpic(CompletedQuest.Add);
		QuestJournal_CompletedQuestList.ItemsSource = CompletedQuest.GroupBy(o => o.Group);
	}
	#endregion

	#region Methods
	// ------------------------------------------------------------------
	// 
	//  ProgressTab
	// 
	// ------------------------------------------------------------------
	private void SearcherRule_GotFocus(object sender, RoutedEventArgs e)
	{
		SearcherOption.IsOpen = true;
	}

	private void SearcherRule_LostFocus(object sender, RoutedEventArgs e)
	{
		if (!IsHoverOption) SearcherOption.IsOpen = false;
	}

	private void SearcherOption_MouseEnter(object sender, MouseEventArgs e)
	{
		IsHoverOption = true;
	}

	private void SearcherOption_MouseLeave(object sender, MouseEventArgs e)
	{
		IsHoverOption = false;
		SearcherOption.IsOpen = false;
		_viewModel.Source?.Refresh();
	}

	private void SearcherOption_Checked(object sender, RoutedEventArgs e)
	{
		if (e.Source is FrameworkElement element && element.DataContext is Enum seq)
			_viewModel.SetFlag(seq, true);
	}

	private void SearcherOption_Unchecked(object sender, RoutedEventArgs e)
	{
		if (e.Source is FrameworkElement element && element.DataContext is Enum seq)
			_viewModel.SetFlag(seq, false);
	}

	private void QuestJournal_ProgressQuestList_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		QuestJournal_QuestInfoHolder.DataContext = e.NewValue;
	}

	private void QuestJournal_QuestInfoHolder_DataChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not Quest quest) return;

		QuestJournal_QuestInfo_Description.Arguments = [null, quest];
		QuestJournal_QuestInfo_Description.DataContext = quest.Acquisition.Value.FirstOrDefault();

		#region Reward
		var reward = quest.GetRewards().LastOrDefault();
		QuestJournal_QuestInfo_Holder.SetVisiable(reward != null);
		QuestJournal_RewardInfo_QuestInfo_Attraction_Guide.SetVisiable(quest.Category == CategorySeq.Attraction);

		if (reward != null)
		{
			static void UpdateValue(FrameworkElement parent, BnsCustomLabelWidget label, int value)
			{
				parent.SetVisiable(value > 0);
				label.String.LabelText = (value * 1).ToString();
			}

			static void UpdateItem(BnsCustomImageWidget widget, ModelElement slot, short count, sbyte SkillVarIdx = 0)
			{
				if (slot is null)
				{
					widget.Visibility = Visibility.Collapsed;
				}
				else if (slot is Item item)
				{
					widget.Visibility = Visibility.Visible;
					widget.ToolTip = new ItemTooltipPanel() { DataContext = item };
					widget.ExpansionComponentList["IconImage"]?.SetValue(item.FrontIcon);
					widget.ExpansionComponentList["Count"]?.SetValue(count.ToString());
					widget.ExpansionComponentList["Grade_Image"]?.SetValue(null);
					widget.ExpansionComponentList["CanSaleItem"]!.bShow = item.Auctionable;
					widget.ExpansionComponentList["SymbolImage"]?.SetExpansionShow(false);
					widget.ExpansionComponentList["SymbolImage_Chacked"]?.SetExpansionShow(false);
					widget.InvalidateVisual();
				}
				else if (slot is Skill3 skill)
				{
					widget.Visibility = Visibility.Visible;
				}
			}

			QuestJournal_RewardInfo_Final_FixedReward_money.SetVisiable(reward.Money > 0);
			QuestJournal_RewardInfo_Final_FixedReward_money.String.LabelText = reward.Money.Money;
			QuestJournal_RewardInfo_Final_Exp.SetVisiable(reward.BasicExp > 0);
			QuestJournal_RewardInfo_Final_FixedReward_exp_boost.SetVisiable(false);
			QuestJournal_RewardInfo_Final_FixedReward_exp.String.LabelText = (reward.BasicExp * 1).ToString();
			QuestJournal_RewardInfo_Final_HonorExp.SetVisiable(reward.BasicAccountExp > 0);
			QuestJournal_RewardInfo_Final_FixedReward_Honorexp_boost.SetVisiable(false);
			QuestJournal_RewardInfo_Final_FixedReward_Honorexp.String.LabelText = (reward.BasicAccountExp * 1).ToString();

			UpdateValue(QuestJournal_RewardInfo_Final_Duel_Point, QuestJournal_RewardInfo_Final_Duel_Point_Value, reward.BasicDuelPoint);
			UpdateValue(QuestJournal_RewardInfo_Final_PartyBattle_Point, QuestJournal_RewardInfo_Final_PartyBattle_Point_Value, reward.BasicPartyBattlePoint);
			UpdateValue(QuestJournal_RewardInfo_Final_FieldPlay_Point, QuestJournal_RewardInfo_Final_FieldPlay_Point_Value, reward.BasicFieldPlayPoint);
			UpdateValue(QuestJournal_RewardInfo_Final_Guild_Reputation, QuestJournal_RewardInfo_Final_Guild_Reputation_Value, reward.BasicGuildReputation);
			UpdateValue(QuestJournal_RewardInfo_Final_Reputation, QuestJournal_RewardInfo_Final_FixedReward_Reputation, reward.BasicFactionReputation);
			UpdateValue(QuestJournal_RewardInfo_Final_Production, QuestJournal_RewardInfo_Final_Production_Value, reward.BasicProductionExp);
			UpdateValue(QuestJournal_RewardInfo_Final_Masterty, QuestJournal_RewardInfo_Final_Masterty_Name, reward.BasicMasteryLevel);

			UpdateItem(QuestJournal_RewardInfo_Final_FixedReward_Icon_1, reward.FixedCommonSlot[0], reward.FixedCommonItemCount[0], reward.FixedCommonSkillVarIdx[0]);
			UpdateItem(QuestJournal_RewardInfo_Final_FixedReward_Icon_2, reward.FixedCommonSlot[1], reward.FixedCommonItemCount[1], reward.FixedCommonSkillVarIdx[1]);
			UpdateItem(QuestJournal_RewardInfo_Final_FixedReward_Icon_3, reward.FixedCommonSlot[2], reward.FixedCommonItemCount[2], reward.FixedCommonSkillVarIdx[2]);
			UpdateItem(QuestJournal_RewardInfo_Final_FixedReward_Icon_4, reward.FixedCommonSlot[3], reward.FixedCommonItemCount[3], reward.FixedCommonSkillVarIdx[3]);

			QuestJournal_RewardInfo_Final_DayofweekReward.SetVisiable(reward.DayofweekFixedItem.Any(x => x.HasValue));
			UpdateItem(QuestJournal_RewardInfo_Final_DayofweekReward_Icon_1, reward.DayofweekFixedItem[0], reward.DayofweekFixedItemCount[0]);
			UpdateItem(QuestJournal_RewardInfo_Final_DayofweekReward_Icon_2, reward.DayofweekFixedItem[1], reward.DayofweekFixedItemCount[1]);

			QuestJournal_RewardInfo_Final_OptionalReward.SetVisiable(reward.OptionalCommonSlot.Any(x => x.HasValue));
			UpdateItem(QuestJournal_RewardInfo_Final_OptionalReward_Icon_1, reward.OptionalCommonSlot[0], reward.OptionalCommonItemCount[0]);
			UpdateItem(QuestJournal_RewardInfo_Final_OptionalReward_Icon_2, reward.OptionalCommonSlot[1], reward.OptionalCommonItemCount[1]);
			UpdateItem(QuestJournal_RewardInfo_Final_OptionalReward_Icon_3, reward.OptionalCommonSlot[2], reward.OptionalCommonItemCount[2]);
			UpdateItem(QuestJournal_RewardInfo_Final_OptionalReward_Icon_4, reward.OptionalCommonSlot[3], reward.OptionalCommonItemCount[3]);

			var RewardHongmoon = reward.HongmoonItem[0].HasValue;
			var RewardMembership = reward.MembershipItem[0].HasValue;
			var RewardPCCafe = reward.PccafeItem[0].HasValue;
			QuestJournal_RewardInfo_Final_ExtraRewardHolder.SetVisiable(RewardHongmoon | RewardMembership | RewardPCCafe);
			UpdateItem(QuestJournal_RewardInfo_Final_ExtraReward_Icon_1, reward.HongmoonItem[0], reward.HongmoonItemCount[0]);
			UpdateItem(QuestJournal_RewardInfo_Final_ExtraReward_Icon_2, reward.MembershipItem[0], reward.MembershipItemCount[0]);
			UpdateItem(QuestJournal_RewardInfo_Final_ExtraReward_Icon_3, reward.PccafeItem[0], reward.PccafeItemCount[0]);

			#region QuestBonusReward
			var QuestBonusRewardSetting = Globals.GameData.Provider.GetTable<QuestBonusRewardSetting>().FirstOrDefault(record => record.Quest == quest);
			if (QuestBonusRewardSetting is null) QuestJournal_RewardInfo_AdditionalReward.SetVisiable(false);
			else
			{
				QuestJournal_RewardInfo_AdditionalReward.SetVisiable(true);
				QuestJournal_AdditionalReward_Dungeon.Visibility = QuestJournal_AdditionalReward_Dungeons.Visibility = QuestJournal_AdditionalReward_Dungeons_level.Visibility = Visibility.Collapsed;
				QuestJournal_AttractionReward_ChanceNum.String.LabelText = QuestBonusRewardSetting.BasicQuota.Value?.MaxValue.ToString();
				QuestJournal_AttractionReward_ChargeChanceNum.String.LabelText = "0";
				QuestJournal_AdditionalReward_GuideMsg.String.LabelText = QuestBonusRewardSetting.BasicQuota.Value?.DungeonBonusRewardDesc;

				var QuestBonusReward = QuestBonusRewardSetting.Reward.Value;
				UpdateItem(QuestJournal_AdditionalReward_ChanceItem_Icon_1, QuestBonusReward.FixedItem[0], QuestBonusReward.FixedItemCount[0]);
				UpdateItem(QuestJournal_AdditionalReward_ChanceItem_Icon_2, QuestBonusReward.FixedItem[1], QuestBonusReward.FixedItemCount[1]);
				UpdateItem(QuestJournal_AdditionalReward_ChanceItem_Icon_3, QuestBonusReward.FixedItem[2], QuestBonusReward.FixedItemCount[2]);
				UpdateItem(QuestJournal_AdditionalReward_ChanceItem_Icon_4, QuestBonusReward.FixedItem[3], QuestBonusReward.FixedItemCount[3]);
				UpdateItem(QuestJournal_AdditionalReward_ChanceItem_Icon_5, null, 0);
				UpdateItem(QuestJournal_AdditionalReward_ChanceItem_Icon_6, null, 0);
			}
			#endregion
		}
		#endregion
	}

	private void OnMissionClick(object sender, MouseButtonEventArgs e)
	{
		if (sender is not FrameworkElement fe) return;

		var cases = fe.DataContext switch
		{
			Acquisition acquisition => acquisition.Case,
			Mission mission => mission.Case,
			_ => null
		};

		if (cases != null)
		{
			foreach (var c in cases)
			{
				// should by type, but I am lazy
				var messgae = c.Attributes.Get<NpcResponse>("npc-response")?.TalkMessage.Value ??
					c.Attributes.Get<NpcTalkMessage>("msg");

				if (messgae != null) new NpcTalkPanel() { DataContext = messgae }.Show(this);
			}
		}
	}


	// ------------------------------------------------------------------
	// 
	//  CompletedTab
	// 
	// ------------------------------------------------------------------
	private void CompletedTab_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not Quest quest) return;

		QuestJournal_CompletedQuestDesc.String.LabelText = quest.Attributes["completed-desc"].GetText();
	}
	#endregion

	#region Private Fields
	private bool IsHoverOption;
	#endregion
}