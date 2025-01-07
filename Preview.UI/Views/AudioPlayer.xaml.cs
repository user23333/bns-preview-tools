using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using HandyControl.Data;

using Xylia.Preview.UI.Audio;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Views;
public partial class AudioPlayer
{
	#region Constructors
	static AudioPlayer? _instance;
	readonly AudioPlayerViewModel _viewModel;

	public AudioPlayer()
	{
		DataContext = _viewModel = new AudioPlayerViewModel();
		InitializeComponent();
	}

	public static AudioPlayer Instance
	{
		get
		{
			if (_instance is null ||
				(PresentationSource.FromVisual(_instance)?.IsDisposed ?? true))
				_instance = new();

			return _instance;
		}
	}
	#endregion

	#region Methods
	public void Load(AudioFile file) => _viewModel.AddToPlaylist(file);
	public void Play(AudioFile file) => _viewModel.PlayNewCommand.Execute(file);

	private void OnDeviceSwap(object sender, SelectionChangedEventArgs e)
	{
		//if (sender is not ComboBox { SelectedItem: MMDevice selectedDevice })
		//    return;

		//UserSettings.Default.AudioDeviceId = selectedDevice.DeviceID;
		//_applicationView.AudioPlayer.Device();
	}

	private void OnAudioFileMouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		Play(_viewModel.Selected);
	}

	private void OnSearchStarted(object sender, FunctionEventArgs<string> e)
	{
		var filters = e.Info.Trim().Split(' ');
		_viewModel.AudioFilesView.Filter = o => { return o is AudioFile audio && filters.All(x => audio.Name.Contains(x, StringComparison.OrdinalIgnoreCase)); };
		_viewModel.AudioFilesView.Refresh();
	}

	// Drop
	private void AudioList_Drop(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			var files = (string[])e.Data.GetData(DataFormats.FileDrop);
			foreach (var file in files) Load(new AudioFile(new FileInfo(file)));
		}
	}

	protected override void OnClosed(EventArgs e)
	{
		_viewModel.Dispose();

		if (_instance == this) _instance = null;
	}


	//------------------------------------------------------
	//
	//  Volumn
	//
	//------------------------------------------------------
	private void Grid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
	{
		if (!this.VolumeButtonPopup.IsOpen)
		{
			this.VolumeButtonPopup.IsOpen = true;
		}

		var StepValue = (float)1 / 100;
		this._viewModel.Volume += StepValue * Math.Sign(e.Delta);
	}

	private void VolumeButton_MouseEnter(object sender, MouseEventArgs e)
	{
		this.VolumeButtonPopup.IsOpen = false;
		this.VolumeButtonPopup.IsOpen = true;
	}
	#endregion
}