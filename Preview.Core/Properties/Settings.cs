using System.ComponentModel;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Properties;
public class Settings() : IniSettings(Path.Combine(ApplicationData, "Settings.config"))
{
	#region Constructors
	public static string ApplicationData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Xylia");

	protected internal static Settings Default { get; protected set; } = new();

	static Settings()
	{
		// prevent exception when save
		Directory.CreateDirectory(ApplicationData);
	}
	#endregion

	#region Common
	public string GameFolder
	{
		get => GetValue<string>() ?? throw new WarningException("You must set game folder.");
		set
		{
			if (!Directory.Exists(value)) return;

			SetValue(value);
		}
	}

	public string OutputFolder
	{
		get => GetValue<string>() ?? throw new WarningException("You must set output folder.");
		set
		{
			if (!Directory.Exists(value)) return;

			SetValue(value);
			OutputFolderResource ??= value + "\\Paks";
		}
	}

	public string OutputFolderResource { get => GetValue<string>(); set => SetValue(value); }

	public bool UseDebugMode { get => GetValue<bool>(); set => SetValue(value); }

	public bool UseUserDefinition { get => GetValue<bool>(); set => SetValue(value); }
	public EPublisher DefitionType { get => GetValue<EPublisher>(); set => SetValue(value); }
	public string DefitionKey { get => GetValue<string>(); set => SetValue(value); }
	#endregion

	#region Preview
	public JobSeq Job
	{
		get => GetValue<string>(section: "Preview").ToEnum(JobSeq.검사);
		set => SetValue(value, section: "Preview");
	}

	public bool Text_LoadData { get => GetValue<bool>(); set => SetValue(value); }
	#endregion
}