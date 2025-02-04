using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using HandyControl.Controls;
using Serilog;
using Vanara.PInvoke;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.UI.Helpers;
using Xylia.Preview.UI.Resources.Themes;
using Xylia.Preview.UI.Services;
using MessageBox = HandyControl.Controls.MessageBox;

namespace Xylia.Preview.UI;
public partial class App : Application
{
	#region Application
	protected override void OnStartup(StartupEventArgs e)
	{
		AppDomain.CurrentDomain.UnhandledException += OnFatalException;
		InitializeService.InitializeApp();
		InitializeArgs(e.Args);

#if DEVELOP
		TestProvider.Set(@"D:\Tencent\BnsData\GameData_ZTx", new()
		{ 
			Publisher = EPublisher.ZTx,
			AdditionalPublisher = EPublisher.Tencent,
			Language = ELanguage.ChineseS
		});

		//new GameUI.Scene.Game_Tooltip.AttractionMapUnitToolTipPanel().Show();
		//new GameUI.Scene.Game_Achievement.AchievementDetailPanel().Show();
		//new GameUI.Scene.Game_Tooltip2.MuseumCardTooltipPanel().Show();
		new GameUI.Scene.Game_Tooltip.RewardTooltipPanel().Show();
#else
		MainWindow = new MainWindow();
		MainWindow.Show();
#endif
	}

	internal static void UpdateSkin(SkinType skin, int night)
	{
		var skins0 = Current.Resources.MergedDictionaries[0];
		skins0.MergedDictionaries.Clear();
		skins0.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml") });
		skins0.MergedDictionaries.Add(SkinHelpers.GetDayNight(night));
		skins0.MergedDictionaries.Add(SkinHelpers.GetSkin(skin));

		var skins1 = Current.Resources.MergedDictionaries[1];
		skins1.MergedDictionaries.Clear();
		skins1.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml") });
		skins1.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/Preview.UI;component/Resources/Themes/Theme.xaml") });

		Current.MainWindow?.OnApplyTemplate();
		SkinHelpers.UpdateXshd("XML");
	}

	private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs arg)
	{
		arg.Handled = true;

		// if advanced exception
		var exception = arg.Exception;
		if (exception is TargetInvocationException or TypeInitializationException or XamlParseException)
			exception = exception.InnerException;

		switch (exception)
		{
			case WarningException: break;  // not to write log
			default: Log.Error(exception, "OnUnhandledException"); break;
		}

		Growl.Error(exception?.Message);
	}

	private void OnFatalException(object sender, UnhandledExceptionEventArgs e)
	{
		var exception = e.ExceptionObject as Exception;
		string str = StringHelper.Get("Application_CrashMessage", exception!.Message);

		Log.Fatal(exception, "OnCrash");
		MessageBox.Show(str, "Crash", MessageBoxButton.OK, MessageBoxImage.Stop);
	}
	#endregion

	#region Command
	/// <summary>
	/// Process the command-line arguments
	/// </summary>
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
		var type = args.GetValueOrDefault("type");

		if (command == "query")
		{
			switch (type)
			{
				case "ue":
				{
					if (!args.TryGetValue("path", out var path))
					{
						Console.Clear();
						Console.WriteLine("Enter search rule to continue...");
						path = Console.ReadLine()!;
					}

					Commands.QueryAsset(path, args.GetValueOrDefault("class"));
					break;
				}

				default: throw new WarningException();
			}

			Console.ReadKey();
		}
		else if (command == "output")
		{
			UpdateService.Register();

			switch (type)
			{
				case "fontset": Commands.OutputFontSet(); break;
				case "soundwave": Commands.OutputSoundWave(); break;
				default: Commands.TableOutput(type); break;
			}
		}
		else throw new WarningException("bad params: " + command);
	}
	#endregion
}