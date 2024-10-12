using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Xylia.Preview.Data.Engine.BinData.Helpers;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Common.Interactivity;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Auction;
internal partial class AuctionPanelViewModel : ObservableObject
{
	#region Fields
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

	private object? _tag;
	public object? Tag
	{
		get => _tag;
		set
		{
			SetProperty(ref _tag, value);
			Changed?.Invoke(this, EventArgs.Empty);
		}
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