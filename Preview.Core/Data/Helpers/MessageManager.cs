using Serilog;

namespace Xylia.Preview.Data.Helpers;
internal class MessageManager : HashSet<string>
{
	public void Warning(string message)
	{
		if (!this.Contains(message))
		{
			this.Add(message);
			Log.Warning(message);
		}
	}
}