using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IniParser;
using IniParser.Model;
using Xylia.Preview.Common.Extension;
using Xylia.Preview.Data.Engine.DatData;
using Xylia.Preview.Data.Helpers;
using Xylia.Preview.Data.Models.Sequence;

namespace Xylia.Preview.Properties;
public class Settings : INotifyPropertyChanged
{
	#region Constructors
	public static string ApplicationData => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Xylia");

	protected internal static Settings Default { get; protected set; } = new();

	protected Settings()
	{
		// prevent exception when save
		Directory.CreateDirectory(ApplicationData);

		ConfigPath = Path.Combine(ApplicationData, "Settings.config");
		Configuration = File.Exists(ConfigPath) ?
			new FileIniDataParser().ReadFile(ConfigPath) :
			new IniData();
	}
	#endregion

	#region PropertyChange	 
	protected string ConfigPath;
	protected IniData Configuration;
	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(storage, value))
			return false;

		storage = value;
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		return true;
	}

	public T GetValue<T>(string section = "Common", [CallerMemberName] string name = null)
	{
		// group
		if (name.Contains('_'))
		{
			var split = name.Split('_', 2);
			section = split[0];
			name = split[1];
		}

		// value
		var value = Configuration[section][name];
		if (string.IsNullOrEmpty(value)) return default;

		var type = typeof(T);
		if (type.IsEnumerable())
		{
			// convert to array
			var strs = value.Split(',');
			var subtype = type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0];
			var data = Array.CreateInstance(subtype, strs.Length);
			for (int i = 0; i < data.Length; i++) data.SetValue(strs[i].To(subtype), i);

			// convert target type
			if (type.IsArray) return data.To<T>();
			else return Activator.CreateInstance(type, [data]).To<T>();
		}
		else return value.To<T>();
	}

	public T GetValue<T>(ref T value, string section = "Common", [CallerMemberName] string name = null) => value = GetValue<T>(section, name);

	public void SetValue<T>(T value, string section = "Common", [CallerMemberName] string name = null)
	{
		// group
		if (name.Contains('_'))
		{
			var split = name.Split('_', 2);
			section = split[0];
			name = split[1];
		}

		// write
		Configuration[section][name] = value switch
		{
			IEnumerable objs when value is not string => string.Join(",", objs.Cast<object>()),
			_ => value?.ToString()
		};

		new FileIniDataParser().WriteFile(ConfigPath, Configuration);
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
			FileCache.Clear();  // close current database
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
	internal string DefitionKey { get => GetValue<string>(); set => SetValue(value); }
	#endregion

	#region Preview
	public JobSeq Job
	{
		get => GetValue<string>("Preview").ToEnum(JobSeq.검사);
		set => SetValue(value, "Preview");
	}

	public bool Text_LoadData { get => GetValue<bool>(); set => SetValue(value); }
	#endregion
}