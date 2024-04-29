using CUE4Parse.BNS.Assets.Exports;
using CUE4Parse.UE4.Objects.UObject;
using Xylia.Preview.Common.Attributes;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models.QuestData;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public class Quest : ModelElement, IHaveName
{
	#region Attributes
	public LazyList<Acquisition> Acquisition { get; set; }

	public LazyList<Mission_Step> MissionStep { get; set; }

	public LazyList<Completion> Completion { get; set; }

	public LazyList<Transit> Transit { get; set; }


    [Side(ReleaseSide.Server)]
    public BroadcastCategorySeq BroadcastCategory { get; set; }
    public enum BroadcastCategorySeq
    {
        None,
        Field,
        Always,
        [Name("solo-quartet")]
        SoloQuartet,
        Sextet,
    }

    [Side(ReleaseSide.Server), Repeat(3)]
    public Ref<Achievement>[] ExtraQuestCompleteAchievement { get; set; }

    [Side(ReleaseSide.Server)]
    public Ref<Cinematic> ReplayEpicZoneLeaveCinematic { get; set; }


    public enum Category
    {
        Epic,
        Normal,
        Job,
        Dungeon,
        Attraction,
        TendencySimple,
        TendencyTendency,
        Mentoring,
        Hunting,
        COUNT
    }

    public enum ContentType
    {
        None,
        Gather,
        Production,
        PvpReward,
        Festival,
        EliteSkill,
        Duel,
        PartyBattle,
        Special,
        SideEpisode,
        Hidden,
        COUNT
    }

    public enum SaveType
    {
        All,

        /// <summary>
        /// 25000~25500
        /// </summary>
        Nothing,

        /// <summary>
        /// 20000~23000
        /// </summary>
        [Name("except-completion")]
        ExceptCompletion,

        /// <summary>
        /// 28000~
        /// </summary>
        [Name("except-completion-and-logout-save")]
        ExceptCompletionAndLogoutSave,
    }
    #endregion

    #region Methods
    public string Name => this.Attributes["name2"]?.GetText();

	public string Title => this.Attributes["group2"]?.GetText();

	public ImageProperty FrontIcon => new() { BaseImageTexture = GetImageTexture() };

	public ImageProperty FrontIconOver => new() { BaseImageTexture = GetImageTexture(true) };

	private FPackageIndex GetImageTexture(bool over = false)
	{
		string name;

		bool repeat = Attributes["reset-type"].ToEnum<ResetType>() != ResetType.None;
		var category = Attributes["category"].ToEnum<Category>();
		var contentType = Attributes["content-type"].ToEnum<ContentType>();
		switch (category)
		{
			case Category.Epic: name = "Map_Epic_Start"; break;
			case Category.Job: name = "Map_Job_Start"; break;
			case Category.Dungeon: return null;
			case Category.Attraction: name = "Map_attraction_start"; break;
			case Category.TendencySimple: name = "Map_System_start"; break;
			case Category.TendencyTendency: name = "Map_System_start"; break;
			case Category.Mentoring: name = "mento_mentoring_start"; break;
			case Category.Hunting: name = repeat ? "Map_Hunting_repeat_start" : "Map_Hunting_start"; break;
			case Category.Normal:
			{
				// faction quest
				if (Attributes["main-faction"] != null)
				{
					name = repeat ? "Map_Faction_repeat_start" : "Map_Faction_start";
				}
				else
				{
					name = contentType switch
					{
						ContentType.Festival => repeat ? "Map_Festival_repeat_start" : "Map_Festival_start",
						ContentType.Duel or ContentType.PartyBattle => repeat ? "Map_Faction_repeat_start" : "Map_Faction_start",
						ContentType.SideEpisode => "Map_side_episode_start",
						ContentType.Special => "Map_Job_Start",
						ContentType.Hidden => "Map_Hidden_Start",
						_ => repeat ? "Map_Repeat_start" : "Map_Normal_Start",
					};
				}
			}
			break;

			default: throw new NotImplementedException();
		}


		if (over) name += "_over";

		return new MyFPackageIndex($"BNSR/Content/Art/UI/GameUI/Resource/GameUI_Map_Indicator/{name}");
	}
	#endregion
}