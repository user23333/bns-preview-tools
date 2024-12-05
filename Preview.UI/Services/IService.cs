using System.Collections.ObjectModel;

namespace Xylia.Preview.UI.Services;
public interface IService
{
	/// <summary>
	/// Initiaze service
	/// </summary>
	/// <returns>regist result</returns>
	bool Register();
}

public class ServiceManager	: Collection<IService>
{
	/// <summary>
	/// Register services
	/// </summary>
	public void RegisterAll()
	{
		foreach (var s in this)
		{
			s.Register();
		}
	}
}