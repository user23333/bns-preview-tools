using System.Data;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CUE4Parse.Compression;
using HandyControl.Controls;
using HandyControl.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xylia.Preview.Common;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Engine.Definitions;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views.Dialogs;

namespace Xylia.Preview.UI.Views.Editor;
public partial class DatabaseManager
{
	#region Constructor	
	private readonly string? TOKEN;

	public DatabaseManager(System.Windows.Window? owner, bool global = false)
	{
		Owner = owner;
		TOKEN = owner is DatabaseStudio ? DatabaseStudio.TOKEN : null;
		IsGlobalData = global;

		InitializeComponent();
		InitializeCommand();
	}
	#endregion

	#region Fields
	internal bool IsGlobalData = false;
	private bool IsConnecting = false;
	private string? LastPath;

	public IEngine? Engine { get; private set; }
	#endregion

	#region Methods
	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);

		if (IsGlobalData)
		{
			Provider_ModeHolder.Visibility = Visibility.Collapsed;
			ProviderSearch.Visibility = Visibility.Visible;

			try { ProviderSearch.Text = UserSettings.Default.GameFolder; }
			catch { }
		}
		else
		{
			Provider_GlobalMode.IsChecked = true;
		}
	}

	private async void ProviderMode_Changed(object sender, RoutedEventArgs? e)
	{
		if (Provider_GlobalMode.IsChecked == true)
		{
			ProviderSearch.Visibility = Visibility.Collapsed;
			await Provider_CheckFolder(UserSettings.Default.GameFolder);
		}
		else if (Provider_GameMode.IsChecked == true)
		{
			ProviderSearch.Visibility = Visibility.Visible;
			await Provider_CheckFolder(ProviderSearch.Text);
		}
		else if (Provider_FolderMode.IsChecked == true)
		{
			ProviderSearch.Visibility = Visibility.Visible;
			await Provider_CheckFolder(ProviderSearch.Text);
		}
	}

	private void ProviderSearch_SearchStarted(object sender, FunctionEventArgs<string> e)
	{
		if (SettingsView.TryBrowseFolder(out var path))
		{
			ProviderSearch.Text = path;
			if (IsGlobalData) UserSettings.Default.GameFolder = path;
		}
	}

	private async void ProviderSearch_TextChanged(object sender, TextChangedEventArgs e)
	{
		await Provider_CheckFolder(ProviderSearch.Text);
	}

	private async Task Provider_CheckFolder(string path)
	{
		try
		{
			if (path == LastPath) return;
			DefinitionList.ItemsSource = null;
			Run_Version.Text = null;

			// check path
			if (!Directory.Exists(LastPath = path)) return;
			var locale = new Locale(path);
			Run_Version.Text = string.Format(" ({0}_{1})", locale.Publisher, locale.ClientVersion);

			// update definitions
			var commits = (await GetCommits(locale.Publisher)).OrderByDescending(x => x.Version);
			DefinitionList.ItemsSource = commits;
			DefinitionList.SelectedItem = commits.FirstOrDefault(x => locale.ClientVersion.CompareTo(x.Version) >= 0) ??
				throw new VersionNotFoundException(StringHelper.Get("DatabaseStudio_Definition_NoMatched"));
			DefinitionList.ScrollIntoView(DefinitionList.SelectedItem);
		}
		catch (Exception ex)
		{
			Growl.Error(ex.Message, TOKEN);
		}
		finally
		{
			CommandManager.InvalidateRequerySuggested();
		}
	}


	private void InitializeCommand()
	{
		CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, ConnectCommand, CanExecuteConnect));
	}

	private void CanExecuteConnect(object sender, CanExecuteRoutedEventArgs e)
	{
		e.CanExecute = !IsConnecting && DefinitionList.SelectedItem is Commit;
	}

	private async void ConnectCommand(object sender, RoutedEventArgs e)
	{
		try
		{
			IsConnecting = true;
			var definition = DefinitionList.SelectedItem is Commit commit ? await commit.LoadData() : throw new NotSupportedException();

			if (IsGlobalData)
			{
				UserSettings.Default.DefitionType = new Locale(UserSettings.Default.GameFolder).Publisher;
				UserSettings.Default.DefitionKey = definition.Key;
				Globals.ClearData();
			}
			else if (Provider_GlobalMode.IsChecked == true)
			{
				UserSettings.Default.DefitionType = new Locale(UserSettings.Default.GameFolder).Publisher;
				UserSettings.Default.DefitionKey = definition.Key;
				Globals.Definition = definition;
				Engine = Globals.GameData;
				IsGlobalData = true;
			}
			else
			{
				var path = ProviderSearch.Text;
				ArgumentException.ThrowIfNullOrWhiteSpace(path, StringHelper.Get("Exception_InvalidPath"));

				IDataProvider? provider;
				if (Provider_GameMode.IsChecked == true) provider = DefaultProvider.Load(path, DatSelectDialog.Instance);
				else if (Provider_FolderMode.IsChecked == true) provider = new FolderProvider(path);
				else throw new NotSupportedException();

				Engine = new BnsDatabase(provider, definition);
			}

			DialogResult = true;
			Close();
		}
		catch (Exception ex)
		{
			Growl.Error(ex.Message, TOKEN);
		}
		finally
		{
			IsConnecting = false;
		}
	}
	#endregion

	#region Helpers
	const string owner = "xyliaup";
	const string repo = "bns-definition";
	const string auth = "Z2l0aHViX3BhdF8xMUFLTDdCSkEwSmF4WEV2ejZuNEcyX0R4MHoxdkNlcWw5WUNOa0RXakJoZVIxQUVkbms3b2I2Vk9UcTJjeE1xdE9FVzJRT09FUEhRMDdzOEpO";

	private static async Task<IEnumerable<Branch>> GetBranches()
	{
		var client = new HttpClient();
		client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
		client.DefaultRequestHeaders.Add("authorization", Encoding.UTF8.GetString(Convert.FromBase64String(auth)));

		var response = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/branches");
		response.EnsureSuccessStatusCode();

		return JsonConvert.DeserializeObject<List<Branch>>(await response.Content.ReadAsStringAsync())!;
	}

	private static async Task<IEnumerable<Commit>> GetCommits(string branch)
	{
		var client = new HttpClient();
		client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
		client.DefaultRequestHeaders.Add("authorization", Encoding.UTF8.GetString(Convert.FromBase64String(auth)));

		var response = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/commits?sha={branch}");
		response.EnsureSuccessStatusCode();

		return JsonConvert.DeserializeObject<JArray>(await response.Content.ReadAsStringAsync())!.Select(token => new Commit()
		{
			Branch = branch,
			SHA = token.Value<string>("sha"),
			Time = token["commit"]!["author"]!.Value<DateTime>("date"),
			Message = token["commit"]!.Value<string>("message")!,
		}).ToArray();
	}

	private static async Task<IEnumerable<Commit>> GetCommits(EPublisher publisher) => await GetCommits(publisher switch
	{
		EPublisher.Tencent => "TENCENT",
		EPublisher.ZNcs => "ZNCS",
		EPublisher.ZTx => "ZTX",
		_ => "NCSOFT",
	});

	public class Branch
	{
		public string? Name { get; set; }
		public bool Protected { get; set; }
	}

	public class Commit
	{
		internal string? Branch { get; set; }
		public string? SHA { get; set; }
		public DateTime Time { get; set; }
		public BnsVersion Version { get; set; }
		public string? Text { get; set; }

		public string Message
		{
			set
			{
				var strings = value.Split(',')
					.Where(x => x.Contains('='))
					.ToDictionary(x => x[..x.IndexOf('=')], x => x[(x.IndexOf('=') + 1)..], StringComparer.OrdinalIgnoreCase);

				Version = BnsVersion.Parse(strings.GetValueOrDefault("version", value));
				Text = strings.GetValueOrDefault("text", Version.ToString());
			}
		}

		public async Task<DatafileDefinition> LoadData()
		{
			ArgumentNullException.ThrowIfNull(SHA);

			Stream stream;
			string path = Path.Combine(UserSettings.Default.OutputFolder, ".download", SHA);
			if (File.Exists(path)) stream = File.OpenRead(path);
			else
			{
				var client = new HttpClient();
				client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");

				var response = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/tarball/{SHA}"); //zipball
				response.EnsureSuccessStatusCode();

				stream = await response.Content.ReadAsStreamAsync();
				await stream.SaveAsync(path); // write local cache
				stream.Seek(0, SeekOrigin.Begin);
			}

			return new CompressDatafileDefinition(stream, CompressionMethod.Gzip) { Key = SHA };
		}
	}
	#endregion
}