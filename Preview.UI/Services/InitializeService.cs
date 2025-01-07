using System.Windows;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.GameUI.Scene.Game_Achievement;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip2;
using Xylia.Preview.UI.Views.Dialogs;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Xylia.Preview.UI.Services;
/// <summary>
/// Initialize process
/// </summary>
internal class InitializeService : IService
{
	public static void InitializeApp()
	{
		new ServiceManager() { new LogService(), new JumpListService(), new InitializeService() }.RegisterAll();
	}

	void IService.Register()
	{
		Globals.MessageBox = new MessageService();
		Globals.DatSelector = DatSelectDialog.Instance;

		#region Template
		BnsTooltipHolder.RegisterTemplate<AchievementDetailPanel>(typeof(Achievement));
		BnsTooltipHolder.RegisterTemplate<EffectTooltipPanel>(typeof(Effect));
		BnsTooltipHolder.RegisterTemplate<GlyphInventoryTooltipPanel>(typeof(Glyph));
		BnsTooltipHolder.RegisterTemplate<RewardTooltipPanel>(typeof(GlyphReward));
		BnsTooltipHolder.RegisterTemplate<ItemTooltipPanel>(typeof(Item));
		BnsTooltipHolder.RegisterTemplate<RewardTooltipPanel>(typeof(ItemCombination));
		BnsTooltipHolder.RegisterTemplate<ItemGraphReceipeTooltipPanel>(typeof(ItemGraph));
		BnsTooltipHolder.RegisterTemplate<NpcTooltipPanel>(typeof(Npc));
		BnsTooltipHolder.RegisterTemplate<RewardTooltipPanel>(typeof(Reward));
		BnsTooltipHolder.RegisterTemplate<Skill3ToolTipPanel_1>(typeof(Skill3));
		BnsTooltipHolder.RegisterTemplate<RewardTooltipPanel>(typeof(WorldAccountCombination));
		BnsTooltipHolder.RegisterTemplate<AttractionMapUnitToolTipPanel>(typeof(IAttraction));
		#endregion
	}

	private class MessageService : IMessageBox
	{
		public Func<string, bool> Show => (s) => MessageBox.Show(s, null, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
	}
}