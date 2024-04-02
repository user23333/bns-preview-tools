﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Controls;
using Xylia.Preview.UI.Extensions;
using Xylia.Preview.UI.GameUI.Scene.Game_Tooltip;
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
		RegisterTemplate<ItemTooltipPanel>(typeof(Item));
		RegisterTemplate<Skill3ToolTipPanel_1>(typeof(Skill3));
	}

	public RecordTooltip()
	{
		Loaded += OnLoaded;
	}
	#endregion

	#region Methods
	protected override void OnVisualParentChanged(DependencyObject oldParent)
	{
		base.OnVisualParentChanged(oldParent);

		// fix issue when backgorund is white
		var parent = this.GetParent<Border>();
		if (parent != null) parent.Background = new SolidColorBrush(BnsCustomWindowWidget.BackgroundColor);
	}

	private void OnLoaded(object sender, RoutedEventArgs e)
	{
		FrameworkElement? visualTree;
		switch (DataContext)
		{
			case ModelElement model when ModelTemplate.TryGetValue(model.GetType(), out var template):
			{
				visualTree = template.Value;
				visualTree.DataContext = DataContext;
			}
			break;

			case Record record when RecordTemplate.TryGetValue(record.Owner.Name ?? string.Empty, out var template):
			{
				visualTree = template.Value;
				visualTree.DataContext = record.As<ModelElement>();
			}
			break;

			default:
				if (DataContext is null) return;
				visualTree = new TextBlock();
				visualTree.SetValue(TextBlock.TextProperty, DataContext.ToString());
				break;
		}

		this.Content = visualTree;
	}
	#endregion


	#region Data
	static readonly Dictionary<string, Lazy<FrameworkElement>> RecordTemplate = new(StringComparer.OrdinalIgnoreCase);
	static readonly Dictionary<Type, Lazy<FrameworkElement>> ModelTemplate = [];

	static void RegisterTemplate<T>(Type type, string? name = null) where T : FrameworkElement, new()
	{
		name ??= type.Name.TitleLowerCase();
		ModelTemplate[typeof(Item)] = RecordTemplate[name] = new(() => new T());
	}
	#endregion
}