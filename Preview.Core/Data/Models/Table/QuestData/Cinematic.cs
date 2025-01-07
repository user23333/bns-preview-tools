namespace Xylia.Preview.Data.Models;
public sealed class Cinematic : ModelElement 
{
	#region Attribute
	public string Alias { get; set; }

	public string CinemaName { get; set; }

	public string CinemaResourceName { get; set; }

	public string TeenCinemaName { get; set; }

	public string TeenCinemaResourceName { get; set; }

	public Ref<Item> Costume { get; set; }

	public SkippableSeq Skippable { get; set; }

	public enum SkippableSeq
	{
		None,
		Skip,
		PartySkip,
		TeamSkip,
		AutoSkip,
		COUNT
	}

	public bool PointCamera { get; set; }

	public bool NameplateVisible { get; set; }
	#endregion
}