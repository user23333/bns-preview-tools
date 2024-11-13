using System.Windows;
using HandyControl.Controls;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip2;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views.Selector;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Xylia.Preview.UI.Services;
/// <summary>
/// Initialize process
/// </summary>
internal class RegisterService : IService
{
	public bool Register()
	{
		// effects 
		UserSettings.Default.UsePerformanceMonitor = UserSettings.Default.UsePerformanceMonitor;
		UserSettings.Default.CopyMode = UserSettings.Default.CopyMode;
		UserSettings.Default.SkinType = UserSettings.Default.SkinType;

		// init	   
		Globals.MessageBox = new MessageService();
		Globals.DatSelector = DatSelectDialog.Instance;
		RegisterTemplate();

		// ask
		if (UserSettings.Default.UseUserDefinition)
		{
			Growl.Ask(StringHelper.Get("Settings_UseUserDefinition_Ask"), isConfirmed =>
			{
				UserSettings.Default.UseUserDefinition = isConfirmed;
				return true;
			});
		}

		return true;
	}

	private static void RegisterTemplate()
	{
		BnsTooltipHolder.RegisterTemplate<AttractionMapUnitToolTipPanel>(typeof(IAttraction));
		BnsTooltipHolder.RegisterTemplate<EffectTooltipPanel>(typeof(Effect));
		BnsTooltipHolder.RegisterTemplate<GlyphInventoryTooltipPanel>(typeof(Glyph));
		BnsTooltipHolder.RegisterTemplate<RewardTooltipPanel>(typeof(GlyphReward));
		BnsTooltipHolder.RegisterTemplate<ItemTooltipPanel>(typeof(Item));
		BnsTooltipHolder.RegisterTemplate<RewardTooltipPanel>(typeof(ItemCombination));
		BnsTooltipHolder.RegisterTemplate<RewardTooltipPanel>(typeof(WorldAccountCombination));
		BnsTooltipHolder.RegisterTemplate<NpcTooltipPanel>(typeof(Npc));
		BnsTooltipHolder.RegisterTemplate<RewardTooltipPanel>(typeof(Reward));
		BnsTooltipHolder.RegisterTemplate<Skill3ToolTipPanel_1>(typeof(Skill3));
	}
}

public class MessageService : IMessageBox
{
	public Func<string, bool> Show => (s) => MessageBox.Show(s, null, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
}