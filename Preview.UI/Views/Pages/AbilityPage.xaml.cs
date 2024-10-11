using System.Windows;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Views.Pages;
public partial class AbilityPage
{
	#region Constructor
	public AbilityPage()
	{
		InitializeComponent();
		DataContext = _viewModel = new AbilityPageViewModel();
	}
	#endregion

	#region Methods
	private void OpenSetting_Click(object sender, RoutedEventArgs e)
	{
		new AbilitySetting().Show();
	}

	private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
	{
		if (e.NewValue is not AbilityFunction ability) return;

		_viewModel.Selected = ability;
		LevelText.IsEnabled = ability.Φ != 0 || ability.LevelFactors.Count > 1;
		LevelText.Value = DEFAULT_LEVEL;
	}
	#endregion

	#region Private Fields
	private AbilityPageViewModel _viewModel;
	const sbyte DEFAULT_LEVEL = 60;
	#endregion
}