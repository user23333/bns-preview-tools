using System.IO;
using System.Net.Http;
using System.Windows;
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
	public DatabaseManager(System.Windows.Window owner)
	{
		InitializeComponent();

		Owner = owner;
		CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, ConnectCommand, CanExecuteConnect));
	}
	#endregion

	#region Fields
	public IEngine? Engine { get; private set; }

	internal bool IsGlobalData = false;
	private bool IsConnecting = false;
	#endregion

	#region Methods
	protected override void OnInitialized(EventArgs e)
	{
		base.OnInitialized(e);
		ProviderMode_Changed(this, null);
	}

	private async void ProviderMode_Changed(object sender, RoutedEventArgs? e)
	{
		if (!IsInitialized) return;
		Run_Version.Text = null;
		DefinitionList.ItemsSource = null;

		if (Provider_GlobalMode.IsChecked == true)
		{
			await Provider_CheckFolder(UserSettings.Default.GameFolder);
		}
		else if (Provider_GameMode.IsChecked == true)
		{
			await Provider_CheckFolder(ProviderSearch.Text);
		}
	}

	private async void ProviderSearch_SearchStarted(object sender, FunctionEventArgs<string> e)
	{
		if (!SettingsView.TryBrowseFolder(out var path))
			return;

		await Provider_CheckFolder(ProviderSearch.Text = path);
	}

	private async Task Provider_CheckFolder(string path)
	{
		var directory = new DirectoryInfo(path);
		if (!directory.Exists) return;

		try
		{
			var locale = new Locale(directory);
			Run_Version.Text = string.Format(" ({0})", locale.ProductVersion);

			// get and update defs
			var commits = (await GetCommits(locale.Publisher)).OrderByDescending(x => x.Version);
			DefinitionList.ItemsSource = commits;
			DefinitionList.SelectedItem = commits.FirstOrDefault(x => locale.ProductVersion.CompareTo(x.Version) >= 0) ?? throw new Exception("No matched definition version");
			DefinitionList.ScrollIntoView(DefinitionList.SelectedItem);

			CommandManager.InvalidateRequerySuggested();
		}
		catch (Exception ex)
		{
			Growl.Error(ex.Message, DatabaseStudio.TOKEN);
		}
	}

	private void CanExecuteConnect(object sender, CanExecuteRoutedEventArgs e)
	{
		e.CanExecute = !IsConnecting && DefinitionList.SelectedItem is Commit commit;
	}

	private async void ConnectCommand(object sender, RoutedEventArgs e)
	{
		try
		{
			IsConnecting = true;
			var definition = DefinitionList.SelectedItem is Commit commit ? await commit.LoadData() : null;

			if (Provider_GlobalMode.IsChecked == true)
			{
				IsGlobalData = true;
				Globals.Definition = definition;  //set global definition
				Engine = Globals.GameData;
				UserSettings.Default.DefitionType = Globals.GameData.Provider.Locale.Publisher;
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
			Growl.Error(ex.Message, DatabaseStudio.TOKEN);
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
			string path = Path.Combine(UserSettings.Default.OutputFolder, ".download", SHA);
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