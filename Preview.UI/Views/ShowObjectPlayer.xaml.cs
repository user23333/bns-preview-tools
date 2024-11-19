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
		string path = @"Tuto_45LV_Def_Voice.q_1669_1_icon_msg_1_voice1";
		_viewModel.ShowObject = Xylia.Preview.Common.Globals.GameProvider.LoadObject<UShowObject>(path);
#endif
	}

	public ShowObjectPlayer(UShowObject Source) : this()
	{
		_viewModel.ShowObject = Source;
	}
	#endregion

	#region Methods
	private void OnSearchStarted(object sender, FunctionEventArgs<string> e)
	{

	}

	private void OnSelectedItem(object sender, SelectionChangedEventArgs e)
	{
		if (ObjectList.SelectedItem is not ShowKeyBase value) return;

		TextEditor.Text = JsonConvert.SerializeObject(value, Formatting.Indented);
	}

	private void OnDoubleClick(object sender, MouseButtonEventArgs e)
	{
		if (ObjectList.SelectedItem is not ShowKeyBase value) return;

		var wave = value.GetWave();
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
				Name = value.Name,
			});
		}
	}
	#endregion

	#region Fields
	ShowObjectPlayerViewModel _viewModel;
	AudioPlayer? audioPlayer;
	#endregion
}