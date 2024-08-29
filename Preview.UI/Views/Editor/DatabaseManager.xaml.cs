using System.IO;
using System.Net.Http;
using System.Windows;
using HandyControl.Controls;
using HandyControl.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xylia.Preview.Data.Client;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Properties;
using Xylia.Preview.UI.ViewModels;
using Xylia.Preview.UI.Views.Selector;

namespace Xylia.Preview.UI.Views.Editor;
public partial class DatabaseManager
{
	#region Constructor	
	public IEngine? Engine { get; private set; }

	internal bool IsGlobalData = false;

	public DatabaseManager()
	{
		InitializeComponent();
	}
	#endregion

	#region Methods
	private async void ProviderMode_Changed(object sender, RoutedEventArgs e)
	{
		Run_Version.Text = null;

		if (Provider_GlobalMode.IsChecked == true)
		{
			await Provider_CheckFolder(UserSettings.Default.GameFolder);
		}
	}

	private async void ProviderSearch_SearchStarted(object sender, FunctionEventArgs<string> e)
	{
		if (!SettingsView.TryBrowseFolder(out var path))
			return;

		await Provider_CheckFolder(ProviderSearch.Text = path);
	}

	private async Task Provider_CheckFolder(string path, bool mode = false)
	{
		try
		{
			var locale = mode ? default : new Locale(path);
			Run_Version.Text = string.Format(" ({0})", locale.ProductVersion);

			var commits = (await GetCommits(locale.IsNeo ? "NEO" : "LIVE")).OrderByDescending(x => x.Version);
			DefinitionList.ItemsSource = commits;
			DefinitionList.SelectedItem = commits.FirstOrDefault(x => locale.ProductVersion.CompareTo(x.Version) >= 0) ?? throw new Exception("No matched definition version");
		}
		catch (Exception ex)
		{
			Growl.Error(ex.Message, DatabaseStudio.TOKEN);
		}
	}

	private async void Connect_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (DefinitionList.SelectedItem is Commit commit)
			{
				await commit.Download();
			}

			if (Provider_GlobalMode.IsChecked == true)
			{
				IsGlobalData = true;
				Engine = FileCache.Data;
			}
			else
			{
				var path = ProviderSearch.Text;
				ArgumentException.ThrowIfNullOrWhiteSpace(path, StringHelper.Get("Text.InvalidPath"));

				IDataProvider? provider;
				if (Provider_GameMode.IsChecked == true) provider = DefaultProvider.Load(path, DatSelectDialog.Instance);
				else if (Provider_FolderMode.IsChecked == true) provider = new FolderProvider(path);
				else throw new NotSupportedException();

				Engine = new BnsDatabase(provider);
			}

			DialogResult = true;
			Close();
		}
		catch (Exception ex)
		{
			Growl.Error(ex.Message, DatabaseStudio.TOKEN);
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

		public async Task Download()
		{
			ArgumentNullException.ThrowIfNull(SHA);

			string folder = Path.Combine(UserSettings.Default.OutputFolder, "definition");
			if (Directory.Exists(folder)) Directory.Delete(folder, true);

			// cache
			var temp = Path.Combine(Settings.ApplicationData, "download", SHA);
			if (!File.Exists(temp))
			{
				var client = new HttpClient();
				client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");

				var response = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/tarball/{SHA}"); //zipball
				if (!response.IsSuccessStatusCode) throw new HttpRequestException();

				await using var fs = File.OpenWrite(temp);
				await (await response.Content.ReadAsStreamAsync()).CopyToAsync(fs);
				await fs.FlushAsync();
			}
		}
	}
	#endregion
}