using System.Windows;
using System.Windows.Controls;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip2;
using Xylia.Preview.UI.Views;

namespace Xylia.Preview.UI.Common.Interactivity;
/// <summary>
/// Register tooltip in <see cref="TableView"/>
/// </summary>
public sealed class RecordTooltip : ContentControl
{
	#region Constructors
	static RecordTooltip()
	{
		RegisterTemplate<EffectTooltipPanel>(typeof(Effect));
		RegisterTemplate<GlyphInventoryTooltipPanel>(typeof(Glyph));
		RegisterTemplate<RewardTooltipPanel>(typeof(GlyphReward));
		RegisterTemplate<ItemTooltipPanel>(typeof(Item));
		RegisterTemplate<RewardTooltipPanel>(typeof(ItemCombination));
		RegisterTemplate<RewardTooltipPanel>(typeof(WorldAccountCombination));
		RegisterTemplate<NpcTooltipPanel>(typeof(Npc));
		RegisterTemplate<RewardTooltipPanel>(typeof(Reward));
		RegisterTemplate<Skill3ToolTipPanel_1>(typeof(Skill3));
	}

	public RecordTooltip()
	{
		DataContextChanged += OnDataContextChanged;
	}
	#endregion

	#region Methods
	private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		FrameworkElement? visualTree;
		switch (e.NewValue)
		{
			case ModelElement model when ModelTemplate.TryGetValue(model.GetBaseType(typeof(ModelElement)), out var template):
				visualTree = template.Value;
				visualTree.DataContext = DataContext;
				break;

			case Record record when RecordTemplate.TryGetValue(record.Owner.Name, out var template):
				visualTree = template.Value;
				visualTree.DataContext = record.As<ModelElement>();
				break;

			default:
				visualTree = new TextBlock();
				visualTree.SetValue(TextBlock.TextProperty, DataContext?.ToString());
				break;
		}

		Content = visualTree;
		Name = visualTree?.Name;
	}
	#endregion


	#region Data
	static readonly Dictionary<string, Lazy<FrameworkElement>> RecordTemplate = new(StringComparer.OrdinalIgnoreCase);
	static readonly Dictionary<Type, Lazy<FrameworkElement>> ModelTemplate = [];

	static void RegisterTemplate<T>(Type type, string? name = null) where T : FrameworkElement, new()
	{
		name ??= type.Name.TitleLowerCase();
		ModelTemplate[type] = RecordTemplate[name] = new(() => new T());
	}
	#endregion
}