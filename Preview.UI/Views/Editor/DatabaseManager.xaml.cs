using System.IO;
using System.Net.Http;
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
using Xylia.Preview.UI.Views.Selector;

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
	private bool IsConnecting = false;
	internal bool IsGlobalData = false;

	public IEngine? Engine { get; private set; }
	#endregion

	#region Methods
	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);
		Provider_GlobalMode.IsChecked = true;

		if (IsGlobalData)
		{
			Provider_ModeHolder.Visibility = Visibility.Collapsed;
			ProviderSearch.Visibility = Visibility.Visible;
			ProviderSearch.Text = UserSettings.Default.GameFolder;
		}
	}

	private async void ProviderMode_Changed(object sender, RoutedEventArgs? e)
	{
		Run_Version.Text = null;
		DefinitionList.ItemsSource = null;
		ProviderSearch.Visibility = Visibility.Visible;

		if (Provider_GlobalMode.IsChecked == true)
		{
			ProviderSearch.Visibility = Visibility.Collapsed;
			await Provider_CheckFolder(UserSettings.Default.GameFolder);
		}
		else if (Provider_GameMode.IsChecked == true)
		{
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
		if (!Directory.Exists(path)) return;

		try
		{
			var locale = new Locale(path);
			Run_Version.Text = string.Format(" ({0})", locale.ProductVersion);

			// get and update defs
			var commits = (await GetCommits(locale.Publisher)).OrderByDescending(x => x.Version);
			DefinitionList.ItemsSource = commits;
			DefinitionList.SelectedItem = commits.FirstOrDefault(x => locale.ProductVersion.CompareTo(x.Version) >= 0) ?? 
				throw new Exception(StringHelper.Get("DatabaseStudio_Definition_NoMatched"));
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

			if (Provider_GlobalMode.IsChecked == true)
			{
				UserSettings.Default.DefitionType = new Locale(UserSettings.Default.GameFolder).Publisher;
				UserSettings.Default.DefitionKey = definition.Key;

				if (IsGlobalData)
				{
					Globals.ClearData();
				}
				else
				{
					Globals.Definition = definition;
					Engine = Globals.GameData;
					IsGlobalData = true;
				}
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

	public static async Task<IEnumerable<Branch>> GetBranches()
	{
		var client = new HttpClient();
		client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");

		var response = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/branches");
		if (!response.IsSuccessStatusCode) throw new HttpRequestException();

		return JsonConvert.DeserializeObject<List<Branch>>(await response.Content.ReadAsStringAsync())!;
	}

	public static async Task<IEnumerable<Commit>> GetCommits(string branch)
	{
		var client = new HttpClient();
		client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");

		var commits = new List<Commit>();
		var response = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/commits?sha={branch}");
		if (response.IsSuccessStatusCode)
		{
			foreach (var token in (JArray)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync())!)
			{
				commits.Add(new()
				{
					Branch = branch,
					SHA = token.Value<string>("sha"),
					Time = token["commit"]!["author"]!.Value<DateTime>("date"),
					Message = token["commit"]!.Value<string>("message"),
				});
			}
		}

		return commits;
	}

	public static async Task<IEnumerable<Commit>> GetCommits(EPublisher publisher) => await GetCommits(publisher switch
	{
		EPublisher.ZTx => "ZTX",
		EPublisher.ZNcs => "ZNCS",
		_ => "LIVE",
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

		private string? _message;
		public string? Message
		{
			get => _message;
			set
			{
				_message = value;
				Version = BnsVersion.TryParse(value, out var version) ? version : default;
			}
		}

		public async Task<DatafileDefinition> LoadData()
		{
			ArgumentNullException.ThrowIfNull(SHA);

			Stream stream;
			string path = System.IO.Path.Combine(UserSettings.Default.OutputFolder, ".download", SHA);
			if (File.Exists(path)) stream = File.OpenRead(path);
			else
			{
				var client = new HttpClient();
				client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");

				var response = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/tarball/{SHA}"); //zipball
				if (!response.IsSuccessStatusCode) throw new HttpRequestException();

				stream = await response.Content.ReadAsStreamAsync();
				await stream.SaveAsync(path); // write local cache
			}

			return new CompressDatafileDefinition(stream, CompressionMethod.Gzip) { Key = SHA };
		}
	}
	#endregion
}