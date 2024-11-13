using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class ItemRandomAbilitySlot : ModelElement
{
	#region Attributs
	public int Id { get; set; }

	public string Alias { get; set; }

	public MainAbilitySeq Ability { get; set; }

	public int ValueMin { get; set; }

	public int ValueMax { get; set; }

	public int InitialValueMax { get; set; }

	public sbyte[] ItemAbilitySectionPercent { get; set; }

	public Ref<ItemRandomAbilitySection>[] ItemAbilitySection { get; set; }
	#endregion

	#region Methods
	public string Description
	{
		get
		{
			var name = SequenceExtensions.GetText(Ability);
			return $"{name} {ValueMin}~{ValueMax}";
		}
	}
	#endregion
}