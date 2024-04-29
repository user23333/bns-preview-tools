using Serilog;

namespace Xylia.Preview.Data.Helpers;
internal class MessageManager : HashSet<string>
{
	public void Warning(string message)
	{
		if (Check(message))
		{
			Log.Warning(message);
		}
	}

	public void Debug(string message)
	{
		if (Check(message))
		{
			Log.Debug(message);
		}
	}


	private bool Check(string message)
	{
		lock (this)
		{
			var flag = !this.Contains(message);
			if (flag) this.Add(message);

			return flag;
		}
	}
}