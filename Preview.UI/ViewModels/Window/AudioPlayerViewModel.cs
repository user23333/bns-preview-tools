using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Xylia.Preview.UI.Audio;

namespace Xylia.Preview.UI.ViewModels;
internal partial class AudioPlayerViewModel : ObservableObject, IDisposable
{
	#region Fields
	private readonly PlaybackService service;

	[ObservableProperty] private bool showPause;
	[ObservableProperty] private bool isLoadingTrack;
	public bool ShowLoopNone => service.LoopMode == LoopMode.None;
	public bool ShowLoopOne => service.LoopMode == LoopMode.One;
	public bool ShowLoopAll => service.LoopMode == LoopMode.All;
	public bool IsShuffle { get => service.Shuffle; set => service.Shuffle = value; }

	public TimeSpan CurrentTime => service.GetCurrentTime;
	public TimeSpan TotalTime => service.GetTotalTime;
	public double Progress { get => service.Progress; set => service.SkipProgress(value); }
	public bool CanReportProgress => !service.IsStopped;

	public float Volume { get => service.Volume; set => service.Volume = value; }
	public bool IsMute { get => service.Mute || Volume == 0; set => service.Mute = value; }


	public ICollectionView AudioFilesView { get; set; }

	public ICollectionView AudioDevicesView { get; set; }

	public AudioFile Selected
	{
		get => service.CurrentTrack;
		set => service.CurrentTrack = value;
	}

	public AudioDevice SelectedAudioDevice
	{
		get => service.audioDevice;
		set
		{
			// Due to two-way binding, this can be null when the list is being filled.
			if (value != null)
			{
				service.SwitchAudioDevice(value);
			}

			OnPropertyChanged();
		}
	}
	#endregion

	#region Constructors
	public AudioPlayerViewModel()
	{
		service = new();

		// Event handlers
		service.PlaybackFailed += (_, __) => this.ShowPause = false;
		service.PlaybackPaused += (_, __) => this.ShowPause = false;
		service.PlaybackResumed += (_, __) => this.ShowPause = true;
		service.PlaybackStopped += (_, __) => this.ShowPause = false;
		service.PlaybackSuccess += (_, __) => { this.ShowPause = true; OnPropertyChanged(nameof(Selected)); };
		service.PlaybackProgressChanged += (_, __) => this.UpdateTime();
		service.PlaybackLoopChanged += (_, __) => this.UpdateLoop();
		service.PlaybackShuffleChanged += (_, __) => OnPropertyChanged(nameof(IsShuffle));
		service.LoadingTrack += (isLoadingTrack) => this.IsLoadingTrack = isLoadingTrack;

		// Volume
		service.PlaybackVolumeChanged += (_, __) => UpdateVolume();
		service.PlaybackMuteChanged += (_, __) => OnPropertyChanged(nameof(IsMute));

		// Initial status
		this.AudioFilesView = CollectionViewSource.GetDefaultView(service.Queue);
		this.AudioDevicesView = CollectionViewSource.GetDefaultView(service.GetAllAudioDevicesAsync());

		service.Volume = 1;
	}
	#endregion

	#region Commands
	[RelayCommand] async Task PlayPause() => await service.PlayOrPauseAsync();
	[RelayCommand] async Task PlayNew(AudioFile file) => await service.PlaySelectedAsync(file ?? Selected);
	[RelayCommand] async Task Previous() => await service.PlayPreviousAsync();
	[RelayCommand] async Task Next() => await service.PlayNextAsync();

	[RelayCommand] void Loop() => service.LoopMode = service.LoopMode switch
	{
		LoopMode.None => LoopMode.All,
		LoopMode.All => LoopMode.One,
		LoopMode.One => LoopMode.None,
		_ => LoopMode.None,
	};
	[RelayCommand] void Shuffle() => IsShuffle = !IsShuffle;
	[RelayCommand] void Mute() => IsMute = !IsMute;


	[RelayCommand]
	public void Remove(AudioFile file)
	{
		service.Queue.Remove(file ?? Selected);
	}

	[RelayCommand]
	public void Save(AudioFile file)
	{
		file ??= Selected;
		var saveFileDialog = new SaveFileDialog
		{
			Title = "Save Audio",
			FileName = file.Name,
			Filter = "ogg file|*.ogg",
			//InitialDirectory = UserSettings.Default.AudioDirectory
		};
		if (saveFileDialog.ShowDialog() != true) return;
		var path = saveFileDialog.FileName;

		using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
		using var writer = new BinaryWriter(stream);
		writer.Write(file.Data);
		writer.Flush();
	}

	[RelayCommand]
	public void SavePlaylist()
	{
		Application.Current.Dispatcher.Invoke(() =>
		{
			foreach (var a in service.Queue)
				Save(a);
		});
	}
	#endregion

	#region Methods
	private void UpdateTime()
	{
		OnPropertyChanged(nameof(CurrentTime));
		OnPropertyChanged(nameof(TotalTime));
		OnPropertyChanged(nameof(Progress));
		OnPropertyChanged(nameof(CanReportProgress));
	}

	private void UpdateLoop()
	{
		OnPropertyChanged(nameof(this.ShowLoopNone));
		OnPropertyChanged(nameof(this.ShowLoopOne));
		OnPropertyChanged(nameof(this.ShowLoopAll));
	}

	private void UpdateVolume()
	{
		OnPropertyChanged(nameof(Volume));
		OnPropertyChanged(nameof(IsMute));
	}

	public void AddToPlaylist(AudioFile file)
	{
		service.Queue.Add(file);
	}

	public void Dispose()
	{
		service.Stop();
	}
	#endregion
}