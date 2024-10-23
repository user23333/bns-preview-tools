using System.Windows;
using System.Windows.Controls;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.Controls;
public sealed class BnsTooltipHolder : ContentControl
{
	#region Constructors
	static BnsTooltipHolder()
	{
		DataContextProperty.OverrideMetadata(typeof(BnsTooltipHolder), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDataContextChanged)));
	}

	public BnsTooltipHolder()
	{

	}

	public BnsTooltipHolder(object data)
	{
		DataContext = data;
	}
	#endregion

	#region Methods
	private static void OnDataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		var tooltip = (BnsTooltipHolder)d;
		tooltip.OnDataContextChanged(e);
	}

	private void OnDataContextChanged(DependencyPropertyChangedEventArgs e)
	{
		FrameworkElement? visualTree;
		switch (e.NewValue)
		{
			case null: visualTree = null; break;
			case ModelElement model when ModelTemplate.TryGetValue(model.GetBaseType(typeof(ModelElement)), out var template):
				visualTree = template.Invoke();
				visualTree.DataContext = model;
				break;
			case Record record when RecordTemplate.TryGetValue(record.Owner.Name, out var template):
				visualTree = template.Invoke();
				visualTree.DataContext = record.To<ModelElement>();
				break;

			default:
			{	   
				var template = ModelTemplate.FirstOrDefault(x => x.Key.IsInterface && x.Key.IsAssignableFrom(e.NewValue.GetType())).Value;
				if (template != null)
				{
					visualTree = template.Invoke();
					visualTree.DataContext = e.NewValue;
				}
				else
				{
					visualTree = new TextBlock();
					visualTree.SetValue(TextBlock.TextProperty, e.NewValue?.ToString());
				}
				break;
			}
		}

		Content = visualTree;
		Name = visualTree?.Name;
	}
	#endregion

	#region Data
	static readonly Dictionary<string, Func<FrameworkElement>> RecordTemplate = new(StringComparer.OrdinalIgnoreCase);
	static readonly Dictionary<Type, Func<FrameworkElement>> ModelTemplate = [];

	public static void RegisterTemplate<T>(Type type, string? name = null) where T : FrameworkElement, new()
	{
		name ??= type.Name.TitleLowerCase();
		ModelTemplate[type] = RecordTemplate[name] = new(() => new T());
	}
	#endregion
}