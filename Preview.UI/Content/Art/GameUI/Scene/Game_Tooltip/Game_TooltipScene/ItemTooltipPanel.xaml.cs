using System.Windows;
using System.Windows.Controls;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Converters;
using Xylia.Preview.UI.Extensions;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
public partial class ItemTooltipPanel
{
	#region Constructors
	public ItemTooltipPanel()
	{
		InitializeComponent();
#if DEVELOP
		DataContext = FileCache.Data.Provider.GetTable<Item>()["General_Accessory_Ring_3034_031"];
#endif
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not Item record) return;

		// get job
		var jobs = record.EquipJobCheck.Where(x => x != JobSeq.JobNone);
		var jobfilter = !jobs.Any() && record.RandomOptionGroupId != 0;
		if (jobfilter) jobs = [UserSettings.Default.Job];

		#region Common
		TextArguments arguments = [null, record];

		ItemName.String.LabelText = record.ItemName;
		ItemIcon.ExpansionComponentList["BackgroundImage"]?.SetValue(record.BackIcon);
		ItemIcon.ExpansionComponentList["IconImage"]?.SetValue(record.FrontIcon);
		ItemIcon.ExpansionComponentList["UnusableImage"]?.SetValue(record.UnusableImage);
		ItemIcon.ExpansionComponentList["Grade_Image"]?.SetValue(null);
		ItemIcon.ExpansionComponentList["CanSaleItem"]?.SetValue(record.CanSaleItemImage);
		ItemIcon.InvalidateVisual();

		// Effect
		var SetItem = record.SetItem.Instance;
		if (SetItem is null) SetItemEffect.Visibility = Visibility.Collapsed;
		else
		{
			SetItemEffect.Visibility = Visibility.Visible;
			SetItemEffect_Name.String.LabelText = SetItem.Name;
			SetItemEffect_Effect.String.LabelText = SetItem.Description;
		}

		// Description7
		ItemDescription7.String.LabelText = new List<string?>
		{
			record.Attributes["description7"].GetText(),
			string.Join("<br/>", record.ItemCombat.SelectNotNull(x => x.Instance?.Description)),
			record.Attributes.Get<Record>("skill3")?.Attributes["description-weapon-soul-gem"]?.GetText(),
		}.Join();

		// Seal
		SealEnable.SetVisiable(record.SealRenewalAuctionable);
		if (record.SealRenewalAuctionable)
		{
			var SealConsumeItem1 = record.Attributes.Get<Record>("seal-consume-item-1")?.As<Item>();
			var SealConsumeItem2 = record.Attributes.Get<Record>("seal-consume-item-2")?.As<Item>();
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
		required.Add(new List<string?>
		{
			record.CannotTrade && !record.Auctionable ? "Name.Item.Cannot.Trade.All.Global".GetText() :
			record.CannotTrade && record.Auctionable ? "Name.Item.Cannot.Trade.Player.Global".GetText() :
			record.CannotTrade ? "Name.Item.Cannot.Trade.Auction.Global".GetText() : null,
			record.CannotSell ? "Name.Item.Cannot.Sell.Global".GetText() : null,
			record.CannotDispose ? "Name.Item.Cannot.Dispose.Global".GetText() : null,
		}.Join("Name.Item.Cannot.Comma".GetText()));

		Required.String.LabelText = required.Join();
		#endregion

		#region Combat Holder
		Combat_Holder.Children.Clear();
		Combat_Holder.Visibility = Visibility.Collapsed;

		if (record.RandomOptionGroupId != 0)
		{
			var Job = jobs.FirstOrDefault();
			var RandomOptionGroup = FileCache.Data.Provider.GetTable<ItemRandomOptionGroup>()[record.RandomOptionGroupId + ((long)Job << 32)];
			if (RandomOptionGroup != null)
			{
				if (RandomOptionGroup.AbilityListTotalCount > 0)
				{
					// TODO: add random tag
				}

				if (RandomOptionGroup.SkillTrainByItemListTotalCount > 0)
				{
					// title
					Combat_Holder.Children.Add(Combat_Holder_Title);
					Combat_Holder_Title.String.LabelText = string.Format("{0} ({1}-{2})", RandomOptionGroup.SkillTrainByItemListTitle,
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
								BaseImageProperty = IconTexture.Parse(SkillTrainByItem.Icon),
								Width = 32,
								Height = 32,
								Margin = new Thickness(0, 0, 10, 0),
								VerticalAlignment = VerticalAlignment.Top,
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

				Combat_Holder.Visibility = Visibility.Visible;
			}
		}
		#endregion

		#region Decompose 
		DecomposeDescription_Title.Visibility = Visibility.Collapsed;
		DecomposeDescription.Children.Clear();

		var pages = DecomposePage.LoadFrom(record.DecomposeInfo);
		if (pages.Count > 0)
		{
			DecomposeDescription_Title.Visibility = Visibility.Visible;
			DecomposeDescription_Title.String.LabelText = (record is Item.Grocery ? "UI.ItemTooltip.RandomboxPreview.Title" : "UI.ItemTooltip.Decompose.Title").GetText();

			var page = pages[0];
			page.Update(DecomposeDescription.Children);
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

			var info = DecomposeReward.GetInfos().OrderByDescending(x => x.Item.ItemGrade);
			info.Where(x => x.Group.Item1 is "fixed").ForEach(item =>
			{
				collection.Add(new BnsCustomLabelWidget()
				{
					Arguments = [null, item.Item, item.Min, item.Max],
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
					Arguments = [null, item.Item, item.Min],
					String = new StringProperty() { LabelText = "UI.ItemTooltip.RandomboxPreview.Selected".GetText() }
				});
			});
			info.Where(x => x.Group.Item1 is "random" or "group-1" or "group-2" or "group-3" or "group-4" or "group-5" or "rare").ForEach(item =>
			{
				collection.Add(new BnsCustomLabelWidget()
				{
					Arguments = [null, item.Item, item.Min, item.Max],
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