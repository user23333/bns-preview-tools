using System.Windows;

namespace Xylia.Preview.UI.Controls;
public class WidgetTemplate : DataTemplate
{
	#region Constructors
	public WidgetTemplate()
	{
	}

	public WidgetTemplate(FrameworkElementFactory root)
	{
		VisualTree = root;
	}
	#endregion
}