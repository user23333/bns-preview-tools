using System.Collections.ObjectModel;
using System.Windows;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Properties;
using Xylia.Preview.UI.Resources.Themes;
using Xylia.Preview.UI.Services;

namespace Xylia.Preview.UI.ViewModels;
internal partial class UserSettings : Settings
{
	#region Constructors 
	public new static UserSettings Default { get; } = new UserSettings();

	private UserSettings()
	{
		Settings.Default = this;
	}
	#endregion

	#region Common 
	public Size ContainerSize { get; set; } = new Size(900, 600);

	public ObservableCollection<ELanguage> Languages => new(StringHelper.EnumerateLanguages());

	/// <summary>
	/// Gets or sets public language
	/// </summary>
	public ELanguage Language
	{
		set => SetValue(StringHelper.Current!.Language = value);
		get
		{
			var e = GetValue<string>().ToEnum<ELanguage>();
			return e > ELanguage.None ? e : StringHelper.Current!.Language;
		}
	}

	/// <summary>
	/// Gets or sets update mode
	/// </summary>
	public UpdateService.UpdateMode UpdateMode
	{
		get => (UpdateService.UpdateMode)GetValue<int>();
		set
		{
			SetValue((int)value);
			new UpdateService().Register();
		}
	}

	/// <summary>
	/// Gets or sets notice id
	/// </summary>
	public int NoticeId
	{
		get => GetValue<int>();
		set { if (value > 0) SetValue(value); }
	}

	/// <summary>
	/// Gets or sets night mode
	/// </summary>
	public bool? NightMode
	{
		get => GetValue<bool?>();
		set
		{
			SetValue(value);
			((App)Application.Current).UpdateSkin(SkinType, NightMode);
		}
	}

	/// <summary>
	/// Gets or sets skin type
	/// </summary>
	public SkinType SkinType
	{
		get => (SkinType)GetValue<int>();
		set
		{
			SetValue((int)value);
			((App)Application.Current).UpdateSkin(value, NightMode);
		}
	}

	/// <summary>
	/// Gets or sets log clear day
	/// </summary>
	public int KeepLogTime
	{
		get => int.TryParse(GetValue<string>(), out var result) ? result : 15;
		set => SetValue(value);
	}

	public bool UsePerformanceMonitor
	{
		get => GetValue<bool>();
		set
		{
			SetValue(value);

			if (value) ProcessFloatWindow.Instance.Show();
			else ProcessFloatWindow.Instance.Close();
		}
	}
	#endregion
}