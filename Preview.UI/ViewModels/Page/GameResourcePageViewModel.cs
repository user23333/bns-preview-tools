﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CUE4Parse.BNS;
using CUE4Parse.BNS.Conversion;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Pak;
using CUE4Parse.Utils;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using Microsoft.Win32;
using Newtonsoft.Json;
using Ookii.Dialogs.Wpf;
using Serilog;
using SkiaSharp;
using SkiaSharp.Views.WPF;
using Xylia.Preview.UI.Common;
using Xylia.Preview.UI.Common.Converters;
using Xylia.Preview.UI.Helpers.Output.Textures;
using Xylia.Preview.UI.Views;
using Xylia.Preview.UI.Views.Dialogs;
using Xylia.Preview.UI.Views.Editor;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Xylia.Preview.UI.ViewModels;
internal partial class GameResourcePageViewModel : ObservableObject
{
	#region Common
	[RelayCommand]
	public void BrowerOutFolder() => new SettingsView().ShowDialog();

	[RelayCommand]
	public void BrowerGameFolder() => new DatabaseManager(Application.Current.MainWindow, true).ShowDialog();
	#endregion

	#region Asset
	[ObservableProperty] ObservableCollection<PackageParam> packages = [];
	[ObservableProperty] PackageParam? selectedPackage;
	[ObservableProperty] PackageParam.FileParam? selectedFile;

	public void LoadPackage(string path)
	{
		var p = JsonConvert.DeserializeObject<PackageParam>(File.ReadAllText(path))!;
		foreach (var f in p.Files) f.Owner = p;

		Packages.Add(p);
	}

	[RelayCommand]
	private void LoadPackageInfo()
	{
		var dialog = new VistaOpenFileDialog()
		{
			Filter = "configuration file|*.json",
		};
		if (dialog.ShowDialog() != true) return;

		LoadPackage(dialog.FileName);
		SelectedPackage = Packages.FirstOrDefault();
	}

	[RelayCommand]
	private void SavePackageInfo()
	{
		if (SelectedPackage is null) return;

		var dialog = new SaveFileDialog()
		{
			Filter = "configuration file|*.json",
			FileName = $"RepackInfo_{SelectedPackage.Name}.json",
		};
		if (dialog.ShowDialog() != true) return;

		File.WriteAllText($"{dialog.FileName}", JsonConvert.SerializeObject(SelectedPackage, Formatting.Indented));
	}

	[RelayCommand]
	private void AddPackageInfo()
	{
		Packages.Add(new());
	}

	[RelayCommand]
	private void RemovePackageInfo()
	{
		if (SelectedPackage != null)
			Packages.Remove(SelectedPackage);
	}


	[RelayCommand]
	private async Task AddFileInfo()
	{
		var file = await Dialog.Show<VfsFileInfoDialog>().GetResultAsync<PackageParam.FileParam>();
		if (file is null) return;

		file.Owner = this.SelectedPackage;
		this.SelectedPackage?.Files.Add(file);
	}

	[RelayCommand]
	private async Task UpdateFileInfo()
	{
		var dialog = new VfsFileInfoDialog() { Result = SelectedFile };
		if (dialog.Result is null) return;

		await Dialog.Show(dialog).GetResultAsync<PackageParam.FileParam>();

		// update
		var temp = SelectedPackage;
		SelectedPackage = null;
		SelectedPackage = temp;
	}

	[RelayCommand]
	private void RemoveFileInfo()
	{
		this.SelectedPackage.Files.Remove(SelectedFile);
	}


