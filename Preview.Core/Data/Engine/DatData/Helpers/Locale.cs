using System.Diagnostics;
using IniParser;
using IniParser.Model;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;

namespace Xylia.Preview.Data.Engine.DatData;
public struct Locale
{
	#region Fields	 
	public BnsVersion ClientVersion;
	public BnsVersion ProductVersion;
	public EPublisher Publisher;
	public ELanguage Language;
	public EPublisher AdditionalPublisher;
	public int Universe;
	#endregion

	#region Constructor
	public Locale(EPublisher publisher)
	{
		Publisher = publisher;
	}

	public Locale(string path)
	{
		var root = new DirectoryInfo(path);
		var win64 = root.GetDirectories("Win64", SearchOption.AllDirectories).FirstOrDefault() ?? root;

		var local = win64?.GetFiles("local.ini").FirstOrDefault();
		if (local is not null)
		{
			var locale = new FileIniDataParser().ReadFile(local.FullName)["Locale"];
			Publisher = locale["Publisher"].ToEnum<EPublisher>();
			Language = locale["Language"].ToEnum<ELanguage>();
			Universe = locale["Universe"].To<int>();
			AdditionalPublisher = locale["AdditionalPublisher"].ToEnum<EPublisher>();
		}

		var version = win64?.GetFiles("version.ini").FirstOrDefault();
		if (version is not null)
		{
			var config = new FileIniDataParser().ReadFile(version.FullName);
			ProductVersion = BnsVersion.Parse(config["Version"]["ProductVersion"]);
			ClientVersion = BnsVersion.Parse(config["Version"]["ClientVersion"]);   // custom config
		}

		// read from client if exist
		var client = win64?.GetFiles("BNSR.exe").FirstOrDefault();
		if (client is not null)
		{
			var info = FileVersionInfo.GetVersionInfo(client.FullName);
			ClientVersion = BnsVersion.Parse(info.FileVersion);
			Publisher = (EPublisher)info.FileMinorPart;
		}
	}

	internal static Locale Current { get; set; }
	#endregion

	#region Methods
	public readonly void Save(string folder)
	{
		var locale = new SectionData("Locale");
		locale.Keys["Publisher"] = Publisher.ToString();
		locale.Keys["Language"] = Language.ToString();
		if (AdditionalPublisher != default) locale.Keys["AdditionalPublisher"] = AdditionalPublisher.ToString();
		new FileIniDataParser().WriteFile(Path.Combine(folder, "local.ini"), new IniData([locale]));

		var version = new SectionData("Version");
		version.Keys["ClientVersion"] = ClientVersion.ToString();
		version.Keys["ProductVersion"] = ProductVersion.ToString();
		new FileIniDataParser().WriteFile(Path.Combine(folder, "version.ini"), new IniData([version]));
	}
	#endregion
}