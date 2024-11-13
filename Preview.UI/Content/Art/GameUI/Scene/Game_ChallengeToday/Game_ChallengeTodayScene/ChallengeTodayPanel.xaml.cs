using Xylia.Preview.Common;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Models;

namespace Xylia.Preview.UI.GameUI.Scene.Game_ChallengeToday;
public partial class ChallengeTodayPanel
{
	public ChallengeTodayPanel()
	{
		InitializeComponent();
	}


	#region Methods
	public static void Test()
	{
		var record = Globals.GameData.Provider.GetTable<ChallengeList>().First(record => record.ChallengeType == ChallengeList.ChallengeTypeSeq.Mon);

		record.ChallengeQuestBasic.SelectNotNull(item => item.Instance).ForEach(item =>
		{
			Console.WriteLine(item.Name);
		});
	}
	#endregion
}