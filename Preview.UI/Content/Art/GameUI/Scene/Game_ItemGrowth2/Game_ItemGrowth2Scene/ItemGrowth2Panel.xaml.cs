using System.Windows;

namespace Xylia.Preview.UI.GameUI.Scene.Game_ItemGrowth2;
public partial class ItemGrowth2Panel
{
	#region Constructors 
	public ItemGrowth2Panel()
	{
		InitializeComponent();

		foreach(UIElement element in ItemGrowth2Panel_Growth_Holder.Children)
		{
			element.Visibility = Visibility.Collapsed;
		}
	}
	#endregion
}