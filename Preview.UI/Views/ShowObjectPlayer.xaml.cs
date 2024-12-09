using System.Windows.Controls;
using System.Windows.Input;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.BNS.Conversion;
using HandyControl.Data;
using Newtonsoft.Json;
using Xylia.Preview.UI.Audio;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Views;
public partial class ShowObjectPlayer
{
	#region Constructors
	public ShowObjectPlayer()
	{
		DataContext = _viewModel = new ShowObjectPlayerViewModel();
		InitializeComponent();

#if DEVELOP
		//_viewModel.Source = Xylia.Preview.Common.Globals.GameProvider.LoadObject<UShowObject>(path);
#endif
	}

	public ShowObjectPlayer(UShowObject Source) : this()
	{
		_viewModel.Source = Source;
	}
	#endregion

	#region Methods
	private void OnSearchStarted(object sender, FunctionEventArgs<string> e)
	{

	}

	private void OnSelectedItem(object sender, SelectionChangedEventArgs e)
	{
		TextEditor.Text = JsonConvert.SerializeObject(_viewModel.SelectedKey, Formatting.Indented);
	}

	private void OnDoubleClick(object sender, MouseButtonEventArgs e)
	{
		var wave = _viewModel.SelectedKey.GetWave();
		if (wave != null)
		{
			if (audioPlayer is null)
			{
				audioPlayer = new();
				audioPlayer.Closed += (_, _) => audioPlayer = null;
				audioPlayer.Show();
			}

			audioPlayer.Load(new AudioFile(wave, "ogg")
			{
				Name = _viewModel.SelectedKey!.Name,
			});
		}
	}
	#endregion

	#region Fields
	ShowObjectPlayerViewModel _viewModel;
	AudioPlayer? audioPlayer;
	#endregion
}