using System.Windows.Controls;
using System.Windows.Input;
using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.BNS.Conversion;
using CUE4Parse.UE4.Assets.Exports;
using HandyControl.Data;
using Newtonsoft.Json;
using Xylia.Preview.UI.Audio;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Views;
public partial class ShowObjectPlayer
{
	#region Constructors
	readonly ShowObjectPlayerViewModel _viewModel;

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
		PlaySound(_viewModel.SelectedKey!);
	}

	internal static void PlaySound(UObject obj)
	{
		var wave = obj.GetWave();
		if (wave != null)
		{
			var file = new AudioFile(wave, "ogg")
			{	
				Path = obj.GetFullName(),
				Name = obj.Name,
			};

			var player = AudioPlayer.Instance;
			player.Load(file);
			player.Play(file);
			player.Show();
		}
	}
	#endregion
}