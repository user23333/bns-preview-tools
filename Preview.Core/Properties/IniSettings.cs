using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using IniParser;
using IniParser.Model;
using Xylia.Preview.Common.Extension;

namespace Xylia.Preview.Properties;
public class IniSettings : INotifyPropertyChanged
{
	#region Fields	 
	public string ConfigPath { get; private set; }
	protected IniData Configuration;
	public event PropertyChangedEventHandler PropertyChanged;
	#endregion

	#region Constructors
	public IniSettings(string path)
	{
		ConfigPath = path;
		Configuration = File.Exists(ConfigPath) ?
			new FileIniDataParser().ReadFile(ConfigPath, Encoding.UTF8) :
			new IniData();
	}
	#endregion

	#region Methods
	protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(storage, value))
			return false;

		storage = value;
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		return true;
	}

	private static void GetKeyName(ref string section, ref string name)
	{
		ArgumentNullException.ThrowIfNull(name);

		if (section is null)
		{
			if (name.Contains('_'))
			{
				var split = name.IndexOf('_');
				section = name[..split];
				name = name[split..];
			}
			else
			{
				section = "Common";
			}
		}
	}


	public T GetValue<T>(T def = default, [CallerMemberName] string name = null, string section = null)
	{
		GetKeyName(ref section, ref name);
		var value = Configuration[section][name];
		if (string.IsNullOrEmpty(value)) return def;

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

	public T GetValue<T>(ref T value, [CallerMemberName] string name = null, string section = null) => value = GetValue(value, name, section);

	public void SetValue<T>(T value, [CallerMemberName] string name = null, string section = null)
	{
		GetKeyName(ref section, ref name);
		Configuration[section][name] = value switch
		{
			IEnumerable objs when value is not string => string.Join(",", objs.Cast<object>()),
			_ => value?.ToString()
		};

		new FileIniDataParser().WriteFile(ConfigPath, Configuration, Encoding.UTF8);
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}

	public void RemoveKey([CallerMemberName] string name = null, string section = null)
	{
		GetKeyName(ref section, ref name);
		Configuration[section].RemoveKey(name);

		new FileIniDataParser().WriteFile(ConfigPath, Configuration, Encoding.UTF8);
	}

	public KeyDataCollection GetKeys(string section) => Configuration[section];
	#endregion
}