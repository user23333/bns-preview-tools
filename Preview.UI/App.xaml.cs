using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using HandyControl.Controls;
using Serilog;
using Vanara.PInvoke;
using Xylia.Preview.UI.Helpers;
using Xylia.Preview.UI.Helpers.Output;
using Xylia.Preview.UI.Helpers.Output.Tables;
using Xylia.Preview.UI.Resources.Themes;
using Xylia.Preview.UI.Services;

namespace Xylia.Preview.UI;
public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		// Process the command-line arguments
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

		// Register base services
		new ServiceManager() { new LogService(), new JumpListService() }.RegisterAll();

		InitializeArgs(e.Args);

#if DEVELOP && true
		TestProvider.Set();
		UpdateSkin(SkinType.Default, true);

		//new Xylia.Preview.UI.Content.TestPanel().Show();
		//var _ = PreviewModel.SnooperViewer;
		new Xylia.Preview.UI.GameUI.Scene.Game_Tooltip.Skill3ToolTipPanel_1().Show();
#else
		MainWindow = new MainWindow();
		MainWindow.Show();
#endif
	}

	internal void UpdateSkin(SkinType skin, bool? night)
	{
		var skins0 = Resources.MergedDictionaries[0];
		skins0.MergedDictionaries.Clear();
		skins0.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml") });
		skins0.MergedDictionaries.Add(SkinHelpers.GetDayNight(night));
		skins0.MergedDictionaries.Add(SkinHelpers.GetSkin(skin));

		var skins1 = Resources.MergedDictionaries[1];
		skins1.MergedDictionaries.Clear();
		skins1.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") });
		skins1.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Preview.UI;component/Resources/Themes/Theme.xaml") });

		Current.MainWindow?.OnApplyTemplate();
	}


	#region Exception	
	private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		e.Handled = true;

		// if advanced exception
		var exception = e.Exception;
		if (exception is TargetInvocationException or XamlParseException)
			exception = exception.InnerException;

		// not to write log
		if (exception is not WarningException)
			Log.Error(exception, "OnUnhandledException");

		Growl.Error(exception?.Message);
	}

	private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		var exception = e.ExceptionObject as Exception;
		string str = StringHelper.Get("Application_CrashMessage", exception!.Message);

		Log.Fatal(exception, "OnCrash");
		SendMessage(str, "Crash");
	}

	internal static void SendMessage(string message, string? title = null)
	{
		HandyControl.Controls.MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Stop);
	}
	#endregion

	#region Command
	private static void InitializeArgs(string[] Args)
	{
		var args = Args
			.Where(x => x[0] == '-' && x.IndexOf('=') > 0)
			.ToLookup(
				x => x[1..x.IndexOf('=')].ToLower(),
				x => x[(x.IndexOf('=') + 1)..])
			.ToDictionary(x => x.Key, x => x.First());

		if (args.TryGetValue("command", out var command))
		{
			Kernel32.AllocConsole();
			Kernel32.SetConsoleCP(65001);
			Kernel32.SetConsoleTitle(Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyTitleAttribute>()!.Title);

			try
			{
				Command(command, args);
			}
			catch (Exception error)
			{
				Console.WriteLine(error is WarningException ? error.Message : error);
				Console.ReadKey();
			}

			Kernel32.FreeConsole();
			Environment.Exit(-1);
		}
		else Console.SetOut(new LogService());
	}

	private static void Command(string? command, Dictionary<string, string> args)
	{
		if (command == "query")
		{
			bool pause;
			switch (args["type"])
			{
				case "ue":
				{
					if (!args.TryGetValue("path", out var path))
					{
						Console.Clear();
						Console.WriteLine("please enter search rule...");
						path = Console.ReadLine()!;
					}

					pause = Commands.QueryAsset(path, args.GetValueOrDefault("class"));
					break;
				}

				default: throw new WarningException();
			}

			if (!pause) Console.WriteLine($"no result!");
			Console.ReadKey();
		}
		else if (command == "output")
		{
			new UpdateService().Register();

			switch (args["type"])
			{
				case "soundwave": Commands.Soundwave_output(); break;
				case "skill3": OutSet.Start<Skill3Out>().Wait(); break;
			}
		}
		else throw new WarningException("bad params: " + command);
	}
	#endregion
}