using System.Windows;
using System.Windows.Controls;
using CUE4Parse.BNS.Assets.Exports;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Views;

namespace Xylia.Preview.UI.GameUI.Scene.Game_NpcTalk;
public partial class NpcTalkPanel
{
	#region Constructors
	public NpcTalkPanel()
	{
		InitializeComponent();
#if DEVELOP
		NpcTalkPanel_Searcher.Visibility = Visibility.Visible;
		NpcTalkPanel_Searcher.Text = "q_2343_5N";
#endif
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not NpcTalkMessage record) return;

		TestList.ItemsSource = record.GetSteps().Where(x => x.IsValid);
		this.InvalidateMeasure();
	}

	private void OnPlayStepShow(object sender, RoutedEventArgs e)
	{
		if (sender is not FrameworkElement fe || fe.DataContext is not NpcTalkMessage.NpcTalkMessageStep step) return;

		var show = step.Show.LoadObject<UShowObject>();
		if (show != null) ShowObjectPlayer.PlaySound(show);
	}

	private void Searcher_TextChanged(object sender, TextChangedEventArgs e)
	{
		DataContext = Globals.GameData.Provider.GetTable<NpcTalkMessage>()[NpcTalkPanel_Searcher.Text];
	}
	#endregion
}