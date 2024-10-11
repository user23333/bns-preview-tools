using IniParser;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Common.DataStruct;
using Xylia.Preview.Data.Common.Exceptions;

namespace Xylia.Preview.Data.Engine.DatData;
public struct Locale
{
	#region Fields	   
	public BnsVersion ProductVersion;
	public EPublisher Publisher;
	public ELanguage Language;
	public EPublisher AdditionalPublisher;
	public int Universe;
	#endregion

	#region Properties
    internal static Locale Current { get; set; }

    /// <summary>
    /// Indicates whether the provider is a special server
    /// </summary>
    public readonly bool IsNeo => Publisher is EPublisher.ZNcs or EPublisher.ZTx;
    #endregion

	#region Methods
	public Locale(EPublisher publisher)
	{
		Publisher = publisher;
		Current = this;
	}

	public Locale(string folder)
	{
		if (string.IsNullOrEmpty(folder)) return;
		var directory = new DirectoryInfo(folder);

		#region mode
		var Win64 = directory.GetDirectories("Win64", SearchOption.AllDirectories).FirstOrDefault();
		if (Win64 is not null)
		{
			var version = Win64?.GetFiles("version.ini").FirstOrDefault();
			if (version is not null)
			{
				var config = new FileIniDataParser().ReadFile(version.FullName);
				ProductVersion = BnsVersion.Parse(config["Version"]["ProductVersion"]);
			}

			var local = Win64?.GetFiles("local.ini").FirstOrDefault();
			if (local is not null)
			{
				var config = new FileIniDataParser().ReadFile(local.FullName);

				Publisher = config["Locale"]["Publisher"].ToEnum<EPublisher>();
				Language = config["Locale"]["Language"].ToEnum<ELanguage>();
				Universe = config["Locale"]["Universe"].To<int>();
				AdditionalPublisher = config["Locale"]["AdditionalPublisher"].ToEnum<EPublisher>();
				return;
			}
		}
		#endregion

		#region mode2
		var data = (directory.GetDirectories("Content", SearchOption.AllDirectories).FirstOrDefault() ?? directory)
			.GetDirectories("local").FirstOrDefault()?
			.GetDirectories().FirstOrDefault();
		if (data is null) throw BnsDataException.InvalidGame();
		else
		{
			Publisher = data.Name.ToEnum<EPublisher>();
			Language = (data.GetDirectories().Where(o => o.Name != "data").FirstOrDefault()?.Name).ToEnum<ELanguage>();

			if (Publisher is EPublisher.RTx or EPublisher.ZTx) AdditionalPublisher = EPublisher.Tencent;
			return;
		}
		#endregion
	}
	#endregion
}