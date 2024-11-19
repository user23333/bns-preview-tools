using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using HandyControl.Controls;
using Serilog;
using Vanara.PInvoke;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.UI.Helpers;
using Xylia.Preview.UI.Helpers.Output;
using Xylia.Preview.UI.Resources.Themes;
using Xylia.Preview.UI.Services;

namespace Xylia.Preview.UI;
public partial class App : Application
{
	#region Application
	protected override void OnStartup(StartupEventArgs e)
	{
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

		// Register base services
		new ServiceManager() { new LogService(), new JumpListService() }.RegisterAll();

		InitializeArgs(e.Args);																																											   
		//Task.Run(() => PreviewWorld.Execute("bnsr/content/neo_art/area/zncs_interserver_001_p.umap"));
		//return;

#if DEVELOP

		UpdateSkin(SkinType.Default, true);
		TestProvider.Set(@"D:\Tencent\BnsData\GameData_ZTx", EPublisher.ZTx);

		new GameUI.Scene.Game_Tooltip.AttractionMapUnitToolTipPanel().Show();
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

		MainWindow?.OnApplyTemplate();
		SkinHelpers.UpdateXshd("XML");
	}
	#endregion

	#region Exception	
	private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs arg)
	{
		arg.Handled = true;

		// if advanced exception
		var exception = arg.Exception;
		if (exception is TargetInvocationException or XamlParseException) exception = exception.InnerException;

		switch (exception)
		{
			//case BnsDataException e when e.Code is BnsDataExceptionCode.Definition_NotFound:
			//	new DatabaseManager(MainWindow).ShowDialog();
			//	return;

			// not to write log
			case WarningException: break;
			default: Log.Error(exception, "OnUnhandledException"); break;
		}

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
			new UpdateService(false).Register();

			switch (type)
			{
				case "soundwave": Commands.Soundwave_output(); break;

				default:
				{
					var sets = OutSet.Find();
					var intance = sets.FirstOrDefault(x => x.Name.Equals(type, StringComparison.OrdinalIgnoreCase));
					if (intance is null)
					{
						EnterNumber:
						int idx = 0;
						Console.WriteLine("Enter specified number to continue...");
						sets.ForEach(x => Console.WriteLine("   [{0}] {1}", idx++, x.Name));

						// retry
						if (!int.TryParse(Console.ReadLine()!, out var i) || (intance = sets.ElementAtOrDefault(i)) is null)
						{
							Console.WriteLine();
							goto EnterNumber;
						}	
					}

					intance.Execute();
				}
				break;
			}
		}
		else throw new WarningException("bad params: " + command);
	}
	#endregion
}