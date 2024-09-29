namespace Xylia.Preview.Data.Models;
public sealed class FeedbackBossNpc : ModelElement 
{
	#region Attributes
	public string Alias { get; set; }

	public Ref<Npc> Npc { get; set; }

	public Ref<FeedbackSkillScore>[] SkillScore { get; set; }
	#endregion
}