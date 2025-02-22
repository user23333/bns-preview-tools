﻿using System.Windows;
using System.Windows.Controls;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.Core;
using Xylia.Preview.Common;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Controls.Helpers;
using Xylia.Preview.UI.Converters;
using Xylia.Preview.UI.Extensions;
using Xylia.Preview.UI.ViewModels;
using static Xylia.Preview.Data.Models.Item;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
public partial class ItemTooltipPanel
{
	#region Constructors
	public ItemTooltipPanel()
	{
		InitializeComponent();
#if DEVELOP
		DataContext = Globals.GameData.Provider.GetTable<Item>()["Cash_Grocery_GuildMaterial_0007"];
#endif
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		#region Data		
		if (e.NewValue is not Item record) return;

		var jobs = record.EquipJobCheck.Where(x => x != JobSeq.JobNone);  // get job
		var jobfilter = !jobs.Any() && record.RandomOptionGroupId != 0;
		if (jobfilter) jobs = [UserSettings.Default.Job];
		#endregion

		#region Common	
		TextArguments arguments = [null, record];
		ItemName.String.LabelText = record.ItemName;
		ItemIcon.ExpansionComponentList["BackgroundImage"]?.SetValue(record.BackgroundImage);
		ItemIcon.ExpansionComponentList["IconImage"]?.SetValue(record.FrontIcon);
		ItemIcon.ExpansionComponentList["UnusableImage"]?.SetValue(record.UnusableImage);
		ItemIcon.ExpansionComponentList["Grade_Image"]?.SetValue(null);
		ItemIcon.ExpansionComponentList["CanSaleItem"]?.SetValue(record.CanSaleItemImage);
		ItemIcon.InvalidateVisual();

		#region Substitute
		List<string> Substitute1 = [], Substitute2 = [];
		Substitute1.Add(record.Attributes.Get<Record>("main-info").GetText(arguments));
		Substitute2.Add(record.Attributes.Get<Record>("sub-info").GetText(arguments));

		#region Ability
		var data = new Dictionary<MainAbilitySeq, long>();

		var AttackPowerEquipMin = record.Attributes.Get<short>("attack-power-equip-min");
		var AttackPowerEquipMax = record.Attributes.Get<short>("attack-power-equip-max");
		data[MainAbilitySeq.AttackPowerEquipMinAndMax] = (AttackPowerEquipMin + AttackPowerEquipMax) / 2;

		var PveBossLevelNpcAttackPowerEquipMin = record.Attributes.Get<short>("pve-boss-level-npc-attack-power-equip-min");
		var PveBossLevelNpcAttackPowerEquipMax = record.Attributes.Get<short>("pve-boss-level-npc-attack-power-equip-max");
		data[MainAbilitySeq.PveBossLevelNpcAttackPowerEquipMinAndMax] = (PveBossLevelNpcAttackPowerEquipMin + PveBossLevelNpcAttackPowerEquipMax) / 2;

		var PvpAttackPowerEquipMin = record.Attributes.Get<short>("pvp-attack-power-equip-min");
		var PvpAttackPowerEquipMax = record.Attributes.Get<short>("pvp-attack-power-equip-max");
		data[MainAbilitySeq.PvpAttackPowerEquipMinAndMax] = (PvpAttackPowerEquipMin + PvpAttackPowerEquipMax) / 2;

		// HACK: Actually, the ability value is directly get
		foreach (var seq in Enum.GetValues<MainAbilitySeq>())
		{
			if (seq == MainAbilitySeq.None) continue;

			var name = seq.ToString().TitleLowerCase();
			var value = Convert.ToInt32(record.Attributes[name]);
			if (value != 0) data[seq] = value;
			else if (seq != MainAbilitySeq.AttackAttributeValue && seq != MainAbilitySeq.AttackCriticalDamageValue)
			{
				var value2 = Convert.ToInt32(record.Attributes[name + "-equip"]);
				if (value2 != 0) data[seq] = value2;
			}
		}

		// HACK: Actually, the MainAbilitySeq is not this sequence
		var MainAbility1 = record.Attributes.Get<MainAbilitySeq>("main-ability-1");
		var MainAbility2 = record.Attributes.Get<MainAbilitySeq>("main-ability-2");

		foreach (var ability in data)
		{
			if (ability.Value == 0) continue;

			var text = ability.Key.GetText(ability.Value);
			if (ability.Key == MainAbility1 || ability.Key == MainAbility2) Substitute1.Add(text);
			else Substitute2.Add(text);
		}


		if (record is Gem)
		{
			var MainAbilitySeqFixed = record.Attributes.Get<ItemRandomAbilitySlot>("main-ability-fixed");
			var SubAbilityFixed = record.Attributes.Get<ItemRandomAbilitySlot>("sub-ability-fixed");
			var SubAbilityRandomCount = record.Attributes.Get<sbyte>("sub-ability-random-count");
			var SubAbilityRandom = record.Attributes.Get<ItemRandomAbilitySlot[]>("sub-ability-random");

			if (MainAbilitySeqFixed != null) Substitute1.Add(MainAbilitySeqFixed.Description);
			if (SubAbilityFixed != null) Substitute2.Add(SubAbilityFixed.Description);
			if (SubAbilityRandomCount > 0)
			{
				Substitute2.Add(StringHelper.Get("UI.ItemRandomOption.Undetermined", SubAbilityRandomCount));
				SubAbilityRandom.ForEach(x => Substitute2.Add(x.Description + " <Image imagesetpath='00015590.Tag_Random' enablescale='true' scalerate='1.2'/>"), true);
			}
		}
		#endregion

		#region Effect
		for (int i = 1; i <= 4; i++)
		{
			var EffectEquip = record.Attributes.Get<Record>("effect-equip-" + i);
			if (EffectEquip is null) continue;

			Substitute1.Add(EffectEquip.Attributes.Get<Record>("name3").GetText());
			Substitute2.Add(EffectEquip.Attributes.Get<Record>("description3").GetText());
		}
		#endregion

		CollectionSubstituteText.String.LabelText = LinqExtensions.Join(BR.Tag, Substitute1);
		CollectionSubstitute2Text.String.LabelText = LinqExtensions.Join(BR.Tag, Substitute2);
		ProbabilityText.String.LabelText = null;
		#endregion


		// Effect
		SetItemEffect.Visibility = Visibility.Collapsed;
		if (record.SetItem != null) 
		{
			SetItemEffect.Visibility = Visibility.Visible;
			SetItemEffect_Name.String.LabelText = record.SetItem.Name;
			SetItemEffect_Effect.String.LabelText = record.SetItem.Description;
		}

		// Decompose
		var pages = DecomposePage.LoadFrom(record.DecomposeInfo);
		DecomposeDescription_Title.SetVisiable(pages.Count > 0);
		DecomposeDescription.Children.Clear();
		if (pages.Count > 0)
		{
			DecomposeDescription_Title.String.LabelText = (record is Grocery grocery2 && grocery2.GroceryType == Grocery.GroceryTypeSeq.RandomBox ?
				"UI.ItemTooltip.RandomboxPreview.Title" : "UI.ItemTooltip.Decompose.Title").GetText();

			pages[0].Update(DecomposeDescription.Children);
		}

		// Description
		ItemDescription.String.LabelText = record.Attributes["description2"].GetText(arguments);
		ItemDescription_4_Title.String.LabelText = record.Attributes["description4-title"].GetText(arguments);
		ItemDescription_5_Title.String.LabelText = record.Attributes["description5-title"].GetText(arguments);
		ItemDescription_6_Title.String.LabelText = record.Attributes["description6-title"].GetText(arguments);
		ItemDescription_4.String.LabelText = record.Attributes["description4"].GetText(arguments);
		ItemDescription_5.String.LabelText = record.Attributes["description5"].GetText(arguments);
		ItemDescription_6.String.LabelText = record.Attributes["description6"].GetText(arguments);
		ItemDescription7.String.LabelText = LinqExtensions.Join(BR.Tag,
			record.Attributes["description7"].GetText(),
			string.Join(BR.Tag, record.ItemCombat.SelectNotNull(x => x.Value?.Description)),
			record.Attributes.Get<Record>("skill3")?.Attributes["description-weapon-soul-gem"]?.GetText());

		// Seal
		SealEnable.SetVisiable(record.SealRenewalAuctionable);
		if (record.SealRenewalAuctionable)
		{
			var SealConsumeItem1 = record.Attributes.Get<Item>("seal-consume-item-1");
			var SealConsumeItem2 = record.Attributes.Get<Item>("seal-consume-item-2");
			var SealConsumeItemCount1 = record.Attributes.Get<short>("seal-consume-item-count-1");
			var SealConsumeItemCount2 = record.Attributes.Get<short>("seal-consume-item-count-2");
			// seal-acquire-item
			var SealKeepLevel = record.Attributes.Get<bool>("seal-keep-level");
			var SealEnableCount = record.Attributes.Get<sbyte>("seal-enable-count");

			SealEnable.String.LabelText = (SealEnableCount == 0 ? "UI.Item.Tooltip.SealEnable" : "UI.Item.Tooltip.SealEnable.Count")
				.GetText([SealConsumeItem1, SealConsumeItemCount1, SealEnableCount]);
		}
		#endregion

		#region Required
		var required = new List<string?>
		{
			"Name.Item.Required.Level".GetTextIf(record.Attributes.Get<sbyte>("equip-level") > 1, arguments),
			"Name.Item.Required.Result".GetTextIf(record.Attributes["equip-faction"] != null, ["Name.Item.Required.Faction".GetText(arguments)]),
			"Name.Item.Required.Result".GetTextIf(record.EquipRace != null || record.EquipSex != SexSeq2.All, ["Name.Item.Required.Race".GetText(arguments) + "Name.Item.Required.Sex".GetText(arguments)]),

			(record.Attributes.Get<sbyte>("skill-limit-level") > 0 ? record.Attributes.Get<sbyte>("skill-limit-level-max") > 0 ? "Name.Item.SkillLimitLevel" : "Name.Item.SkillLimitLevel.OnlyMin" : null) .GetText(arguments),
			"Name.Item.SkillLimitMasteryLevel".GetTextIf(record.Attributes.Get<sbyte>("skill-limit-mastery-level") > 0, arguments),
		};

		if (jobfilter) required.Add("UI.ItemRandomOption.EquipFilter.Warning".GetText());
		if (jobs.Any()) required.Add("Name.Item.Required.Result".GetText([string.Join("", jobs.Select(x => "Name.Item.Required.Job2".GetText([.. arguments, Job.GetJob(x)])))]));

		required.Add("UI.ItemTooltip.AccountUsed".GetTextIf(record.AccountUsed));
		required.Add(LinqExtensions.Join("Name.Item.Cannot.Comma".GetText(),
			record.CannotTrade && !record.Auctionable ? "Name.Item.Cannot.Trade.All.Global".GetText() :
			record.CannotTrade && record.Auctionable ? "Name.Item.Cannot.Trade.Player.Global".GetText() :
			record.CannotTrade ? "Name.Item.Cannot.Trade.Auction.Global".GetText() : null,
			record.CannotSell ? "Name.Item.Cannot.Sell.Global".GetText() : null,
			record.CannotDispose ? "Name.Item.Cannot.Dispose.Global".GetText() : null));

		Required.String.LabelText = LinqExtensions.Join(BR.Tag, required);
		#endregion

		#region Combat Holder
		Combat_Holder.Children.Clear();
		Combat_Holder.Visibility = Visibility.Collapsed;

		if (record.RandomOptionGroupId != 0)
		{
			Combat_Holder.Visibility = Visibility.Visible;
			var RandomOptionGroup = Globals.GameData.Provider.GetTable<ItemRandomOptionGroup>()[record.RandomOptionGroupId + ((long)jobs.FirstOrDefault() << 32)];
			if (RandomOptionGroup != null)
			{
				LinqExtensions.For(RandomOptionGroup.AbilityListTotalCount, (i) => ProbabilityText.String.LabelText += "UI.ItemRandomOption.SubAbility.Undetermined".GetText() + BR.Tag);
				LinqExtensions.For(RandomOptionGroup.SkillBuildUpGroupListTotalCount, (i) => ProbabilityText.String.LabelText += "UI.ItemRandomOption.SkillEnhancement.Undetermined".GetText() + BR.Tag);

				if (RandomOptionGroup.SkillTrainByItemListTotalCount > 0)
				{
					// title
					Combat_Holder.Children.Add(Combat_Holder_Title);
					Combat_Holder_Title.String.LabelText = RandomOptionGroup.SkillTrainByItemListTitle.GetText();

					foreach (var SkillTrainByItemList in RandomOptionGroup.SkillTrainByItemList.Values())
					{
						var ChangeSets = SkillTrainByItemList.ChangeSet.Values();
						if (ChangeSets.Count() > 1) Combat_Holder.Children.Add(new BnsCustomLabelWidget() { Text = StringHelper.Get("UI.ItemRandomOption.Undetermined", 1) });

						foreach (var SkillTrainByItem in ChangeSets)
						{
							var box = Combat_Holder.Children.Add(new HorizontalBox()
							{
								Margin = new Thickness(0, 0, 0, 3)
							}, FLayout.Anchor.Full);

							// icon
							if (SkillTrainByItem.Icon != null)
							{
								box.Children.Add(new BnsCustomImageWidget
								{
									BaseImageProperty = SkillTrainByItem.Icon?.GetImage(),
									Width = 32,
									Height = 32,
									Margin = new Thickness(0, 0, 10, 0),
									VerticalAlignment = System.Windows.VerticalAlignment.Top,
									//DataContext = SkillTrainByItem.ChangeSkill[0].Value,
									//ToolTip = new Skill3ToolTipPanel_1()
								});
							}

							// description
							box.Children.Add(new BnsCustomLabelWidget() { Text = SkillTrainByItem.Description2 });
						}
					}
				}
			}
		}

		if (record is Grocery)
		{
			var SkillTrainByItem = record.Attributes.Get<SkillTrainByItem>("skill-train-by-item-for-transmit");
			if (SkillTrainByItem != null)
			{
				Combat_Holder.Visibility = Visibility.Visible;
				CollectionSubstituteText.String.LabelText += "UI.ItemTooltip.SkillTrainByItemExtract".GetText([SkillTrainByItem.ItemEquipType.GetText(), SkillTrainByItem.Job.GetText()]);

				var box = Combat_Holder.Children.Add(new HorizontalBox()
				{
					Margin = new Thickness(0, 0, 0, 3)
				}, FLayout.Anchor.Full);

				// icon
				box.Children.Add(new BnsCustomImageWidget
				{
					BaseImageProperty = SkillTrainByItem.Icon?.GetImage(),
					Width = 32,
					Height = 32,
					Margin = new Thickness(0, 0, 10, 0),
					VerticalAlignment = System.Windows.VerticalAlignment.Top,
				});

				// description
				box.Children.Add(new BnsCustomLabelWidget() { Text = SkillTrainByItem.Description2 });
			}
		}
		else if (record is Weapon)
		{
			var SkillByEquipment = record.Attributes.Get<SkillByEquipment>("skill-by-equipment");
			if (SkillByEquipment != null)
			{
				Combat_Holder.Visibility = Visibility.Visible;
				Combat_Holder.Children.Add(Combat_Holder_Title);
				Combat_Holder_Title.String.LabelText = "UI.ItemTooltip.SkillChanged.Title".GetText();

				for (int i = 0; i < 4; i++)
				{
					var Skill3Id = SkillByEquipment.Skill3Id[i];
					if (Skill3Id == 0) continue;

					var Skill3 = Globals.GameData.Provider.GetTable<Skill3>()[new Ref(Skill3Id, 1)];

					var icon = new BnsCustomImageWidget
					{
						BaseImageProperty = Skill3?.FrontIcon,
						Width = 32,
						Height = 32,
						Margin = new Thickness(0, 0, 10, 0),
					};
					var description = new BnsCustomLabelWidget();
					description.String.LabelText = "UI.ItemGrowth.SkillByEquipment.Skill".GetText([null, Skill3, SkillByEquipment.GetTooltipText(i)]);

					var box = new HorizontalBox() { Margin = new Thickness(7, 0, 0, 3) };
					LayoutData.SetAnchors(box, FLayout.Anchor.Full);
					Combat_Holder.Children.Add(box);

					box.Children.Add(icon);
					box.Children.Add(description);
				}
			}
		}
		#endregion

		//record.Attributes.Get<ItemEvent>("event-info")?.IsExpiration;
	}
	#endregion

