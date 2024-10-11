﻿using HandyControl.Controls;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views.Selector;

namespace Xylia.Preview.UI.Services;
/// <summary>
/// Initialize process
/// </summary>
internal class RegisterService : IService
{
	public bool Register()
	{
		// effects 
		UserSettings.Default.UsePerformanceMonitor = UserSettings.Default.UsePerformanceMonitor;
		UserSettings.Default.CopyMode = UserSettings.Default.CopyMode;
		UserSettings.Default.SkinType = UserSettings.Default.SkinType;

		// init
		FileCache.DatSelector = DatSelectDialog.Instance;

		// ask
		if (UserSettings.Default.UseUserDefinition)
		{
			Growl.Ask(StringHelper.Get("Settings_UseUserDefinition_Ask"), isConfirmed =>
			{
				UserSettings.Default.UseUserDefinition = isConfirmed;
				return true;
			});
		}

		return true;
	}
}