using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Xylia.Preview.Common;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Sequence;
using Xylia.Preview.UI.Common.Interactivity;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Auction;
internal class AuctionPanelViewModel : ObservableObject
{
	public event EventHandler? Changed;

	#region Properties
	private ICollectionView? _source;
	public ICollectionView Source
	{
		get => _source;
		set
		{
			_source = value;
			_source.Filter = OnFilter;
			OnPropertyChanged(nameof(Source));
		}
	}

	private string? _nameFilter;
	public string? NameFilter
	{
		get => _nameFilter;
		set
		{
			SetProperty(ref _nameFilter, value);
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}

	private bool _auctionable;
	public bool Auctionable
	{
		get => _auctionable;
		set
		{
			SetProperty(ref _auctionable, value);
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}

	private sbyte _grade;
	public sbyte Grade
	{
		get => _grade;
		set
		{
			SetProperty(ref _grade, value);
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}

	private JobSeq _job;
	public JobSeq Job
	{
		get => _job;
		set
		{
			SetProperty(ref _job, value);
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}


	private HashList? _hashList;
	public HashList? HashList
	{
		get => _hashList;
		set
		{
			SetProperty(ref _hashList, value);
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}

	private object? _category;
	public object? Category
	{
		get => _category;
		set
		{
			SetProperty(ref _category, value);
			Changed?.Invoke(this, EventArgs.Empty);
		}
	}
	#endregion

	#region Methods
	// TODO: NEO
	public IEnumerable<JobSeq> Jobs => [JobSeq.JobNone, .. Data.Models.Job.PcJobs];

	public IEnumerable<MarketCategory2Group> MarketCategory
	{
		get
		{
			var MarketCategory2Group = Globals.GameData.Provider.GetTable<MarketCategory2Group>()?.ToList();
			if (MarketCategory2Group.IsEmpty())
			{
				MarketCategory2Group =
				[
					new MarketCategory2Group.Favorite() { Visible = true, Name = "UI.Market.Category.Favorites".GetText() },
					.. SequenceExtensions.MarketCategory2Group().Select(group => new MarketCategory2Group.MarketCategory2()
					{
						SortNo = 100,
						Visible = true,
						Name = group.Key.GetText(),
						Marketcategory2 = group.Key,
						MarketCategory3Group = group.Value?.Select(x => new Ref<MarketCategory3Group>(new MarketCategory3Group()
						{
							Visible = true,
							Name = x.GetText(),
							MarketCategory3 = [x]
						})).ToArray(),
					}),
				];
			}

			MarketCategory2Group!.Add(new MarketCategory2Group.All() { Visible = true, Name = "UI.Market.Category.All".GetText() });
			return MarketCategory2Group.Where(x => x.Visible).OrderBy(x => x.SortNo);
		}
	}

	private bool OnFilter(object obj)
	{
		#region Initialize
		if (obj is Record record) { }
		else if (obj is ModelElement model) record = model.Source;
		else return false;

		if (HashList != null && HashList.CheckFailed(record.PrimaryKey)) return false;
		#endregion

		#region Filter
		// category
		if (Category is MarketCategory2Group.All or null)
		{
			if (HashList is null && string.IsNullOrEmpty(NameFilter)) return false;
		}
		else if (Category is MarketCategory2Group MarketCategory2Group && !MarketCategory2Group.Filter(record)) return false;
		else if (Category is MarketCategory3Group MarketCategory3Group && !MarketCategory3Group.Filter(record)) return false;

		// auctionable
		if (Auctionable &&
			!record.Attributes.Get<bool>("auctionable") &&
			!record.Attributes.Get<bool>("seal-renewal-auctionable")) return false;

		// grade
		if (Grade > 0 && record.Attributes.Get<sbyte>("item-grade") != Grade) return false;
		if (Job != default && !record.Attributes.Get<JobSeq[]>("equip-job-check").CheckSeq(Job)) return false;

		// rule
		// skip compare alias if integer
		if (string.IsNullOrEmpty(NameFilter)) return true;
		else if (int.TryParse(NameFilter, out int id))
		{
			if (record.PrimaryKey.Id == id) return true;
		}
		else if (record.Attributes.Get<string>("alias")?.Contains(NameFilter, StringComparison.OrdinalIgnoreCase) ?? false) return true;

		if (record.Attributes.Get<Record>("name2").GetText()?.Contains(NameFilter, StringComparison.OrdinalIgnoreCase) ?? false) return true;
		return false;
		#endregion
	}
	#endregion
}

public class SetFavoriteCommand : RecordCommand
{
	public event EventHandler? Executed;

	protected override List<string> Type => [];  // manual register

	protected override void Execute(Record record)
	{
		var key = record.PrimaryKey;

		if (record.OwnerName == "item")
		{
			// add or remove
			var favorites = MarketCategory2Group.Favorite.Favorites;
			if (!favorites.Remove(key)) favorites.Add(key);

			// save config
			MarketCategory2Group.Favorite.Favorites = favorites;
			Executed?.Invoke(null, EventArgs.Empty);
		}
	}
}