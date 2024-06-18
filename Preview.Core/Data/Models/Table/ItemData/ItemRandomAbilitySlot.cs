using Xylia.Preview.Data.Common.Abstractions;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Data.Models;
public sealed class ItemRandomAbilitySlot : ModelElement, IHaveDesc
{
	#region Attributs
	public int Id { get; set; }

	public string Alias { get; set; }

	public MainAbility Ability { get; set; }

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