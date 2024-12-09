using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Xylia.Preview.UI.Services;
public interface IService
{
	/// <summary>
	/// Initiaze service
	/// </summary>
	/// <returns>regist result</returns>
	void Register();
}

public class ServiceManager : Collection<IService>
{
	/// <summary>
	/// Register services
	/// </summary>
	public void RegisterAll()
	{
		foreach (var s in this)
		{
			try
			{
				s.Register();
			}
			catch (Exception ex)
			{
				Debug.Fail(string.Format("{0} register failed.", s.GetType()), ex.Message);
			}
		}
	}
}