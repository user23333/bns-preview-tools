using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Auction;
internal partial class AuctionPanelViewModel : ObservableObject
{
	public ICollectionView? Source;
	public event EventHandler? Changed;

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


	public bool WorldBoss;
	public MarketCategory2Seq MarketCategory2;
	public MarketCategory3Seq MarketCategory3;

	public void ByTag(object data)
	{
		MarketCategory2 = default;
		MarketCategory3 = default;
		WorldBoss = data is "WorldBoss";

		switch (data)
		{
			case MarketCategory2Seq seq: MarketCategory2 = seq; break;
			case MarketCategory3Seq seq: MarketCategory3 = seq; break;
		}

		Changed?.Invoke(this, EventArgs.Empty);
	}
}