	public static async Task UeExporter(string filter, bool ContainType) => await Task.Run(() =>
	{
		using var provider = new GameFileProvider(UserSettings.Default.GameFolder);
		filter = provider.FixPath(filter) ?? filter;

		Parallel.ForEach(provider.Files.Values, gamefile =>
		{
			if (gamefile.Extension != "uasset" || !gamefile.Path.Contains(filter.SubstringBeforeLast('.'), StringComparison.OrdinalIgnoreCase))
				return;

			try
			{
				new Exporter(UserSettings.Default.OutputFolderResource)
					.Run(provider.LoadPackage(gamefile), ContainType);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		});
	});

	public static async Task UeRepack(string folder, IEnumerable<PackageParam> packages) => await Task.Run(() =>
	{
		foreach (var package in packages)
		{
			var reader = new MyPakFileReader(package.MountPoint);
			foreach (var file in package.Files)
			{
				if (!file.IsValid) continue;

				reader.Add(file.Path, file.Vfs, file.Compression);
			}

			reader.WriteToDir(folder, package.Name + ".pak");
		}
	});
	#endregion

	#region Icon
	[ObservableProperty] string? icon_OutputFolder = Path.Combine(UserSettings.Default.OutputFolderResource, "Extract");
	[ObservableProperty] string? icon_ItemListPath;

	[RelayCommand]
	private void Icon_BrowerOutputFolder()
	{
		var dialog = new VistaFolderBrowserDialog() { };
		if (dialog.ShowDialog() == true) Icon_OutputFolder = dialog.SelectedPath;
	}

	[RelayCommand]
	private void Icon_BrowerItemList()
	{
		var dialog = new VistaOpenFileDialog() { Filter = @"|*.chv|All files|*.*" };
		if (dialog.ShowDialog() == true) Icon_ItemListPath = dialog.FileName;
	}


	readonly CancellationTokenSource?[] sources = new CancellationTokenSource[20];

	public void Run(IconOutBase instance, string format, int id) => Task.Run(() =>
	{
		#region Token
		var source = this.sources[id];
		if (source != null)
		{
			if (MessageBox.Show(StringHelper.Get("Text.TaskCancel_Ask_Exist"), StringHelper.Get("Message_Tip"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				source?.Cancel();
				source = null;
			}

			return;
		}

		source = this.sources[id] = new CancellationTokenSource();
		#endregion

		#region Action 
		var process = Growl2.Add(new()
		{
			Message = StringHelper.Get("Text.TaskBegin"),
			Stop = new Action(() =>
			{
				source.Dispose();
				this.sources[id] = null;
			}),
		});
		#endregion

		try
		{
			DateTime start = DateTime.Now;
			instance.Initialize(source.Token);
			instance.Execute(format, (arg) =>
				process.Message = StringHelper.Get("Text.TaskProcess2", arg, StringHelper.Get("IconOut_" + instance.GetType().Name)),
				source.Token);

			process.Type = InfoType.Success;
			process.Message = StringHelper.Get("Text.TaskCompleted2", TimeConverter.Convert(DateTime.Now - start, null));
		}
		catch (Exception ee)
		{
			process.Type = InfoType.Error;
			process.Message = StringHelper.Get("Text.TaskException", ee.Message);
			Log.Fatal(ee, "Exception at IconOut");
		}
		finally
		{
			instance.Dispose();
			process.Stop?.Invoke();
			ProcessFloatWindow.ClearMemory();
		}
	});
	#endregion

	#region Merge
	SKBitmap _mergeIcon_Source;
	public SKBitmap MergeIcon_Source
	{
		get => _mergeIcon_Source;
		set
		{
			_mergeIcon_Source = value;
			MergeIcon();
		}
	}

	bool _mergeIcon_BackgroundMode = true;
	public bool MergeIcon_BackgroundMode
	{
		get => _mergeIcon_BackgroundMode;
		set
		{
			_mergeIcon_BackgroundMode = value;
			MergeIcon();
		}
	}


	NameObject<sbyte> _mergeIcon_Grade;
	public NameObject<sbyte> MergeIcon_Grade
	{
		get => _mergeIcon_Grade;
		set
		{
			_mergeIcon_Grade = value;
			MergeIcon();
		}
	}
	public List<NameObject<sbyte>> GradeList => new()
	{
		{ new(StringHelper.Get("MergeIcon_Grade1"),1) },
		{ new(StringHelper.Get("MergeIcon_Grade2"),2) },
		{ new(StringHelper.Get("MergeIcon_Grade3"),3) },
		{ new(StringHelper.Get("MergeIcon_Grade4"),4) },
		{ new(StringHelper.Get("MergeIcon_Grade5"),5) },
		{ new(StringHelper.Get("MergeIcon_Grade6"),6) },
		{ new(StringHelper.Get("MergeIcon_Grade7"),7) },
		{ new(StringHelper.Get("MergeIcon_Grade8"),8) },
		{ new(StringHelper.Get("MergeIcon_Grade9"),9) },
	};


	NameObject<SKBitmap> _mergeIcon_BottomLeft;
	public NameObject<SKBitmap> MergeIcon_BottomLeft
	{
		get => _mergeIcon_BottomLeft;
		set
		{
			_mergeIcon_BottomLeft = value;
			MergeIcon();
		}
	}

	public List<NameObject<SKBitmap>> BottomLeftList =>
	[
		GetImage("None"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/Weapon_Lock_01"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/Weapon_Lock_02"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/Weapon_Lock_03"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/Weapon_Lock_04"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/Weapon_Lock_05"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/Weapon_Lock_06"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_lock"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_lock_2"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_KeyLock"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_AccountContents"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_Achievement"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_Area"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_Awaken"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_DuelRating"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_expired"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_olditem_1"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_olditem_2"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_olditem_3"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_package"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/unuseable_PcRoomOnly"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/useable_PcRoomOnly"),
	];


	NameObject<SKBitmap> _mergeIcon_TopRight;
	public NameObject<SKBitmap> MergeIcon_TopRight
	{
		get => _mergeIcon_TopRight;
		set
		{
			_mergeIcon_TopRight = value;
			MergeIcon();
		}
	}
	public List<NameObject<SKBitmap>> TopRightList =>
	[
		GetImage("None"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/SlotItem_marketBusiness"),
		GetImage("Art/GameUI/Resource/GameUI_Icon3_R/SlotItem_privateSale"),
	];



	[ObservableProperty]
	ImageSource mergeIcon_Image;

	public void MergeIcon()
	{
		var grade = _mergeIcon_Grade?.Value ?? 1;
		var info = GetImage(MergeIcon_BackgroundMode ?
			$"Art/GameUI/Resource/GameUI_Window_R/ItemIcon_Bg_Grade_{grade}" :
			$"Art/GameUI/Resource/GameUI_Window/ItemIcon_Bg_Grade_{grade}");

		var bitmap = info.Value;

		if (_mergeIcon_Source != null)
			bitmap = bitmap.Compose(_mergeIcon_Source);

		bitmap = bitmap.Compose(_mergeIcon_BottomLeft?.Value);
		bitmap = bitmap.Compose(_mergeIcon_TopRight?.Value);

		MergeIcon_Image = bitmap.ToWriteableBitmap();
	}

	[RelayCommand]
	public void MergeIcon_Save()
	{
		var dialog = new SaveFileDialog()
		{
			FileName = "item",
			Filter = "png file|*.png",
		};

		if (dialog.ShowDialog() == true)
		{
			using var fs = new FileStream(dialog.FileName, FileMode.Create);
			var encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create((BitmapSource)MergeIcon_Image));
			encoder.Save(fs);
			fs.Flush();
		}
	}


	public static NameObject<SKBitmap> GetImage(string path)
	{
		if (path == "None") return new(StringHelper.Get("Text.None"), null);

		var resource = new Uri($"/Preview.UI;component/Content/{path}.png", UriKind.Relative);
		using var stream = Application.GetResourceStream(resource).Stream;
		return new(StringHelper.Get(path.SubstringAfterLast('/')), SKBitmap.Decode(stream));
	}
	#endregion
}

public class PackageParam
{
	public string Name { get; set; } = "Xylia_P";

	public string MountPoint { get; set; } = "BNSR/Content";

	public ObservableCollection<FileParam> Files { get; set; } = [];

	public class FileParam
	{
		public string? Vfs { get; set; }

		public string? Path { get; set; }

		public CompressionMethod Compression { get; set; }


		[JsonIgnore]
		public bool IsValid => File.Exists(Path);

		[JsonIgnore]
		internal PackageParam? Owner { get; set; }

		public override string ToString() => string.Join('/', Owner!.MountPoint, Vfs);
	}
}