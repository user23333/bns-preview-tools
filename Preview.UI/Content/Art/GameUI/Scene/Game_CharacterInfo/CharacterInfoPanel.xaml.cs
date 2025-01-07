using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Microsoft.Web.WebView2.Core;
using Xylia.Preview.Common;
using Xylia.Preview.Data.Models;
using Xylia.Preview.Data.Models.Configuration;

namespace Xylia.Preview.UI.GameUI.Scene.Game_CharacterInfo;
public partial class CharacterInfoPanel
{
	#region Constructors
	public CharacterInfoPanel()
	{
		InitializeComponent();

		CharacterInfoPanelWeb.Initialized += CharacterInfoPanelWeb_Initialized;
		CharacterInfoPanelWeb.PostMessage += CharacterInfoPanelWeb_PostMessage;

		var config = Globals.GameData.Provider.GetFiles("release.config2.xml").FirstOrDefault();
		var release = Table.LoadFrom<Release>(config) ?? throw new FileNotFoundException();

		var group = release["in-game-web"];
		CharacterInfoUrl = group["character-info-url"]?.Value;
		CharacterInfoUrl2 = group["character-info-url-2"]?.Value;
		CharacterInfoHomeUrn = group["character-info-home-urn"]?.Value;
		CharacterInfoOtherHomeUrn = group["character-info-other-home-urn"]?.Value;
		CharacterInfoDiffHomeUrn = group["character-info-diff-home-urn"]?.Value;

		InitUrl(new Creature() { WorldId = 1911, Name = "三千问乀" });
	}
	#endregion

	#region Methods
	private async void CharacterInfoPanelWeb_Initialized(object? sender, CoreWebView2 e)
	{
		await e.AddScriptToExecuteOnDocumentCreatedAsync($$"""
		onmouseover = (e) => {
		    var obj = e.target; 
			if (obj.tagName != 'IMG') return;

		    obj.removeAttribute('title');

			var parent = $(obj).parent('.item-img');
			if (parent != null) chrome.webview.hostObjects.WebObject.Message(parent.data('tooltip')); 
		};
		""");
	}

	private async void CharacterInfoPanelWeb_PostMessage(object? sender, string meaasge)
	{
		await Task.Run(() =>
		{
			var data = meaasge.Split('.').Select(int.Parse).ToArray();
			Trace.WriteLine(data.Aggregate("", (sum, now) => sum + now + ";"));


			//var uri = new Uri(meaasge);
			//if (uri.Scheme == "nc")
			//{
			//	var query = HttpUtility.ParseQueryString(uri.Query);
			//	if (uri.Host == "bns.charinfo" && uri.AbsolutePath == "/ItemTooltip")
			//	{
			//		var data = query["item"].Split('.').Select(int.Parse).ToArray();
			//		Trace.WriteLine(data.Aggregate("", (sum, now) => sum + now + ";"));

			//		//Task.Run(() => Globals.GameData.Item[data[0], data[1]].PreviewShow());
			//	}
			//}
		});
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		//CharacterInfoPanelWeb.Dispose();
		CharacterInfoPanelWeb = null;
	}

	public void InitUrl(Creature creature)
	{
		CharacterInfoPanelWeb.Source = new UriBuilder(CharacterInfoUrl.Replace("%s", creature.WorldId.ToString()[..2]) + CharacterInfoHomeUrn) { Query = $"c={creature.Name}&s={creature.WorldId}" }.Uri;
	}
	#endregion


	#region Private Fields
	private string? CharacterInfoUrl;
	private string? CharacterInfoUrl2;

	private string? CharacterInfoHomeUrn;
	private string? CharacterInfoOtherHomeUrn;
	private string? CharacterInfoDiffHomeUrn;
	#endregion
}