	#region Helpers
	internal sealed class DecomposePage
	{
		#region Fields
		public JobSeq Job;

		public Reward? DecomposeReward;
		public Item? DecomposeByItem2;
		public short DecomposeByItem2StackCount;
		#endregion

		#region Methods
		public static List<DecomposePage> LoadFrom(ItemDecomposeInfo info)
		{
			var pages = new List<DecomposePage>();

			// reward
			for (int index = 0; index < info.DecomposeReward.Length; index++)
			{
				var reward = info.DecomposeReward[index];
				if (reward is null) continue;

				pages.Add(new DecomposePage()
				{
					DecomposeReward = reward,
					DecomposeByItem2 = info.DecomposeByItem2[info.DecomposeRewardByConsumeIndex ? index : 0],
					DecomposeByItem2StackCount = info.DecomposeByItem2StackCount[info.DecomposeRewardByConsumeIndex ? index : 0],
				});
			}

			// job reward
			var job = UserSettings.Default.Job;
			if (info.DecomposeJobRewards.TryGetValue(job, out var jobreward) && jobreward != null)
			{
				pages.Add(new DecomposePage()
				{
					Job = job,
					DecomposeReward = jobreward,
					DecomposeByItem2 = info.JobDecomposeByItem2[0],
					DecomposeByItem2StackCount = info.JobDecomposeByItem2StackCount[0],
				});
			}

			return pages;
		}

