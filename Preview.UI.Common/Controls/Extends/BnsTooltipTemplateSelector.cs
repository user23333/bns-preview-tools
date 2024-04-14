using System.Windows;
using System.Windows.Controls;

namespace Xylia.Preview.UI.Controls;
public class BnsTooltipTemplateSelector : DataTemplateSelector
{
	public DataTemplate? PlainTemplate { get; set; }

	public override DataTemplate? SelectTemplate(object item, DependencyObject container)
	{
		if (item is FrameworkElement) return null;
		else return PlainTemplate;
	}
}