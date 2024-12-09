using System.ComponentModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CUE4Parse.BNS.Assets.Exports;

namespace Xylia.Preview.UI.ViewModels;
internal class ShowObjectPlayerViewModel : ObservableObject
{
	#region Properties 
	private UShowObject? _source;
	public UShowObject? Source
	{
		get => _source;
		set
		{
			SetProperty(ref _source, value);
			if (value is null) return;

			// create view
			EventKeys = CollectionViewSource.GetDefaultView(value.EventKeys.Select(x => x.Load()));
			OnPropertyChanged(nameof(Source));
		}
	}

	private ICollectionView? _eventKeys;
	public ICollectionView? EventKeys
	{
		get => _eventKeys;
		set => SetProperty(ref _eventKeys, value);
	}

	private ShowKeyBase? _selectedKey;
	public ShowKeyBase? SelectedKey
	{
		get => _selectedKey;
		set => SetProperty(ref _selectedKey, value);
	}
	#endregion
}