		public void Update(UIElementCollection collection)
		{
			ArgumentNullException.ThrowIfNull(DecomposeReward);

			var info = DecomposeReward.GetRewards().OrderByDescending(x => x.Data?.ItemGrade ?? 0);
			info.Where(x => x.Group is "fixed" or "smart-fixed-reward").ForEach(item =>
			{
				collection.Add(new BnsCustomLabelWidget()
				{
					Arguments = [null, item.Data, item.Min, item.Max],
					String = new StringProperty()
					{
						LabelText = (item.Min == item.Max ? item.Min == 1 ?
						"UI.ItemTooltip.RandomboxPreview.Fixed" :
						"UI.ItemTooltip.RandomboxPreview.Fixed.Min" :
						"UI.ItemTooltip.RandomboxPreview.Fixed.MinMax").GetText(),
					}
				});
			});
			info.Where(x => x.Group is "selected").ForEach(item =>
			{
				collection.Add(new BnsCustomLabelWidget()
				{
					Arguments = [null, item.Data, item.Min],
					String = new StringProperty() { LabelText = "UI.ItemTooltip.RandomboxPreview.Selected".GetText() }
				});
			});
			info.Where(x => x.Group is "group-1" or "group-2" or "group-3" or "group-4" or "group-5" or "rare" or
				"smart-group-1-reward" or "smart-group-2-reward" or "smart-group-3-reward" or "smart-group-4-reward" or "smart-group-5-reward" or "smart-rare-reward").ForEach(item =>
			{
				collection.Add(new BnsCustomLabelWidget()
				{
					Arguments = [null, item.Data, item.Min, item.Max],
					String = new StringProperty()
					{
						LabelText = (item.Min == item.Max ? item.Min == 0 ?
						"UI.ItemTooltip.RandomboxPreview.Random" :
						"UI.ItemTooltip.RandomboxPreview.Random.Min" :
						"UI.ItemTooltip.RandomboxPreview.Random.MinMax").GetText(),
					}
				});
			});
		}
		#endregion
	}
	#endregion
}