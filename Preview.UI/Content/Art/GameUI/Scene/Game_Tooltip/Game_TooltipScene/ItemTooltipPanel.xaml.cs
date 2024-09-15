using System.Windows;
using System.Windows.Controls;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Document;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Controls;
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
		DataContext = FileCache.Data.Provider.GetTable<Item>()["Cash_Grocery_GuildMaterial_0007"];
#endif
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not Item record) return;

		#region Data
		// get job
		var jobs = record.EquipJobCheck.Where(x => x != JobSeq.JobNone);
		var jobfilter = !jobs.Any() && record.RandomOptionGroupId != 0;
		if (jobfilter) jobs = [UserSettings.Default.Job];
		#endregion

		#region Common
		TextArguments arguments = [null, record];

		ItemName.String.LabelText = record.ItemName;
		ItemIcon.ExpansionComponentList["BackgroundImage"]?.SetValue(record.BackIcon);
		ItemIcon.ExpansionComponentList["IconImage"]?.SetValue(record.FrontIcon);
		ItemIcon.ExpansionComponentList["UnusableImage"]?.SetValue(record.UnusableImage);
		ItemIcon.ExpansionComponentList["Grade_Image"]?.SetValue(null);
		ItemIcon.ExpansionComponentList["CanSaleItem"]?.SetValue(record.CanSaleItemImage);
		ItemIcon.InvalidateVisual();

		#region Substitute
		List<string> Substitute1 = [], Substitute2 = [];
		Substitute1.Add(record.Attributes.Get<Record>("main-info").GetText());
		Substitute2.Add(record.Attributes.Get<Record>("sub-info").GetText());

		#region Ability
		var data = new Dictionary<MainAbility, long>();

		var AttackPowerEquipMin = record.Attributes.Get<short>("attack-power-equip-min");
		var AttackPowerEquipMax = record.Attributes.Get<short>("attack-power-equip-max");
		data[MainAbility.AttackPowerEquipMinAndMax] = (AttackPowerEquipMin + AttackPowerEquipMax) / 2;

		var PveBossLevelNpcAttackPowerEquipMin = record.Attributes.Get<short>("pve-boss-level-npc-attack-power-equip-min");
		var PveBossLevelNpcAttackPowerEquipMax = record.Attributes.Get<short>("pve-boss-level-npc-attack-power-equip-max");
		data[MainAbility.PveBossLevelNpcAttackPowerEquipMinAndMax] = (PveBossLevelNpcAttackPowerEquipMin + PveBossLevelNpcAttackPowerEquipMax) / 2;

		var PvpAttackPowerEquipMin = record.Attributes.Get<short>("pvp-attack-power-equip-min");
		var PvpAttackPowerEquipMax = record.Attributes.Get<short>("pvp-attack-power-equip-max");
		data[MainAbility.PvpAttackPowerEquipMinAndMax] = (PvpAttackPowerEquipMin + PvpAttackPowerEquipMax) / 2;

		// HACK: Actually, the ability value is directly get
		foreach (var seq in Enum.GetValues<MainAbility>())
		{
			if (seq == MainAbility.None) continue;

			var name = seq.ToString().TitleLowerCase();
			var value = Convert.ToInt32(record.Attributes[name]);
			if (value != 0) data[seq] = value;
			else if (seq != MainAbility.AttackAttributeValue && seq != MainAbility.AttackCriticalDamageValue)
			{
				var value2 = Convert.ToInt32(record.Attributes[name + "-equip"]);
				if (value2 != 0) data[seq] = value2;
			}
		}

		// HACK: Actually, the MainAbility is not this sequence
		var MainAbility1 = record.Attributes.Get<MainAbility>("main-ability-1");
		var MainAbility2 = record.Attributes.Get<MainAbility>("main-ability-2");

		foreach (var ability in data)
		{
			if (ability.Value == 0) continue;

			var text = ability.Key.GetText(ability.Value);
			if (ability.Key == MainAbility1 || ability.Key == MainAbility2) Substitute1.Add(text);
			else Substitute2.Add(text);
		}


		if (record is Gem)
		{
			var MainAbilityFixed = record.Attributes.Get<ItemRandomAbilitySlot>("main-ability-fixed");
			var SubAbilityFixed = record.Attributes.Get<ItemRandomAbilitySlot>("sub-ability-fixed");
			var SubAbilityRandomCount = record.Attributes.Get<sbyte>("sub-ability-random-count");
			var SubAbilityRandom = LinqExtensions.For(8, (id) => record.Attributes.Get<ItemRandomAbilitySlot>("sub-ability-random-" + id));

			if (MainAbilityFixed != null) Substitute1.Add(MainAbilityFixed.Description);
			if (SubAbilityFixed != null) Substitute2.Add(SubAbilityFixed.Description);
			if (SubAbilityRandomCount > 0)
			{
				Substitute2.Add("UI.ItemRandomOption.Undetermined".GetText([SubAbilityRandomCount]));
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
		var SetItem = record.SetItem.Instance;
		if (SetItem is null) SetItemEffect.Visibility = Visibility.Collapsed;
		else
		{
			SetItemEffect.Visibility = Visibility.Visible;
			SetItemEffect_Name.String.LabelText = SetItem.Name;
			SetItemEffect_Effect.String.LabelText = SetItem.Description;
		}

		// Decompose
		var pages = DecomposePage.LoadFrom(record.DecomposeInfo);
		DecomposeDescription_Title.SetVisiable(pages.Count > 0);
		DecomposeDescription.Children.Clear();
		if (pages.Count > 0)
		{
			DecomposeDescription_Title.String.LabelText = (record is Item.Grocery ? "UI.ItemTooltip.RandomboxPreview.Title" : "UI.ItemTooltip.Decompose.Title").GetText();

			var page = pages[0];
			page.Update(DecomposeDescription.Children);
		}

		// Description
		ItemDescription.String.LabelText = record.Attributes["description2"].GetText();
		ItemDescription_4_Title.String.LabelText = record.Attributes["description4-title"].GetText();
		ItemDescription_5_Title.String.LabelText = record.Attributes["description5-title"].GetText();
		ItemDescription_6_Title.String.LabelText = record.Attributes["description6-title"].GetText();
		ItemDescription_4.String.LabelText = record.Attributes["description4"].GetText();
		ItemDescription_5.String.LabelText = record.Attributes["description5"].GetText();
		ItemDescription_6.String.LabelText = record.Attributes["description6"].GetText();
		ItemDescription7.String.LabelText = LinqExtensions.Join(BR.Tag,
			record.Attributes["description7"].GetText(),
			string.Join(BR.Tag, record.ItemCombat.SelectNotNull(x => x.Instance?.Description)),
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
			var SealKeepLevel = record.Attributes.Get<BnsBoolean>("seal-keep-level");
			var SealEnableCount = record.Attributes.Get<sbyte>("seal-enable-count");

			SealEnable.String.LabelText = (SealEnableCount == 0 ? "UI.Item.Tooltip.SealEnable" : "UI.Item.Tooltip.SealEnable.Count")
				.GetText([SealConsumeItem1, SealConsumeItemCount1, SealEnableCount]);
		}
		#endregion

		#region Required
		var required = new List<string?>
		{
			"Name.Item.Required.Level".GetText(arguments),
		};
		AddRequired(required, "Name.Item.Required.Faction".GetText(arguments));
		AddRequired(required, "Name.Item.Required.Race".GetText(arguments) + "Name.Item.Required.Sex".GetText(arguments));

		if (jobfilter) required.Add("UI.ItemRandomOption.EquipFilter.Warning".GetText());
		AddRequired(required, string.Join("", jobs.Select(x => "Name.Item.Required.Job2".GetText([.. arguments, Job.GetJob(x)]))));

		if (record.AccountUsed) required.Add("UI.ItemTooltip.AccountUsed".GetText());
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
			var RandomOptionGroup = FileCache.Data.Provider.GetTable<ItemRandomOptionGroup>()[record.RandomOptionGroupId + ((long)jobs.FirstOrDefault() << 32)];
			if (RandomOptionGroup != null)
			{
				for (int i = 0; i < RandomOptionGroup.AbilityListTotalCount; i++)
				{
					ProbabilityText.String.LabelText += "UI.ItemRandomOption.SubAbility.Undetermined".GetText() + BR.Tag;
				}

				if (RandomOptionGroup.SkillTrainByItemListTotalCount > 0)
				{
					// title
					Combat_Holder.Children.Add(Combat_Holder_Title);
					Combat_Holder_Title.String.LabelText = string.Format("{0} ({1}-{2})", RandomOptionGroup.SkillTrainByItemListTitle.GetText(),
						RandomOptionGroup.SkillTrainByItemListSelectMin, RandomOptionGroup.SkillTrainByItemListSelectMax);

					foreach (var SkillTrainByItemList in RandomOptionGroup.SkillTrainByItemList.SelectNotNull(x => x.Instance))
					{
						var ChangeSets = SkillTrainByItemList.ChangeSet.SelectNotNull(x => x.Instance);
						if (ChangeSets.Count() > 1) Combat_Holder.Children.Add(new BnsCustomLabelWidget() { Text = "UI.ItemRandomOption.Undetermined".GetText([1]) });

						foreach (var SkillTrainByItem in ChangeSets)
						{
							// element
							var icon = new BnsCustomImageWidget
							{
								BaseImageProperty = SkillTrainByItem.Icon?.GetImage(),
								Width = 32,
								Height = 32,
								Margin = new Thickness(0, 0, 10, 0),
								VerticalAlignment = System.Windows.VerticalAlignment.Top,
								//DataContext = SkillTrainByItem.ChangeSkill[0].Instance,
								//ToolTip = new Skill3ToolTipPanel_1()
							};

							// description
							var description = new BnsCustomLabelWidget();
							description.String.LabelText = SkillTrainByItem.Description2;

							// layout
							var box = new HorizontalBox() { Margin = new Thickness(0, 0, 0, 3) };
							LayoutData.SetAnchors(box, FLayoutData.Anchor.Full);
							Combat_Holder.Children.Add(box);

							box.Children.Add(icon);
							box.Children.Add(description);
						}
					}
				}
			}
		}

		if (record is Weapon)
		{
			var SkillByEquipment = record.Attributes.Get<SkillByEquipment>("skill-by-equipment");
			if (SkillByEquipment is not null)
			{
				Combat_Holder.Visibility = Visibility.Visible;
				Combat_Holder.Children.Add(Combat_Holder_Title);
				Combat_Holder_Title.String.LabelText = "UI.ItemTooltip.SkillChanged.Title".GetText();

				for (int i = 0; i < 4; i++)
				{
					var Skill3Id = SkillByEquipment.Skill3Id[i];
					if (Skill3Id == 0) continue;

					var Skill3 = FileCache.Data.Provider.GetTable<Skill3>()[new Ref(Skill3Id, 1)];

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
					LayoutData.SetAnchors(box, FLayoutData.Anchor.Full);
					Combat_Holder.Children.Add(box);

					box.Children.Add(icon);
					box.Children.Add(description);
				}
			}
		}
		#endregion
	}

	private static void AddRequired(List<string?> strings, string? str)
	{
		if (string.IsNullOrWhiteSpace(str)) return;
		if (str == "All") strings.Add("Name.Item.Required.Everyone".GetText([]));
		else strings.Add("Name.Item.Required.Result".GetText([str]));
	}
	#endregion

	#region Helpers
	internal sealed class DecomposePage
	{
		#region Fields
		public JobSeq Job;

		public Reward? DecomposeReward { get; private set; }

		public Tuple<Item, short>? OpenItem { get; private set; }
		#endregion

		#region Methods
		public static List<DecomposePage> LoadFrom(ItemDecomposeInfo info)
		{
			var pages = new List<DecomposePage>();

			#region reward
			for (int index = 0; index < info.DecomposeReward.Length; index++)
			{
				var reward = info.DecomposeReward[index];
				var item2 = info.Decompose_By_Item2[index];
				if (reward is null) continue;

				pages.Add(new DecomposePage() { DecomposeReward = reward, OpenItem = item2 });
			}
			#endregion

			#region job reward
			var group_job = info.DecomposeJobRewards
				.Where(x => x.Value is not null)
				.Select(x => new DecomposePage() { Job = x.Key, DecomposeReward = x.Value, });

			if (group_job.Any())
			{
				// combine data according to cell num
				//int num = group_job.Sum(group => group.Preview.Count);
				//if (num >= 30) pages.AddRange(group_job);
				//else pages.Add(new DecomposePage()
				//{
				//	Job = JobSeq.JobNone,
				//	DecomposeReward = group_job.FirstOrDefault().DecomposeReward,
				//	OpenItem = info.Job_Decompose_By_Item2.FirstOrDefault(),
				//	Preview = group_job.SelectMany(group => group.Preview).ToList(),
				//});
			}
			#endregion

			return pages;
		}

		public void Update(UIElementCollection collection)
		{
			ArgumentNullException.ThrowIfNull(DecomposeReward);

			var info = DecomposeReward.GetInfo().OrderByDescending(x => x.Data.ItemGrade);
			info.Where(x => x.Group.Item1 is "fixed").ForEach(item =>
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
			info.Where(x => x.Group.Item1 is "selected").ForEach(item =>
			{
				collection.Add(new BnsCustomLabelWidget()
				{
					Arguments = [null, item.Data, item.Min],
					String = new StringProperty() { LabelText = "UI.ItemTooltip.RandomboxPreview.Selected".GetText() }
				});
			});
			info.Where(x => x.Group.Item1 is "random" or "group-1" or "group-2" or "group-3" or "group-4" or "group-5" or "rare").ForEach(item =>
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