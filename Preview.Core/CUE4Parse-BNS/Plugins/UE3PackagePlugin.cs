using System.Text.RegularExpressions;
using CUE4Parse.Utils;
using UE4Config.Parsing;

namespace CUE4Parse.BNS.Plugins;
internal class UE3PackagePlugin
{
	private readonly GameFileProvider Provider;
	private readonly Dictionary<string, string> FolderRedirects = [];

	public UE3PackagePlugin(GameFileProvider provider)
	{
		Provider = provider;
		if (!provider.TryCreateReader("BNSR/Plugins/Editor/UE3PackagePlugin/Config/UserUE3PackagePlugin.ini", out var archive))
			throw new FileNotFoundException();

		var config = new ConfigIni();
		config.Read(new StreamReader(archive));
		archive.Dispose();

		foreach (var token in config.Sections.FirstOrDefault(s => s.Name == "UE3CoreRedirects").Tokens)
		{
			if (token is not InstructionToken it) continue;

			var UE3Name = new Regex("(?<=UE3Name=\").*?(?=\")").Match(it.Value).Value;
			var UE4Name = new Regex("(?<=UE4Name=\").*?(?=\")").Match(it.Value).Value;

			switch (it.Key)
			{
				case "UE3StructRedirects":
					break;

				case "UE3ClassRedirects":
					break;

				case "UE3ClassesDeprecated":
					break;

				case "UE3FolderRedirects":
					FolderRedirects[UE3Name.PadLeft(8, '0')] = "/Game/" + UE4Name;
					break;
			}
		}
	}

	public string Redirect(string path)
	{	  
		// actually associated through the AssetRegistry, but loading is time-consuming	
		if (path.Contains('.') && !path.Contains('/'))
		{	
			string UE4Path, package = path.SubstringBefore('.');

			if (FolderRedirects.TryGetValue(package, out var redirect)) UE4Path = redirect + path[package.Length..];
			else if (package.Equals("Gadget_Object", StringComparison.OrdinalIgnoreCase)) UE4Path = "/Game/bns/Package/World/GameDesign/commonpackage/env_object/gadget_object/" + path[14..];
			else if (path.StartsWith("MiniMap_", StringComparison.OrdinalIgnoreCase)) UE4Path = "/Game/bns/Package/World/GameDesign/commonpackage/" + path;
			else lock (this) { return Provider.LoadAssetRegistry().ObjectRef.TryGetValue(path, out path) ? path : null; }

			UE4Path = UE4Path.Replace('.', '/');
			return string.Concat(UE4Path, ".", UE4Path.Split('/')[^1]);
		}

		return path;
	}
}