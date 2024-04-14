using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public abstract class FieldZone : ModelElement, IAttraction
{
	#region Attributes
	public sealed class Normal : FieldZone
	{
	}

	public sealed class GuildBattleFieldEntrance : FieldZone
	{
		public Ref<GuildBattleFieldZone> GuildBattleFieldZone { get; set; }

		public sbyte MinFixedChannel { get; set; }
	}
	#endregion

	#region IAttraction
	public string Name => this.Attributes["name2"].GetText();

	public string Description => this.Attributes["desc"].GetText();
	#endregion
}