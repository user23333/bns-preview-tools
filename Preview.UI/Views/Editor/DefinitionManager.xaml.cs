using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using CUE4Parse.Utils;
using ICSharpCode.SharpZipLib.Tar;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.UI.Views;
public partial class DefinitionManager
{
	#region Constructors
	public DefinitionManager()
	{
		InitializeComponent();
	}

	public DefinitionManager(BnsVersion version) : this()
	{
		CurrentVersion = version;
	}

	private async void DefinitionManager_Initialized(object sender, EventArgs e)
	{
		var commits = await GetCommits("NEO");
		CommitsList.ItemsSource = commits;

		var test = commits.OrderByDescending(x => x.Version).FirstOrDefault(x => CurrentVersion.CompareTo(x.Version) >= 0);
		Debug.WriteLineIf(test is null, "No matching available version: " + CurrentVersion);
	}
	#endregion

	#region Helpers
	public BnsVersion CurrentVersion { get; } = new(9, 0, 10041, 20);

	static readonly string owner = "xyliaup";
	static readonly string repo = "bns-definition";

	private static async Task<List<Branch>> GetBranchs()
	{
		var client = new HttpClient();
		client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");

		var response = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/branches");
		if (!response.IsSuccessStatusCode) throw new HttpRequestException();

		return JsonConvert.DeserializeObject<List<Branch>>(await response.Content.ReadAsStringAsync())!;
	}

	private static async Task<List<Commit>> GetCommits(string branch)
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

	public partial class Branch
	{
		public string? Name { get; set; }
		public bool Protected { get; set; }
	}

	public partial class Commit
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

		[RelayCommand]
		public async Task Download()
		{
			string folder = @"D:\Tencent\BnsData\definition";
			if (Directory.Exists(folder)) Directory.Delete(folder, true);

			var client = new HttpClient();
			client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");

			var response = await client.GetAsync($"https://api.github.com/repos/{owner}/{repo}/tarball/{SHA}"); //zipball
			if (response.IsSuccessStatusCode)
			{
				using var stream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress);
				using var tar = new TarInputStream(stream, Encoding.UTF8);

				string root = tar.GetNextEntry().Name;  //root folder entry
				while (true)
				{
					var entry = tar.GetNextEntry();
					if (entry is null) break;

					// field
					var name = entry.Name.SubstringAfter(root);
					if (name.EndsWith('/')) continue;

					byte[] buffer = new byte[entry.Size];
					tar.Read(buffer, 0, buffer.Length);

					// create
					var path = Path.Combine(folder, name);
					Directory.CreateDirectory(Path.GetDirectoryName(path)!);
					File.WriteAllBytes(path, buffer);
				}

				File.WriteAllText(Path.Combine(folder, "head"), $"[head]\nbranch={Branch}\nversion={Version}");
			}
			else
			{
				throw new HttpRequestException();
			}
		}
	}
	#endregion
}