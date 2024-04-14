using Xylia.Preview.Data.Common.Abstractions;

namespace Xylia.Preview.Data.Models;
public abstract class DuelBotChallenge : ModelElement, IAttraction
{
	#region Attributes
	public sealed class TimeAttackMode : ModelElement
	{
		public short TotalTimeoutDurationSecond;
	}

	public sealed class RoundMode : ModelElement
	{
		public sbyte TotalRound; 

	}
	#endregion

	#region IAttraction
	public string Name => this.Attributes["dungeon-name2"].GetText();
	public string Description => this.Attributes["dungeon-desc"].GetText();
	#endregion
}