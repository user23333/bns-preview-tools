using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Highlighting;

namespace Xylia.Preview.UI.Resources.Themes;
internal static class SkinHelpers
{
	public static ResourceDictionary GetDayNight(bool? night)
	{
		// Indeterminate represents automatic
		night ??= DateTime.Now.Hour < 6 || DateTime.Now.Hour >= 18;

		return new ResourceDictionary
		{
			Source = new Uri($"pack://application:,,,/Preview.UI;component/Resources/Themes/Skins/Basic/{(night.Value ? "Dark" : "Day")}.xaml")
		};
	}

	public static ResourceDictionary GetSkin(SkinType skin)
	{
		try
		{
			return new ResourceDictionary
			{
				Source = new Uri($"pack://application:,,,/Preview.UI;component/Resources/Themes/Skins/{skin}.xaml")
			};
		}
		catch
		{
			if (skin == SkinType.Default) throw;

			return GetSkin(SkinType.Default);
		}
	}

	public static void UpdateXshd(string rule)
	{
		var highlighting = HighlightingManager.Instance.GetDefinition(rule);
		if (highlighting is null) return;
		rule += "_";

		foreach (var color in highlighting.NamedHighlightingColors)
		{
			if (Application.Current.TryFindResource(rule + color.Name) is Color c)
				color.Foreground = new SimpleHighlightingBrush(c);
		}
	}
}

public enum SkinType
{
	Default,

	Violet,
}