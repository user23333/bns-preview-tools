using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace Xylia.Preview.UI.Controls;
[ContentProperty("Children")]
[DesignTimeVisible(false)]
public class BnsCustomUISceneWidget : FrameworkElement
{
	#region Properties
	public Collection<FrameworkElement> Children { get; } = [];
	#endregion

	#region Children
	protected override IEnumerator LogicalChildren => Children.GetEnumerator();
	#endregion
}