using System.Windows;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Data.Models;
using Xylia.Preview.UI.Extensions;

namespace Xylia.Preview.UI.GameUI.Scene.Game_Achievement;
public partial class AchievementDetailPanel
{
	#region Constructors
	public AchievementDetailPanel()
	{
		InitializeComponent();
#if DEVELOP
		DataContext = Xylia.Preview.Common.Globals.GameData.Provider.GetTable<Achievement>()["687_consumable_BACKCHUNG_Hidden_step1"];
#endif
	}
	#endregion

	#region Methods
	protected override void OnDataChanged(DependencyPropertyChangedEventArgs e)
	{
		if (e.NewValue is not Achievement record) return;

		AchievementPanel_Information_DetailList_Column_1_1_Info_Name.String.LabelText = record.Name2.GetText();
		AchievementPanel_Information_DetailList_Column_1_1_Info_Description.String.LabelText = record.Description2.GetText();
		AchievementPanel_Information_DetailList_Column_1_1_Info_Icon.ExpansionComponentList["IconImage"]!.SetValue(record.Icon);
		AchievementPanel_Information_DetailList_Column_1_1_Info_Score.String.LabelText = record.CurrentStepScore.ToString();

		AchievementPanel_Information_DetailList_Column_1_1_Info_Set_Title.SetVisiable(record.TitleName.HasValue);  		
		AchievementPanel_Information_DetailList_Column_1_1_Info_Set_Title.String.fontset = new MyFPackageIndex(record.TitleFontset);
		AchievementPanel_Information_DetailList_Column_1_1_Info_Set_Title.String.LabelText = record.TitleName.GetText();
		AchievementPanel_Information_DetailList_Column_1_1_Info_Set_Ability.SetVisiable(false);

		AchievementPanel_Information_DetailList_Column_1_1_Register_1_Name.String.LabelText = record.RegisterRef[0].Instance?.Name;
		AchievementPanel_Information_DetailList_Column_1_1_Register_2_Name.String.LabelText = record.RegisterRef[1].Instance?.Name;
		AchievementPanel_Information_DetailList_Column_1_1_Register_3_Name.String.LabelText = record.RegisterRef[2].Instance?.Name;
		AchievementPanel_Information_DetailList_Column_1_1_Register_4_Name.String.LabelText = record.RegisterRef[3].Instance?.Name;
		AchievementPanel_Information_DetailList_Column_1_1_Register_5_Name.String.LabelText = record.RegisterRef[4].Instance?.Name;
	}
	#endregion
}