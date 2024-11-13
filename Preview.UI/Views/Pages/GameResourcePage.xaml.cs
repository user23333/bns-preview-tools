using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using HandyControl.Controls;
using SkiaSharp;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.UI.Common.Converters;
using Xylia.Preview.UI.Helpers.Output.Textures;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Views.Pages;
public partial class GameResourcePage
{
	#region Constructor
	readonly GameResourcePageViewModel _viewModel;

	public GameResourcePage()
	{
		DataContext = _viewModel = new GameResourcePageViewModel();

		InitializeComponent();
		this.Loaded += Page_Loaded;
	}

	private void Page_Loaded(object sender, RoutedEventArgs e)
	{
		Reset_Click(sender, e);
	}
	#endregion


	#region Asset
	private void AssetRepack_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop))
			e.Effects = DragDropEffects.Copy;
	}

	private void AssetRepack_DragDrop(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			var files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files.Length == 0) return;

			files.ForEach(_viewModel.LoadPackage);
		}
	}


	private async void Extract_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(Selector.Text))
				throw new WarningException(StringHelper.Get("Exception_InvalidPath"));

			DateTime dt = DateTime.Now;
			Extract.IsEnabled = false;
			await GameResourcePageViewModel.UeExporter(Selector.Text, OutputClassName.IsChecked ?? true);

			Growl.Success(StringHelper.Get("Text.TaskCompleted2", TimeConverter.Convert(DateTime.Now - dt, null)));
		}
		finally
		{
			Extract.IsEnabled = true;
		}
	}

	private async void Repack_Click(object sender, RoutedEventArgs e)
	{
		ArgumentNullException.ThrowIfNull(_viewModel.Packages);

		try
		{
			var folder = new DirectoryInfo(UserSettings.Default.GameFolder)
				.GetDirectories("Paks", SearchOption.AllDirectories)
				.FirstOrDefault()?.FullName ?? throw new DirectoryNotFoundException();
			folder = Path.Combine(folder, "Mods");

			Repack.IsEnabled = false;
			await GameResourcePageViewModel.UeRepack(folder, [.. _viewModel.Packages]);

			Growl.Success(StringHelper.Get("Text.TaskCompleted"));
		}
		finally
		{
			Repack.IsEnabled = true;
		}
	}
	#endregion

	#region Icon
	private void OutputItemIcon(object sender, RoutedEventArgs e)
	{
		// filter
		var ItemListPath = _viewModel.Icon_ItemListPath;
		if (!string.IsNullOrWhiteSpace(ItemListPath) && !File.Exists(ItemListPath)) throw new WarningException(StringHelper.Get("IconOut_Error1"));
		else if (this.FilterMode.IsChecked == true && !File.Exists(ItemListPath)) throw new WarningException(StringHelper.Get("IconOut_Error2"));

		// format
		var format = CheckFormat();

		_viewModel.Run(new ItemIcon(UserSettings.Default.GameFolder, _viewModel.Icon_OutputFolder + @"\Items")
		{
			HashesPath = ItemListPath,
			UseBackground = this.UseBackground.IsChecked == true,
			IsWhiteList = this.FilterMode.IsChecked == true,

		}, format, 0);
	}

	private void OutputGoodIcon(object sender, RoutedEventArgs e)
	{
		_viewModel.Run(new GoodIcon(UserSettings.Default.GameFolder, _viewModel.Icon_OutputFolder + @"\Goods"), string.Empty, 1);
	}

	private void OutputSkillIcon(object sender, RoutedEventArgs e)
	{
		var format = CheckFormat();

		_viewModel.Run(new SkillIcon(UserSettings.Default.GameFolder, _viewModel.Icon_OutputFolder + @"\Skills"), format, 2);
	}


	/// <summary>
	/// check format string
	/// </summary>
	/// <returns></returns>
	private string CheckFormat()
	{
		var format = this.NameFormat.Text;
		if (string.IsNullOrWhiteSpace(format) || !format.Contains('['))
		{
			throw new WarningException(StringHelper.Get("IconOut_Error3"));
		}
		else
		{
			format = format.ToLower();
			format = new Regex(@"\[\s+", RegexOptions.Singleline).Replace(format, "[");
			format = new Regex(@"\s+\]", RegexOptions.Singleline).Replace(format, "]");
		}

		return format!;
	}
	#endregion

	#region Merge
	private void MergeIcon_DragEnter(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop))
			e.Effects = DragDropEffects.Copy;
	}

	private void MergeIcon_DragDrop(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.FileDrop))
		{
			var files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files.Length == 0) return;

			try
			{
				_viewModel.MergeIcon_Source = SKBitmap.Decode(File.ReadAllBytes(files[0]));
			}
			catch
			{

			}
		}
	}

	private void Reset_Click(object sender, RoutedEventArgs e)
	{
		_viewModel.MergeIcon_Source = null;
		Combobox_Grade.SelectedIndex = 6;
		Combobox_BottomLeft.SelectedIndex = 0;
		Combobox_TopRight.SelectedIndex = 0;
	}
	#endregion
}