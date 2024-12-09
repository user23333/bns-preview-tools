using HandyControl.Controls;
using Xylia.Preview.UI.ViewModels;

namespace Xylia.Preview.UI.Services;
internal class RegisterService : IService
{
	public void Register()
	{
		// effects 
		UserSettings.Default.UsePerformanceMonitor = UserSettings.Default.UsePerformanceMonitor;
		UserSettings.Default.CopyMode = UserSettings.Default.CopyMode;
		UserSettings.Default.SkinType = UserSettings.Default.SkinType;

		// ask
		if (UserSettings.Default.UseUserDefinition)
		{
			Growl.Ask(StringHelper.Get("Settings_UseUserDefinition_Ask"), isConfirmed =>
			{
				UserSettings.Default.UseUserDefinition = isConfirmed;
				return true;
			});
		}
	}
}