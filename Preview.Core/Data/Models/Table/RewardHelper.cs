namespace Xylia.Preview.Data.Models;
public interface IRewardHelper
{
	object Data { get; }
	string Text { get; }
	string Group { get; }
	string GroupText { get; }
	string ProbabilityInfo { get; }
}

public interface IReward
{
	IEnumerable<IRewardHelper> GetRewards();
}